using Notion;
using Notion.Model;

using NSubstitute;

using System;
using System.Collections.Generic;
using System.Text.Json;

namespace MarkdownExporter.Tests;

public class NumberedListConverterTests
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
                        new NumberedListConverter(Substitute.For<INotion>())

            )
    };

    [Fact]
    public void ParseNumberedList_WithChildren_Succeds()
    {
        var parentId = Guid.NewGuid(); 
        var parent = new Block.NumberedListItem
        {
            Id = parentId,
            HasChildren = true,
            Text = new RichText[]
             {
                new RichText.Text
                {
                    Content = "Parent",
                    PlainText = "Parent"
                }
             }
        };
        var child = new Block.NumberedListItem
        {
            HasChildren = false,
            Text = new RichText[]
            {
                new RichText.Text
                {
                    Content = "Child",
                    PlainText = "Child"
                }
            },
        };
        var notion = Substitute.For<INotion>();
        notion.GetBlocksChildrenAsync(parentId, 100, default).Returns(Arrays.Of<Block>(child).Paginated().ToTask());
        var converter = new NumberedListConverter(notion);
        var settings = new ConverterSettings
        {
            Converter = 
                new CompositeConverter(
                    converter,
                    new RichTextConverter(Applicable.ToAplicable((RichText _, string text) => text)))
        };
        var comparer = new OptionComparer<string>(EqualityComparer<string>.Default);
        var expectedResult = Lists
            .Of("1. Parent", "&nbsp;&nbsp;&nbsp;&nbsp;1. Child")
            .ToOption()
            .Select(text => JsonSerializer.Serialize(text));

        var actualResult = converter.Convert(parent, settings)
            .Select(text => JsonSerializer.Serialize(text));

        Assert.Equal(expectedResult, actualResult, comparer);

    }

    [Fact]
    public void ParseNumberedList_WithGrandChildren_Succeds()
    {
        var parentId = Guid.NewGuid();
        var childId = Guid.NewGuid();
        var parent = new Block.NumberedListItem
        {
            Id = parentId,
            HasChildren = true,
            Text = new RichText[]
            {
                new RichText.Text
                {
                    Content = "Parent",
                    PlainText = "Parent"
                }
            }
        };
        var child = new Block.NumberedListItem
        {
            Id = childId,
            HasChildren = true,
            Text = new RichText[]
            {
                new RichText.Text
                {
                    Content = "Child",
                    PlainText = "Child"
                }
            },
        };
        var grandChild = new Block.NumberedListItem
        {
            Text = new RichText[]
            {
                new RichText.Text
                {
                    Content = "Grandchild",
                    PlainText = "Grandchild"
                }
            }
        };
        var notion = Substitute.For<INotion>();
        notion.GetBlocksChildrenAsync(parentId, 100, default).Returns(Arrays.Of<Block>(child).Paginated().ToTask());
        notion.GetBlocksChildrenAsync(childId, 100, default).Returns(Arrays.Of<Block>(grandChild).Paginated().ToTask());

        var converter = new NumberedListConverter(notion);
        var settings = new ConverterSettings
        {
            Converter =
                new CompositeConverter(
                    converter,
                    new RichTextConverter(Applicable.ToAplicable((RichText _, string text) => text)))
        };
        var comparer = new OptionComparer<string>();
        var expectedResult = 
            Lists.Of("1. Parent", "&nbsp;&nbsp;&nbsp;&nbsp;1. Child", "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;1. Grandchild")
            .ToOption()
            .Select(text => JsonSerializer.Serialize(text));

        var actual = converter.Convert(parent, settings)
            .Select(list => JsonSerializer.Serialize(list));

        Assert.Equal(expectedResult, actual, comparer);
    }


    [Theory]
    [MemberData(nameof(Blocks))]
    public void ConvertNumberedList_Succeds(Block.NumberedListItem numberedListItem, string expectedText)
    {
        var actualResult = Lists.Of(expectedText).ToOption().Select(list => JsonSerializer.Serialize(list));

        var result = Converter.Convert(numberedListItem, Settings).Select(list => JsonSerializer.Serialize(list));

        result.Should().Be(actualResult);
    }


    public static TheoryData<Block.NumberedListItem, string> Blocks { get; } = new()
    {
        {
            new Block.NumberedListItem
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
            "1. Some text here and there"
        },
        {
            new Block.NumberedListItem
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
            "1. Some text here and there"
        },
    };
}
