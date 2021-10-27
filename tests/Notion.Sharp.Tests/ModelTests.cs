namespace Notion.Sharp.Tests;

public class ModelTests : NotionTestsBase
{

    #region Setup

    public ModelTests()
    {
        
    }

    #endregion

    #region Users

    [Fact]
    public async Task GetUsers()
    {
        PaginationList<User> users = await SUT.GetUsersAsync();
        users.Should().NotBeNull();
    }

    [Fact]
    public async Task GetMe()
    {
        User me = await SUT.GetMeAsync();
        me.Should().BeOfType<User.Bot>();
    }

    [Fact]
    public async Task GetUser_Fails_OnInvalidId()
    {
        await SUT.Awaiting(sut => sut.GetUserAsync(Guid.NewGuid()))
            .Should()
            .ThrowAsync<NotionException>();
    }

    [Fact]
    public async Task GetUser_Succeds_OnValidId()
    {
        User user = await SUT.GetUserAsync(ValidUserId);
        user.Should().NotBeNull();
    }

    #endregion

    #region Databases

    [Fact]
    public async Task UpdateDatabase_Succeds() => await RetryAsync(async () =>
    {
        var database = await SUT.GetDatabaseAsync(SimpleDatabase);
        database = database with
        {
            Title = new RichText[]
            {
                new RichText.Text
                {
                    Content = "some new title"
                }
            }
        };
        await SUT.UpdateDatabaseAsync(database);
    
    }, 3);

    [Fact]
    public async Task CreateDatabase_Succeds() => await RetryAsync(async () =>
    {
        var database = new Database
        {
            Parent = new Parent.Page
            {
                Id = ValidPageId
            },
            Title = new RichText[]
            {
                new RichText.Text
                {
                    Content = "A new database is born"
                }
            },
            Properties = new Dictionary<string, Property>
            {
                ["Name"] = new Property.Title
                {
                }
            }
        };
        database = database with { Parent = new Parent.Page { Id = ValidPageId } };
        await SUT.CreateDatabaseAsync(database);
    }, 3);

    [Fact]
    public async Task GetDatabase_Fails_OnInvalidId()
    {
        await SUT.Awaiting(sut => sut.GetDatabaseAsync(Guid.NewGuid()))
            .Should()
            .ThrowAsync<NotionException>();
    }

    [Fact]
    public async Task GetDatabase_Succeds_OnValidId()
    {
        Database database = await SUT.GetDatabaseAsync(ValidDatabaseId);
        database.Should().NotBeNull();
    }

    [Fact]
    public async Task QueryDatabase_Succeds() => await RetryAsync(async () =>
    {
        PaginationList<Page> results = await SUT.QueryDatabaseAsync(ValidDatabaseId, new
        {
    
        });
        results.Should().NotBeNull();
    }, 3);


    [Fact]
    public async Task QueryDatabase_Fails_OnInvalidId() => await RetryAsync(async () =>
    {
        await SUT.Awaiting(sut => sut.QueryDatabaseAsync(Guid.NewGuid(), new
        {
    
        })).Should().ThrowAsync<NotionException>();
    }, 3);

    #endregion

    #region Pages

    [Fact]
    public async Task UpdatePage_Succeds_OnValidId() => await RetryAsync(async () =>
    {
        var page = await SUT.GetPageAsync(ValidPageId);
        var result = SUT.UpdatePageAsync(page);
        result.Should().NotBeNull();
    }, 3);

    [Fact]
    public async Task UpdatePage_Fails_OnInvalidId() => await RetryAsync(async () =>
    {
        var page = await SUT.GetPageAsync(ValidPageId);
        page = page with { Id = Guid.NewGuid() };
        await SUT.Awaiting(sut => sut.UpdatePageAsync(page)).Should().ThrowAsync<NotionException>();
    }, 3);

    [Fact]
    public async Task GetPage_Fails_OnInvalidId()
    {
        await SUT.Awaiting(sut => sut.GetPageAsync(Guid.NewGuid())).Should().ThrowAsync<NotionException>();
    }

    [Fact]
    public async Task GetPage_Succeds_OnValidId()
    {
        Page page = await SUT.GetPageAsync(ValidPageId);
        page.Should().NotBeNull();
    }

    [Fact]
    public async Task GetPageFromDatabase_Succeds()
    {
        Page page = await SUT.GetPageAsync(PageFromDatabase);
        page.Should().NotBeNull();
    }

    [Fact]
    public async Task CreatePage_Succeds() => await RetryAsync(async () =>
    {
        var result = await SUT.CreatePageAsync(new Page
        {
            Parent = new Parent.Page
            {
                Id = ValidPageId
            },
            Properties = new Dictionary<string, PropertyValue>
            {
                ["title"] = new PropertyValue.Title
                {
                    Content = new RichText[]
                    {
                        new RichText.Text
                        {
                            Content = "Some content here and there"
                        }
                    }
                }
            }
        });
        result.Should().NotBeNull();
    }, 3);

    [Fact]
    public async Task CreatePageWithChildren_Succeds() => await RetryAsync(async () =>
    {
        var result = await SUT.CreatePageAsync(new Page
        {
            Parent = new Parent.Page
            {
                Id = ValidPageId
            },
            Properties = new Dictionary<string, PropertyValue>
            {
                ["title"] = new PropertyValue.Title
                {
                    Content = new RichText[]
                    {
                        new RichText.Text
                        {
                            Content = "Page with content"
                        }
                    }
                }
            }
        }, new Block[]
        {
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
        });
        result.Should().NotBeNull();
    }, 3);


    #endregion

    #region Blocks

    [Fact]
    public async Task GetBlocksChildren_Fails_OnInvalidId()
    {
        await SUT.Awaiting(sut => sut.GetBlocksChildrenAsync(Guid.NewGuid())).Should().ThrowAsync<NotionException>();
    }

    [Fact]
    public async Task GetBlocks_Succeds_OnValidId()
    {
        var blocks = await SUT.GetBlocksChildrenAsync(ValidPageId);
        blocks.Should().NotBeNull();
    }

    [Fact]
    public async Task GetBlock_Fails_OnInvalidId()
    {
        await SUT.Awaiting(sut => sut.GetBlockAsync(Guid.NewGuid())).Should().ThrowAsync<NotionException>();
    }

    [Fact]
    public async Task GetBlock_Succeds_OnValidId()
    {
        var block = await SUT.GetBlockAsync(ValidBlockId);
        block.Should().NotBeNull();
    }



    [Fact]
    public async Task AppendChildren_Fails_OnInvalidId() => await RetryAsync(async () =>
    {
        var result = await SUT.Awaiting(sut => sut.AppendBlockChildrenAsync(Guid.NewGuid(), new List<Block>
               {
                   new Block.Heading2
                   {
                       Text = new RichText[]
                       {
                           new RichText.Text
                           {
                               Content = "Brave new world"
                           }
                       }
                   }
               })).Should().ThrowAsync<NotionException>();
    }, 3);

    [Fact]
    public async Task UpdateBlock_Succeds() => await RetryAsync(async () =>
    {
        var block = await SUT.GetBlockAsync(ValidBlockId);
        var result = await SUT.UpdateBlockAsync(block);
        result.Should().NotBeNull();
    }, 3);

    [Fact]
    public async Task UpdateBlock_Fails_OnInvalidId() => await RetryAsync(async () =>
    {
        var block = await SUT.GetBlockAsync(ValidBlockId);
        block = block with
        {
            Id = Guid.NewGuid()
        };
        await SUT.Awaiting(sut => sut.UpdateBlockAsync(block)).Should().ThrowAsync<NotionException>();
    }, 3);

    [Fact]
    public async Task DeleteBlock_Fails_OnInvalidId() => await RetryAsync(async () =>
    {
        await SUT.Awaiting(sut => sut.DeleteBlockAsync(Guid.NewGuid()))
            .Should()
            .ThrowAsync<NotionException>();
    }, 3);

    [Fact]
    public async Task DeleteBlock_Succeds() => await RetryAsync(async () =>
    {
        var result = await SUT.AppendBlockChildrenAsync(ValidPageId, new List<Block>
           {
               new Block.Heading2
               {
                   Text = new RichText[]
                   {
                       new RichText.Text
                       {
                           Content = "Brave new world"
                       }
                   }
               }
           });
        var result2 = await SUT.DeleteBlockAsync(result.Results[0].Id);
        result2.Should().NotBeNull();
    }, 3);

    #endregion

    #region Search

    [Fact]
    public async Task Search_Succeds_OnValidParameter() => await RetryAsync(async () =>
    {
        //var result = await SUT.SearchAsync("foo");
        PaginationList<PageOrDatabase> result = await SUT.SearchAsync(
            new(
            //query: "foo",
            //sort: new Sort(direction: "ascending", timestamp: "last_edited_time"),
            //filter: new Filter(value: "database", property: "object"),
            //page_size: 100
            )
            );
        result.Should().NotBeNull();
    }, 3);

    #endregion

    #region Data

    
    #endregion
}
