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
    public async Task JsonShouldMatch(string fileName, string pageId)
    {
        // arrange
        var path = Path.Combine("Json", fileName);
        var expectedJson = await File.ReadAllTextAsync(path);
        
        // act
        var page = (await SUT.GetPageRawAsync(Guid.Parse(pageId))).Formatted();
        
        // assert
        page
            .Should()
            .Be(expectedJson);
    }
}