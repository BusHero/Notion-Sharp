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
    [InlineData("Parent.json", Pages.Parent)]
    public async Task JsonPage(string fileName, string pageId)
    {
        // arrange
        var path = Path.Combine("Resources", "Pages", fileName);
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
    [InlineData("Page.json", Blocks.Page)]
    [InlineData("LinkToPage.json", Blocks.LinkToPage)]
    [InlineData("ImageUnsplash.json", Blocks.ImageUnsplash)]
    [InlineData("ImageLink.json", Blocks.ImageLink)]
    [InlineData("ImageEmpty.json", Blocks.ImageEmpty)]
    [InlineData("WebBookmark.json", Blocks.WebBookmark)]
    [InlineData("WebBookmarkEmpty.json", Blocks.WebBookmarkEmpty)]
    [InlineData("Video.json", Blocks.Video)]
    [InlineData("VideoEmpty.json", Blocks.VideoEmpty)]
    [InlineData("AudioEmpty.json", Blocks.AudioEmpty)]
    [InlineData("Code.json", Blocks.Code)]
    [InlineData("CodeWithCaption.json", Blocks.CodeWithCaption)]
    [InlineData("CodeCSharp.json", Blocks.CodeCSharp)]
    [InlineData("File.json", Blocks.File)]
    [InlineData("FileEmpty.json", Blocks.FileEmpty)]
    [InlineData("FileWithCaption.json", Blocks.FileWithCaption)]
    [InlineData("Audio.json", Blocks.Audio)]
    [InlineData("Equation.json", Blocks.Equation)]
    [InlineData("TableOfContents.json", Blocks.TableOfContents)]
    [InlineData("EquationEmpty.json", Blocks.EquationEmpty)]
    [InlineData("Breadcumb.json", Blocks.Breadcrumb)]
    [InlineData("SyncBlockOriginal.json", Blocks.SyncBlockOriginal)]
    [InlineData("SyncdBlockCopy.json", Blocks.SyncdBlockCopy)]
    [InlineData("Button.json", Blocks.Button)]
    [InlineData("ToggledHeading3.json", Blocks.ToggledHeading3)]
    [InlineData("ToggledHeading2.json", Blocks.ToggledHeading2)]
    [InlineData("ToggledHeading1.json", Blocks.ToggledHeading1)]
    [InlineData("Column.json", Blocks.Column)]
    [InlineData("Embed.json", Blocks.Embed)]
    [InlineData("TwitterEmbed.json", Blocks.TwitterEmbed)]
    [InlineData("ChildDatabase.json", Blocks.ChildDatabase)]
    [InlineData("LinkedDatabase.json", Blocks.LinkedDatabase)]
    [InlineData("BlockWithChildren.json", Blocks.BlockWithChildren)]
    [InlineData("ChildBlock.json", Blocks.ChildBlock)]
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
    
    [Theory]
    [InlineData("SimpleText.json", RichTexts.SimpleText)]
    [InlineData("MentionPerson.json", RichTexts.MentionPerson)]
    [InlineData("MentionBlock.json", RichTexts.MentionBlock)]
    [InlineData("MentionToday.json", RichTexts.MentionToday)]
    [InlineData("MentionTomorrow.json", RichTexts.MentionTomorrow)]
    [InlineData("LinkPage.json", RichTexts.LinkPage)]
    [InlineData("LinkToWebsite.json", RichTexts.LinkToWebsite)]
    [InlineData("Bold.json", RichTexts.Bold)]
    [InlineData("Italic.json", RichTexts.Italic)]
    [InlineData("Underline.json", RichTexts.Underline)]
    [InlineData("StrikeThrough.json", RichTexts.StrikeThrough)]
    [InlineData("Background.json", RichTexts.Background)]
    [InlineData("Foreground.json", RichTexts.Foreground)]
    [InlineData("Code.json", RichTexts.Code)]
    [InlineData("Equation.json", RichTexts.Equation)]
    [InlineData("BoldAndItalic.json", RichTexts.BoldAndItalic)]
    [InlineData("BoldThenItalic.json", RichTexts.BoldThenItalic)]
    public async Task JsonRichText(string fileName, string pageId)
    {
        // arrange
        var path = Path.Combine("Resources", "RichTexts", fileName);
        var expectedJson = await File.ReadAllTextAsync(path);
        
        // act
        var block = (await SUT.GetBlockRawAsync(Guid.Parse(pageId))).Formatted();
        
        // assert
        block
            .Should()
            .Be(expectedJson);
    }
}