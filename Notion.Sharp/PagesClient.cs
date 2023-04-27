using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Notion.Converters;
using Notion.Model;

namespace Notion;

internal class PagesClient : IPagesClient
{
    private readonly Guid _pageId;
    private readonly HttpClient _httpClient;

    public PagesClient(Guid pageId, HttpClient httpClient)
    {
        _pageId = pageId;
        _httpClient = httpClient;
    }

    public async Task<Page> Get(CancellationToken cancellationToken)
    {
        using var response = await _httpClient
            .GetAsync($"pages/{_pageId}", cancellationToken);

        var options = new JsonSerializerOptions
        {
            Converters =
            {
                new ColorConverter(),
                new UserConverter(),
                new BlockConverter(),
                new RichTextConverter(),
                new ParentConverter(),
                new PropertyConverter(),
                new PropertyValueConverter(),
                new PageOrDatabaseConverter(),
                new FileConverter(),
                new IconConverter(),
                new CoverConverter(),
            }
        };
        
        var page = await response
            .EnsureSuccessStatusCode()
            .Content
            .ReadFromJsonAsync<Page>(options, cancellationToken);
        
        if (page is null)
        {
            throw new InvalidOperationException();
        }
        return page;
    }
}