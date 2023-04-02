using FluentAssertions.Execution;
using Notion.Sharp.Tests.Utils;
using File = Notion.Model.File;
using Users = Notion.Sharp.Tests.Utils.Users;

namespace Notion.Sharp.Tests;

public class BlockTests: NotionTestsBase
{
    [Fact]
    public async Task GetParagraph()
    {
        // arrange
        
        // act
        var block = await SUT.GetBlockAsync(Blocks.Paragraph.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var paragraph = block as Block.Paragraph;
            paragraph.Should().NotBeNull();
            paragraph?.Id.Should().Be(Blocks.Paragraph);
            paragraph?.CreatedTime.Should().Be(DateTime.Parse("2023-03-26T19:27:00.000Z"));
            paragraph?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-27T05:51:00.000Z"));
            paragraph?.Archived.Should().BeFalse();
            paragraph?.HasChildren.Should().BeFalse();
            paragraph?.Color.Should().Be(Color.Default);
            paragraph?.Text.Should().ContainSingle();
            
            var richText = paragraph?.Text[0] as RichText.Text;
            richText.Should().NotBeNull();
            richText?.Content.Should().Be("Paragraph");
            richText?.Link.Should().BeNull();
            richText?.PlainText.Should().Be("Paragraph");
            richText?.Annotations.Bold.Should().BeFalse();
            richText?.Annotations.Italic.Should().BeFalse();
            richText?.Annotations.Underline.Should().BeFalse();
            richText?.Annotations.Strikethrough.Should().BeFalse();
            richText?.Annotations.Code.Should().BeFalse();
            richText?.Annotations.Color.Should().Be(Color.Default);
            
            var parent = paragraph?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            paragraph?.LastEditedBy?.Id.Should().Be(Users.Me);
            paragraph?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }
    
    [Fact]
    public async Task GetToggle()
    {
        // arrange
        
        // act
        var block = await SUT.GetBlockAsync(Blocks.ToggleList.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var toggle = block as Block.Toggle;
            toggle.Should().NotBeNull();
            toggle?.Id.Should().Be(Blocks.ToggleList);
            toggle?.CreatedTime.Should().Be(DateTime.Parse("2023-03-26T20:00:00.000Z"));
            toggle?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-26T20:00:00.000Z"));
            toggle?.Archived.Should().BeFalse();
            toggle?.HasChildren.Should().BeFalse();
            toggle?.Color.Should().Be(Color.Default);
            toggle?.Text.Should().ContainSingle();
            
            var richText = toggle?.Text[0] as RichText.Text;
            richText.Should().NotBeNull();
            richText?.Content.Should().Be("Toggle list");
            richText?.Link.Should().BeNull();
            richText?.PlainText.Should().Be("Toggle list");
            richText?.Annotations.Bold.Should().BeFalse();
            richText?.Annotations.Italic.Should().BeFalse();
            richText?.Annotations.Underline.Should().BeFalse();
            richText?.Annotations.Strikethrough.Should().BeFalse();
            richText?.Annotations.Code.Should().BeFalse();
            richText?.Annotations.Color.Should().Be(Color.Default);
            
            var parent = toggle?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            toggle?.LastEditedBy?.Id.Should().Be(Users.Me);
            toggle?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }
    
    [Fact]
    public async Task GetToDo()
    {
        // arrange
        
        // act
        var block = await SUT.GetBlockAsync(Blocks.ToDoChecked.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var todo = block as Block.ToDo;
            todo.Should().NotBeNull();
            todo?.Id.Should().Be(Blocks.ToDoChecked);
            todo?.CreatedTime.Should().Be(DateTime.Parse("2023-03-26T19:29:00.000Z"));
            todo?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-26T19:30:00.000Z"));
            todo?.Archived.Should().BeFalse();
            todo?.HasChildren.Should().BeFalse();
            todo?.Color.Should().Be(Color.Default);
            todo?.Text.Should().ContainSingle();
            todo?.Checked.Should().BeTrue();
            
            var richText = todo?.Text[0] as RichText.Text;
            richText.Should().NotBeNull();
            richText?.Content.Should().Be("ToDo Checked");
            richText?.Link.Should().BeNull();
            richText?.PlainText.Should().Be("ToDo Checked");
            richText?.Annotations.Bold.Should().BeFalse();
            richText?.Annotations.Italic.Should().BeFalse();
            richText?.Annotations.Underline.Should().BeFalse();
            richText?.Annotations.Strikethrough.Should().BeFalse();
            richText?.Annotations.Code.Should().BeFalse();
            richText?.Annotations.Color.Should().Be(Color.Default);
            
            var parent = todo?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            todo?.LastEditedBy?.Id.Should().Be(Users.Me);
            todo?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }
    
    [Fact]
    public async Task GetCallout()
    {
        // arrange
        
        // act
        var block = await SUT.GetBlockAsync(Blocks.Callout.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var callout = block as Block.Callout;
            callout.Should().NotBeNull();
            callout?.Id.Should().Be(Blocks.Callout);
            callout?.CreatedTime.Should().Be(DateTime.Parse("2023-03-26T20:01:00.000Z"));
            callout?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-26T20:01:00.000Z"));
            callout?.Archived.Should().BeFalse();
            callout?.HasChildren.Should().BeFalse();
            callout?.Color.Should().Be(Color.GrayBackground);
            callout?.Text.Should().ContainSingle();
            
            var richText = callout?.Text[0] as RichText.Text;
            richText.Should().NotBeNull();
            richText?.Content.Should().Be("Callout");
            richText?.Link.Should().BeNull();
            richText?.PlainText.Should().Be("Callout");
            richText?.Annotations.Bold.Should().BeFalse();
            richText?.Annotations.Italic.Should().BeFalse();
            richText?.Annotations.Underline.Should().BeFalse();
            richText?.Annotations.Strikethrough.Should().BeFalse();
            richText?.Annotations.Code.Should().BeFalse();
            richText?.Annotations.Color.Should().Be(Color.Default);

            var icon = callout?.Icon;
            icon?.Value.Should().Be("💡");
            
            var parent = callout?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            callout?.LastEditedBy?.Id.Should().Be(Users.Me);
            callout?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }
    
    [Fact]
    public async Task GetBookmark()
    {
        // arrange
        
        // act
        var block = await SUT.GetBlockAsync(Blocks.WebBookmark.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var bookmark = block as Block.Bookmark;
            bookmark.Should().NotBeNull();
            bookmark?.Id.Should().Be(Blocks.WebBookmark);
            bookmark?.CreatedTime.Should().Be(DateTime.Parse("2023-03-27T05:57:00.000Z"));
            bookmark?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-27T05:57:00.000Z"));
            bookmark?.Archived.Should().BeFalse();
            bookmark?.HasChildren.Should().BeFalse();
            bookmark?.Caption.Should().BeEmpty();
            bookmark?.Url.Should().Be("https://www.google.com/");
            
            var parent = bookmark?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            bookmark?.LastEditedBy?.Id.Should().Be(Users.Me);
            bookmark?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }
    
    [Fact]
    public async Task GetQuote()
    {
        // arrange
        
        // act
        var block = await SUT.GetBlockAsync(Blocks.Quote.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var quote = block as Block.Quote;
            quote.Should().NotBeNull();
            quote?.Id.Should().Be(Blocks.Quote);
            quote?.CreatedTime.Should().Be(DateTime.Parse("2023-03-26T20:00:00.000Z"));
            quote?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-26T20:00:00.000Z"));
            quote?.Archived.Should().BeFalse();
            quote?.HasChildren.Should().BeFalse();
            quote?.Color.Should().Be(Color.Default);
            quote?.Text.Should().ContainSingle();
            
            var richText = quote?.Text[0] as RichText.Text;
            richText.Should().NotBeNull();
            richText?.Content.Should().Be("Quote");
            richText?.Link.Should().BeNull();
            richText?.PlainText.Should().Be("Quote");
            richText?.Annotations.Bold.Should().BeFalse();
            richText?.Annotations.Italic.Should().BeFalse();
            richText?.Annotations.Underline.Should().BeFalse();
            richText?.Annotations.Strikethrough.Should().BeFalse();
            richText?.Annotations.Code.Should().BeFalse();
            richText?.Annotations.Color.Should().Be(Color.Default);
            
            var parent = quote?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            quote?.LastEditedBy?.Id.Should().Be(Users.Me);
            quote?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }
    
    [Fact]
    public async Task GetEquation()
    {
        // arrange
        
        // act
        var block = await SUT.GetBlockAsync(Blocks.Equation.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var equation = block as Block.Equation;
            equation.Should().NotBeNull();
            equation?.Id.Should().Be(Blocks.Equation);
            equation?.CreatedTime.Should().Be(DateTime.Parse("2023-03-27T16:24:00.000Z"));
            equation?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-27T16:24:00.000Z"));
            equation?.Archived.Should().BeFalse();
            equation?.HasChildren.Should().BeFalse();
            equation?.Expression.Should().Be("1 + 1");
            
            var parent = equation?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            equation?.LastEditedBy?.Id.Should().Be(Users.Me);
            equation?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }
    
    [Fact]
    public async Task GetCode()
    {
        // arrange
        
        // act
        var block = await SUT.GetBlockAsync(Blocks.Code.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var code = block as Block.Code;
            code.Should().NotBeNull();
            code?.Id.Should().Be(Blocks.Code);
            code?.CreatedTime.Should().Be(DateTime.Parse("2023-03-27T06:01:00.000Z"));
            code?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-27T06:01:00.000Z"));
            code?.Archived.Should().BeFalse();
            code?.HasChildren.Should().BeFalse();
            code?.Text.Should().ContainSingle();
            code?.Language.Should().Be("javascript");
            
            var richText = code?.Text![0] as RichText.Text;
            richText.Should().NotBeNull();
            richText?.Content.Should().Be("Some Code here and there");
            richText?.Link.Should().BeNull();
            richText?.PlainText.Should().Be("Some Code here and there");
            richText?.Annotations.Bold.Should().BeFalse();
            richText?.Annotations.Italic.Should().BeFalse();
            richText?.Annotations.Underline.Should().BeFalse();
            richText?.Annotations.Strikethrough.Should().BeFalse();
            richText?.Annotations.Code.Should().BeFalse();
            richText?.Annotations.Color.Should().Be(Color.Default);
            
            var parent = code?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            code?.LastEditedBy?.Id.Should().Be(Users.Me);
            code?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }
    
    [Fact]
    public async Task GetBulletedListItem()
    {
        // arrange
        
        // act
        var block = await SUT.GetBlockAsync(Blocks.BulletedListItem.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var bulletedListItem = block as Block.BulletedListItem;
            bulletedListItem.Should().NotBeNull();
            bulletedListItem?.Id.Should().Be(Blocks.BulletedListItem);
            bulletedListItem?.CreatedTime.Should().Be(DateTime.Parse("2023-03-26T19:58:00.000Z"));
            bulletedListItem?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-26T19:58:00.000Z"));
            bulletedListItem?.Archived.Should().BeFalse();
            bulletedListItem?.HasChildren.Should().BeFalse();
            bulletedListItem?.Color.Should().Be(Color.Default);
            bulletedListItem?.Text.Should().ContainSingle();
            
            var richText = bulletedListItem?.Text[0] as RichText.Text;
            richText.Should().NotBeNull();
            richText?.Content.Should().Be("Bulleted list item");
            richText?.Link.Should().BeNull();
            richText?.PlainText.Should().Be("Bulleted list item");
            richText?.Annotations.Bold.Should().BeFalse();
            richText?.Annotations.Italic.Should().BeFalse();
            richText?.Annotations.Underline.Should().BeFalse();
            richText?.Annotations.Strikethrough.Should().BeFalse();
            richText?.Annotations.Code.Should().BeFalse();
            richText?.Annotations.Color.Should().Be(Color.Default);
            
            var parent = bulletedListItem?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            bulletedListItem?.LastEditedBy?.Id.Should().Be(Users.Me);
            bulletedListItem?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }

    [Fact]
    public async Task GetNumberedListItem()
    {
        // arrange
        
        // act
        var block = await SUT.GetBlockAsync(Blocks.NumberedListItem.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var numberedListItem = block as Block.NumberedListItem;
            numberedListItem.Should().NotBeNull();
            numberedListItem?.Id.Should().Be(Blocks.NumberedListItem);
            numberedListItem?.CreatedTime.Should().Be(DateTime.Parse("2023-03-26T19:58:00.000Z"));
            numberedListItem?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-26T19:58:00.000Z"));
            numberedListItem?.Archived.Should().BeFalse();
            numberedListItem?.HasChildren.Should().BeFalse();
            numberedListItem?.Color.Should().Be(Color.Default);
            numberedListItem?.Text.Should().ContainSingle();
            
            var richText = numberedListItem?.Text[0] as RichText.Text;
            richText.Should().NotBeNull();
            richText?.Content.Should().Be("Numbered list item");
            richText?.Link.Should().BeNull();
            richText?.PlainText.Should().Be("Numbered list item");
            richText?.Annotations.Bold.Should().BeFalse();
            richText?.Annotations.Italic.Should().BeFalse();
            richText?.Annotations.Underline.Should().BeFalse();
            richText?.Annotations.Strikethrough.Should().BeFalse();
            richText?.Annotations.Code.Should().BeFalse();
            richText?.Annotations.Color.Should().Be(Color.Default);
            
            var parent = numberedListItem?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            numberedListItem?.LastEditedBy?.Id.Should().Be(Users.Me);
            numberedListItem?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }
    
    [Fact]
    public async Task GetHeading1()
    {
        // arrange
        
        // act
        var block = await SUT.GetBlockAsync(Blocks.Heading1.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var heading1 = block as Block.Heading1;
            heading1.Should().NotBeNull();
            heading1?.Id.Should().Be(Blocks.Heading1);
            heading1?.CreatedTime.Should().Be(DateTime.Parse("2023-03-26T19:14:00.000Z"));
            heading1?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-26T19:14:00.000Z"));
            heading1?.Archived.Should().BeFalse();
            heading1?.HasChildren.Should().BeFalse();
            heading1?.Color.Should().Be(Color.Default);
            heading1?.Text.Should().ContainSingle();
            heading1?.IsToggable.Should().BeFalse();
            
            var richText = heading1?.Text[0] as RichText.Text;
            richText.Should().NotBeNull();
            richText?.Content.Should().Be("Heading 1");
            richText?.Link.Should().BeNull();
            richText?.PlainText.Should().Be("Heading 1");
            richText?.Annotations.Bold.Should().BeFalse();
            richText?.Annotations.Italic.Should().BeFalse();
            richText?.Annotations.Underline.Should().BeFalse();
            richText?.Annotations.Strikethrough.Should().BeFalse();
            richText?.Annotations.Code.Should().BeFalse();
            richText?.Annotations.Color.Should().Be(Color.Default);

            var parent = heading1?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            heading1?.LastEditedBy?.Id.Should().Be(Users.Me);
            heading1?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }

    [Fact]
    public async Task GetHeading2()
    {
        // arrange
        
        // act
        var block = await SUT.GetBlockAsync(Blocks.Heading2.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var heading2 = block as Block.Heading2;
            heading2.Should().NotBeNull();
            heading2?.Id.Should().Be(Blocks.Heading2);
            heading2?.CreatedTime.Should().Be(DateTime.Parse("2023-03-26T19:18:00.000Z"));
            heading2?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-26T19:19:00.000Z"));
            heading2?.Archived.Should().BeFalse();
            heading2?.HasChildren.Should().BeFalse();
            heading2?.Color.Should().Be(Color.Default);
            heading2?.Text.Should().ContainSingle();
            heading2?.IsToggable.Should().BeFalse();
            
            var richText = heading2?.Text[0] as RichText.Text;
            richText.Should().NotBeNull();
            richText?.Content.Should().Be("Heading 2");
            richText?.Link.Should().BeNull();
            richText?.PlainText.Should().Be("Heading 2");
            richText?.Annotations.Bold.Should().BeFalse();
            richText?.Annotations.Italic.Should().BeFalse();
            richText?.Annotations.Underline.Should().BeFalse();
            richText?.Annotations.Strikethrough.Should().BeFalse();
            richText?.Annotations.Code.Should().BeFalse();
            richText?.Annotations.Color.Should().Be(Color.Default);

            var parent = heading2?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            heading2?.LastEditedBy?.Id.Should().Be(Users.Me);
            heading2?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }

    [Fact]
    public async Task GetHeading3()
    {
        // arrange
        
        // act
        var block = await SUT.GetBlockAsync(Blocks.Heading3.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var heading1 = block as Block.Heading3;
            heading1.Should().NotBeNull();
            heading1?.Id.Should().Be(Blocks.Heading3);
            heading1?.CreatedTime.Should().Be(DateTime.Parse("2023-03-26T19:19:00.000Z"));
            heading1?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-26T19:19:00.000Z"));
            heading1?.Archived.Should().BeFalse();
            heading1?.HasChildren.Should().BeFalse();
            heading1?.Color.Should().Be(Color.Default);
            heading1?.Text.Should().ContainSingle();
            heading1?.IsToggable.Should().BeFalse();
            
            var richText = heading1?.Text[0] as RichText.Text;
            richText.Should().NotBeNull();
            richText?.Content.Should().Be("Heading 3");
            richText?.Link.Should().BeNull();
            richText?.PlainText.Should().Be("Heading 3");
            richText?.Annotations.Bold.Should().BeFalse();
            richText?.Annotations.Italic.Should().BeFalse();
            richText?.Annotations.Underline.Should().BeFalse();
            richText?.Annotations.Strikethrough.Should().BeFalse();
            richText?.Annotations.Code.Should().BeFalse();
            richText?.Annotations.Color.Should().Be(Color.Default);

            var parent = heading1?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            heading1?.LastEditedBy?.Id.Should().Be(Users.Me);
            heading1?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }
    
    [Fact]
    public async Task GetToggledHeading1()
    {
        // arrange
        
        // act
        var block = await SUT.GetBlockAsync(Blocks.ToggledHeading1.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var heading1 = block as Block.Heading1;
            heading1.Should().NotBeNull();
            heading1?.Id.Should().Be(Blocks.ToggledHeading1);
            heading1?.CreatedTime.Should().Be(DateTime.Parse("2023-03-27T16:31:00.000Z"));
            heading1?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-27T16:32:00.000Z"));
            heading1?.Archived.Should().BeFalse();
            heading1?.HasChildren.Should().BeFalse();
            heading1?.Color.Should().Be(Color.Default);
            heading1?.Text.Should().ContainSingle();
            heading1?.IsToggable.Should().BeTrue();
            
            var richText = heading1?.Text[0] as RichText.Text;
            richText.Should().NotBeNull();
            richText?.Content.Should().Be("Toggled Heading 1");
            richText?.Link.Should().BeNull();
            richText?.PlainText.Should().Be("Toggled Heading 1");
            richText?.Annotations.Bold.Should().BeFalse();
            richText?.Annotations.Italic.Should().BeFalse();
            richText?.Annotations.Underline.Should().BeFalse();
            richText?.Annotations.Strikethrough.Should().BeFalse();
            richText?.Annotations.Code.Should().BeFalse();
            richText?.Annotations.Color.Should().Be(Color.Default);

            var parent = heading1?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            heading1?.LastEditedBy?.Id.Should().Be(Users.Me);
            heading1?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }
    
    [Fact]
    public async Task GetAudio()
    {
        // arrange
        
        // act
        var block = await SUT.GetBlockAsync(Blocks.Audio.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var audio = block as Block.Audio;
            audio.Should().NotBeNull();
            audio?.Id.Should().Be(Blocks.Audio);
            audio?.CreatedTime.Should().Be(DateTime.Parse("2023-03-27T16:16:00.000Z"));
            audio?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-27T16:17:00.000Z"));
            audio?.Archived.Should().BeFalse();
            audio?.HasChildren.Should().BeFalse();

            var file = audio?.File as File.External;
            file.Should().NotBeNull();
            file?.Caption.Should().BeNullOrEmpty();
            file?.Uri.Should().Be("https://file-examples.com/storage/feb401d325641db2fa1dfe7/2017/11/file_example_MP3_700KB.mp3");

            var parent = audio?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            audio?.LastEditedBy?.Id.Should().Be(Users.Me);
            audio?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }
    
    [Fact]
    public async Task GetVideo()
    {
        // arrange
        
        // act
        var block = await SUT.GetBlockAsync(Blocks.Video.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var video = block as Block.Video;
            video.Should().NotBeNull();
            video?.Id.Should().Be(Blocks.Video);
            video?.CreatedTime.Should().Be(DateTime.Parse("2023-03-27T05:58:00.000Z"));
            video?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-27T05:59:00.000Z"));
            video?.Archived.Should().BeFalse();
            video?.HasChildren.Should().BeFalse();

            var file = video?.File as File.External;
            file.Should().NotBeNull();
            file?.Caption.Should().BeNullOrEmpty();
            file?.Uri.Should().Be("https://www.youtube.com/watch?v=Qs50Pbtm8ts");

            var parent = video?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            video?.LastEditedBy?.Id.Should().Be(Users.Me);
            video?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }
    
    [Fact]
    public async Task GetVideoUploaded()
    {
        // arrange
        
        // act
        var block = await SUT.GetBlockAsync(Blocks.VideoUploaded.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var video = block as Block.Video;
            video.Should().NotBeNull();
            video?.Id.Should().Be(Blocks.VideoUploaded);
            video?.CreatedTime.Should().Be(DateTime.Parse("2023-03-27T16:19:00.000Z"));
            video?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-27T16:20:00.000Z"));
            video?.Archived.Should().BeFalse();
            video?.HasChildren.Should().BeFalse();

            var file = video?.File as File.Internal;
            file.Should().NotBeNull();
            file?.Caption.Should().BeNullOrEmpty();
            file?.Uri.Should().NotBeNull();
            file?.ExpireTime.Should().BeSameDateAs(DateTime.Today);

            var parent = video?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            video?.LastEditedBy?.Id.Should().Be(Users.Me);
            video?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }
    
    [Fact]
    public async Task GetFileWithCaption()
    {
        // arrange
        
        // act
        var block = await SUT.GetBlockAsync(Blocks.FileWithCaption.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var fileBlock = block as Block.FileBlock;
            fileBlock.Should().NotBeNull();
            fileBlock?.Id.Should().Be(Blocks.FileWithCaption);
            fileBlock?.CreatedTime.Should().Be(DateTime.Parse("2023-03-27T06:07:00.000Z"));
            fileBlock?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-27T16:24:00.000Z"));
            fileBlock?.Archived.Should().BeFalse();
            fileBlock?.HasChildren.Should().BeFalse();
            
            var file = fileBlock?.File as File.External;
            file.Should().NotBeNull();
            file?.Uri.Should().Be("http://www.africau.edu/images/default/sample.pdf");
            
            file?.Caption.Should().ContainSingle();
            var richText = file?.Caption[0] as RichText.Text;
            richText.Should().NotBeNull();
            richText?.Content.Should().Be("File with caption");
            richText?.Link.Should().BeNull();
            richText?.PlainText.Should().Be("File with caption");
            richText?.Annotations.Bold.Should().BeFalse();
            richText?.Annotations.Italic.Should().BeFalse();
            richText?.Annotations.Underline.Should().BeFalse();
            richText?.Annotations.Strikethrough.Should().BeFalse();
            richText?.Annotations.Code.Should().BeFalse();
            richText?.Annotations.Color.Should().Be(Color.Default);

            var parent = fileBlock?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            fileBlock?.LastEditedBy?.Id.Should().Be(Users.Me);
            fileBlock?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }

    [Fact]
    public async Task GetEmbed()
    {
        // arrange
        
        // act
        var block = await SUT.GetBlockAsync(Blocks.Embed.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var embed = block as Block.Embed;
            embed.Should().NotBeNull();
            embed?.Id.Should().Be(Blocks.Embed);
            embed?.CreatedTime.Should().Be(DateTime.Parse("2023-03-27T16:38:00.000Z"));
            embed?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-27T16:38:00.000Z"));
            embed?.Archived.Should().BeFalse();
            embed?.HasChildren.Should().BeFalse();
            embed?.Url.Should().Be("https://boards.greenhouse.io/notion/jobs/4750859003");
            
            var parent = embed?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            embed?.LastEditedBy?.Id.Should().Be(Users.Me);
            embed?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }
    
    [Fact]
    public async Task GetBreadcumb()
    {
        // arrange
        
        // act
        var block = await SUT.GetBlockAsync(Blocks.Breadcrumb.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var breadcrumb = block as Block.Breadcrumb;
            breadcrumb.Should().NotBeNull();
            breadcrumb?.Id.Should().Be(Blocks.Breadcrumb);
            breadcrumb?.CreatedTime.Should().Be(DateTime.Parse("2023-03-27T16:26:00.000Z"));
            breadcrumb?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-27T16:26:00.000Z"));
            breadcrumb?.Archived.Should().BeFalse();
            breadcrumb?.HasChildren.Should().BeFalse();
            
            var parent = breadcrumb?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            breadcrumb?.LastEditedBy?.Id.Should().Be(Users.Me);
            breadcrumb?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }
    
    [Fact]
    public async Task GetDivider()
    {
        // arrange
        
        // act
        var block = await SUT.GetBlockAsync(Blocks.Divider.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var divider = block as Block.Divider;
            divider.Should().NotBeNull();
            divider?.Id.Should().Be(Blocks.Divider);
            divider?.CreatedTime.Should().Be(DateTime.Parse("2023-03-26T20:01:00.000Z"));
            divider?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-26T20:01:00.000Z"));
            divider?.Archived.Should().BeFalse();
            divider?.HasChildren.Should().BeFalse();
            
            var parent = divider?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            divider?.LastEditedBy?.Id.Should().Be(Users.Me);
            divider?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }
    
    [Fact]
    public async Task GetColumnList()
    {
        // arrange
        
        // act
        var block = await SUT.GetBlockAsync(Blocks.ColumnList.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var columnList = block as Block.ColumnList;
            columnList.Should().NotBeNull();
            columnList?.Id.Should().Be(Blocks.ColumnList);
            columnList?.CreatedTime.Should().Be(DateTime.Parse("2023-03-27T16:34:00.000Z"));
            columnList?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-27T16:34:00.000Z"));
            columnList?.Archived.Should().BeFalse();
            columnList?.HasChildren.Should().BeTrue();
            
            var parent = columnList?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            columnList?.LastEditedBy?.Id.Should().Be(Users.Me);
            columnList?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }
    
    [Fact]
    public async Task GetColumn()
    {
        // arrange
        
        // act
        var block = await SUT.GetBlockAsync(Blocks.Column.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var column = block as Block.Column;
            column.Should().NotBeNull();
            column?.Id.Should().Be(Blocks.Column);
            column?.CreatedTime.Should().Be(DateTime.Parse("2023-03-27T16:34:00.000Z"));
            column?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-27T16:36:00.000Z"));
            column?.Archived.Should().BeFalse();
            column?.HasChildren.Should().BeTrue();
            
            var parent = column?.Parent as Parent.Block;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Blocks.ColumnList);

            column?.LastEditedBy?.Id.Should().Be(Users.Me);
            column?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }
    
    [Fact]
    public async Task GetTableOfContents()
    {
        // arrange
        
        // act
        var block = await SUT.GetBlockAsync(Blocks.TableOfContents.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var tableOfContents = block as Block.TableOfContents;
            tableOfContents.Should().NotBeNull();
            tableOfContents?.Id.Should().Be(Blocks.TableOfContents);
            tableOfContents?.CreatedTime.Should().Be(DateTime.Parse("2023-03-27T16:23:00.000Z"));
            tableOfContents?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-27T16:23:00.000Z"));
            tableOfContents?.Archived.Should().BeFalse();
            tableOfContents?.HasChildren.Should().BeFalse();
            tableOfContents?.Color.Should().Be(Color.Default);
            
            var parent = tableOfContents?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            tableOfContents?.LastEditedBy?.Id.Should().Be(Users.Me);
            tableOfContents?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }
    
    [Fact]
    public async Task GetButton()
    {
        // arrange
        
        // act
        var block = await SUT.GetBlockAsync(Blocks.Button.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var unsupported = block as Block.Unsupported;
            unsupported.Should().NotBeNull();
            unsupported?.Id.Should().Be(Blocks.Button);
            unsupported?.CreatedTime.Should().Be(DateTime.Parse("2023-03-27T16:30:00.000Z"));
            unsupported?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-27T16:30:00.000Z"));
            unsupported?.Archived.Should().BeFalse();
            unsupported?.HasChildren.Should().BeFalse();
            
            var parent = unsupported?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            unsupported?.LastEditedBy?.Id.Should().Be(Users.Me);
            unsupported?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }
    
    [Fact]
    public async Task GetPdf()
    {
        // arrange
        
        // act
        var block = await SUT.GetBlockAsync(Blocks.Pdf.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var pdf = block as Block.Pdf;
            pdf.Should().NotBeNull();
            pdf?.Id.Should().Be(Blocks.Pdf);
            pdf?.CreatedTime.Should().Be(DateTime.Parse("2023-03-29T16:29:00.000Z"));
            pdf?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-29T16:30:00.000Z"));
            pdf?.Archived.Should().BeFalse();
            pdf?.HasChildren.Should().BeFalse();

            var file = pdf?.File as File.External;
            file.Should().NotBeNull();
            file?.Caption.Should().BeNullOrEmpty();
            file?.Uri.Should().Be("https://www.africau.edu/images/default/sample.pdf");

            var parent = pdf?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            pdf?.LastEditedBy?.Id.Should().Be(Users.Me);
            pdf?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }
    
    [Fact]
    public async Task GetImage()
    {
        // arrange
        
        // act
        var block = await SUT.GetBlockAsync(Blocks.ImageLink.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var image = block as Block.Image;
            image.Should().NotBeNull();
            image?.Id.Should().Be(Blocks.ImageLink);
            image?.CreatedTime.Should().Be(DateTime.Parse("2023-03-27T05:49:00.000Z"));
            image?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-27T05:49:00.000Z"));
            image?.Archived.Should().BeFalse();
            image?.HasChildren.Should().BeFalse();

            var file = image?.File as File.External;
            file.Should().NotBeNull();
            file?.Caption.Should().BeNullOrEmpty();
            file?.Uri.Should().Be("https://www.google.com/images/branding/googlelogo/1x/googlelogo_light_color_272x92dp.png");

            var parent = image?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            image?.LastEditedBy?.Id.Should().Be(Users.Me);
            image?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }
    
    [Fact]
    public async Task GetPage()
    {
        // arrange
        
        // act
        var block = await SUT.GetBlockAsync(Blocks.Page.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var childPage = block as Block.ChildPage;
            childPage.Should().NotBeNull();
            childPage?.Id.Should().Be(Blocks.Page);
            childPage?.CreatedTime.Should().Be(DateTime.Parse("2023-03-26T20:01:00.000Z"));
            childPage?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-27T05:46:00.000Z"));
            childPage?.Archived.Should().BeFalse();
            childPage?.HasChildren.Should().BeTrue();
            childPage?.Title.Should().Be("Page");

            var parent = childPage?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            childPage?.LastEditedBy?.Id.Should().Be(Users.Me);
            childPage?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }
    
    [Fact]
    public async Task GetDatabase()
    {
        // arrange
        
        // act
        var block = await SUT.GetBlockAsync(Blocks.LinkedDatabase.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var childDatabase = block as Block.ChildDatabase;
            childDatabase.Should().NotBeNull();
            childDatabase?.Id.Should().Be(Blocks.LinkedDatabase);
            childDatabase?.CreatedTime.Should().Be(DateTime.Parse("2023-03-27T16:43:00.000Z"));
            childDatabase?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-27T16:43:00.000Z"));
            childDatabase?.Archived.Should().BeFalse();
            childDatabase?.HasChildren.Should().BeFalse();
            childDatabase?.Title.Should().Be("");

            var parent = childDatabase?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            childDatabase?.LastEditedBy?.Id.Should().Be(Users.Me);
            childDatabase?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }
    
    [Fact]
    public async Task GetTable()
    {
        // arrange
        
        // act
        var block = await SUT.GetBlockAsync(Blocks.Table.ToGuid());
        
        // assert
        using (new AssertionScope())
        {
            var table = block as Block.Table;
            table.Should().NotBeNull();
            table?.Id.Should().Be(Blocks.Table);
            table?.CreatedTime.Should().Be(DateTime.Parse("2023-03-26T20:01:00.000Z"));
            table?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-26T20:06:00.000Z"));
            table?.Archived.Should().BeFalse();
            table?.HasChildren.Should().BeTrue();
            table?.TableWidth.Should().Be(2);
            table?.HasColumnHeader.Should().BeFalse();
            table?.HasRowHeader.Should().BeFalse();
            
            var parent = table?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            table?.LastEditedBy?.Id.Should().Be(Users.Me);
            table?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }

    [Fact]
    public async Task GetLinkPreview()
    {
        // arrange

        // act
        var block = await SUT.GetBlockAsync(Blocks.LinkPreview.ToGuid());

        // assert
        using (new AssertionScope())
        {
            var table = block as Block.LinkPreview;
            table.Should().NotBeNull();
            table?.Id.Should().Be(Blocks.LinkPreview);
            table?.CreatedTime.Should().Be(DateTime.Parse("2023-03-31T05:50:00.000Z"));
            table?.LastEditedTime.Should().Be(DateTime.Parse("2023-04-01T22:35:00.000Z"));
            table?.Archived.Should().BeFalse();
            table?.HasChildren.Should().BeFalse();
            table?.Url.Should().Be("https://github.com/BusHero/Notion-Sharp");

            var parent = table?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            table?.LastEditedBy?.Id.Should().Be(Guid.Parse("6dc47f86-cc8e-4afb-9163-2b43ea363b93"));
            table?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }

    [Fact]
    public async Task GetSyncdBlock()
    {
        // arrange

        // act
        var block = await SUT.GetBlockAsync(Blocks.SyncdBlockCopy.ToGuid());

        // assert
        using (new AssertionScope())
        {
            var syncBlock = block as Block.SyncBlock;
            syncBlock.Should().NotBeNull();
            syncBlock?.Id.Should().Be(Blocks.SyncdBlockCopy);
            syncBlock?.CreatedTime.Should().Be(DateTime.Parse("2023-03-27T16:28:00.000Z"));
            syncBlock?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-27T16:28:00.000Z"));
            syncBlock?.Archived.Should().BeFalse();
            syncBlock?.HasChildren.Should().BeTrue();
            syncBlock?.From.Id.Should().Be(Blocks.SyncBlockOriginal);

            var parent = syncBlock?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            syncBlock?.LastEditedBy?.Id.Should().Be(Users.Me);
            syncBlock?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }
    
    [Fact]
    public async Task GetSyncdBlockOriginal()
    {
        // arrange

        // act
        var block = await SUT.GetBlockAsync(Blocks.SyncBlockOriginal.ToGuid());

        // assert
        using (new AssertionScope())
        {
            var syncBlock = block as Block.SyncBlock;
            syncBlock.Should().NotBeNull();
            syncBlock?.Id.Should().Be(Blocks.SyncBlockOriginal);
            syncBlock?.CreatedTime.Should().Be(DateTime.Parse("2023-03-27T16:27:00.000Z"));
            syncBlock?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-27T16:28:00.000Z"));
            syncBlock?.Archived.Should().BeFalse();
            syncBlock?.HasChildren.Should().BeTrue();
            syncBlock?.From.Should().BeNull();

            var parent = syncBlock?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            syncBlock?.LastEditedBy?.Id.Should().Be(Users.Me);
            syncBlock?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }
}