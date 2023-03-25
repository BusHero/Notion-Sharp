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

    [Fact]
    public async Task GetPageAsJson()
    {
        // arrange
        const string expectedJson = """
        {
          "object": "page",
          "id": "12f52fe3-11ae-4c8a-b7eb-6b9ffb22c305",
          "created_time": "2023-03-24T19:49:00.000Z",
          "last_edited_time": "2023-03-24T20:38:00.000Z",
          "created_by": {
            "object": "user",
            "id": "6591a4d4-ca53-4b54-a3ca-7d2420d6b902"
          },
          "last_edited_by": {
            "object": "user",
            "id": "6591a4d4-ca53-4b54-a3ca-7d2420d6b902"
          },
          "cover": null,
          "icon": null,
          "parent": {
            "type": "page_id",
            "page_id": "b5d54483-4cb1-4fd3-a80e-897da4827770"
          },
          "archived": false,
          "properties": {
            "title": {
              "id": "title",
              "type": "title",
              "title": [
                {
                  "type": "text",
                  "text": {
                    "content": "Test page",
                    "link": null
                  },
                  "annotations": {
                    "bold": false,
                    "italic": false,
                    "strikethrough": false,
                    "underline": false,
                    "code": false,
                    "color": "default"
                  },
                  "plain_text": "Test page",
                  "href": null
                }
              ]
            }
          },
          "url": "https://www.notion.so/Test-page-12f52fe311ae4c8ab7eb6b9ffb22c305"
        }
        """;
        
        // act
        var page = (await SUT.GetPageRawAsync(ValidPageId)).Formatted();
        
        // assert
        page
            .Should()
            .Be(expectedJson);
    }
}

internal static class JsonExtensions
{
    internal static string Formatted(this string json)
    {
        var document = JsonDocument.Parse(json);
        var jsonSerialized = JsonSerializer.Serialize(
            document, 
            new JsonSerializerOptions
            {
                WriteIndented = true
            });
        return jsonSerialized;
    }
    
    internal static bool IsValidJson(this string json)
    {
        try
        {
            _ = JsonDocument.Parse(json);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }
}