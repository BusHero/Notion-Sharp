using System;
using System.Net.Http;

namespace Notion;

public class Credentials
{
    public Credentials(string token) => Token = token;

    internal string Token { get; }
}

public class NotionClient
{
    private readonly HttpClient _httpClient;

    public NotionClient(Credentials credentials)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.notion.com/v1/"),
            DefaultRequestHeaders =
            {
                { "accept", "application/json" },
                { "Notion-Version", "2022-06-28" },
                { "Authorization", credentials.Token },
            },
        };
    }

    public IPagesClient Page(Guid id) => new PagesClient(id, _httpClient);

    public IBlocksClient Block(Guid id) => new BlocksClient(id, _httpClient);
}