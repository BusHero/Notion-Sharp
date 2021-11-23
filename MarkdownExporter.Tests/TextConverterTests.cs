using FluentAssertions;

using Notion.Model;

using Xunit;

namespace MarkdownExporter.Tests;

public class TextConverterTests
{
    public ConverterSettings Settings { get; }

    public TextConverterTests()
    {
        Settings = new ConverterSettings
        {
            Converter = new TextConverter(
                Applicable.Bold(Formatters.FormatBold)
                + Applicable.Italic(Formatters.FormatItalic)
                + Applicable.Strikethrough(Formatters.FormatStike)
                + Applicable.Underline(Formatters.FormatUnderline)
                + Applicable.FormatCode(Formatters.FormatCode)
                + Applicable.FormatColor(Formatters.FormatColor))
                + new UserMentionConverter()
        };
    }

    [Fact]
    public void UserMention_Convert_Succeds()
    {
        var userMention = new RichText.UserMention
        {
            User = new User.Person
            {
                Name = "Cervac Petru",
                Email = "petru.cervac@gmail.com"
            }
        };
        var actualText = Converter.Convert(userMention, Settings).ValueOrDefault(string.Empty);
        actualText.Should().Be("[@Cervac Petru](mailto:petru.cervac@gmail.com)");
    }

    [Theory]
    [MemberData(nameof(Texts))]
    public void Text_Convert_Succeds(RichText.Text richText, string expectedString)
    {
        var actualString = Converter.Convert(richText, Settings).ValueOrDefault(string.Empty);
        actualString.Should().Be(expectedString);
    }

    public static TheoryData<RichText.Text, string> Texts { get; } = new()
    {
        { new RichText.Text { Content = "Some text here and there" }, "Some text here and there" },
        { new RichText.Text { Content = "Some text here and there", Annotations = new Annotations
            {
                Bold = true
            }
        }, "*Some text here and there*" },
        {
            new RichText.Text
            {
                Content = "Some text here and there",
                Annotations = new Annotations
                {
                    Strikethrough = true
                }
            },
            "~~Some text here and there~~"
        },
        {
            new RichText.Text
            {
                Content = "Some text here and there",
                Annotations = new Annotations
                {
                    Underline = true
                }
            },
            "<u>Some text here and there</u>"
        },
        {
            new RichText.Text
            {
                Content = "Some text here and there",
                Annotations = new Annotations
                {
                    Code = true
                }
            },
            "`Some text here and there`"
        },
        {
            new RichText.Text
            {
                Content = "Some text here and there",
                Annotations = new Annotations
                {
                    Color = Color.Blue
                }
            },
            "<span style=\"color: blue\">Some text here and there</span>"
        },
        {
            new RichText.Text
            {
                Content = "Some text here and there",
                Annotations = new Annotations
                {
                    Italic = true,
                    Bold = true,
                }
            },
            "***Some text here and there***"
        },
    };
}
