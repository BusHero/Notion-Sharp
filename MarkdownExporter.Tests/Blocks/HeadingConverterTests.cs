
using Notion;
using Notion.Model;

using NSubstitute;

using System.Collections.Generic;
using System.Text.Json;

namespace MarkdownExporter.Tests;

public class HeadingConverterTests
{
    private ConverterSettings? Settings { get; } = new ConverterSettings
    {
        Converter = new CompositeConverter(
            new RichTextConverter(
                Applicable.Link(Formatters.FormatLink)
                + Applicable.Bold(Formatters.FormatBold)
                + Applicable.Italic(Formatters.FormatItalic)
                + Applicable.Strikethrough(Formatters.FormatStike)
                + Applicable.Underline(Formatters.FormatUnderline)
                + Applicable.FormatCode(Formatters.FormatCode)
                + Applicable.FormatColor(Formatters.FormatColor)),
            new FooConverter<Block.Heading1>(Substitute.For<INotion>(), h1 => h1.Text, text => $"# {text}", _ => _),
            new FooConverter<Block.Heading2>(Substitute.For<INotion>(), h2 => h2.Text, text => $"## {text}", _ => _),
            new FooConverter<Block.Heading3>(Substitute.For<INotion>(), h3 => h3.Text, text => $"### {text}", _ => _))
    };

    [Theory]
    [MemberData(nameof(Blocks))]
    public void Convert_Passes(Block block, string expectedText)
    {
        var result = Converter.Convert(block, Settings).Select(list => JsonSerializer.Serialize(list));

        var expectedResult = new List<string> { expectedText }.ToOption().Select(list => JsonSerializer.Serialize(list));

        Assert.Equal(expectedResult, result, new OptionComparer<string>());
    }

    public static TheoryData<Block, string> Blocks { get; } = new()
    {
        { 
            new Block.Heading1
            {
                Text = new[] 
                {
                    new RichText.Text
                    {
                        Content = "Some text here and there",
                        PlainText = "Some text here and there"
                    } as RichText
                }
            }, 
            "# Some text here and there"
        },
        {
            new Block.Heading1
            {
                Text = new[]
                {
                    new RichText.Text
                    {
                        Content = "Some text here and there",
                        Annotations = new()
                        {
                            Bold = true
                        },
                        PlainText = "Some text here and there"
                    } as RichText
                }
            },
            "# *Some text here and there*"
        },

        {
            new Block.Heading2
            {
                Text = new[]
                {
                    new RichText.Text
                    {
                        Content = "Some text here and there",
                        PlainText = "Some text here and there"
                    }
                }
            },
            "## Some text here and there"
        },
        {
            new Block.Heading2
            {
                Text = new[]
                {
                    new RichText.Text
                    {
                        Content = "Some text",
                        PlainText = "Some text"
                    },
                    new RichText.Text
                    {
                        Content = " here and there",
                        PlainText = " here and there"
                    }
                }
            },
            "## Some text here and there"
        },
        {
            new Block.Heading3
            {
                Text = new[]
                {
                    new RichText.Text
                    {
                        Content = "Some text here and there",
                        PlainText = "Some text here and there"
                    }
                }
            },
            "### Some text here and there"
        },
        {
            new Block.Heading3
            {
                Text = new[]
                {
                    new RichText.Text
                    {
                        Content = "Some text",
                        PlainText = "Some text"
                    },
                    new RichText.Text
                    {
                        Content = " here and there",
                        PlainText = " here and there"
                    }
                }
            },
            "### Some text here and there"
        },
    };
}
