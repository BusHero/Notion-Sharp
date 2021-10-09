using FluentAssertions;

using Microsoft.Extensions.Configuration;

using Xunit;
using System.Threading.Tasks;
using System;
using System.Text.Json;
using System.Linq;

namespace Notion.Sdk.Tests
{
    public class ModelTests
    {
        #region Ids
        private INotion SUT { get; }

        private Guid ValidUserId { get; }
     
        private Guid ValidDatabaseId {  get; }
        
        private Guid ValidPageId { get; }
        
        private Guid ValidBlockId { get; }

        #endregion

        public ModelTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<ModelTests>()
                .Build();

            SUT = Notion.NewClient(bearerToken: configuration["Notion"], "2021-08-16");
            ValidUserId = Guid.Parse(configuration["userId"]);
            ValidDatabaseId = Guid.Parse(configuration["databaseId"]);
            ValidPageId = Guid.Parse(configuration["pageId"]);
            ValidBlockId = Guid.Parse(configuration["blockId"]);
        }

        #region Users

        [Fact]
        public async Task GetUsers()
        {
            var users = await SUT.GetUsersAsync();
            users.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetMe()
        {
            var me = await SUT.GetMeAsync();
            me.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetUser_Fails_OnInvalidId()
        {
            await SUT.Awaiting(sut => sut.GetUserAsync(System.Guid.NewGuid()))
                .Should()
                .ThrowAsync<NotionException>();
        }

        [Fact]
        public async Task GetUser_Succeds_OnValidId()
        {
            var user = await SUT.GetUserAsync(ValidUserId);
            user.Should().NotBeNullOrEmpty();
        }

        #endregion

        #region Databases

        [Fact]
        public async Task UpdateDatabase_Succeds()
        {
            await SUT.UpdateDatabaseAsync(ValidDatabaseId, new
            {
                title = new object[]
                {
                    new
                    {
                        text = new
                        {
                            content = "some new title"
                        }
                    }
                }
            });
        }

        [Fact]
        public async Task CreateDatabase_Succeds()
        {
            await SUT.CreateDatabaseAsync(new
            {
                parent = new
                {
                    page_id = ValidPageId
                },
                title = new object[]
                {
                    new
                    {
                        text = new
                        {
                            content = "some new title"
                        }
                    }
                },
                properties = new
                {
                    Name = new
                    {
                        title = new object()
                    }
                }
            });
        }

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
            var database = await SUT.GetDatabaseAsync(ValidDatabaseId);
            database.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task QueryDatabase_Succeds()
        {
            string results = await SUT.QueryDatabaseAsync(ValidDatabaseId, new
            {

            });
            results.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task QueryDatabase_Fails_OnInvalidId()
        {
            await SUT.Awaiting(sut => sut.QueryDatabaseAsync(Guid.NewGuid(), new
            {

            })).Should().ThrowAsync<NotionException>();
        }

        #endregion

        #region Pages

        [Fact]
        public async Task UpdatePage_Succeds_OnValidId()
        {
            await SUT.UpdatePageAsync(ValidPageId, new
            {
                properties = new
                {
                    title = new
                    {
                        title = new object[]
                        {
                            new
                            {
                                text = new
                                {
                                    content = "some new title"
                                }
                            }
                        }
                    }
                }
            });
        }

        [Fact]
        public async Task UpdatePage_Fails_OnInvalidId()
        {
            await SUT.Awaiting(sut => sut.UpdatePageAsync(Guid.NewGuid(), new
            {
                properties = new
                {
                    title = new
                    {
                        title = new object[]
                        {
                            new
                            {
                                text = new
                                {
                                    content = "some new title"
                                }
                            }
                        }
                    }
                }
            })).Should().ThrowAsync<NotionException>();
        }

        [Fact]
        public async Task GetPage_Fails_OnInvalidId()
        {
            await SUT.Awaiting(sut => sut.GetPageAsync(Guid.NewGuid())).Should().ThrowAsync<NotionException>();
        }

        [Fact]
        public async Task GetPage_Succeds_OnValidId()
        {
            var user = await SUT.GetPageAsync(ValidPageId);
            user.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task CreatePage_Succeds()
        {
            await SUT.CreatePageAsync(new
            {
                parent = new
                {
                    page_id = ValidPageId
                },
                properties = new
                {
                    title = new
                    {
                        title = new object[]
                        {
                            new
                            {
                                text = new
                                {
                                    content = "some new title"
                                }
                            }
                        }
                    }
                }
            });
        }

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
            blocks.Should().NotBeNullOrEmpty(); 
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
            block.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task AppendChildren_Succeds()
        {
            var result = await SUT.AppendBlockChildrenAsync(ValidPageId, new
            {
                children = new object[]
                {
                    new
                    {
                        heading_2 = new
                        {
                            text = new object[]
                            {
                                new
                                {
                                    text = new
                                    {
                                        content = "Brave new world!"
                                    }
                                }
                            }
                        }
                    }
                }
            });
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task AppendChildren_Fails_OnInvalidId()
        {
            var result = await SUT.Awaiting(sut => sut.AppendBlockChildrenAsync(Guid.NewGuid(), new
            {
                children = new object[]
                {
                    new
                    {
                        heading_2 = new
                        {
                            text = new object[]
                            {
                                new
                                {
                                    text = new
                                    {
                                        content = "Brave new world!"
                                    }
                                }
                            }
                        }
                    }
                }
            })).Should().ThrowAsync<NotionException>();
        }

        [Fact]
        public async Task UpdateBlock_Succeds()
        {
            var result = await SUT.UpdateBlockAsync(ValidBlockId, new
            {
                paragraph = new
                {
                    text = new object[]
                    {
                        new
                        {
                            text = new
                            {
                                content = "hello to you"
                            }
                        }
                    }
                }
            });
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task UpdateBlock_Fails_OnInvalidId()
        {
            await SUT.Awaiting(sut => sut.UpdateBlockAsync(Guid.NewGuid(), new
            {
                paragraph = new
                {
                    text = new object[]
                    {
                        new
                        {
                            text = new
                            {
                                content = "hello to you"
                            }
                        }
                    }
                }
            })).Should().ThrowAsync<NotionException>();
        }

        [Fact]
        public async Task DeleteBlock_Fails_OnInvalidId()
        {
            await SUT.Awaiting(sut => sut.DeleteBlockAsync(Guid.NewGuid()))
                .Should()
                .ThrowAsync<NotionException>();
        }

        [Fact]
        public async Task DeleteBlock_Succeds()
        {
            var result = await SUT.AppendBlockChildrenAsync(ValidPageId, new
            {
                children = new object[]
                {
                    new
                    {
                        heading_2 = new
                        {
                            text = new object[]
                            {
                                new
                                {
                                    text = new
                                    {
                                        content = "Brave new world!"
                                    }
                                }
                            }
                        }
                    }
                }
            });

            using var document = JsonDocument.Parse(result);
            var root = document.RootElement;
            var id = root.GetProperty("results").EnumerateArray().First().GetProperty("id").GetString();
            result = await SUT.DeleteBlockAsync(Guid.Parse(id));
            result.Should().NotBeNullOrEmpty();
        }

        #endregion

        [Fact]
        public async Task Search_Succeds_OnValidParameter()
        {
            string result = await SUT.SearchAsync(new(
                query: "foo",
                sort: new Sort(direction: "ascending", timestamp: "last_edited_time"),
                filter: new Filter(value: "database", property: "object"),
                page_size: 100));
            result.Should().NotBeNullOrEmpty();
        }
    }
}
