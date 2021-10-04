using FluentAssertions;

using Microsoft.Extensions.Configuration;

using Xunit;
using Notion;

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
        public void ValidateNotionKey()
        {
        }
    }
}
