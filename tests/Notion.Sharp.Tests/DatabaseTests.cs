using FluentAssertions.Execution;
using Notion.Sharp.Tests.Utils;
using Users = Notion.Sharp.Tests.Utils.Users;

namespace Notion.Sharp.Tests;

public class DatabaseTests: NotionTestsBase
{
    [Fact]
    public async Task Name()
    {
        // arrange
        
        // act
        var database = await Sut.GetDatabaseAsync(Databases.Name.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            database.Id.Should().Be(Databases.Name);
            database.Cover.Should().BeNull();
            database.Icon.Should().BeNull();
            database.CreatedTime.Should().Be(DateTimeOffset.Parse("2023-04-10T19:34:00.000Z"));
            database.LastEditedTime.Should().Be(DateTimeOffset.Parse("2023-04-10T19:45:00.000Z"));
            database.IsInline.Should().Be(true);
            database.Archived.Should().Be(false);
            database.Url.Should().Be("https://www.notion.so/8c58fac02aac4c688315dd8b82e5d024");
            
            database.Title.Should().HaveCount(1);
            var text = database.Title?[0] as RichText.Text;
            text?.PlainText.Should().Be("Name");

            database.Properties.Should().ContainKey("Name");
            var name = database.Properties?["Name"] as Property.Title;
            name?.Name.Should().Be("Name");
            name?.Id.Should().Be("title");
            
            var parent = database.Parent as Parent.Page;
            parent?.Id.Should().Be(Pages.PageWithDatabases);

            var createdBy = database.CreatedBy as User.Person;
            createdBy?.Id.Should().Be(Users.Me);

            var lastEditedBy = database.LastEditedBy as User.Person;
            lastEditedBy?.Id.Should().Be(Users.Me);
        }
    }
}