using FluentAssertions.Execution;
using Notion.Sharp.Tests.Utils;
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
}