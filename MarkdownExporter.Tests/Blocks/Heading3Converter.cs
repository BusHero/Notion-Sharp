using MarkdownExporter.Converters;

using Notion.Model;

using System.Text.Json;

namespace MarkdownExporter.Tests;

public class Heading3ConverterTests
{
    public ConverterSettings Settings { get; } = new ConverterSettings
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
            new Heading3Converter()
            )
    };

    [Theory]
    [MemberData(nameof(Blocks))]
    public void ConvertHeading3_Succeds(Block.Heading3 heading3, string expectedText)
    {
        var actualResult = Lists.Of(expectedText).ToOption().Select(list => JsonSerializer.Serialize(list));

        var result = Converter.Convert(heading3, Settings).Select(list => JsonSerializer.Serialize(list));

        result.Should().Be(actualResult);
    }


    public static TheoryData<Block.Heading3, string> Blocks { get; } = new ()
    {
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
            }, "### Some text here and there" },
    };
}
