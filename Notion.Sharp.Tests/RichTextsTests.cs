using FluentAssertions.Execution;
using Notion.Sharp.Tests.Utils;
using Users = Notion.Sharp.Tests.Utils.Users;

namespace Notion.Sharp.Tests;

public class RichTextsTests : NotionTestsBase
{
    [Fact]
    public async Task GetBlockWithBold()
    {
        // arrange
        
        // act
        var block = await NotionClient.Block(RichTexts.Bold.ToGuid()).Get();
        
        // assert
        using (new AssertionScope())
        {
            var paragraph = block as Block.Paragraph;
            var richText = paragraph?.Text?[0] as RichText.Text;
            richText.Should().NotBeNull();
            richText?.Content.Should().Be("bold");
            richText?.Link.Should().BeNull();
            richText?.PlainText.Should().Be("bold");
            richText?.Annotations?.Bold.Should().BeTrue();
            richText?.Annotations?.Italic.Should().BeFalse();
            richText?.Annotations?.Underline.Should().BeFalse();
            richText?.Annotations?.Strikethrough.Should().BeFalse();
            richText?.Annotations?.Code.Should().BeFalse();
            richText?.Annotations?.Color.Should().Be(Color.Default);
        }
    }
    
    [Fact]
    public async Task GetItalic()
    {
        // arrange
        
        // act
        var block = await NotionClient.Block(RichTexts.Italic.ToGuid()).Get();
        
        // assert
        using (new AssertionScope())
        {
            var paragraph = block as Block.Paragraph;
            paragraph?.Text.Should().HaveCount(1);
            var richText = paragraph?.Text?[0] as RichText.Text;
            richText.Should().NotBeNull();
            richText?.Content.Should().Be("italic");
            richText?.Link.Should().BeNull();
            richText?.PlainText.Should().Be("italic");
            richText?.Annotations?.Bold.Should().BeFalse();
            richText?.Annotations?.Italic.Should().BeTrue();
            richText?.Annotations?.Underline.Should().BeFalse();
            richText?.Annotations?.Strikethrough.Should().BeFalse();
            richText?.Annotations?.Code.Should().BeFalse();
            richText?.Annotations?.Color.Should().Be(Color.Default);
        }
    }
    
    [Fact]
    public async Task GetUnderline()
    {
        // arrange
        
        // act
        var block = await NotionClient.Block(RichTexts.Underline.ToGuid()).Get();
        
        // assert
        using (new AssertionScope())
        {
            var paragraph = block as Block.Paragraph;
            paragraph?.Text.Should().HaveCount(1);
            var richText = paragraph?.Text?[0] as RichText.Text;
            richText.Should().NotBeNull();
            richText?.Content.Should().Be("underline");
            richText?.Link.Should().BeNull();
            richText?.PlainText.Should().Be("underline");
            richText?.Annotations?.Bold.Should().BeFalse();
            richText?.Annotations?.Italic.Should().BeFalse();
            richText?.Annotations?.Underline.Should().BeTrue();
            richText?.Annotations?.Strikethrough.Should().BeFalse();
            richText?.Annotations?.Code.Should().BeFalse();
            richText?.Annotations?.Color.Should().Be(Color.Default);
        }
    }
    
    [Fact]
    public async Task GetStrikethrough()
    {
        // arrange
        
        // act
        var block = await NotionClient.Block(RichTexts.StrikeThrough.ToGuid()).Get();
        
        // assert
        using (new AssertionScope())
        {
            var paragraph = block as Block.Paragraph;
            paragraph?.Text.Should().HaveCount(1);
            var richText = paragraph?.Text?[0] as RichText.Text;
            richText.Should().NotBeNull();
            richText?.Content.Should().Be("strikethrough");
            richText?.Link.Should().BeNull();
            richText?.PlainText.Should().Be("strikethrough");
            richText?.Annotations?.Bold.Should().BeFalse();
            richText?.Annotations?.Italic.Should().BeFalse();
            richText?.Annotations?.Underline.Should().BeFalse();
            richText?.Annotations?.Strikethrough.Should().BeTrue();
            richText?.Annotations?.Code.Should().BeFalse();
            richText?.Annotations?.Color.Should().Be(Color.Default);
        }
    }
    
    [Fact]
    public async Task GetCode()
    {
        // arrange
        
        // act
        var block = await NotionClient.Block(RichTexts.Code.ToGuid()).Get();
        
        // assert
        using (new AssertionScope())
        {
            var paragraph = block as Block.Paragraph;
            paragraph?.Text.Should().HaveCount(1);
            var richText = paragraph?.Text?[0] as RichText.Text;
            richText.Should().NotBeNull();
            richText?.Content.Should().Be("code");
            richText?.Link.Should().BeNull();
            richText?.PlainText.Should().Be("code");
            richText?.Annotations?.Bold.Should().BeFalse();
            richText?.Annotations?.Italic.Should().BeFalse();
            richText?.Annotations?.Underline.Should().BeFalse();
            richText?.Annotations?.Strikethrough.Should().BeFalse();
            richText?.Annotations?.Code.Should().BeTrue();
            richText?.Annotations?.Color.Should().Be(Color.Default);
        }
    }
    
    [Fact]
    public async Task GetColor()
    {
        // arrange
        
        // act
        var block = await NotionClient.Block(RichTexts.Foreground.ToGuid()).Get();
        
        // assert
        using (new AssertionScope())
        {
            var paragraph = block as Block.Paragraph;
            paragraph?.Text.Should().HaveCount(1);
            var richText = paragraph?.Text?[0] as RichText.Text;
            richText.Should().NotBeNull();
            richText?.Content.Should().Be("Foreground");
            richText?.Link.Should().BeNull();
            richText?.PlainText.Should().Be("Foreground");
            richText?.Annotations?.Bold.Should().BeFalse();
            richText?.Annotations?.Italic.Should().BeFalse();
            richText?.Annotations?.Underline.Should().BeFalse();
            richText?.Annotations?.Strikethrough.Should().BeFalse();
            richText?.Annotations?.Code.Should().BeFalse();
            richText?.Annotations?.Color.Should().Be(Color.Blue);
        }
    }
    
    [Fact]
    public async Task GetEquation()
    {
        // arrange
        
        // act
        var block = await NotionClient.Block(RichTexts.Equation.ToGuid()).Get();
        
        // assert
        using (new AssertionScope())
        {
            var paragraph = block as Block.Paragraph;
            paragraph?.Text.Should().HaveCount(1);
            var equation = paragraph?.Text?[0] as RichText.Equation;
            equation.Should().NotBeNull();
            equation?.PlainText.Should().Be("equation");
            equation?.Annotations?.Bold.Should().BeFalse();
            equation?.Annotations?.Italic.Should().BeFalse();
            equation?.Annotations?.Underline.Should().BeFalse();
            equation?.Annotations?.Strikethrough.Should().BeFalse();
            equation?.Annotations?.Code.Should().BeFalse();
            equation?.Annotations?.Color.Should().Be(Color.Default);
            equation?.Expression.Should().Be("equation");
        }
    }
    
    [Fact]
    public async Task GetMentionToPage()
    {
        // arrange
        
        // act
        var block = await NotionClient.Block(RichTexts.MentionBlock.ToGuid()).Get();
        
        // assert
        using (new AssertionScope())
        {
            var paragraph = block as Block.Paragraph;
            paragraph?.Text.Should().HaveCount(1);
            var mention = paragraph?.Text?[0] as RichText.PageMention;
            mention.Should().NotBeNull();
            mention?.PlainText.Should().Be("Parent page for tests");
            mention?.Annotations?.Bold.Should().BeFalse();
            mention?.Annotations?.Italic.Should().BeFalse();
            mention?.Annotations?.Underline.Should().BeFalse();
            mention?.Annotations?.Strikethrough.Should().BeFalse();
            mention?.Annotations?.Code.Should().BeFalse();
            mention?.Annotations?.Color.Should().Be(Color.Default);
            mention?.Id.Should().Be(Pages.Parent);
        }
    }
    
    [Fact]
    public async Task GetMentionDate()
    {
        // arrange
        
        // act
        var block = await NotionClient.Block(RichTexts.MentionToday.ToGuid()).Get();
        
        // assert
        using (new AssertionScope())
        {
            var paragraph = block as Block.Paragraph;
            paragraph?.Text.Should().HaveCount(1);
            var mention = paragraph?.Text?[0] as RichText.DateMention;
            mention.Should().NotBeNull();
            mention?.PlainText.Should().Be("2023-03-27");
            mention?.Annotations?.Bold.Should().BeFalse();
            mention?.Annotations?.Italic.Should().BeFalse();
            mention?.Annotations?.Underline.Should().BeFalse();
            mention?.Annotations?.Strikethrough.Should().BeFalse();
            mention?.Annotations?.Code.Should().BeFalse();
            mention?.Annotations?.Color.Should().Be(Color.Default);
            mention?.Start.Should().Be(DateTime.Parse("2023-03-27"));
            mention?.End.Should().BeNull();
            mention?.TimeZone.Should().BeNull();
        }
    }
    
    [Fact]
    public async Task GetPersonMention()
    {
        // arrange
        
        // act
        var block = await NotionClient.Block(RichTexts.MentionPerson.ToGuid()).Get();
        
        // assert
        using (new AssertionScope())
        {
            var paragraph = block as Block.Paragraph;
            paragraph?.Text.Should().HaveCount(1);
            var mention = paragraph?.Text?[0] as RichText.UserMention;
            mention.Should().NotBeNull();
            mention?.PlainText.Should().Be("@Petru Cervac");
            mention?.Annotations?.Bold.Should().BeFalse();
            mention?.Annotations?.Italic.Should().BeFalse();
            mention?.Annotations?.Underline.Should().BeFalse();
            mention?.Annotations?.Strikethrough.Should().BeFalse();
            mention?.Annotations?.Code.Should().BeFalse();
            mention?.Annotations?.Color.Should().Be(Color.Default);
            var person = mention?.User as User.Person;
            person?.Id.Should().Be(Users.Me);
            person?.Name.Should().Be("Petru Cervac");
            person?.AvatarUrl.Should().Be("https://lh3.googleusercontent.com/a-/AOh14GhcBvrhyvv32v0kTVHocfT7oex0gofyo0r6OjoHPw=s100");
            person?.Email.Should().Be("petru.cervac@gmail.com");
        }
    }
    
    [Fact]
    public async Task GetBoldAndItalic()
    {
        // arrange
        
        // act
        var block = await NotionClient.Block(RichTexts.BoldAndItalic.ToGuid()).Get();
        
        // assert
        using (new AssertionScope())
        {
            var paragraph = block as Block.Paragraph;
            paragraph?.Text.Should().HaveCount(1);
            var richText = paragraph?.Text?[0] as RichText.Text;
            richText.Should().NotBeNull();
            richText?.Content.Should().Be("Bold and Italic");
            richText?.Link.Should().BeNull();
            richText?.PlainText.Should().Be("Bold and Italic");
            richText?.Annotations?.Bold.Should().BeTrue();
            richText?.Annotations?.Italic.Should().BeTrue();
            richText?.Annotations?.Underline.Should().BeFalse();
            richText?.Annotations?.Strikethrough.Should().BeFalse();
            richText?.Annotations?.Code.Should().BeFalse();
            richText?.Annotations?.Color.Should().Be(Color.Default);
        }
    }
    
    [Fact]
    public async Task GetBoldThenItalic()
    {
        // arrange
        
        // act
        var block = await NotionClient.Block(RichTexts.BoldThenItalic.ToGuid()).Get();
        
        // assert
        using (new AssertionScope())
        {
            var paragraph = block as Block.Paragraph;
            paragraph?.Text.Should().HaveCount(3);
            
            var bold = paragraph?.Text?[0] as RichText.Text;
            bold.Should().NotBeNull();
            bold?.Content.Should().Be("Bold ");
            bold?.Link.Should().BeNull();
            bold?.PlainText.Should().Be("Bold ");
            bold?.Annotations?.Bold.Should().BeTrue();
            bold?.Annotations?.Italic.Should().BeFalse();
            bold?.Annotations?.Underline.Should().BeFalse();
            bold?.Annotations?.Strikethrough.Should().BeFalse();
            bold?.Annotations?.Code.Should().BeFalse();
            bold?.Annotations?.Color.Should().Be(Color.Default);
            
            var normal = paragraph?.Text?[1] as RichText.Text;
            normal.Should().NotBeNull();
            normal?.Content.Should().Be("then ");
            normal?.Link.Should().BeNull();
            normal?.PlainText.Should().Be("then ");
            normal?.Annotations?.Bold.Should().BeFalse();
            normal?.Annotations?.Italic.Should().BeFalse();
            normal?.Annotations?.Underline.Should().BeFalse();
            normal?.Annotations?.Strikethrough.Should().BeFalse();
            normal?.Annotations?.Code.Should().BeFalse();
            normal?.Annotations?.Color.Should().Be(Color.Default);
            
            var italic = paragraph?.Text?[2] as RichText.Text;
            italic.Should().NotBeNull();
            italic?.Content.Should().Be("Italic");
            italic?.Link.Should().BeNull();
            italic?.PlainText.Should().Be("Italic");
            italic?.Annotations?.Bold.Should().BeFalse();
            italic?.Annotations?.Italic.Should().BeTrue();
            italic?.Annotations?.Underline.Should().BeFalse();
            italic?.Annotations?.Strikethrough.Should().BeFalse();
            italic?.Annotations?.Code.Should().BeFalse();
            italic?.Annotations?.Color.Should().Be(Color.Default);
        }
    }
    
    [Fact]
    public async Task GetLink()
    {
        // arrange
        
        // act
        var block = await NotionClient.Block(RichTexts.LinkToWebsite.ToGuid()).Get();
        
        // assert
        using (new AssertionScope())
        {
            var paragraph = block as Block.Paragraph;
            paragraph?.Text.Should().HaveCount(1);
            var richText = paragraph?.Text?[0] as RichText.Text;
            richText.Should().NotBeNull();
            richText?.Content.Should().Be("link to website");
            richText?.Link?.Url.Should().Be("https://google.com");
            richText?.PlainText.Should().Be("link to website");
            richText?.Annotations?.Bold.Should().BeFalse();
            richText?.Annotations?.Italic.Should().BeFalse();
            richText?.Annotations?.Underline.Should().BeFalse();
            richText?.Annotations?.Strikethrough.Should().BeFalse();
            richText?.Annotations?.Code.Should().BeFalse();
            richText?.Annotations?.Color.Should().Be(Color.Default);
        }
    }
    
    [Fact]
    public async Task GetLinkToPage()
    {
        // arrange
        
        // act
        var block = await NotionClient.Block(RichTexts.LinkPage.ToGuid()).Get();
        
        // assert
        using (new AssertionScope())
        {
            var paragraph = block as Block.Paragraph;
            paragraph?.Text.Should().HaveCount(1);
            var richText = paragraph?.Text?[0] as RichText.Text;
            richText.Should().NotBeNull();
            richText?.Content.Should().Be("link to page");
            richText?.Link?.Url.Should().Be("/b5d544834cb14fd3a80e897da4827770");
            richText?.PlainText.Should().Be("link to page");
            richText?.Annotations?.Bold.Should().BeFalse();
            richText?.Annotations?.Italic.Should().BeFalse();
            richText?.Annotations?.Underline.Should().BeFalse();
            richText?.Annotations?.Strikethrough.Should().BeFalse();
            richText?.Annotations?.Code.Should().BeFalse();
            richText?.Annotations?.Color.Should().Be(Color.Default);
        }
    }
}
