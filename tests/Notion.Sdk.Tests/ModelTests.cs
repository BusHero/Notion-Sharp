using FluentAssertions;

using Microsoft.Extensions.Configuration;

using Xunit;
using System.Threading.Tasks;

namespace Notion.Sdk.Tests
{
    public class ModelTests
    {
        private INotion SUT { get; }

        public ModelTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<ModelTests>()
                .Build();

            var bearerToken = configuration["Notion"];
            
            SUT = Notion.NewClient(bearerToken: bearerToken);
        }

        [Fact]
        public async Task Search_Succeds_OnValidParameter()
        {
            string result = await SUT.SearchAsync(new(
                query: "foo",
                sort: new Sort(direction: "ascending", timestamp: "last_edited_time"),
                filter: new Filter(value: "database", property: "object"),
                page_size: 100));
            result.Should().NotBeNullOrEmpty();
        }
    }
}
