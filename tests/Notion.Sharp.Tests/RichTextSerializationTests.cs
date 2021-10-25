using FluentAssertions;

using Notion.Model;

using Xunit;

namespace Notion.Sharp.Tests;

public class AppendBlocksToPage : NotionTestsBase
{
    [Theory]
    [MemberData(nameof(Blocks))]
    public async Task AppendChildren_Succeds(Block block)
    {
        var result = await SUT.AppendBlockChildrenAsync(ValidPageId,
           new List<Block>
           {
                   block
           });
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task AppendImage_Succeds()
    {
        var result = await SUT.AppendBlockChildrenAsync(ValidPageId, new List<Block>
        {
            new Block.Image
            {
                File = new Model.File.External
                {
                    Uri = new Uri("https://i.scdn.co/image/ab6761610000e5ebdef2f4887831f20c342d790e")

                    //Uri = new Uri("https://images.unsplash.com/photo-1533551268962-824e232f7ee1?ixlib=rb-1.2.1&q=85&fm=jpg&crop=entropy&cs=srgb")
                }
            },
        });
        result.Should().NotBeNull();
    }

    public static TheoryData<Block> Blocks { get; } = new TheoryData<Block>
    {
        new Block.Paragraph
            {
                Text = new RichText[]
                {
                    new RichText.Text
                    {
                        Content = "Paragraph"
                    }
                },
                Children = new Block[]
                {
                    new Block.Paragraph
                    {
                        Text = new RichText[]
                        {
                            new RichText.Text
                            {
                                Content = "Child paragraph"
                            }
                        }
                    }
                }
            },
        new Block.Heading1
            {
                Text = new RichText[]
                {
                    new RichText.Text
                    {
                        Content = "Heading 1"
                    }
                }
            },
        new Block.Heading2
            {
                Text = new RichText[]
                {
                    new RichText.Text
                    {
                        Content = "Heading 2"
                    }
                }
            },
        new Block.Heading3
            {
                Text = new RichText[]
                {
                    new RichText.Text
                    {
                        Content = "Heading 3"
                    }
                }
            },
        new Block.Callout
        {
            Text = new RichText[]
            {
                new RichText.Text
                {
                    Content = "Callout"
                }
            }
        },
        new Block.Quote
        {
            Text = new RichText[]
            {
                new RichText.Text
                {
                    Content = "Quote text"
                }
            }
        },
        new Block.BulletedListItem
            {
                Text = new RichText[]
                {
                    new RichText.Text
                    {
                        Content = "Bulleted list item"
                    }
                },
                Children = new Block[]
                {
                    new Block.BulletedListItem
                    {
                        Text = new RichText[]
                        {
                            new RichText.Text
                            {
                                Content = "Child content"
                            }
                        }
                    }
                }
            },
        new Block.NumberedListItem
            {
                Text = new RichText[]
                {
                    new RichText.Text
                    {
                        Content = "Numbered list item"
                    }
                },
                Children = new Block[]
                {
                    new Block.BulletedListItem
                    {
                        Text = new RichText[]
                        {
                            new RichText.Text
                            {
                                Content = "Child content"
                            }
                        }
                    }
                }
            },
        new Block.ToDo
            {
                Text = new RichText[]
                {
                    new RichText.Text
                    {
                        Content = "to do"
                    }
                },
                Children = new Block[]
                {
                    new Block.BulletedListItem
                    {
                        Text = new RichText[]
                        {
                            new RichText.Text
                            {
                                Content = "Child content"
                            }
                        }
                    }
                }
            },
        new Block.Toggle
            {
                Text = new RichText[]
                {
                    new RichText.Text
                    {
                        Content = "toggle"
                    }
                },
                Children = new Block[]
                {
                    new Block.BulletedListItem
                    {
                        Text = new RichText[]
                        {
                            new RichText.Text
                            {
                                Content = "Child content"
                            }
                        }
                    }
                },
            },
        new Block.Code
        {
            Text = new RichText[]
            {
                new RichText.Text
                {
                    Content = "var x = 1 + 1;"
                }
            },
            Language = "c#"
        },
        //new Block.ChildPage
        //{
        //    Title = "Some title here and there",
        //},
        //new Block.ChildDatabase
        //{
        //    Title = "Some title here and there",
        //},
        new Block.Embed
        {
            Url = new Uri("https://www.youtube.com/watch?v=sToqbqP0tFk")
        },
        new Block.Image
        {
            File = new Model.File.External
            {
                Uri = new Uri("https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png")
            }
        },
        new Block.Video
        {
            File = new Model.File.External
            {
                Uri = new Uri("https://www.youtube.com/watch?v=sToqbqP0tFk")
            }
        },
        new Block.FileBlock
        {
            File = new Model.File.External
            {
                Uri = new Uri("http://www.africau.edu/images/default/sample.pdf")
            }
        },
        new Block.Pdf
        {
            File = new Model.File.External
            {
                Uri = new Uri("http://www.africau.edu/images/default/sample.pdf")
            }
        },
        new Block.Bookmark
            {
                Caption = new RichText[]
                {
                    new RichText.Text
                    {
                        Content = "Caption text"
                    }
                },
                Url = new Uri("https://google.com")
            },
        new Block.Equation
        {
            Expression = "1 + 1"
        },
        new Block.Divider
            {
            },
        new Block.TableOfContents
            {
            },
        new Block.Breadcrumb { }
    };
}
