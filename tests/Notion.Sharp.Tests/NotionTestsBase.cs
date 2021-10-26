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

    public NotionTestsBase()
    {
        var configuration = new ConfigurationBuilder()
        .AddUserSecrets<ModelTests>()
        .Build();

        SUT = Notion.NewClient(bearerToken: configuration["Notion"], "2021-08-16");
        ValidUserId = Guid.Parse(configuration["userId"]);
        ValidDatabaseId = Guid.Parse(configuration["databaseId"]);
        ValidPageId = Guid.Parse(configuration["pageId"]);
        ValidBlockId = Guid.Parse(configuration["blockId"]);
        PageFromDatabase = Guid.Parse(configuration["pageFromDatabase"]);
        SimpleDatabase = Guid.Parse(configuration["simpleDatabase"]);
    }

    public async Task Retry(Func<Task> action, int attempts)
    {
        for (var attempt = 0; attempt < attempts; attempt++)
        {
            try
            {
                await action();
                break;
            }
            catch (NotionException ex)
            {
                if (ex.Message == "Conflict occurred while saving. Please try again.")
                    continue;
                throw;
            }
        }
    }
}
