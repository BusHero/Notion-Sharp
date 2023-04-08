using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Serilog;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

// ReSharper disable UnusedMember.Local

namespace _build;

partial class Build : NukeBuild
{
	public static int Main() => Execute<Build>(_ => _.Compile);

	[Parameter] readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
	
	[Solution(GenerateProjects = true, SuppressBuildProjectCheck = true)] readonly Solution Solution = null!;

	[Parameter, Secret] readonly string NotionKey = null!;
	[Parameter, Secret] readonly string GithubToken = null!;

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
	AbsolutePath WarningsOutput = RootDirectory / "Output" / "Artifacts" / "warnings";

	Target DisplayNbrWarnings => _ => _
		.Consumes(Compile, nameof(CompileOutput))
		.DependsOn(EnsureArtifactsDirectoryExists)
		.Executes(() =>
		{
			var output = CompileOutput.ToList()[^4];
			var match = Warnings().Match(output.Text).Groups["warnings"];
			Directory.GetParent(WarningsOutput);
			File.WriteAllText(WarningsOutput, match.Value);
		});

	Target EnsureArtifactsDirectoryExists => _ => _
		.Executes(() =>
		{
			var parent = Directory
				.GetParent(WarningsOutput)
				?.FullName ?? throw new Exception($"Cannot get the parent of {WarningsOutput}");
			Directory.CreateDirectory(parent);
		});
	
	Target Test => _ => _
		.DependsOn(StoreSecrets)
		.Executes(() =>
		{
			DotNetTest(_ => _
				.SetProjectFile(Solution)
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
				.EnableIncludeSource()
				.SetVersionSuffix($"{GitHubActions.RunNumber}.{runAttempt}")
				.SetSymbolPackageFormat("snupkg")
				.SetOutputDirectory(PublishFolder)
				.EnableNoRestore());
		});

	Target NugetPublish => _ => _
		.DependsOn(Pack, Test)
		.Executes(() =>
		{
			DotNetNuGetPush(_ => _
				.SetTargetPath(PublishFolder / "*")
				.SetSource("https://nuget.pkg.github.com/BusHero/index.json")
				.SetApiKey(GithubToken)
				.EnableSkipDuplicate());
		});

    [GeneratedRegex("""\s*(?'warnings'\d+) Warning\(s\)""")]
    private static partial Regex Warnings();
}