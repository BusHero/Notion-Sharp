using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
// ReSharper disable UnusedMember.Local

namespace _build;

class Build : NukeBuild
{
	public static int Main() => Execute<Build>(_ => _.Compile);

	[Parameter] readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
	
	[Solution(GenerateProjects = true)] readonly Solution Solution;

	[Secret] readonly string NotionKey;

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
		.Executes(() =>
		{
			DotNet($"""
			user-secrets set "Notion" "{NotionKey}" --project {Solution.tests.Notion_Sharp_Tests}
			""");
		});
}