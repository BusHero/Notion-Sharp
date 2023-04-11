using FluentAssertions.Execution;
using Notion.Sharp.Tests.Utils;
using Property = Notion.Model.Property;
using Users = Notion.Sharp.Tests.Utils.Users;

namespace Notion.Sharp.Tests;

public class DatabaseTests : NotionTestsBase
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

    [Fact]
    public async Task Text()
    {
        // arrange

        // act
        var database = await Sut.GetDatabaseAsync(Databases.Text.ToGuid());

        // assert
        using (new AssertionScope())
        {
            var text = database.Properties?["Text"] as Property.RichTextProperty;
            text?.Name.Should().Be("Text");
            text?.Id.Should().Be("%3C%3DW%3D");
        }
    }

    [Fact]
    public async Task Checkbox()
    {
        // arrange

        // act
        var database = await Sut.GetDatabaseAsync(Databases.Checkbox.ToGuid());

        // assert
        using (new AssertionScope())
        {
            var text = database.Properties?["Checkbox"] as Property.Checkbox;
            text?.Name.Should().Be("Checkbox");
            text?.Id.Should().Be("d%5BFd");
        }
    }

    [Fact]
    public async Task CreatedBy()
    {
        // arrange

        // act
        var database = await Sut.GetDatabaseAsync(Databases.CreatedBy.ToGuid());

        // assert
        using (new AssertionScope())
        {
            var text = database.Properties?["Created by"] as Property.CreatedBy;
            text?.Name.Should().Be("Created by");
            text?.Id.Should().Be("%5Ekx%7B");
        }
    }

    [Fact]
    public async Task CreatedTime()
    {
        // arrange

        // act
        var database = await Sut.GetDatabaseAsync(Databases.CreatedTime.ToGuid());

        // assert
        using (new AssertionScope())
        {
            var text = database.Properties?["Created time"] as Property.CreatedTime;
            text?.Name.Should().Be("Created time");
            text?.Id.Should().Be("LasM");
        }
    }

    [Fact]
    public async Task Date()
    {
        // arrange

        // act
        var database = await Sut.GetDatabaseAsync(Databases.Date.ToGuid());

        // assert
        using (new AssertionScope())
        {
            var text = database.Properties?["Date"] as Property.Date;
            text?.Name.Should().Be("Date");
            text?.Id.Should().Be("b%3C%7Cq");
        }
    }

    [Fact]
    public async Task Email()
    {
        // arrange

        // act
        var database = await Sut.GetDatabaseAsync(Databases.Email.ToGuid());

        // assert
        using (new AssertionScope())
        {
            var text = database.Properties?["Email"] as Property.Email;
            text?.Name.Should().Be("Email");
            text?.Id.Should().Be("k%7CQn");
        }
    }

    [Fact]
    public async Task Files()
    {
        // arrange

        // act
        var database = await Sut.GetDatabaseAsync(Databases.Files.ToGuid());

        // assert
        using (new AssertionScope())
        {
            var text = database.Properties?["Files \u0026 media"] as Property.Files;
            text?.Name.Should().Be("Files \u0026 media");
            text?.Id.Should().Be("%3EGgT");
        }
    }

    [Fact]
    public async Task Formula()
    {
        // arrange

        // act
        var database = await Sut.GetDatabaseAsync(Databases.Formula.ToGuid());

        // assert
        using (new AssertionScope())
        {
            var formula = database.Properties?["Formula"] as Property.Formula;
            formula?.Name.Should().Be("Formula");
            formula?.Id.Should().Be("aOy%3C");
            formula?.Expression.Should().Be("prop(\u0022Name\u0022)");
        }
    }

    [Fact]
    public async Task LastEditedBy()
    {
        // arrange

        // act
        var database = await Sut.GetDatabaseAsync(Databases.LastEditedBy.ToGuid());

        // assert
        using (new AssertionScope())
        {
            var formula = database.Properties?["Last edited by"] as Property.LastEditedBy;
            formula?.Name.Should().Be("Last edited by");
            formula?.Id.Should().Be("%5DHfN");
        }
    }

    [Fact]
    public async Task LastEditedTime()
    {
        // arrange

        // act
        var database = await Sut.GetDatabaseAsync(Databases.LastEditedTime.ToGuid());

        // assert
        using (new AssertionScope())
        {
            var formula = database.Properties?["Last edited time"] as Property.LastEditedTime;
            formula?.Name.Should().Be("Last edited time");
            formula?.Id.Should().Be("%3DVc%7D");
        }
    }

    [Fact]
    public async Task Number()
    {
        // arrange

        // act
        var database = await Sut.GetDatabaseAsync(Databases.Number.ToGuid());

        // assert
        using (new AssertionScope())
        {
            var formula = database.Properties?["Number"] as Property.Number;
            formula?.Name.Should().Be("Number");
            formula?.Id.Should().Be("u%5Ex%3B");
            formula?.Format.Should().Be("number");
        }
    }

    [Fact]
    public async Task Person()
    {
        // arrange

        // act
        var database = await Sut.GetDatabaseAsync(Databases.Person.ToGuid());

        // assert
        using (new AssertionScope())
        {
            var formula = database.Properties?["Person"] as Property.People;
            formula?.Name.Should().Be("Person");
            formula?.Id.Should().Be("zK%3E_");
        }
    }

    [Fact]
    public async Task Phone()
    {
        // arrange

        // act
        var database = await Sut.GetDatabaseAsync(Databases.Phone.ToGuid());

        // assert
        using (new AssertionScope())
        {
            var formula = database.Properties?["Phone"] as Property.PhoneNumber;
            formula?.Name.Should().Be("Phone");
            formula?.Id.Should().Be("ODkW");
        }
    }

    [Fact]
    public async Task Url()
    {
        // arrange

        // act
        var database = await Sut.GetDatabaseAsync(Databases.Url.ToGuid());

        // assert
        using (new AssertionScope())
        {
            var formula = database.Properties?["URL"] as Property.Url;
            formula?.Name.Should().Be("URL");
            formula?.Id.Should().Be("%40iW_");
        }
    }

    [Fact]
    public async Task Option()
    {
        // arrange

        // act
        var database = await Sut.GetDatabaseAsync(Databases.Option.ToGuid());

        // assert
        using (new AssertionScope())
        {
            var formula = database.Properties?["Select"] as Property.Select;
            formula?.Name.Should().Be("Select");
            formula?.Id.Should().Be("u%3EXc");
            formula?.Options.Should().BeEquivalentTo(new List<Option>
            {
                new()
                {
                    Id = "D|lf",
                    Name = "Option 2",
                    Color = "default"
                },
                new()
                {
                    Id = "nZ:R",
                    Name = "Option 3",
                    Color = "yellow"
                },
                new()
                {
                    Id = "~WVP",
                    Name = "Option 1",
                    Color = "brown"
                }
            });
        }
    }

    [Fact]
    public async Task MultiSelect()
    {
        // arrange

        // act
        var database = await Sut.GetDatabaseAsync(Databases.MultiSelect.ToGuid());

        // assert
        using (new AssertionScope())
        {
            var formula = database.Properties?["Multi-select"] as Property.MultiSelect;
            formula?.Name.Should().Be("Multi-select");
            formula?.Id.Should().Be("ekY%40");
            formula?.Options.Should().BeEquivalentTo(new List<Option>
            {
                new()
                {
                    // ReSharper disable once StringLiteralTypo
                    Id = "MMXX",
                    Name = "Option 3",
                    Color = "green"
                },
                new()
                {
                    // ReSharper disable once StringLiteralTypo
                    Id = "JTFE",
                    Name = "Option 2",
                    Color = "gray"
                },
                new()
                {
                    Id = "dTsU",
                    Name = "Option 1",
                    Color = "orange"
                }
            });
        }
    }
}