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
            var richTextProperty = database.Properties?["Text"] as Property.RichTextProperty;
            richTextProperty?.Name.Should().Be("Text");
            richTextProperty?.Id.Should().Be("%3C%3DW%3D");
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
            var checkbox = database.Properties?["Checkbox"] as Property.Checkbox;
            checkbox?.Name.Should().Be("Checkbox");
            checkbox?.Id.Should().Be("d%5BFd");
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
            var createdBy = database.Properties?["Created by"] as Property.CreatedBy;
            createdBy?.Name.Should().Be("Created by");
            createdBy?.Id.Should().Be("%5Ekx%7B");
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
            var createdTime = database.Properties?["Created time"] as Property.CreatedTime;
            createdTime?.Name.Should().Be("Created time");
            createdTime?.Id.Should().Be("LasM");
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
            var date = database.Properties?["Date"] as Property.Date;
            date?.Name.Should().Be("Date");
            date?.Id.Should().Be("b%3C%7Cq");
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
            var email = database.Properties?["Email"] as Property.Email;
            email?.Name.Should().Be("Email");
            email?.Id.Should().Be("k%7CQn");
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
            var lastEditedBy = database.Properties?["Last edited by"] as Property.LastEditedBy;
            lastEditedBy?.Name.Should().Be("Last edited by");
            lastEditedBy?.Id.Should().Be("%5DHfN");
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
            var lastEditedTime = database.Properties?["Last edited time"] as Property.LastEditedTime;
            lastEditedTime?.Name.Should().Be("Last edited time");
            lastEditedTime?.Id.Should().Be("%3DVc%7D");
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
            var number = database.Properties?["Number"] as Property.Number;
            number?.Name.Should().Be("Number");
            number?.Id.Should().Be("u%5Ex%3B");
            number?.Format.Should().Be("number");
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
            var people = database.Properties?["Person"] as Property.People;
            people?.Name.Should().Be("Person");
            people?.Id.Should().Be("zK%3E_");
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
            var phoneNumber = database.Properties?["Phone"] as Property.PhoneNumber;
            phoneNumber?.Name.Should().Be("Phone");
            phoneNumber?.Id.Should().Be("ODkW");
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
            var url = database.Properties?["URL"] as Property.Url;
            url?.Name.Should().Be("URL");
            url?.Id.Should().Be("%40iW_");
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
            var select = database.Properties?["Select"] as Property.Select;
            select?.Name.Should().Be("Select");
            select?.Id.Should().Be("u%3EXc");
            select?.Options.Should().BeEquivalentTo(new List<Option>
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
            var multiSelect = database.Properties?["Multi-select"] as Property.MultiSelect;
            multiSelect?.Name.Should().Be("Multi-select");
            multiSelect?.Id.Should().Be("ekY%40");
            multiSelect?.Options.Should().BeEquivalentTo(new List<Option>
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
    
    [Fact]
    public async Task Status()
    {
        // arrange

        // act
        var database = await Sut.GetDatabaseAsync(Databases.Status.ToGuid());

        // assert
        using (new AssertionScope())
        {
            var status = database.Properties?["Status"] as Property.Status;
            status?.Name.Should().Be("Status");
            status?.Id.Should().Be("e%3FiD");
            status?.Options.Should().BeEquivalentTo(new List<Property.Status.Option>
            {
                new()
                {
                    Id = Guid.Parse("8e233849-de45-48cd-b191-b655ecbd46e2"),
                    Name = "Not started",
                    Color = "default"
                },
                new()
                {
                    Id = Guid.Parse("9d19bd7a-b686-413c-886a-f66e480c565a"),
                    Name = "In progress",
                    Color = "blue"
                },
                new()
                {
                    Id = Guid.Parse("09fce32d-da47-43b7-8d49-91bbed9424c0"),
                    Name = "Done",
                    Color = "green"
                }
            });
            status?.Groups.Should().BeEquivalentTo(new List<Property.Status.Group>
            {
                new()
                {
                    Id = Guid.Parse("a45ea91c-fcd3-4b0b-b4e2-bebc5d8c02c9"),
                    Name = "To-do",
                    Color = "gray",
                    OptionIds = new List<Guid>()
                    {
                        Guid.Parse("8e233849-de45-48cd-b191-b655ecbd46e2")
                    }
                },
                new()
                {
                    Id = Guid.Parse("6defbcb0-cadf-423b-adbb-8583606055d9"),
                    Name = "In progress",
                    Color = "blue",
                    OptionIds = new List<Guid>
                    {
                        Guid.Parse("9d19bd7a-b686-413c-886a-f66e480c565a")
                    }
                },
                new()
                {
                    Id = Guid.Parse("aa70d2a9-7f9a-4c3c-9484-2839343bc298"),
                    Name = "Complete",
                    Color = "green",
                    OptionIds = new List<Guid>()
                    {
                        Guid.Parse("09fce32d-da47-43b7-8d49-91bbed9424c0")
                    }
                },
            });
        }
    }
}