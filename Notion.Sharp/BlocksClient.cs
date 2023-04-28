using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Notion.Converters;
using Notion.Model;

namespace Notion;

internal class BlocksClient : IBlocksClient
{
    private readonly Guid _blockId;
    private readonly HttpClient _httpClient;

    public BlocksClient(Guid blockId, HttpClient httpClient)
    {
        _blockId = blockId;
        _httpClient = httpClient;
    }
    
    public async Task<Block> Get(CancellationToken cancellationToken)
    {
        using var response = await _httpClient
            .GetAsync($"blocks/{_blockId}", cancellationToken);

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
            },
        };

        var block = await response
                        .EnsureSuccessStatusCode()
                        .Content
                        .ReadFromJsonAsync<Block>(options, cancellationToken)
                    ?? throw new InvalidOperationException();
        
        return block;
    }
}