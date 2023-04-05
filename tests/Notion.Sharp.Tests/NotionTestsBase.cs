using Microsoft.Extensions.Configuration;

namespace Notion.Sharp.Tests;

public class NotionTestsBase
{
    internal INotion Sut { get; }

    protected NotionTestsBase()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<ModelTests>()
            .Build();

        Sut = Notion.NewClient(bearerToken: configuration["Notion"], "2022-06-28");
    }

    protected static async Task RetryAsync(Func<Task> action, int attempts)
    {
        for (var attempt = 0; attempt < attempts; attempt++)
        {
            try
            {
                await action();
                break;
            }
            catch (NotionException ex) when (ex.Message != "Conflict occured while saving. Please try again.")
            {
                throw;
            }
        }
    }
}
