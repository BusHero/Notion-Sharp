namespace Notion.Sharp.Tests;

using System.Reflection;
using System.Threading.Tasks;

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

    [Fact(Skip = "It's broken")]
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

    [Fact(Skip = "It's broken")]
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

    [Fact(Skip = "It's broken")]
    public async Task GetDatabase_Fails_OnInvalidId()
    {
        await SUT.Awaiting(sut => sut.GetDatabaseAsync(Guid.NewGuid()))
            .Should()
            .ThrowAsync<NotionException>();
    }

    [Fact(Skip = "It's broken")]
    public async Task GetDatabase_Succeds_OnValidId()
    {
        Database database = await SUT.GetDatabaseAsync(ValidDatabaseId);
        database.Should().NotBeNull();
    }

    [Fact(Skip = "It's broken")]
    public async Task QueryDatabase_Succeds() => await RetryAsync(async () =>
    {
        var results = await SUT.QueryDatabaseAsync(ValidDatabaseId, new
        {

        });
        results.Should().NotBeNull();
    }, 3);


    [Fact(Skip = "It's broken")]
    public async Task QueryDatabase_Fails_OnInvalidId() => await RetryAsync(async () =>
    {
        await SUT.Awaiting(sut => sut.QueryDatabaseAsync(Guid.NewGuid(), new
        {

        })).Should().ThrowAsync<NotionException>();
    }, 3);

    #endregion

    #region Pages

    [Fact(Skip = "It's broken")]
    public async Task UpdatePage_Succeds_OnValidId() => await RetryAsync(async () =>
    {
        var page = await SUT.GetPageAsync(ValidPageId);
        var result = SUT.UpdatePageAsync(page);
        result.Should().NotBeNull();
    }, 3);

    [Fact(Skip = "It's broken")]
    public async Task UpdatePage_Fails_OnInvalidId() => await RetryAsync(async () =>
    {
        var page = await SUT.GetPageAsync(ValidPageId);
        page = page with { Id = Guid.NewGuid() };
        await SUT.Awaiting(sut => sut.UpdatePageAsync(page)).Should().ThrowAsync<NotionException>();
    }, 3);

    [Fact(Skip = "It's broken")]
    public async Task GetPage_Fails_OnInvalidId()
    {
        await SUT.Awaiting(sut => sut.GetPageAsync(Guid.NewGuid())).Should().ThrowAsync<NotionException>();
    }

    [Fact(Skip = "It's broken")]
    public async Task GetPage_Succeds_OnValidId()
    {
        var page = await SUT.GetPageAsync(ValidPageId);
        page.Should().NotBeNull();
    }

    [Fact(Skip = "It's broken")]
    public async Task GetPageFromDatabase_Succeds()
    {
        Page page = await SUT.GetPageAsync(PageFromDatabase);
        page.Should().NotBeNull();
    }

    [Fact(Skip = "It's broken")]
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

    [Fact(Skip = "It's broken")]
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

    [Fact(Skip = "It's broken")]
    public async Task GetBlocksChildren_Fails_OnInvalidId()
    {
        await SUT.Awaiting(sut => sut.GetBlocksChildrenAsync(Guid.NewGuid())).Should().ThrowAsync<NotionException>();
    }

    [Theory(Skip = "It's broken")]
    [InlineData("85387287-61bb-4913-a93c-76f7d8d074dd")]
    [InlineData("7da8cfa80de14b3685e141afe7ca4a1f")]
    [InlineData("e392eaec-2dae-47ca-b24f-c0e72783ffe5")]
    public async Task GetBlocks_Succeds_OnValidId(string id)
    {
        var blocks = await SUT.GetBlocksChildrenAsync(Guid.Parse(id));
        blocks.Should().NotBeNull();
    }

    [Fact(Skip = "It's broken")]
    public async Task GetBlock_Fails_OnInvalidId()
    {
        await SUT.Awaiting(sut => sut.GetBlockAsync(Guid.NewGuid())).Should().ThrowAsync<NotionException>();
    }

    [Theory(Skip = "It's broken")]
    [InlineData("eb3f156343164743971a4c44f713a127")]
    [InlineData("68d00e3a200b497e80d82285708d58d2")]
    public async Task GetBlock_Succeds_OnValidId(string id)
    {
        var block = await SUT.GetBlockAsync(Guid.Parse(id));
        block.Should().NotBeNull();
    }

    [Fact(Skip = "It's broken")]
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

    [Fact(Skip = "It's broken")]
    public async Task UpdateBlock_Succeds() => await RetryAsync(async () =>
    {
        var block = await SUT.GetBlockAsync(ValidBlockId);
        var result = await SUT.UpdateBlockAsync(block);
        result.Should().NotBeNull();
    }, 3);

    [Fact(Skip = "It's broken")]
    public async Task UpdateBlock_Fails_OnInvalidId() => await RetryAsync(async () =>
    {
        var block = await SUT.GetBlockAsync(ValidBlockId);
        block = block with
        {
            Id = Guid.NewGuid()
        };
        await SUT.Awaiting(sut => sut.UpdateBlockAsync(block)).Should().ThrowAsync<NotionException>();
    }, 3);

    [Fact(Skip = "It's broken")]
    public async Task DeleteBlock_Fails_OnInvalidId() => await RetryAsync(async () =>
    {
        await SUT.Awaiting(sut => sut.DeleteBlockAsync(Guid.NewGuid()))
            .Should()
            .ThrowAsync<NotionException>();
    }, 3);

    [Fact(Skip = "It's broken")]
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

    [Fact(Skip = "It's broken")]
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

    [Theory(Skip = "It's broken")]
    [InlineData("AVTB", "date")]
    [InlineData("Ki=]", "date")]
    [InlineData("kb;E", "date")]
    [InlineData("djkn", "date")]
    [InlineData("DVQN", "formula")]
    [InlineData("Gqpt", "formula")]
    [InlineData("U|OM", "formula")]
    [InlineData("Zfk]", "formula")]
    [InlineData("kC]\\", "formula")]
    [InlineData("G=~V", "multi_select")]
    [InlineData("}Bf`", "multi_select")]
    [InlineData("LqOn", "multi_select")]
    [InlineData("JHTr", "number")]
    [InlineData("Kxm}", "select")]
    [InlineData("O>||", "files")]
    [InlineData("TJeJ", "files")]
    [InlineData("ZjRa", "number")]
    [InlineData("i;<u", "url")]
    [InlineData("uX|q", "email")]
    [InlineData("_x|k", "phone_number")]
    [InlineData("BADZ", "created_time")]
    [InlineData("jIE]", "created_by")]
    [InlineData("L|rs", "last_edited_time")]
    [InlineData("t}ga", "last_edited_by")]
    public async Task GetPageProperty_Succeds_OnValidPageIdAndPropertyId(string id, string propertyName)
    {
        var result = await SUT.GetPagePropertyAsync(PageFromDatabase, id);
        var property = result.GetType().GetRuntimeProperties().FirstOrDefault(p => string.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase));
        property.GetValue(result).Should().NotBeNull();
    }

    [Theory(Skip = "It's broken")]
    [InlineData("title", "title")]
    [InlineData("hiyf", "rich_text")]
    [InlineData("oOIv", "rich_text")]
    [InlineData("}?[O", "people")]
    [InlineData("D?BR", "people")]
    [InlineData("nt@E", "relation")]
    [InlineData(":uV>", "relation")]
    public async Task GetPagePropery_Succeds_OnPaginatedList(string id, string propertyName)
    {
        var result = await SUT.GetPagePropertyAsync(PageFromDatabase, id);
        if (result.results.Length == 0)
            return;
        var property = result
            .results[0]
            .GetType()
            .GetRuntimeProperties().FirstOrDefault(p => string.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase));

        property.Should().NotBeNull();
    }

    [Theory(Skip = "It's broken")]
    //[InlineData("JsFc")]
    [InlineData("Hgj{")]
    [InlineData("{tT>")]
    public async Task GetPageProperty_Succeds_OnRollupProperty(string id)
    {
        var result = await SUT.GetPagePropertyAsync(PageFromDatabase, id);
        result.rollup.Should().NotBeNull();
        result.results.Should().NotBeNull();
    }

    [Fact(Skip = "It's broken")]
    public async Task GetPageProperty_Fails_OnInvalidPageId()
    {
        await new Func<Task>(async () =>
        {
            await SUT.GetPagePropertyAsync(Guid.NewGuid(), "title");
        }).Should().ThrowAsync<NotionException>();
    }

    [Fact(Skip = "It's broken")]
    public async Task GetPageProperty_Fails_OnInvalidPropertyId()
    {
        await new Func<Task>(async () =>
        {
            await SUT.GetPagePropertyAsync(Guid.NewGuid(), "invalid-property");
        }).Should().ThrowAsync<NotionException>();
    }
}
