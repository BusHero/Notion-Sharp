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
        var page = await Sut.GetPageAsync(Pages.Page.ToGuid());
        
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
        var page = await Sut.GetPageAsync(Pages.DeletedPage.ToGuid());
        
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
        var page = await Sut.GetPageAsync(Pages.PageWithEmojiIcon.ToGuid());
        
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
        var page = await Sut.GetPageAsync(Pages.PageWithIcon.ToGuid());
        
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
        var page = await Sut.GetPageAsync(Pages.PageWithCustomLinkIcon.ToGuid());
        
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
        var page = await Sut.GetPageAsync(Pages.PageWithUploadedIcon.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var externalFile = page.Icon as Icon.File;
            externalFile!.Url.Should().NotBeNull();
            externalFile!.ExpiryTime.Should().BeSameDateAs(DateTime.Now);
        }
    }

    [Fact]
    public async Task GetPageWithCover()
    {
        // act
        var page = await Sut.GetPageAsync(Pages.PageWithCover.ToGuid());
        
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
        var page = await Sut.GetPageAsync(Pages.PageWithCustomLinkCover.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var externalFile = page.Cover as Cover.External;
            externalFile!.Url.Should()
                .Be("https://www.google.com/images/branding" +
                    "/googlelogo/1x/googlelogo_light_color_272x92dp.png");
        }
    }
    
    [Fact]
    public async Task GetPageWithUnslpashCover()
    {
        // act
        var page = await Sut.GetPageAsync(Pages.PageWithUnsplashCover.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var externalFile = page.Cover as Cover.External;
            externalFile.Url.Should()
                .Be("https://images.unsplash.com/photo-1511300636408-a63a89df3482" +
                    "?ixlib=rb-4.0.3&q=85&fm=jpg&crop=entropy&cs=srgb");
        }
    }
    
    [Fact]
    public async Task GetPageWithUploadedCover()
    {
        // act
        var page = await Sut.GetPageAsync(Pages.PageWithUploadedCover.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var externalFile = page.Cover as Cover.File;
            externalFile!.Url.Should().NotBeNull();
            externalFile.ExpiryTime.Should().BeSameDateAs(DateTime.Now);
        }
    }
}
