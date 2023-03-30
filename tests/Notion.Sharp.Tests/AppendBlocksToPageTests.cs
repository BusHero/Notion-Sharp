using Notion.Sharp.Tests.Utils;

namespace Notion.Sharp.Tests;

public class AppendBlocksToPageTests : NotionTestsBase
{
    [Theory(Skip = "It's broken")]
    [MemberData(nameof(Blocks))]
    public async Task AppendChildren_Succeds(Block block) => await RetryAsync(async () =>
    {
        var result = await SUT.AppendBlockChildrenAsync(Pages.Page.ToGuid(),
                new List<Block>
                {
                     block
                });
        result.Should().NotBeNull();
        await SUT.DeleteBlockAsync(result.Results[0].Id);
    }, 3);

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
        new Block.Callout
        {
            Text = new RichText[]
            {
                new RichText.Text
                {
                    Content = "Callout"
                }
            },
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
        new Block.Bookmark
        {
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
