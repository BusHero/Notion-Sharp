using FluentAssertions.Execution;
using Notion.Sharp.Tests.Utils;

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
            paragraph?.Children.Should().BeNull();
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
        }
    }
}