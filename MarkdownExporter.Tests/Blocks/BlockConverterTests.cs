using Notion;
using Notion.Model;

using System;
using System.Collections.Generic;
using System.Text.Json;
using NSubstitute;
using System.Linq;

namespace MarkdownExporter.Tests;

public class BlockConverterTests
{

    [Theory]
    [MemberData(nameof(ParentAndChildren))]
    public void ParseBlock_Succeds(List<Block> blocks, List<string> expectedResult1, Func<INotion, Converter> converterGenerator)
    {
        var notion = Substitute.For<INotion>();
        foreach (var (parent, child) in blocks.Zip(blocks.Skip(1)))
        {
            notion
                .GetBlocksChildrenAsync(parent.Id, 100, default)
                .Returns(Arrays.Of(child).Paginated().ToTask());
        }

        var converter = converterGenerator(notion);

        var settings = new ConverterSettings
        {
            Converter = new CompositeConverter(
                converter,
                new RichTextConverter(
                    Applicable.Link(Formatters.FormatLink)
                    + Applicable.Bold(Formatters.FormatBold)
                    + Applicable.Italic(Formatters.FormatItalic)
                    + Applicable.Strikethrough(Formatters.FormatStike)
                    + Applicable.Underline(Formatters.FormatUnderline)
                    + Applicable.FormatCode(Formatters.FormatCode)
                    + Applicable.FormatColor(Formatters.FormatColor)))
        };
        var expectedResult = 
            expectedResult1
            .ToOption()
            .Select(text => JsonSerializer.Serialize(text));
        
        var actualResult = converter
            .Convert(blocks[0], settings)
            .Select(text => JsonSerializer.Serialize(text));
        
        Assert.Equal(expectedResult, actualResult);
    }
    
    private static Converter GetParagraphConverter(INotion notion) => new ParagraphConverter(notion);
    private static Converter GetNumberedListItemConverter(INotion notion) => new NumberedListItemConverter(notion);
    private static Converter GetBulletedListItemConverter(INotion notion) => new BulletedListItemConverter(notion);

    private static Converter GetHeading1Converter(INotion notion) => new Heading1Converter();
    private static Converter GetHeading2Converter(INotion notion) => new Heading2Converter();
    private static Converter GetHeading3Converter(INotion notion) => new Heading3Converter();

    public static TheoryData<IEnumerable<Block>, List<string>, Func<INotion, Converter>> ParentAndChildren { get; } = new()
    {
        {
            Lists.Of<Block>(new Block.Heading1
            {
                Text = new[]
                {
                    new RichText.Text
                    {
                        Content = "Some text here and there",
                        PlainText = "Some text here and there"
                    } as RichText
                }
            }),
            Lists.Of("# Some text here and there"),
            GetHeading1Converter
        },
        {
            Lists.Of<Block>(new Block.Heading1
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
            }),
            Lists.Of("# *Some text here and there*"),
            GetHeading1Converter
        },
        {
            Lists.Of<Block>(new Block.Heading2
            {
                Text = new[]
                {
                    new RichText.Text
                    {
                        Content = "Some text here and there",
                        PlainText = "Some text here and there"
                    }
                }
            }),
            Lists.Of("## Some text here and there"),
            GetHeading2Converter
        },
        {
            Lists.Of<Block>(new Block.Heading2
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
            }),
            Lists.Of("## Some text here and there"),
            GetHeading2Converter
        },
        {
            Lists.Of<Block>(new Block.Heading3
            {
                Text = new[]
                {
                    new RichText.Text
                    {
                        Content = "Some text here and there",
                        PlainText = "Some text here and there"
                    }
                }
            }),
            Lists.Of("### Some text here and there"),
            GetHeading3Converter
        },
        {
            Lists.Of<Block>(new Block.Heading3
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
            }),
            Lists.Of("### Some text here and there"),
            GetHeading3Converter
        },
        {
            Lists.Of<Block>(new Block.Paragraph
            {
                Text = new RichText[]
                {
                    new RichText.Text
                    {
                        Content = "Some text here and there",
                        PlainText = "Some text here and there"
                    }
                }
            }),
            Lists.Of("Some text here and there"),
            GetParagraphConverter
        },
        {
            Lists.Of<Block>(new Block.Paragraph
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
            }),
            Lists.Of("Some text here and there"),
            GetParagraphConverter
        },
        {
            Lists.Of<Block>(new Block.Paragraph
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
            }),
            Lists.Of("*Some text ***here and there**"),
            GetParagraphConverter
        },
        {
            Lists.Of<Block>(new Block.NumberedListItem
            {
                Text = new[]
                {
                    new RichText.Text
                    {
                        Content = "Some text here and there",
                        PlainText = "Some text here and there"
                    }
                }
            }),
            Lists.Of("1. Some text here and there"),
            GetNumberedListItemConverter
        },
        {
            Lists.Of<Block>(new Block.NumberedListItem
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
            }),
            Lists.Of("1. Some text here and there"),
            GetNumberedListItemConverter
        },
        {
            new List<Block>
            {
                new Block.Paragraph
                {
                    Id = Guid.NewGuid(),
                    HasChildren = true,
                    Text = new RichText[]
                    {
                        new RichText.Text
                        {
                            Content = "Some text here and there",
                            PlainText = "Parent"
                        }
                    }
                },
                new Block.Paragraph
                {
                    Text = new RichText[]
                    {
                        new RichText.Text
                        {
                            Content = "Child",
                            PlainText = "Child"
                        }
                    }
                },
            },
            Lists.Of("Parent", "&nbsp;&nbsp;&nbsp;&nbsp;Child"),
            GetParagraphConverter
        },
        {
            new List<Block>
            {
                new Block.Paragraph
                {
                    Id = Guid.NewGuid(),
                    HasChildren = true,
                    Text = new RichText[]
                    {
                        new RichText.Text
                        {
                            Content = "Some text here and there",
                            PlainText = "Parent"
                        }
                    }
                },
                new Block.Paragraph
                {
                    Id = Guid.NewGuid(),
                    HasChildren = true,
                    Text = new RichText[]
                    {
                        new RichText.Text
                        {
                            Content = "Child",
                            PlainText = "Child"
                        }
                    }
                },
                new Block.Paragraph
                {
                    Id = Guid.NewGuid(),
                    Text = new RichText[]
                    {
                        new RichText.Text
                        {
                            Content = "Grandchild",
                            PlainText = "Grandchild"
                        }
                    }
                },
            },
            Lists.Of("Parent", "&nbsp;&nbsp;&nbsp;&nbsp;Child", "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Grandchild"),
            GetParagraphConverter
        },
        {
            Lists.Of<Block>
            (
                new Block.NumberedListItem
                {
                    Id = Guid.NewGuid(),
                    HasChildren = true,
                    Text = new RichText[]
                    {
                        new RichText.Text
                        {
                            Content = "Parent",
                            PlainText = "Parent"
                        }
                    }
                },
                new Block.NumberedListItem
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
                }
            ),
            Lists.Of("1. Parent", "&nbsp;&nbsp;&nbsp;&nbsp;1. Child"),
            GetNumberedListItemConverter
        },
        {
            Lists.Of<Block>(
                new Block.NumberedListItem
                {
                    Id = Guid.NewGuid(),
                    HasChildren = true,
                    Text = new RichText[]
                    {
                        new RichText.Text
                        {
                            Content = "Parent",
                            PlainText = "Parent"
                        }
                    }
                },
                new Block.NumberedListItem
                {
                    Id = Guid.NewGuid(),
                    HasChildren = true,
                    Text = new RichText[]
                    {
                        new RichText.Text
                        {
                            Content = "Child",
                            PlainText = "Child"
                        }
                    },
                },
                new Block.NumberedListItem
                {
                    Text = new RichText[]
                    {
                        new RichText.Text
                        {
                            Content = "Grandchild",
                            PlainText = "Grandchild"
                        }
                    }
                }
            ),
            Lists.Of("1. Parent", "&nbsp;&nbsp;&nbsp;&nbsp;1. Child", "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;1. Grandchild"),
            GetNumberedListItemConverter
        },
        {
            Lists.Of<Block>(new Block.BulletedListItem
            {
                Id = Guid.NewGuid(),
                Text = Arrays.Of(new RichText.Text
                {
                    Content = "Some text here and there",
                    PlainText = "Some text here and there"
                })
            }),
            Lists.Of("* Some text here and there"),
            GetBulletedListItemConverter
        }
    };
}

