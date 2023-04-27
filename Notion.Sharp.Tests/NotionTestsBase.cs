using Microsoft.Extensions.Configuration;

namespace Notion.Sharp.Tests;

public class NotionTestsBase
{
    internal INotion Sut { get; }
    internal NotionClient NotionClient { get; }
    
    protected NotionTestsBase()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<ModelTests>()
            .Build();

        var token = configuration["Notion"] ?? throw new InvalidOperationException();
        Sut = Notion.NewClient(token);
        NotionClient = new NotionClient(new Credentials(token));
    }

    protected static async Task RetryAsync(Func<Task> action, int attempts)
    {
        for (var attempt = 0; attempt < attempts; )
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
