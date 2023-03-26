using FluentAssertions.Execution;
using Notion.Sharp.Tests.Utils;

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
    public async Task GetPageWithIconWorks()
    {
        var action = async () =>  await SUT.GetPageAsync(Pages.PageWithEmojiIcon.ToGuid());
        await action.Should().NotThrowAsync();
    }
}
