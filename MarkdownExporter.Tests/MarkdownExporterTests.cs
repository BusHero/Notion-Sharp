using Microsoft.Extensions.Configuration;

using Notion.Model;

using Xunit;

namespace MarkdownExporter.Tests;

public class MarkdownExporterTests
{
    public Exporter SUT { get; }

    public MarkdownExporterTests ()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<MarkdownExporterTests>()
            .Build();

        SUT = Exporters.MarkdownExporter(configuration["Notion"]);
    }

    [Theory]
    [MemberData(nameof(Blocks))]
    public void CanConvert_Block(Block block, string expectedText)
    {
        string actualResult = SUT.Convert(block);
        Assert.Equal(expectedText, actualResult);
    }

    [Theory]
    [MemberData(nameof(RichTexts))]
    public void CanConvert_RichText(RichText richText, string expectedText)
    {
        Assert.Equal(expectedText, SUT.Convert(richText));
    }

    public static TheoryData<RichText, string> RichTexts { get; } = new ()
    {
        { new RichText.UserMention { User = new User.Person { Name = "Petru Cervac" } }, "**@Petru Cervac**" },
        { new RichText.Text { Content = "Some text here and there" }, "Some text here and there" },
        { new RichText.Text { Content = "Some text here and there", Annotations = new Annotations
            {
                Bold = true
            }
        }, "*Some text here and there*" },
        {
            new RichText.Text
            {
                Content = "Some text here and there",
                Annotations = new Annotations
                {
                    Strikethrough = true
                }
            },
            "~~Some text here and there~~"
        },
        {
            new RichText.Text
            {
                Content = "Some text here and there",
                Annotations = new Annotations
                {
                    Underline = true
                }
            },
            "<u>Some text here and there</u>"
        },
        {
            new RichText.Text
            {
                Content = "Some text here and there",
                Annotations = new Annotations
                {
                    Code = true
                }
            },
            "`Some text here and there`"
        },
        {
            new RichText.Text
            {
                Content = "Some text here and there",
                Annotations = new Annotations
                {
                    Color = Color.Blue
                }
            },
            "<span style=\"color: blue\">Some text here and there</span>"
        },
        {
            new RichText.Text
            {
                Content = "Some text here and there",
                Annotations = new Annotations
                {
                    Italic = true,
                    Bold = true,
                }
            },
            "***Some text here and there***"
        },

        { new RichText.Equation { Expression = "1 + 1" }, "`1 + 1`" }
    };

    public static TheoryData<Block, string> Blocks { get; } = new()
    {
        {
            new Block.Heading1
            {
                Text = new RichText[]
                {
                    new RichText.Text
                    {
                        Content = "Some text here and there"
                    }
                }
            }, "# Some text here and there"
        },
        {
            new Block.Heading2
            {
                Text = new RichText[]
                {
                    new RichText.Text
                    {
                        Content = "Some text here and there"
                    }
                }
            },
            "## Some text here and there"
        },
        {
            new Block.Paragraph
            {
                Text = new RichText[]
                {
                    new RichText.Text
                    {
                        Content = "Some text here and there"
                    }
                }
            },
            "Some text here and there"
        }
    };
}
