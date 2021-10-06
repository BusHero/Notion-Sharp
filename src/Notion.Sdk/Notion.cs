using Refit;

using System;
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
                ExceptionFactory = httpResponseMessage => Task.FromResult<Exception>(new NotionException())
            });
        }
    }
}
