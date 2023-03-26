using FluentAssertions.Execution;

namespace Notion.Sharp.Tests;

public class PageTests: NotionTestsBase
{
    [Fact]
    public async Task GetPageOnValidPageId()
    {
        //arrange
        
        //act 
        var page = await SUT.GetPageAsync(ValidPageId);
        
        //assert
        using (new AssertionScope())
        {
            page.Id.Should().Be(ValidPageId);
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
        var page = await SUT.GetPageAsync(DeletedPage);
        
        //assert
        using (new AssertionScope())
        {
            page.Id.Should().Be(DeletedPage);
            page.Archived.Should().Be(true);
        }
    }
}
