using Notion.Converters;

using Refit;

using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Notion
{
    public static class Notion
    {
        public static INotion NewClient(string bearerToken, string version)
        {
            return RestService.For<INotion>("https://api.notion.com/v1/", new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(bearerToken),
                ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
                {
                    
                    //PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    IgnoreNullValues = true,
                    Converters =
                    {
                        new UserConverter(),
                        new BlockConverter(),
                        new RichTextConverter(),
                        new ParentConverter()
                    }
                }),
                ExceptionFactory = GetException,
                HttpMessageHandlerFactory = () => new AuthHeaderHandler(version)
                {
                    InnerHandler = new HttpClientHandler()
                }
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
        
        private class AuthHeaderHandler : DelegatingHandler
        {
            public AuthHeaderHandler(string version)
            {
                Version = version ?? throw new ArgumentNullException(nameof(version));
            }

            private string Version { get; }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                request.Headers.Add("Notion-Version", Version);
                return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
