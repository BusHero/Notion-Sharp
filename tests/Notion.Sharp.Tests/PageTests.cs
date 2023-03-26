using FluentAssertions.Execution;
using Notion.Sharp.Tests.Utils;
using File = Notion.Model.File;

namespace Notion.Sharp.Tests;

public class PageTests: NotionTestsBase
{
    [Fact]
    public async Task GetPageOnValidPageId()
    {
        //arrange
        
        //act 
        var page = await SUT.GetPageAsync(Pages.Page.ToGuid());
        
        //assert
        using (new AssertionScope())
        {
            page.Id.Should().Be(Pages.Page.ToGuid());
            page.Archived.Should().Be(false);
            page.Properties.Should().ContainKey("title");
            page.Cover.Should().BeNull();
            page.Icon.Should().BeNull();
            (page.Parent as Parent.Page)?.Id.Should().Be(ParentPage);
        }
    }

    [Fact]
    public async Task RemovedPageShouldBeShownAsArchived()
    {
        //arrange
        
        //act 
        var page = await SUT.GetPageAsync(Pages.DeletedPage.ToGuid());
        
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
        var page = await SUT.GetPageAsync(Pages.PageWithEmojiIcon.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var emoji = page.Icon as File.Emoji;
            emoji!.Value.Should().Be("😀");
            emoji!.Name.Should().BeNull();
            emoji!.Caption.Should().BeNull();
        }
    }
    
    [Fact]
    public async Task GetPageWithIconWorks()
    {
        // act
        var page = await SUT.GetPageAsync(Pages.PageWithIcon.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var externalFile = page.Icon as File.External;
            externalFile!.Name.Should().BeNull();
            externalFile!.Caption.Should().BeNull();
            externalFile.Uri.Should().Be("https://www.notion.so/icons/activity_gray.svg");
        }
    }

    [Fact]
    public async Task GetPageWithCustomLinkIconWorks()
    {
        // act
        var page = await SUT.GetPageAsync(Pages.PageWithCustomLinkIcon.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var externalFile = page.Icon as File.External;
            externalFile!.Name.Should().BeNull();
            externalFile!.Caption.Should().BeNull();
            externalFile.Uri.Should()
                .Be("https://www.google.com/images/branding/googlelogo/1x/googlelogo_light_color_272x92dp.png");
        }
    }

    [Fact]
    public async Task GetPageWithUploadedIconWorks()
    {
        // act
        var page = await SUT.GetPageAsync(Pages.PageWithUploadedIcon.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var externalFile = page.Icon as File.Internal;
            externalFile!.Name.Should().BeNull();
            externalFile!.Caption.Should().BeNull();
            externalFile.Uri.Should().NotBeNull();
            externalFile.ExpireTime.Should().BeSameDateAs(DateTime.Now);
        }
    }
}
