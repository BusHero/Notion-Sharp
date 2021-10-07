using Refit;

using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Notion
{
    public static class Notion
    {
        public record ErrorDTO(string message, string code, int status);

        public static INotion NewClient(string bearerToken)
        {
            return RestService.For<INotion>("https://api.notion.com/v1/", new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(bearerToken),
                ExceptionFactory = async httpResponseMessage =>
                {
                    if (httpResponseMessage.IsSuccessStatusCode)
                        return await Task.FromResult(default(Exception));

                    var stream = await httpResponseMessage.Content.ReadAsStreamAsync();
                    var error = await JsonSerializer.DeserializeAsync<ErrorDTO>(stream, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                    var exception = new NotionException(error.message)
                    {
                        Code = error.code,
                        Status = error.status,
                    };
                    return await Task.FromResult(exception);
                }
            });
        }
    }
}
