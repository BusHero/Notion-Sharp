using Notion;
using Notion.Model;

using System;
using System.Collections.Generic;
using System.Text.Json;
using NSubstitute;

namespace MarkdownExporter.Tests;

public class ParagraphConverterTests
{
    private ConverterSettings Settings { get; } = new ConverterSettings
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

    public Converter<Block.Paragraph> Converter { get; } = new BlockConverter<Block.Paragraph>(
         p => p.Text, _ => Array.Empty<Block>(), text => text, text => $"&nbsp;&nbsp;&nbsp;&nbsp;{text}");

    public IEqualityComparer<Option<List<string>>> Comparer = new OptionComparer<List<string>>(new ListSequenceComparer<string>());

    [Theory]
    [MemberData(nameof(Paragraphs))]
    public void ParseParagraph_Passes(Block.Paragraph block, string expectedText)
    {
        var expectedResult = new List<string> { expectedText }.ToOption().Select(list => JsonSerializer.Serialize(list));

        var actualResult = Converter.Convert(block, Settings).Select(list => JsonSerializer.Serialize(list));

        Assert.Equal(expectedResult, actualResult, new OptionComparer<string>(EqualityComparer<string>.Default));
    }

    [Fact]
    public void ParseParagraph_WithChildren_Succeds()
    {
        var parentId = Guid.NewGuid();
        var parent = new Block.Paragraph
        {
            Id = parentId,
            HasChildren = true,
            Text = new RichText[]
            {
                new RichText.Text
                {
                    Content = "Some text here and there",
                    PlainText = "Parent"
                }
            }
        };
        var child = new Block.Paragraph
        {
            Text = new RichText[]
            {
                new RichText.Text
                {
                    Content = "Child",
                    PlainText = "Child"
                }
            }
        };
        var notion = Substitute.For<INotion>();
        notion.GetBlocksChildrenAsync(parentId, 100, default).Returns(Arrays.Of<Block>(child).Paginated().ToTask());
        var converter = new BlockConverter<Block.Paragraph>(
                p => p.Text,
                p => notion.GetBlocksChildrenAsync(p.Id).Result.Results,
                text => text,
                text => $"&nbsp;&nbsp;&nbsp;&nbsp;{text}");
        var settings = new ConverterSettings
        {
            Converter = new CompositeConverter(
                converter,
                new RichTextConverter(Applicable.ToAplicable((RichText _, string text) => text)))
        };
        var comparer = new OptionComparer<string>(EqualityComparer<string>.Default);
        var expectedResult = new List<string> { "Parent", "&nbsp;&nbsp;&nbsp;&nbsp;Child" }
            .ToOption()
            .Select(text => JsonSerializer.Serialize(text));
        
        var actualResult = converter.Convert(parent, settings).Select(text => JsonSerializer.Serialize(text));
        
        Assert.Equal(expectedResult, actualResult, comparer);

    }

    [Fact]
    public void ParseParagraph_WithGrandChildren_Succeds()
    {
        var parentId = Guid.NewGuid();
        var childId = Guid.NewGuid();
        var parent = new Block.Paragraph
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
        var child = new Block.Paragraph
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
        var grandChild = new Block.Paragraph
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

        var converter = new BlockConverter<Block.Paragraph>(
            p => p.Text,
            p => notion.GetBlocksChildrenAsync(p.Id).Result.Results,
            text => text,
            text => $"&nbsp;&nbsp;&nbsp;&nbsp;{text}");

        var settings = new ConverterSettings
        {
            Converter = converter + new RichTextConverter(Applicable.ToAplicable((RichText _, string text) => text))
        };
        var comparer = new OptionComparer<string>();
        var expectedResult = new List<string> { "Parent", "&nbsp;&nbsp;&nbsp;&nbsp;Child", "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Grandchild" }
            .ToOption()
            .Select(text => JsonSerializer.Serialize(text));

        var actual = converter.Convert(parent, settings).Select(list => JsonSerializer.Serialize(list));

        Assert.Equal(expectedResult, actual, comparer);
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
                        Content = "Some text here and there",
                        PlainText = "Some text here and there"
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
                        PlainText = "Some text "
                    },
                    new RichText.Text
                    {
                        Content = "here and there",
                        PlainText = "here and there"
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
                        },
                        PlainText = "Some text ",

                    },
                    new RichText.Text
                    {
                        Content = "here and there",
                        Annotations = new Annotations
                        {
                            Italic = true
                        },
                        PlainText = "here and there",
                    }
                }
            },
            "*Some text ***here and there**"
        },
    };
}

