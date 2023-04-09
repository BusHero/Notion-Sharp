using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitHub;
using Nuke.Common.Tools.Kubernetes;
using Nuke.Common.Utilities.Net;
using Octokit;
using Octokit.Internal;
using Serilog;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

// ReSharper disable UnusedMember.Local

namespace _build;

partial class Build : NukeBuild
{
	public static int Main() => Execute<Build>(_ => _.Compile);

	[Nuke.Common.Parameter] readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
	
	[Solution(GenerateProjects = true, SuppressBuildProjectCheck = true)] readonly Solution Solution = null!;

	[Nuke.Common.Parameter, Secret] readonly string NotionKey = null!;
	[Nuke.Common.Parameter, Secret] readonly string GithubToken = "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";

	readonly AbsolutePath PublishFolder = RootDirectory / "publish";

#pragma warning disable CA1822
	GitHubActions GitHubActions => GitHubActions.Instance;
#pragma warning restore CA1822

	Target Restore => _ => _
		.Executes(() =>
		{
			DotNetRestore(_ => _
				.SetProjectFile(Solution));
		});

	Target Compile => _ => _
		.DependsOn(Restore)
		.Triggers(Test, DisplayNbrWarnings)
		.Produces(nameof(WarningsOutput))
		.Executes(() =>
		{
			CompileOutput = DotNetBuild(_ => _
				.SetProjectFile(Solution)
				.EnableNoRestore()
				.SetConfiguration(Configuration));

		});

	IReadOnlyCollection<Output> CompileOutput = null!;
	readonly AbsolutePath WarningsOutput = RootDirectory / "output" / "artifacts" / "warnings";
	readonly AbsolutePath TestOutput = RootDirectory / "output" / "artifacts"  / "tests";
	int PreviousWarningsCount;
	int CurrentWarningsCount;
	
	Target DisplayNbrWarnings => _ => _
		.Consumes(Compile, nameof(CompileOutput))
		.DependsOn(EnsureArtifactsDirectoryExists)
		.Unlisted()
		.Executes(() =>
		{
			var output = CompileOutput.ToList()[^4];
			var match = Warnings().Match(output.Text).Groups["warnings"];
			CurrentWarningsCount = int.Parse(match.Value);
			Directory.GetParent(WarningsOutput);
			File.WriteAllText(WarningsOutput, match.Value);
		});
	
	Target EnsureArtifactsDirectoryExists => _ => _
		.Unlisted()
		.Executes(() =>
		{
			var parent = Directory
				.GetParent(WarningsOutput)
				?.FullName ?? throw new Exception($"Cannot get the parent of {WarningsOutput}");
			Directory.CreateDirectory(parent);
			Log.Information("Create {Directory}", parent);
		});
	
	Target Test => _ => _
		.DependsOn(StoreSecrets)
		.Executes(() =>
		{
			DotNetTest(_ => _
				.SetProjectFile(Solution)
				.SetLoggers($"trx;LogFileName={TestOutput}")
				.EnableNoBuild()
				.EnableNoRestore()
				.SetConfiguration(Configuration));
		});

	Target StoreSecrets => _ => _
		.OnlyWhenDynamic(() => Host is GitHubActions)
		.Unlisted()
		.Executes(() =>
		{
			DotNet($"""
			user-secrets set "Notion" "{NotionKey}" --project {Solution.tests.Notion_Sharp_Tests}
			""");
		});
	
	Target Pack  => _ => _
		.DependsOn(Compile)
		.Executes(() =>
		{
			var runAttempt = EnvironmentInfo.GetVariable<long>("GITHUB_RUN_ATTEMPT");
			DotNetPack(_ => _
				.SetConfiguration(Configuration)
				.SetProject(Solution.src.Notion_Sharp)
				.EnableNoBuild()
				.EnableNoRestore()
				.EnableNoLogo()
				.EnableDeterministic()
				.EnableIncludeSource()
				.SetVersionSuffix($"{GitHubActions.RunNumber}.{runAttempt}")
				.SetSymbolPackageFormat("snupkg")
				.SetOutputDirectory(PublishFolder)
				.EnableNoRestore());
		});

	Target NugetPublish => _ => _
		.Description("Publish to GitHub nuget index")
		.DependsOn(Pack, Test, CompareWithPreviousNbrOfWarnings)
		.Executes(() =>
		{
			DotNetNuGetPush(_ => _
				.SetTargetPath(PublishFolder / "*")
				.SetSource("https://nuget.pkg.github.com/BusHero/index.json")
				.SetApiKey(GithubToken)
				.EnableSkipDuplicate());
		});

	Target SetupGitHubClient => _ => _
		.Unlisted()
		.Executes(() =>
		{
			var credentials = new Credentials(GithubToken);
			GitHubTasks.GitHubClient = new GitHubClient(
					new ProductHeaderValue("NotionSharp"),
					new InMemoryCredentialStore(credentials));
		});

	Target GetPreviousNbrOfWarnings => _ => _
		.DependsOn(SetupGitHubClient)
		.Unlisted()
		.Executes(async () =>
		{
			var downloadUrl = await GetDownloadUri();
			using var httpClient = new HttpClient();
			httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
			httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {GithubToken}");
			httpClient.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
			httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");
			var responseMessage = await httpClient.GetAsync(downloadUrl);
			await using var zipContent = await responseMessage.Content.ReadAsStreamAsync();
			using var zipArchive = new ZipArchive(zipContent, ZipArchiveMode.Read);
			foreach (var entry in zipArchive.Entries)
			{
				await using var entryContent = entry.Open();
				using var reader = new StreamReader(entryContent);
				PreviousWarningsCount = int.Parse(reader.ReadLine() ?? string.Empty);
			}
		});

	Target CompareWithPreviousNbrOfWarnings => _ => _
		.DependsOn(GetPreviousNbrOfWarnings, DisplayNbrWarnings)
		.Consumes(GetPreviousNbrOfWarnings, DisplayNbrWarnings)
		.Executes(() =>
		{
			Assert.True(PreviousWarningsCount >= CurrentWarningsCount);
		});

	
	async Task<string?> GetDownloadUri()
	{
		using var httpClient = new HttpClient();
		httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
		httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {GithubToken}");
		httpClient.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
		httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");
		var responseMessage = await httpClient.GetAsync("https://api.github.com/repos/BusHero/Notion-Sharp/actions/artifacts");
		if (!responseMessage.IsSuccessStatusCode)
		{
			throw new Exception("Call failed");
		}
		var document = await JsonDocument.ParseAsync(await responseMessage.Content.ReadAsStreamAsync());
		var artifacts = document.RootElement.GetProperty("artifacts");
		foreach (var artifact in artifacts.EnumerateArray())
		{
			if (artifact.TryGetProperty("name", out var name) && name.GetString() == "warnings")
			{
				var archiveDownloadUrl = artifact.GetProperty("archive_download_url");
				return archiveDownloadUrl.GetString();
			}
		}

		throw new Exception("Something wired happened");
	}

    [GeneratedRegex("""\s*(?'warnings'\d+) Warning\(s\)""")]
    private static partial Regex Warnings();
}