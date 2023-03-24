using Microsoft.Extensions.Configuration;

namespace Notion.Sharp.Tests;

public class NotionTestsBase
{
    internal INotion SUT { get; }

    internal Guid ValidUserId { get; }

    internal Guid ValidDatabaseId { get; }

    internal Guid ValidPageId { get; }

    internal Guid ValidBlockId { get; }

    internal Guid PageFromDatabase { get; }

    internal Guid SimpleDatabase { get; }
    internal Guid ParentPage { get; }

    internal Guid DeletedPage { get; }
    protected NotionTestsBase()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<ModelTests>()
            .Build();

        SUT = Notion.NewClient(bearerToken: configuration["Notion"], "2022-06-28");
        // ValidUserId = Guid.Parse(configuration["userId"]);
        ValidDatabaseId = Guid.Parse(configuration["databaseId"]);
        ValidPageId = Guid.Parse(configuration["pageId"]);
        ValidBlockId = Guid.Parse(configuration["blockId"]);
        // PageFromDatabase = Guid.Parse(configuration["pageFromDatabase"]);
        // SimpleDatabase = Guid.Parse(configuration["simpleDatabase"]);
        ParentPage = Guid.Parse((configuration["ParentPage"]));
        DeletedPage = Guid.Parse(configuration["deletedPage"]);
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
