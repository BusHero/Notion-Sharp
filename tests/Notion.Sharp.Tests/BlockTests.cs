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
            paragraph?.Color.Should().Be("default");
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
            
            var richText = code?.Text[0] as RichText.Text;
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
            bulletedListItem?.Color.Should().Be("default");
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
            numberedListItem?.Color.Should().Be("default");
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
            heading1?.Color.Should().Be("default");
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
            heading2?.Color.Should().Be("default");
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
            heading1?.Color.Should().Be("default");
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
            heading1?.Color.Should().Be("default");
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
            var embed = block as Block.Breadcrumb;
            embed.Should().NotBeNull();
            embed?.Id.Should().Be(Blocks.Breadcrumb);
            embed?.CreatedTime.Should().Be(DateTime.Parse("2023-03-27T16:26:00.000Z"));
            embed?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-27T16:26:00.000Z"));
            embed?.Archived.Should().BeFalse();
            embed?.HasChildren.Should().BeFalse();
            
            var parent = embed?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            embed?.LastEditedBy?.Id.Should().Be(Users.Me);
            embed?.CreatedBy?.Id.Should().Be(Users.Me);
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
            var embed = block as Block.Divider;
            embed.Should().NotBeNull();
            embed?.Id.Should().Be(Blocks.Divider);
            embed?.CreatedTime.Should().Be(DateTime.Parse("2023-03-26T20:01:00.000Z"));
            embed?.LastEditedTime.Should().Be(DateTime.Parse("2023-03-26T20:01:00.000Z"));
            embed?.Archived.Should().BeFalse();
            embed?.HasChildren.Should().BeFalse();
            
            var parent = embed?.Parent as Parent.Page;
            parent.Should().NotBeNull();
            parent?.Id.Should().Be(Pages.PageWithBlocks);

            embed?.LastEditedBy?.Id.Should().Be(Users.Me);
            embed?.CreatedBy?.Id.Should().Be(Users.Me);
        }
    }
}