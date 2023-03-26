using Notion.Sharp.Tests.Utils;

namespace Notion.Sharp.Tests;
using File = System.IO.File;

public class JsonTests : NotionTestsBase
{
    [Theory]
    [InlineData("Page.json", "12f52fe311ae4c8ab7eb6b9ffb22c305")]
    [InlineData("PageWithIcon.json", "623dea60eecc4e5d95e9048941712e7d")]
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