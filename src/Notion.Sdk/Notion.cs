using Refit;

using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Notion
{
    public static class Notion
    {
        public static INotion NewClient(string bearerToken)
        {
            return RestService.For<INotion>("https://api.notion.com/v1/", new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(bearerToken),
                ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
                {
                    IgnoreNullValues = true
                }),
                ExceptionFactory = GetException
            });
        }

        private record ErrorDTO(string message, string code, int status);

        private static async Task<Exception> GetException(HttpResponseMessage httpResponseMessage)
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
    }
}
