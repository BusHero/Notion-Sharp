using FluentAssertions;

using Notion.Model;

using Xunit;

namespace MarkdownExporter.Tests;

public class Heading1ConverterTests
{
    private ConverterSettings? Settings { get; } = new ConverterSettings
    {
        Converter =
            new Heading1Converter() + 
            new RichTextConverter(
                        Applicable.Link(Formatters.FormatLink)
                        + Applicable.Bold(Formatters.FormatBold)
                        + Applicable.Italic(Formatters.FormatItalic)
                        + Applicable.Strikethrough(Formatters.FormatStike)
                        + Applicable.Underline(Formatters.FormatUnderline)
                        + Applicable.FormatCode(Formatters.FormatCode)
                        + Applicable.FormatColor(Formatters.FormatColor))
    };

    [Theory]
    [MemberData(nameof(Blocks))]
    public void Convert_Passes(Block.Heading1 block, string expectedText) => Converter
        .Convert(block, Settings)
        .ValueOrDefault(string.Empty)
        .Should()
        .Be(expectedText);

    public static TheoryData<Block.Heading1, string> Blocks { get; } = new()
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
            "# Some text here and there\n"
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
            "# *Some text here and there*\n"
        },
    };
}
