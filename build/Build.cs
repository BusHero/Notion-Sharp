using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitHub;
using Octokit;
using Octokit.Internal;
using Serilog;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using ParameterAttribute = Nuke.Common.ParameterAttribute;

// ReSharper disable UnusedMember.Local

namespace _build;

partial class Build : NukeBuild
{
	public static int Main() => Execute<Build>(_ => _.Compile);

	[Parameter] readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
	[Parameter, Secret] readonly string? NotionKey;
	[Parameter, Secret] readonly string? GithubToken;
	
	[Solution(GenerateProjects = true, SuppressBuildProjectCheck = true)] readonly Solution Solution = null!;
	[GitRepository] readonly GitRepository Repository = null!;

    static AbsolutePath PublishFolder => RootDirectory / "publish";

    static AbsolutePath WarningsOutput => RootDirectory / "output" / "artifacts" / "warnings";

    static AbsolutePath CoverageResultFile => RootDirectory / "output" / "coverage"  / "coverage.json";
    static AbsolutePath TestOutput => RootDirectory / "output" / "artifacts"  / "tests";
	IReadOnlyCollection<Output> CompileOutput { get; set; } = null!;
	int PreviousWarningsCount { get; set; }
	int CurrentWarningsCount { get; set; }
	string Owner => Repository.GetGitHubOwner();
	string Name => Repository.GetGitHubName();
	HttpClient? Client { get; set; }

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
		.Triggers(Test, GetCurrentWarningsCount)
		.Produces(nameof(WarningsOutput))
		.Executes(() =>
		{
			CompileOutput = DotNetBuild(_ => _
				.SetProjectFile(Solution)
				.EnableNoRestore()
				.SetConfiguration(Configuration));

		});
	
	Target GetCurrentWarningsCount => _ => _
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
			Log.Information("Current nbr. of warnings: {Warnings}", match.Value);
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
				.EnableCollectCoverage()
				.SetCoverletOutput(CoverageResultFile)
				.AddProperty("MergeWith", CoverageResultFile)
				.EnableNoBuild()
				.EnableNoRestore()
				.EnableNoLogo()
				.EnableUseSourceLink()
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
			DotNetPack(_ => _
				.SetConfiguration(Configuration)
				.SetProject(Solution.src.Notion_Sharp)
				.EnableNoBuild()
				.EnableNoRestore()
				.EnableNoLogo()
				.EnableDeterministic()
				.EnableIncludeSource()
				.SetVersionSuffix($"{GitHubActions.RunNumber}.{GitHubActions.RunAttempt}")
				.SetSymbolPackageFormat("snupkg")
				.SetOutputDirectory(PublishFolder)
				.EnableNoRestore());
		});

	Target NugetPublish => _ => _
		.Description("Publish to GitHub nuget index")
		.DependsOn(Pack, Test, CompareWithPreviousWarningsCount)
		.Executes(() =>
		{
			DotNetNuGetPush(_ => _
				.SetTargetPath(PublishFolder / "*")
				.SetSource($"https://nuget.pkg.github.com/{Owner}/index.json")
				.SetApiKey(GithubToken)
				.EnableSkipDuplicate());
		});

	Target SetupGitHubClient => _ => _
		.Unlisted()
		.Executes(() =>
		{
			var credentials = new Credentials(GithubToken);
			GitHubTasks.GitHubClient = new GitHubClient(
					new ProductHeaderValue(Name),
					new InMemoryCredentialStore(credentials));
			Log.Information("Authenticated GitHub client created");
		});

	Target GetPreviousWarningsCount => _ => _
		.DependsOn(SetupGitHubClient, SetUpHttpClient)
		.Unlisted()
		.Executes(async () =>
		{
			Client.NotNull();
			var downloadUrl = await GetDownloadUri();
			var responseMessage = await Client!.GetAsync(downloadUrl);
			await using var zipContent = await responseMessage.Content.ReadAsStreamAsync();
			using var zipArchive = new ZipArchive(zipContent, ZipArchiveMode.Read);
			foreach (var entry in zipArchive.Entries)
			{
				await using var entryContent = entry.Open();
				using var reader = new StreamReader(entryContent);
				PreviousWarningsCount = int.Parse(reader.ReadLine() ?? string.Empty);
				Log.Information("Previous nbr. of warnings: {Warnings}", PreviousWarningsCount);
			}
		});

	Target CompareWithPreviousWarningsCount => _ => _
		.DependsOn(GetPreviousWarningsCount, GetCurrentWarningsCount)
		.Consumes(GetPreviousWarningsCount, GetCurrentWarningsCount)
		.Executes(() =>
		{
			var result = PreviousWarningsCount >= CurrentWarningsCount;
			Log.Information(
				"Previous({PreviousWarningCount}) >= Current({CurrentWarningsCount}): {Result}", 
				PreviousWarningsCount, 
				CurrentWarningsCount,
				result);
			Assert.True(result);
		});
	
	Target SetUpHttpClient => _ => _
		.Requires(() => GithubToken)
		.Unlisted()
		.Executes(() =>
		{
			Client = new HttpClient();
			Client.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
			Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GithubToken}");
			Client.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
			Client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");
			Log.Information("Http Client set up");
		});
	
	async Task<string?> GetDownloadUri()
	{
		Client.NotNull();
		var responseMessage = await Client!
			.GetAsync($"https://api.github.com/repos/{Owner}/{Name}/actions/artifacts");
		if (!responseMessage.IsSuccessStatusCode)
		{
			throw new Exception("Call failed");
		}
		var document = await JsonDocument.ParseAsync(await responseMessage.Content.ReadAsStreamAsync());
		var artifacts = document.RootElement.GetProperty("artifacts");
		foreach (var artifact in artifacts.EnumerateArray())
		{
			if (!artifact.TryGetProperty("name", out var name) || name.GetString() != "warnings") continue;
			var archiveDownloadUrl = artifact.GetProperty("archive_download_url");
			return archiveDownloadUrl.GetString();
		}

		throw new Exception("Something wired happened");
	}

    [GeneratedRegex("""\s*(?'warnings'\d+) Warning\(s\)""")]
    private static partial Regex Warnings();
}