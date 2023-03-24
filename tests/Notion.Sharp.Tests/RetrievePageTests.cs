using FluentAssertions.Execution;

namespace Notion.Sharp.Tests;

public class RetrievePageTests: NotionTestsBase
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
            page.Should().NotBeNull();
            page.Id.Should().Be(ValidPageId);
            page.Archived.Should().Be(false);
            page.Properties.Should().ContainKey("title");
            page.Cover.Should().BeNull();
            page.Icon.Should().BeNull();
            (page.Parent as Parent.Page)?.Id.Should().Be(ParentPage);
        }
    }
}