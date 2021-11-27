using FluentAssertions;

using Moq;

using Notion;
using Notion.Model;

using System;
using System.Threading.Tasks;

using Xunit;

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
        + new ParagraphConverter(Mock.Of<INotion>())
    };

    [Theory]
    [MemberData(nameof(Paragraphs))]
    public void ParseParagraph_Passes(Block.Paragraph block, string expectedText) => Converter
        .Convert(block, Settings)
        .ValueOrDefault(string.Empty)
        .Should()
        .Be(expectedText);

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
                    PlainText = "Some text here and there"
                }
            }
        };
        var child = new Block.Paragraph
        {
            Text = new RichText[]
            {
                new RichText.Text
                {
                    Content = "Some text here and there",
                    PlainText = "Some text here and there"
                }
            }
        };
        var mock = new Mock<INotion>();
        mock.Setup(notion => notion.GetBlocksChildrenAsync(parentId, 100, default))
            .Returns(Task.FromResult<PaginationList<Block>>(new() { Results = new[] { child } }));
        var notion = mock.Object;

        var settings = new ConverterSettings
        {
            Converter =
                new RichTextConverter(Applicable.ToAplicable((RichText _, string text) => text))
                + new ParagraphConverter(notion)
        };
        
        var expectedText = "Some text here and there\n&nbsp;&nbsp;&nbsp;&nbsp;Some text here and there\n";

        Converter
            .Convert(parent, settings, string.Empty)
            .Should()
            .Be(expectedText);
    }

//    [Fact]
//    public void ParseParagraph_WithGrandChildren_Succeds()
//    {
//        var parentId = Guid.NewGuid();
//        var childId = Guid.NewGuid();
//        var parent = new Block.Paragraph
//        {
//            Id = parentId,
//            HasChildren = true,
//            Text = new RichText[]
//            {
//                new RichText.Text
//                {
//                    Content = "Parent",
//                    PlainText = "Parent"
//                }
//            }
//        };
//        var child = new Block.Paragraph
//        {
//            Id = childId,
//            HasChildren = true,
//            Text = new RichText[]
//            {
//                new RichText.Text
//                {
//                    Content = "Child",
//                    PlainText = "Child"
//                }
//            },
//        };
//        var grandChild = new Block.Paragraph
//        {
//            Text = new RichText[]
//            {
//                new RichText.Text
//                {
//                    Content = "Grandchild",
//                    PlainText = "Grandchild"
//                }
//            }
//        };


//        var mock = new Mock<INotion>();
//        mock.Setup(notion => notion.GetBlocksChildrenAsync(parentId, 100, default))
//            .Returns(Task.FromResult<PaginationList<Block>>(new() { Results = new[] { child } }));
//        mock.Setup(notion => notion.GetBlocksChildrenAsync(childId, 100, default))
//            .Returns(Task.FromResult<PaginationList<Block>>(new() { Results = new[] { grandChild } }));
//        var notion = mock.Object;

//        var settings = new ConverterSettings
//        {
//            Converter =
//                new RichTextConverter(Applicable.ToAplicable((RichText _, string text) => text))
//                + new ParagraphConverter(notion)
//        };

//        var expectedText = @"Parent\n
//&nbsp;&nbsp;&nbsp;&nbsp;Child\n
//&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Grandchild\n";

//        Converter
//            .Convert(parent, settings, string.Empty)
//            .Should()
//            .Be(expectedText);
//    }

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
            "Some text here and there\n"
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
            "Some text here and there\n"
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
            "*Some text ***here and there**\n"
        },
    };
}
