using MarkdownExporter.Converters;

using Notion.Model;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace MarkdownExporter.Tests;

public class Heading2ConverterTests
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
            new Heading2Converter())
    };

    [Theory]
    [MemberData(nameof(Blocks))]
    public void ConvertHeading2_Succeds(Block.Heading2 heading2, string expectedText)
    {
        var actualResult = Lists.Of(expectedText).ToOption().Select(list => JsonSerializer.Serialize(list));

        var result = Converter.Convert(heading2, Settings).Select(list => JsonSerializer.Serialize(list));

        result.Should().Be(actualResult);
    }


    public static TheoryData<Block.Heading2, string> Blocks { get; } = new TheoryData<Block.Heading2, string>
    {
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
            }, "## Some text here and there" },
    };
}
