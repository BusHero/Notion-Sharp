using FluentAssertions;

using Notion.Model;

using Xunit;

namespace MarkdownExporter.Tests;

public class ParagraphConverterTests
{
    public ConverterSettings Settings { get; }

    public ParagraphConverterTests()
    {
        Settings = new ConverterSettings
        {
            Converter = 
                new TextConverter(
                    Applicable.Bold(Formatters.FormatBold)
                    + Applicable.Italic(Formatters.FormatItalic)
                    + Applicable.Strikethrough(Formatters.FormatStike)
                    + Applicable.Underline(Formatters.FormatUnderline)
                    + Applicable.FormatCode(Formatters.FormatCode)
                    + Applicable.FormatColor(Formatters.FormatColor))
                + new ParagraphConverter()
        };
    }

    [Theory]
    [MemberData(nameof(Paragraphs))]
    public void Foo(Block.Paragraph block, string expectedText)
    {
        var actualText = Converter.Convert(block, Settings).ValueOrDefault(string.Empty);
        actualText.Should().Be(expectedText);
    }

    public static TheoryData<Block.Paragraph, string> Paragraphs { get; } = new()
    {
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
        },
        {
            new Block.Paragraph
            {
                Text = new RichText[]
                {
                    new RichText.Text
                    {
                        Content = "Some text "
                    },
                    new RichText.Text
                    {
                        Content = "here and there"
                    }
                }
            },
            "Some text here and there"
        },
        {
            new Block.Paragraph
            {
                Text = new RichText[]
                {
                    new RichText.Text
                    {
                        Content = "Some text ",
                        Annotations = new Annotations
                        {
                            Bold = true
                        }
                    },
                    new RichText.Text
                    {
                        Content = "here and there",
                        Annotations = new Annotations
                        {
                            Italic = true
                        }
                    }
                }
            },
            "*Some text ***here and there**"
        },
    };
}
