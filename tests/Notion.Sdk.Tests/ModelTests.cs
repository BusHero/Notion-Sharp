using FluentAssertions;

using Microsoft.Extensions.Configuration;

using Xunit;
using Notion;
using System.Threading.Tasks;
using System;

namespace Notion.Sdk.Tests
{
    public class ModelTests
    {
        private IConfiguration Configuration { get; }
        private string NotionApiKey { get; }
        private INotion SUT { get; }

        public ModelTests()
        {
            var builder = new ConfigurationBuilder().AddUserSecrets<ModelTests>();

            Configuration = builder.Build();
            SUT = Notion.NewClient(Configuration["Notion"]);
        }
        
        [Fact]
        public async Task Search_Succeds_OnValidParameter()
        {
            string result = await SUT.SearchAsync(new
            {
                query = "foo",
                sort = new
                {
                    direction = "ascending",
                    timestamp = "last_edited_time"
                },
                filter = new
                {
                    value = "database",
                    property = "object"
                },
                page_size = 100
            });
            result.Should().NotBeNullOrEmpty();
        }


        [Fact]
        public async void Search_Fails_OnInValidParameter()
        {
            await SUT.Awaiting(sut => sut.SearchAsync(new
            {
                query = default(string)
            })).Should().ThrowAsync<NotionException>();
        }
    }
}
