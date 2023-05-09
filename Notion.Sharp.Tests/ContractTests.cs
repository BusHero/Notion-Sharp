using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using Notion.Sharp.Tests.Utils;

namespace Notion.Sharp.Tests;

public enum Endpoints
{
    Pages,
    Blocks,
    RichTexts,
    Databases,
    DatabasePages,
}

[UsesVerify]
public class ContractTests : NotionTestsBase
{
    [ModuleInitializer]
    internal static void Initialize() =>
        VerifyDiffPlex.Initialize();
    
    [Theory]
    [InlineData(Endpoints.Pages, Pages.Page, "Pages.Page")]
    [InlineData(Endpoints.Pages, Pages.PageWithEmojiIcon, "Pages.PageWithEmojiIcon")]
    [InlineData(Endpoints.Pages, Pages.PageWithIcon, "Pages.PageWithIcon")]
    [InlineData(Endpoints.Pages, Pages.DeletedPage, "Pages.DeletedPage")]
    [InlineData(Endpoints.Pages, Pages.PageWithCustomLinkIcon, "Pages.PageWithCustomLinkIcon")]
    [InlineData(Endpoints.Pages, Pages.PageWithCover, "Pages.PageWithCover")]
    [InlineData(Endpoints.Pages, Pages.PageWithCustomLinkCover, "Pages.PageWithCustomLinkCover")]
    [InlineData(Endpoints.Pages, Pages.PageWithUnsplashCover, "Pages.PageWithUnsplashCover")]
    [InlineData(Endpoints.Pages, Pages.PageFromDatabase, "Pages.PageFromDatabase")]
    [InlineData(Endpoints.Pages, Pages.Parent, "Pages.Parent")]
    [InlineData(Endpoints.Blocks, Blocks.Heading1, "Blocks.Heading1")]
    [InlineData(Endpoints.Blocks, Blocks.Heading2, "Blocks.Heading2")]
    [InlineData(Endpoints.Blocks, Blocks.Heading3, "Blocks.Heading3")]
    [InlineData(Endpoints.Blocks, Blocks.Paragraph, "Blocks.Paragraph")]
    [InlineData(Endpoints.Blocks, Blocks.ToDoUnchecked, "Blocks.ToDoUnchecked")]
    [InlineData(Endpoints.Blocks, Blocks.ToDoChecked, "Blocks.ToDoChecked")]
    [InlineData(Endpoints.Blocks, Blocks.BulletedListItem, "Blocks.BulletedListItem")]
    [InlineData(Endpoints.Blocks, Blocks.NumberedListItem, "Blocks.NumberedListItem")]
    [InlineData(Endpoints.Blocks, Blocks.ToggleList, "Blocks.ToggleList")]
    [InlineData(Endpoints.Blocks, Blocks.Quote, "Blocks.Quote")]
    [InlineData(Endpoints.Blocks, Blocks.Divider, "Blocks.Divider")]
    [InlineData(Endpoints.Blocks, Blocks.Callout, "Blocks.Callout")]
    [InlineData(Endpoints.Blocks, Blocks.Table, "Blocks.Table")]
    [InlineData(Endpoints.Blocks, Blocks.Page, "Blocks.Page")]
    [InlineData(Endpoints.Blocks, Blocks.LinkToPage, "Blocks.LinkToPage")]
    [InlineData(Endpoints.Blocks, Blocks.ImageUnsplash, "Blocks.ImageUnsplash")]
    [InlineData(Endpoints.Blocks, Blocks.ImageUploaded, "Blocks.ImageUploaded")]
    [InlineData(Endpoints.Blocks, Blocks.ImageLink, "Blocks.ImageLink")]
    [InlineData(Endpoints.Blocks, Blocks.ImageEmpty, "Blocks.ImageEmpty")]
    [InlineData(Endpoints.Blocks, Blocks.WebBookmark, "Blocks.WebBookmark")]
    [InlineData(Endpoints.Blocks, Blocks.WebBookmarkEmpty, "Blocks.WebBookmarkEmpty")]
    [InlineData(Endpoints.Blocks, Blocks.Video, "Blocks.Video")]
    [InlineData(Endpoints.Blocks, Blocks.VideoEmpty, "Blocks.VideoEmpty")]
    [InlineData(Endpoints.Blocks, Blocks.AudioEmpty, "Blocks.AudioEmpty")]
    [InlineData(Endpoints.Blocks, Blocks.Code, "Blocks.Code")]
    [InlineData(Endpoints.Blocks, Blocks.CodeWithCaption, "Blocks.CodeWithCaption")]
    [InlineData(Endpoints.Blocks, Blocks.CodeCSharp, "Blocks.CodeCSharp")]
    [InlineData(Endpoints.Blocks, Blocks.File, "Blocks.File")]
    [InlineData(Endpoints.Blocks, Blocks.FileEmpty, "Blocks.FileEmpty")]
    [InlineData(Endpoints.Blocks, Blocks.FileWithCaption, "Blocks.FileWithCaption")]
    [InlineData(Endpoints.Blocks, Blocks.Audio, "Blocks.Audio")]
    [InlineData(Endpoints.Blocks, Blocks.AudioUploaded, "Blocks.AudioUploaded")]
    [InlineData(Endpoints.Blocks, Blocks.Equation, "Blocks.Equation")]
    [InlineData(Endpoints.Blocks, Blocks.TableOfContents, "Blocks.TableOfContents")]
    [InlineData(Endpoints.Blocks, Blocks.EquationEmpty, "Blocks.EquationEmpty")]
    [InlineData(Endpoints.Blocks, Blocks.Breadcrumb, "Blocks.Breadcrumb")]
    [InlineData(Endpoints.Blocks, Blocks.SyncBlockOriginal, "Blocks.SyncBlockOriginal")]
    [InlineData(Endpoints.Blocks, Blocks.SyncdBlockCopy, "Blocks.SyncdBlockCopy")]
    [InlineData(Endpoints.Blocks, Blocks.Button, "Blocks.Button")]
    [InlineData(Endpoints.Blocks, Blocks.ToggledHeading3, "Blocks.ToggledHeading3")]
    [InlineData(Endpoints.Blocks, Blocks.ToggledHeading2, "Blocks.ToggledHeading2")]
    [InlineData(Endpoints.Blocks, Blocks.ToggledHeading1, "Blocks.ToggledHeading1")]
    [InlineData(Endpoints.Blocks, Blocks.Column, "Blocks.Column")]
    [InlineData(Endpoints.Blocks, Blocks.ColumnList, "Blocks.ColumnList")]
    [InlineData(Endpoints.Blocks, Blocks.Embed, "Blocks.Embed")]
    [InlineData(Endpoints.Blocks, Blocks.TwitterEmbed, "Blocks.TwitterEmbed")]
    [InlineData(Endpoints.Blocks, Blocks.ChildDatabase, "Blocks.ChildDatabase")]
    [InlineData(Endpoints.Blocks, Blocks.LinkedDatabase, "Blocks.LinkedDatabase")]
    [InlineData(Endpoints.Blocks, Blocks.BlockWithChildren, "Blocks.BlockWithChildren")]
    [InlineData(Endpoints.Blocks, Blocks.ChildBlock, "Blocks.ChildBlock")]
    [InlineData(Endpoints.Blocks, Blocks.Pdf, "Blocks.Pdf")]
    [InlineData(Endpoints.Blocks, Blocks.PdfUploaded, "Blocks.PdfUploaded")]
    [InlineData(Endpoints.Blocks, Blocks.LinkPreview, "Blocks.LinkPreview")]
    [InlineData(Endpoints.RichTexts, RichTexts.SimpleText, "RichTexts.SimpleText")]
    [InlineData(Endpoints.RichTexts, RichTexts.MentionPerson, "RichTexts.MentionPerson")]
    [InlineData(Endpoints.RichTexts, RichTexts.MentionBlock, "RichTexts.MentionBlock")]
    [InlineData(Endpoints.RichTexts, RichTexts.MentionToday, "RichTexts.MentionToday")]
    [InlineData(Endpoints.RichTexts, RichTexts.MentionTomorrow, "RichTexts.MentionTomorrow")]
    [InlineData(Endpoints.RichTexts, RichTexts.LinkPage, "RichTexts.LinkPage")]
    [InlineData(Endpoints.RichTexts, RichTexts.LinkToWebsite, "RichTexts.LinkToWebsite")]
    [InlineData(Endpoints.RichTexts, RichTexts.Bold, "RichTexts.Bold")]
    [InlineData(Endpoints.RichTexts, RichTexts.Italic, "RichTexts.Italic")]
    [InlineData(Endpoints.RichTexts, RichTexts.Underline, "RichTexts.Underline")]
    [InlineData(Endpoints.RichTexts, RichTexts.StrikeThrough, "RichTexts.StrikeThrough")]
    [InlineData(Endpoints.RichTexts, RichTexts.Background, "RichTexts.Background")]
    [InlineData(Endpoints.RichTexts, RichTexts.Foreground, "RichTexts.Foreground")]
    [InlineData(Endpoints.RichTexts, RichTexts.Code, "RichTexts.Code")]
    [InlineData(Endpoints.RichTexts, RichTexts.Equation, "RichTexts.Equation")]
    [InlineData(Endpoints.RichTexts, RichTexts.BoldAndItalic, "RichTexts.BoldAndItalic")]
    [InlineData(Endpoints.RichTexts, RichTexts.BoldThenItalic, "RichTexts.BoldThenItalic")]
    [InlineData(Endpoints.Databases, Databases.Name, "Databases.Name")]
    [InlineData(Endpoints.Databases, Databases.Text, "Databases.Text")]
    [InlineData(Endpoints.Databases, Databases.Number, "Databases.Number")]
    [InlineData(Endpoints.Databases, Databases.Date, "Databases.Date")]
    [InlineData(Endpoints.Databases, Databases.Option, "Databases.Option")]
    [InlineData(Endpoints.Databases, Databases.MultiSelect, "Databases.MultiSelect")]
    [InlineData(Endpoints.Databases, Databases.Status, "Databases.Status")]
    [InlineData(Endpoints.Databases, Databases.Person, "Databases.Person")]
    [InlineData(Endpoints.Databases, Databases.Files, "Databases.Files")]
    [InlineData(Endpoints.Databases, Databases.Checkbox, "Databases.Checkbox")]
    [InlineData(Endpoints.Databases, Databases.Url, "Databases.Url")]
    [InlineData(Endpoints.Databases, Databases.Email, "Databases.Email")]
    [InlineData(Endpoints.Databases, Databases.Phone, "Databases.Phone")]
    [InlineData(Endpoints.Databases, Databases.Formula, "Databases.Formula")]
    [InlineData(Endpoints.Databases, Databases.CreatedTime, "Databases.CreatedTime")]
    [InlineData(Endpoints.Databases, Databases.CreatedBy, "Databases.CreatedBy")]
    [InlineData(Endpoints.Databases, Databases.LastEditedTime, "Databases.LastEditedTime")]
    [InlineData(Endpoints.Databases, Databases.LastEditedBy, "Databases.LastEditedBy")]
    [InlineData(Endpoints.Databases, Databases.RelationParent, "Databases.RelationParent")]
    [InlineData(Endpoints.Databases, Databases.RelationChild, "Databases.RelationChild")]
    [InlineData(Endpoints.Databases, Databases.DatabaseFullPage, "Databases.DatabaseFullPage")]
    [InlineData(Endpoints.Databases, Databases.DatabaseWithCover, "Databases.DatabaseWithCover")]
    [InlineData(Endpoints.Databases, Databases.DatabaseWithIcon, "Databases.DatabaseWithIcon")]
    [InlineData(Endpoints.DatabasePages, Pages.PageCheckBox, "Pages.PageCheckBox")]
    [InlineData(Endpoints.DatabasePages, Pages.PageCreatedBy, "Pages.PageCreatedBy")]
    [InlineData(Endpoints.DatabasePages, Pages.PageCreatedTime, "Pages.PageCreatedTime")]
    [InlineData(Endpoints.DatabasePages, Pages.PageDate, "Pages.PageDate")]
    [InlineData(Endpoints.DatabasePages, Pages.PageEmail, "Pages.PageEmail")]
    [InlineData(Endpoints.DatabasePages, Pages.PageFile, "Pages.PageFile")]
    [InlineData(Endpoints.DatabasePages, Pages.PageFormula, "Pages.PageFormula")]
    [InlineData(Endpoints.DatabasePages, Pages.PageLastEditedBy, "Pages.PageLastEditedBy")]
    [InlineData(Endpoints.DatabasePages, Pages.PageLastEditedTime, "Pages.PageLastEditedTime")]
    [InlineData(Endpoints.DatabasePages, Pages.PageMultiSelect, "Pages.PageMultiSelect")]
    [InlineData(Endpoints.DatabasePages, Pages.PageName, "Pages.PageName")]
    [InlineData(Endpoints.DatabasePages, Pages.PageNumber, "Pages.PageNumber")]
    [InlineData(Endpoints.DatabasePages, Pages.PageOption, "Pages.PageOption")]
    [InlineData(Endpoints.DatabasePages, Pages.PagePerson, "Pages.PagePerson")]
    [InlineData(Endpoints.DatabasePages, Pages.PagePhone, "Pages.PagePhone")]
    [InlineData(Endpoints.DatabasePages, Pages.PageRelationChild, "Pages.PageRelationChild")]
    [InlineData(Endpoints.DatabasePages, Pages.PageRelationParent, "Pages.PageRelationParent")]
    [InlineData(Endpoints.DatabasePages, Pages.PageStatus, "Pages.PageStatus")]
    [InlineData(Endpoints.DatabasePages, Pages.PageText, "Pages.PageText")]
    [InlineData(Endpoints.DatabasePages, Pages.PageUrl, "Pages.PageUrl")]
    public async Task Get(Endpoints endpoints, string guid, string name)
    {
        // arrange
        
        // act
        var json = await GetJson(endpoints, guid);
        var node = JsonNode.Parse(json);
        foreach (var property in IgnoreProperties)
        {
            node.IgnoreProperty(property);
        }
        json = JsonSerializer.Serialize(node);
        
        // assert
        await VerifyJson(json)
            .UseDirectory("verified")
            .UseParameters(endpoints, guid, name);
    }

    private async Task<string> GetJson(Endpoints endpoints, string guid)
    {
        var id = Guid.Parse(guid);
        var json = endpoints switch
        {
            Endpoints.Blocks => await Sut.GetBlockRawAsync(id),
            Endpoints.Pages => await Sut.GetPageRawAsync(id),
            Endpoints.RichTexts => await Sut.GetBlockRawAsync(id),
            Endpoints.Databases => await Sut.GetDatabaseRawAsync(id),
            Endpoints.DatabasePages => await Sut.GetPageRawAsync(id),
            _ => throw new ArgumentOutOfRangeException(nameof(endpoints), endpoints, null),
        };
        var jsonFormatted = json.Formatted();
        return jsonFormatted;
    }

    private string[] IgnoreProperties => new[]
    {
        "pdf.file.url",
        "pdf.file.expiry_time",
        "image.file.url",
        "image.file.expiry_time",
        "audio.file.url",
        "audio.file.expiry_time",
    };
}