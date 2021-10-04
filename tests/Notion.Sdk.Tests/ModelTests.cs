using FluentAssertions;

using Microsoft.Extensions.Configuration;

using Xunit;
using Notion;

namespace Notion.Sdk.Tests
{
    public class ModelTests
    {
        private string NotionApiKey { get; }
        private INotion SUT { get; }

        public ModelTests()
        {
            var builder = new ConfigurationBuilder().AddUserSecrets<ModelTests>();

            var configuration = builder.Build();
            SUT = Notion.NewClient(configuration["Notion"]);
        }


        [Fact]
        public void ValidateNotionKey()
        {

        }
    }
}
