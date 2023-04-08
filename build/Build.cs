using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
// ReSharper disable UnusedMember.Local

namespace _build;

class Build : NukeBuild
{
	public static int Main() => Execute<Build>(_ => _.Compile);

	[Parameter] readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
	
	[Solution(GenerateProjects = true)] readonly Solution Solution = null!;

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
		.Triggers(Test)
		.Executes(() =>
		{
			DotNetBuild(_ => _
				.SetProjectFile(Solution)
				.EnableNoRestore()
				.SetConfiguration(Configuration));
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
			DotNetPack(_ => _
				.SetConfiguration(Configuration)
				.SetProject(Solution.src.Notion_Sharp)
				.EnableIncludeSource()
				.SetVersionSuffix(GitHubActions.RunId.ToString())
				.SetSymbolPackageFormat("snupkg")
				.SetOutputDirectory(PublishFolder)
				.EnableNoRestore());
		});

	Target NugetPublish => _ => _
		.DependsOn(Pack)
		.Executes(() =>
		{
			DotNetNuGetPush(_ => _
				.SetTargetPath(PublishFolder / "*")
				.SetSource("https://nuget.pkg.github.com/BusHero/index.json")
				.SetApiKey(GithubToken)
				.EnableSkipDuplicate());
		});
}