using Notion.Sharp.Tests.Utils;

namespace Notion.Sharp.Tests;
using File = System.IO.File;

public enum Endpoints
{
    Pages,
    Blocks,
    RichTexts,
    Databases
}

public class ContractTests : NotionTestsBase
{
    [Theory]
    [InlineData(Endpoints.Pages, "Page.json", Pages.Page)]
    [InlineData(Endpoints.Pages, "PageWithEmojiIcon.json", Pages.PageWithEmojiIcon)]
    [InlineData(Endpoints.Pages, "PageWithIcon.json", Pages.PageWithIcon)]
    [InlineData(Endpoints.Pages, "DeletedPage.json", Pages.DeletedPage)]
    [InlineData(Endpoints.Pages, "PageWithCustomLinkIcon.json", Pages.PageWithCustomLinkIcon)]
    [InlineData(Endpoints.Pages, "PageWithCover.json", Pages.PageWithCover)]
    [InlineData(Endpoints.Pages, "PageWithCustomLinkCover.json", Pages.PageWithCustomLinkCover)]
    [InlineData(Endpoints.Pages, "PageWithUnsplashCover.json", Pages.PageWithUnsplashCover)]
    [InlineData(Endpoints.Pages, "PageFromDatabase.json", Pages.PageFromDatabase)]
    [InlineData(Endpoints.Pages, "Parent.json", Pages.Parent)]
    [InlineData(Endpoints.Blocks, "Heading1.json", Blocks.Heading1)]
    [InlineData(Endpoints.Blocks, "Heading2.json", Blocks.Heading2)]
    [InlineData(Endpoints.Blocks, "Heading3.json", Blocks.Heading3)]
    [InlineData(Endpoints.Blocks, "Paragraph.json", Blocks.Paragraph)]
    [InlineData(Endpoints.Blocks, "ToDoUnchecked.json", Blocks.ToDoUnchecked)]
    [InlineData(Endpoints.Blocks, "ToDoChecked.json", Blocks.ToDoChecked)]
    [InlineData(Endpoints.Blocks, "BulletedListItem.json", Blocks.BulletedListItem)]
    [InlineData(Endpoints.Blocks, "NumberedListItem.json", Blocks.NumberedListItem)]
    [InlineData(Endpoints.Blocks, "ToggleList.json", Blocks.ToggleList)]
    [InlineData(Endpoints.Blocks, "Quote.json", Blocks.Quote)]
    [InlineData(Endpoints.Blocks, "Divider.json", Blocks.Divider)]
    [InlineData(Endpoints.Blocks, "Callout.json", Blocks.Callout)]
    [InlineData(Endpoints.Blocks, "Table.json", Blocks.Table)]
    [InlineData(Endpoints.Blocks, "Page.json", Blocks.Page)]
    [InlineData(Endpoints.Blocks, "LinkToPage.json", Blocks.LinkToPage)]
    [InlineData(Endpoints.Blocks, "ImageUnsplash.json", Blocks.ImageUnsplash)]
    [InlineData(Endpoints.Blocks, "ImageLink.json", Blocks.ImageLink)]
    [InlineData(Endpoints.Blocks, "ImageEmpty.json", Blocks.ImageEmpty)]
    [InlineData(Endpoints.Blocks, "WebBookmark.json", Blocks.WebBookmark)]
    [InlineData(Endpoints.Blocks, "WebBookmarkEmpty.json", Blocks.WebBookmarkEmpty)]
    [InlineData(Endpoints.Blocks, "Video.json", Blocks.Video)]
    [InlineData(Endpoints.Blocks, "VideoEmpty.json", Blocks.VideoEmpty)]
    [InlineData(Endpoints.Blocks, "AudioEmpty.json", Blocks.AudioEmpty)]
    [InlineData(Endpoints.Blocks, "Code.json", Blocks.Code)]
    [InlineData(Endpoints.Blocks, "CodeWithCaption.json", Blocks.CodeWithCaption)]
    [InlineData(Endpoints.Blocks, "CodeCSharp.json", Blocks.CodeCSharp)]
    [InlineData(Endpoints.Blocks, "File.json", Blocks.File)]
    [InlineData(Endpoints.Blocks, "FileEmpty.json", Blocks.FileEmpty)]
    [InlineData(Endpoints.Blocks, "FileWithCaption.json", Blocks.FileWithCaption)]
    [InlineData(Endpoints.Blocks, "Audio.json", Blocks.Audio)]
    [InlineData(Endpoints.Blocks, "Equation.json", Blocks.Equation)]
    [InlineData(Endpoints.Blocks, "TableOfContents.json", Blocks.TableOfContents)]
    [InlineData(Endpoints.Blocks, "EquationEmpty.json", Blocks.EquationEmpty)]
    [InlineData(Endpoints.Blocks, "Breadcumb.json", Blocks.Breadcrumb)]
    [InlineData(Endpoints.Blocks, "SyncBlockOriginal.json", Blocks.SyncBlockOriginal)]
    [InlineData(Endpoints.Blocks, "SyncdBlockCopy.json", Blocks.SyncdBlockCopy)]
    [InlineData(Endpoints.Blocks, "Button.json", Blocks.Button)]
    [InlineData(Endpoints.Blocks, "ToggledHeading3.json", Blocks.ToggledHeading3)]
    [InlineData(Endpoints.Blocks, "ToggledHeading2.json", Blocks.ToggledHeading2)]
    [InlineData(Endpoints.Blocks, "ToggledHeading1.json", Blocks.ToggledHeading1)]
    [InlineData(Endpoints.Blocks, "Column.json", Blocks.Column)]
    [InlineData(Endpoints.Blocks, "ColumnList.json", Blocks.ColumnList)]
    [InlineData(Endpoints.Blocks, "Embed.json", Blocks.Embed)]
    [InlineData(Endpoints.Blocks, "TwitterEmbed.json", Blocks.TwitterEmbed)]
    [InlineData(Endpoints.Blocks, "ChildDatabase.json", Blocks.ChildDatabase)]
    [InlineData(Endpoints.Blocks, "LinkedDatabase.json", Blocks.LinkedDatabase)]
    [InlineData(Endpoints.Blocks, "BlockWithChildren.json", Blocks.BlockWithChildren)]
    [InlineData(Endpoints.Blocks, "ChildBlock.json", Blocks.ChildBlock)]
    [InlineData(Endpoints.Blocks, "Pdf.json", Blocks.Pdf)]
    [InlineData(Endpoints.Blocks, "LinkPreview.json", Blocks.LinkPreview)]
    [InlineData(Endpoints.RichTexts, "SimpleText.json", RichTexts.SimpleText)]
    [InlineData(Endpoints.RichTexts, "MentionPerson.json", RichTexts.MentionPerson)]
    [InlineData(Endpoints.RichTexts, "MentionBlock.json", RichTexts.MentionBlock)]
    [InlineData(Endpoints.RichTexts, "MentionToday.json", RichTexts.MentionToday)]
    [InlineData(Endpoints.RichTexts, "MentionTomorrow.json", RichTexts.MentionTomorrow)]
    [InlineData(Endpoints.RichTexts, "LinkPage.json", RichTexts.LinkPage)]
    [InlineData(Endpoints.RichTexts, "LinkToWebsite.json", RichTexts.LinkToWebsite)]
    [InlineData(Endpoints.RichTexts, "Bold.json", RichTexts.Bold)]
    [InlineData(Endpoints.RichTexts, "Italic.json", RichTexts.Italic)]
    [InlineData(Endpoints.RichTexts, "Underline.json", RichTexts.Underline)]
    [InlineData(Endpoints.RichTexts, "StrikeThrough.json", RichTexts.StrikeThrough)]
    [InlineData(Endpoints.RichTexts, "Background.json", RichTexts.Background)]
    [InlineData(Endpoints.RichTexts, "Foreground.json", RichTexts.Foreground)]
    [InlineData(Endpoints.RichTexts, "Code.json", RichTexts.Code)]
    [InlineData(Endpoints.RichTexts, "Equation.json", RichTexts.Equation)]
    [InlineData(Endpoints.RichTexts, "BoldAndItalic.json", RichTexts.BoldAndItalic)]
    [InlineData(Endpoints.RichTexts, "BoldThenItalic.json", RichTexts.BoldThenItalic)]
    [InlineData(Endpoints.Databases, "Name.json", Databases.Name)]
    [InlineData(Endpoints.Databases, "Text.json", Databases.Text)]
    [InlineData(Endpoints.Databases, "Number.json", Databases.Number)]
    [InlineData(Endpoints.Databases, "Date.json", Databases.Date)]
    [InlineData(Endpoints.Databases, "Option.json", Databases.Option)]
    [InlineData(Endpoints.Databases, "MultiSelect.json", Databases.MultiSelect)]
    [InlineData(Endpoints.Databases, "Status.json", Databases.Status)]
    [InlineData(Endpoints.Databases, "Person.json", Databases.Person)]
    [InlineData(Endpoints.Databases, "Files.json", Databases.Files)]
    [InlineData(Endpoints.Databases, "Checkbox.json", Databases.Checkbox)]
    [InlineData(Endpoints.Databases, "Url.json", Databases.Url)]
    [InlineData(Endpoints.Databases, "Email.json", Databases.Email)]
    [InlineData(Endpoints.Databases, "Phone.json", Databases.Phone)]
    [InlineData(Endpoints.Databases, "Formula.json", Databases.Formula)]
    [InlineData(Endpoints.Databases, "CreatedTime.json", Databases.CreatedTime)]
    [InlineData(Endpoints.Databases, "CreatedBy.json", Databases.CreatedBy)]
    [InlineData(Endpoints.Databases, "LastEditedTime.json", Databases.LastEditedTime)]
    [InlineData(Endpoints.Databases, "LastEditedBy.json", Databases.LastEditedBy)]
    [InlineData(Endpoints.Databases, "RelationParent.json", Databases.RelationParent)]
    [InlineData(Endpoints.Databases, "RelationChild.json", Databases.RelationChild)]
    public async Task Get(Endpoints endpoints, string fileName, string guid)
    {
        // arrange
        var path = GetPath(endpoints, fileName);
        var expectedJson = await File.ReadAllTextAsync(path);
        
        // act
        var page = await GetJson(endpoints, guid);
        
        // assert
        page
            .Should()
            .Be(expectedJson);
    }

    private async Task<string> GetJson(Endpoints endpoints, string guid)
    {
        var json = endpoints switch
        {
            Endpoints.Blocks => await Sut.GetBlockRawAsync(Guid.Parse(guid)),
            Endpoints.Pages => await Sut.GetPageRawAsync(Guid.Parse(guid)),
            Endpoints.RichTexts => await Sut.GetBlockRawAsync(Guid.Parse(guid)),
            Endpoints.Databases => await Sut.GetDatabaseRawAsync(Guid.Parse(guid)),
            _ => throw new ArgumentOutOfRangeException(nameof(endpoints), endpoints, null)
        };
        return json.Formatted();
    }
    
    private static string GetPath(Endpoints endpoints, string fileName)
    {
        return endpoints switch
        {
            Endpoints.Blocks => Path.Combine("Resources", "Blocks", fileName),
            Endpoints.Pages => Path.Combine("Resources", "Pages", fileName),
            Endpoints.RichTexts => Path.Combine("Resources", "RichTexts", fileName),
            Endpoints.Databases => Path.Combine("Resources", "Databases", fileName),
            _ => throw new ArgumentOutOfRangeException(nameof(endpoints), endpoints, null)
        };
    }
}