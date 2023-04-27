using FluentAssertions.Execution;
using Notion.Sharp.Tests.Utils;
using Cover = Notion.Model.Cover;
using File = Notion.Model.File;

namespace Notion.Sharp.Tests;

public class PageTests: NotionTestsBase
{
    [Fact]
    public async Task GetPageOnValidPageId()
    {
        //arrange
        
        //act 
        var page = await NotionClient.Page(Pages.Page.ToGuid()).Get();
        
        //assert
        using (new AssertionScope())
        {
            page.Id.Should().Be(Pages.Page.ToGuid());
            page.Archived.Should().Be(false);
            page.Properties.Should().ContainKey("title");
            page.Cover.Should().BeNull();
            page.Icon.Should().BeNull();
            (page.Parent as Parent.Page)?.Id.Should().Be(Pages.Parent);
        }
    }

    [Fact]
    public async Task RemovedPageShouldBeShownAsArchived()
    {
        //arrange
        
        //act 
        var page = await NotionClient.Page(Pages.DeletedPage.ToGuid()).Get();
        
        //assert
        using (new AssertionScope())
        {
            page.Id.Should().Be(Pages.DeletedPage.ToGuid());
            page.Archived.Should().Be(true);
        }
    }

    [Fact]
    public async Task GetPageWithEmojiIconWorks()
    {
        // act
        var page = await NotionClient.Page(Pages.PageWithEmojiIcon.ToGuid()).Get();
        
        // assert
        using (new AssertionScope())
        {
            var emoji = page.Icon as Icon.Emoji;
            emoji!.Value.Should().Be("😀");
        }
    }
    
    [Fact]
    public async Task GetPageWithIconWorks()
    {
        // act
        var page = await NotionClient.Page(Pages.PageWithIcon.ToGuid()).Get();
        
        // assert
        using (new AssertionScope())
        {
            var externalFile = page.Icon as Icon.External;
            externalFile!.Url.Should().Be("https://www.notion.so/icons/activity_gray.svg");
        }
    }

    [Fact]
    public async Task GetPageWithCustomLinkIconWorks()
    {
        // act
        var page = await NotionClient.Page(Pages.PageWithCustomLinkIcon.ToGuid()).Get();
        
        // assert
        using (new AssertionScope())
        {
            var externalFile = page.Icon as Icon.External;
            externalFile!.Url.Should()
                .Be("https://www.google.com/images/branding/googlelogo/1x/googlelogo_light_color_272x92dp.png");
        }
    }

    [Fact]
    public async Task GetPageWithUploadedIconWorks()
    {
        // act
        var page = await NotionClient.Page(Pages.PageWithUploadedIcon.ToGuid()).Get();
        
        // assert
        using (new AssertionScope())
        {
            var externalFile = page.Icon as Icon.File;
            externalFile!.Url.Should().NotBeNull();
        }
    }

    [Fact]
    public async Task GetPageWithCover()
    {
        // act
        var page = await NotionClient.Page(Pages.PageWithCover.ToGuid()).Get();
        
        // assert
        using (new AssertionScope())
        {
            var externalFile = page.Cover as Cover.External;
            externalFile!.Url.Should()
                .Be("https://www.notion.so/images/page-cover/gradients_8.png");
        }
    }
    
    [Fact]
    public async Task GetPageWithCustomLinkCover()
    {
        // act
        var page = await NotionClient.Page(Pages.PageWithCustomLinkCover.ToGuid()).Get();
        
        // assert
        using (new AssertionScope())
        {
            var externalFile = page.Cover as Cover.External;
            externalFile!.Url.Should()
                .Be("""https://www.google.com/images/branding/googlelogo/1x/googlelogo_light_color_272x92dp.png""");
        }
    }
    
    [Fact]
    public async Task GetPageWithUnsplashCover()
    {
        // act
        var page = await NotionClient.Page(Pages.PageWithUnsplashCover.ToGuid()).Get();
        
        // assert
        using (new AssertionScope())
        {
            var externalFile = page.Cover as Cover.External;
            externalFile?.Url.Should()
                .Be("""https://images.unsplash.com/photo-1511300636408-a63a89df3482?ixlib=rb-4.0.3&q=85&fm=jpg&crop=entropy&cs=srgb""");
        }
    }
    
    [Fact]
    public async Task GetPageWithUploadedCover()
    {
        // act
        var page = await NotionClient.Page(Pages.PageWithUploadedCover.ToGuid()).Get();
        
        // assert
        using (new AssertionScope())
        {
            var externalFile = page.Cover as Cover.File;
            externalFile!.Url.Should().NotBeNull();
        }
    }

    [Fact]
    public async Task PageName()
    {
        // arrange
        
        // act
        var page = await NotionClient.Page(Pages.PageName.ToGuid()).Get();

        // assert
        using (new AssertionScope())
        {
            var title = page.Properties["Name"] as PropertyValue.Title;
            title?.Content?[0].PlainText.Should().Be("Name");
        }
    }

    [Fact]
    public async Task PageText()
    {
        // arrange
        
        // act
        var page = await NotionClient.Page(Pages.PageText.ToGuid()).Get();

        // assert
        using (new AssertionScope())
        {
            var text = page.Properties["Text"] as PropertyValue.Text;
            text?.Content?[0].PlainText.Should().Be("Some text");
        }
    }
    
    [Fact]
    public async Task PageCheckbox()
    {
        // arrange
        
        // act
        var page = await NotionClient.Page(Pages.PageCheckBox.ToGuid()).Get();

        // assert
        using (new AssertionScope())
        {
            var checkbox = page.Properties["Checkbox"] as PropertyValue.Checkbox;
            checkbox?.Checked.Should().BeTrue();
        }
    }
    
    [Fact]
    public async Task PageCreatedTime()
    {
        // arrange
        
        // act
        var page = await NotionClient.Page(Pages.PageCreatedTime.ToGuid()).Get();

        // assert
        using (new AssertionScope())
        {
            var checkbox = page.Properties["Created time"] as PropertyValue.CreatedTime;
            checkbox?.Value.Should().Be(DateTimeOffset.Parse("2023-04-10T19:47:00.000Z"));
        }
    }
    
    [Fact]
    public async Task PageDate()
    {
        // arrange
        
        // act
        var page = await NotionClient.Page(Pages.PageDate.ToGuid()).Get();

        // assert
        using (new AssertionScope())
        {
            var date = page.Properties["Date"] as PropertyValue.Date;
            date?.Start.Should().Be(DateTime.Parse("2023-04-10"));
            date?.End.Should().BeNull();
            date?.TimeZone.Should().BeNull();
        }
    }
    
    [Fact]
    public async Task PageEmail()
    {
        // arrange
        
        // act
        var page = await NotionClient.Page(Pages.PageEmail.ToGuid()).Get();

        // assert
        using (new AssertionScope())
        {
            var email = page.Properties["Email"] as PropertyValue.Email;
            email?.Value.Should().Be("petru.cervac@gmail.com");
        }
    }
    
    [Fact]
    public async Task PageLastEditedTime()
    {
        // arrange
        
        // act
        var page = await NotionClient.Page(Pages.PageLastEditedTime.ToGuid()).Get();

        // assert
        using (new AssertionScope())
        {
            var lastEditedTime = page.Properties["Last edited time"] as PropertyValue.LastEditedTime;
            lastEditedTime?.Value.Should().Be(DateTimeOffset.Parse("2023-04-10T19:48:00.000Z"));
        }
    }
    
    [Fact]
    public async Task PageFormula()
    {
        // arrange
        
        // act
        var page = await NotionClient.Page(Pages.PageFormula.ToGuid()).Get();

        // assert
        using (new AssertionScope())
        {
            var formula = page.Properties["Formula"] as PropertyValue.StringFormula;
            formula?.Value.Should().Be("Formula");
        }
    }
    
    [Fact]
    public async Task PagePhone()
    {
        // arrange
        
        // act
        var page = await NotionClient.Page(Pages.PagePhone.ToGuid()).Get();

        // assert
        using (new AssertionScope())
        {
            var phoneNumber = page.Properties["Phone"] as PropertyValue.PhoneNumber;
            phoneNumber?.Value.Should().Be("+37379153404");
        }
    }
    
    [Fact]
    public async Task PageUrl()
    {
        // arrange
        
        // act
        var page = await NotionClient.Page(Pages.PageUrl.ToGuid()).Get();

        // assert
        using (new AssertionScope())
        {
            var url = page.Properties["URL"] as PropertyValue.Url;
            url?.Link.Should().Be("https://google.com");
        }
    }
    
    [Fact]
    public async Task PageNumber()
    {
        // arrange
        
        // act
        var page = await NotionClient.Page(Pages.PageNumber.ToGuid()).Get();

        // assert
        using (new AssertionScope())
        {
            var number = page.Properties["Number"] as PropertyValue.Number;
            number?.Value.Should().Be(123);
        }
    }
    
    [Fact]
    public async Task PageOption()
    {
        // arrange
        
        // act
        var page = await NotionClient.Page(Pages.PageOption.ToGuid()).Get();

        // assert
        using (new AssertionScope())
        {
            var select = page.Properties["Select"] as PropertyValue.Select;
            select?.Option?.Id.Should().Be("~WVP");
            select?.Option?.Color.Should().Be("brown");
            select?.Option?.Name.Should().Be("Option 1");
        }
    }
    
    [Fact]
    public async Task PageMultiSelect()
    {
        // arrange
        
        // act
        var page = await NotionClient.Page(Pages.PageMultiSelect.ToGuid()).Get();

        // assert
        using (new AssertionScope())
        {
            var select = page.Properties["Multi-select"] as PropertyValue.MultiSelect;
            select?.Options.Should().BeEquivalentTo(new List<Option>()
            {
                new()
                {
                    Id = "dTsU",
                    Name = "Option 1",
                    Color = "orange"
                },
                new()
                {
                    // ReSharper disable once StringLiteralTypo
                    Id = "JTFE",
                    Name = "Option 2",
                    Color = "gray"
                },
            });
        }
    }
    
    [Fact]
    public async Task PageFile()
    {
        // arrange
        
        // act
        var page = await NotionClient.Page(Pages.PageFile.ToGuid()).Get();

        // assert
        using (new AssertionScope())
        {
            var files = page.Properties["Files \u0026 media"] as PropertyValue.Files;
            var file = files?.Value?[0] as File.External;
            file?.Name.Should().Be("https://www.notion.so/Databases-001e7c51c1a04ccf871ab2e483155bf2");
            file?.Uri.Should().Be("https://www.notion.so/Databases-001e7c51c1a04ccf871ab2e483155bf2");
        }
    }

    [Fact]
    public async Task PageCreatedBy()
    {
        // arrange
        
        // act
        var page = await NotionClient.Page(Pages.PageCreatedBy.ToGuid()).Get();

        // assert
        using (new AssertionScope())
        {
            var createdBy = page.Properties["Created by"] as PropertyValue.CreatedBy;
            var person = createdBy?.Value as User.Person;
            person?.Name.Should().Be("Petru Cervac");
            person?.AvatarUrl.Should().Be("https://lh3.googleusercontent.com/a-/AOh14GhcBvrhyvv32v0kTVHocfT7oex0gofyo0r6OjoHPw=s100");
            person?.Email.Should().Be("petru.cervac@gmail.com");
        }
    }

    [Fact]
    public async Task PageLastEditedBy()
    {
        // arrange
        
        // act
        var page = await NotionClient.Page(Pages.PageLastEditedBy.ToGuid()).Get();

        // assert
        using (new AssertionScope())
        {
            var lastEditedBy = page.Properties["Last edited by"] as PropertyValue.LastEditedBy;
            var person = lastEditedBy?.Value as User.Person;
            person?.Name.Should().Be("Petru Cervac");
            person?.AvatarUrl.Should().Be("https://lh3.googleusercontent.com/a-/AOh14GhcBvrhyvv32v0kTVHocfT7oex0gofyo0r6OjoHPw=s100");
            person?.Email.Should().Be("petru.cervac@gmail.com");
        }
    }

    [Fact]
    public async Task PagePerson()
    {
        // arrange
        
        // act
        var page = await NotionClient.Page(Pages.PagePerson.ToGuid()).Get();

        // assert
        using (new AssertionScope())
        {
            var lastEditedBy = page.Properties["Person"] as PropertyValue.People;
            var person = lastEditedBy?.Value?[0] as User.Person;
            person?.Name.Should().Be("Petru Cervac");
            person?.AvatarUrl.Should().Be("https://lh3.googleusercontent.com/a-/AOh14GhcBvrhyvv32v0kTVHocfT7oex0gofyo0r6OjoHPw=s100");
            person?.Email.Should().Be("petru.cervac@gmail.com");
        }
    }

    [Fact]
    public async Task PageRelationParent()
    {
        // arrange
        
        // act
        var page = await NotionClient.Page(Pages.PageRelationParent.ToGuid()).Get();

        // assert
        using (new AssertionScope())
        {
            var relation = page.Properties["Relation Child"] as PropertyValue.Relation;
            relation?.Pages?[0].Id.Should().Be(Pages.PageRelationChild);
        }
    }

    [Fact]
    public async Task PageRelationChild()
    {
        // arrange
        
        // act
        var page = await NotionClient.Page(Pages.PageRelationChild.ToGuid()).Get();

        // assert
        using (new AssertionScope())
        {
            var relation = page.Properties["Relation Parent Single"] as PropertyValue.Relation;
            relation?.Pages?[0].Id.Should().Be(Pages.PageRelationParent);

            var arrayRollup = page.Properties["Rollup"] as PropertyValue.ArrayRollup;
            arrayRollup?.Function.Should().Be("show_original");
            var title = arrayRollup?.Value?[0] as PropertyValue.Title;
            title?.Content?[0].PlainText.Should().Be("Parent");
        }
    }

    [Fact]
    public async Task PageStatus()
    {
        // arrange
        
        // act
        var page = await NotionClient.Page(Pages.PageStatus.ToGuid()).Get();

        // assert
        using (new AssertionScope())
        {
            var status = page.Properties["Status"] as PropertyValue.Status;
            status?.Value.Should().BeEquivalentTo(new Property.Status.Option
            {
                Id = Guid.Parse("8e233849-de45-48cd-b191-b655ecbd46e2"),
                Name = "Not started",
                Color = "default",
            });
        }
    }
}
