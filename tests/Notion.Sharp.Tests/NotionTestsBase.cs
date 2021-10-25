using Microsoft.Extensions.Configuration;

using System;

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
}
