using Microsoft.Extensions.Configuration;

using Notion.Model;

using Xunit;

namespace MarkdownExporter.Tests;

public class MarkdownExporterTests
{
    public MarkdownExporter SUT { get; }

    public MarkdownExporterTests ()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<MarkdownExporterTests>()
            .Build();

        SUT = new MarkdownExporter(configuration["Notion"]);
    }

    [Fact]
    public void CanConvertHeading1()
    {
        var heading = new Block.Heading1
        {
            Text = new RichText[]
            {
                new RichText.Text
                {
                    Content = "Some text here and there"
                },
                new RichText.Text
                {
                    Content = "Some text here and there"
                }
            }
        };

        string actualResult = SUT.Convert(heading);
        Assert.Equal("# Some text here and there", actualResult);
    }

    [Fact]
    public void CanConvertRichText()
    {
        var richText = new RichText.Text
        {
            Content = "Some text here and there"
        };
        string actualString = SUT.Convert(richText);
        Assert.Equal("Some text here and there", actualString);
    }
}
