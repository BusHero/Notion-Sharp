using Notion.Sharp.Tests.Utils;

namespace Notion.Sharp.Tests;
using File = System.IO.File;

public class JsonTests : NotionTestsBase
{
    [Theory]
    [InlineData("Page.json", Pages.Page)]
    [InlineData("PageWithEmojiIcon.json", Pages.PageWithEmojiIcon)]
    [InlineData("PageWithIcon.json", Pages.PageWithIcon)]
    [InlineData("DeletedPage.json", Pages.DeletedPage)]
    [InlineData("PageWithCustomLinkIcon.json", Pages.PageWithCustomLinkIcon)]
    [InlineData("PageWithCover.json", Pages.PageWithCover)]
    [InlineData("PageWithCustomLinkCover.json", Pages.PageWithCustomLinkCover)]
    [InlineData("PageWithUnsplashCover.json", Pages.PageWithUnsplashCover)]
    [InlineData("PageFromDatabase.json", Pages.PageFromDatabase)]
    [InlineData("ParentPage.json", Pages.ParentPage)]
    public async Task JsonShouldMatch(string fileName, string pageId)
    {
        // arrange
        var path = Path.Combine("Resources", "Json", fileName);
        var expectedJson = await File.ReadAllTextAsync(path);
        
        // act
        var page = (await SUT.GetPageRawAsync(Guid.Parse(pageId))).Formatted();
        
        // assert
        page
            .Should()
            .Be(expectedJson);
    }

    [Theory]
    [InlineData("Heading1.json", Blocks.Heading1)]
    [InlineData("Heading2.json", Blocks.Heading2)]
    [InlineData("Heading3.json", Blocks.Heading3)]
    [InlineData("Paragraph.json", Blocks.Paragraph)]
    [InlineData("ToDoUnchecked.json", Blocks.ToDoUnchecked)]
    [InlineData("ToDoChecked.json", Blocks.ToDoChecked)]
    [InlineData("BulletedListItem.json", Blocks.BulletedListItem)]
    [InlineData("NumberedListItem.json", Blocks.NumberedListItem)]
    [InlineData("ToggleList.json", Blocks.ToggleList)]
    [InlineData("Quote.json", Blocks.Quote)]
    [InlineData("Divider.json", Blocks.Divider)]
    [InlineData("Callout.json", Blocks.Callout)]
    [InlineData("Table.json", Blocks.Table)]
    public async Task JsonBlock(string fileName, string pageId)
    {
        // arrange
        var path = Path.Combine("Resources", "Blocks", fileName);
        var expectedJson = await File.ReadAllTextAsync(path);
        
        // act
        var block = (await SUT.GetBlockRawAsync(Guid.Parse(pageId))).Formatted();
        
        // assert
        block
            .Should()
            .Be(expectedJson);
    }
}