using FluentAssertions;

using Notion.Model;

using System.Collections.Generic;
using System.Text.Json;

using Xunit;

namespace MarkdownExporter.Tests;

public class Heading1ConverterTests
{
    private ConverterSettings? Settings { get; } = new ConverterSettings
    {
        Converter =
            new RichTextConverter(
                        Applicable.Link(Formatters.FormatLink)
                        + Applicable.Bold(Formatters.FormatBold)
                        + Applicable.Italic(Formatters.FormatItalic)
                        + Applicable.Strikethrough(Formatters.FormatStike)
                        + Applicable.Underline(Formatters.FormatUnderline)
                        + Applicable.FormatCode(Formatters.FormatCode)
                        + Applicable.FormatColor(Formatters.FormatColor))
    };

    private Converter<Block.Heading1> Converter { get; } = new Heading1Converter(text => $"# {text}");

    private IEqualityComparer<IOption<List<string>>> Comparer { get; } = new OptionComparer<List<string>>(new ListSequenceComparer<string>());

    [Theory]
    [MemberData(nameof(Blocks))]
    public void Convert_Passes(Block.Heading1 block, string expectedText)
    {
        var result = Converter.Convert(block, Settings).Select(list => JsonSerializer.Serialize(list));

        var expectedResult = new List<string> { expectedText }.ToOption().Select(list => JsonSerializer.Serialize(list));

        Assert.Equal(expectedResult, result, new OptionComparer<string>());
    }

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
    };
}
