using FluentAssertions;

using Notion.Model;

using System.Collections.Generic;

using Xunit;

namespace MarkdownExporter.Tests;

public class RichTextConverterTests
{
    private Converter<RichText> Converter { get; } = new RichTextConverter(
        Applicable.Bold(Formatters.FormatBold)
        + Applicable.Italic(Formatters.FormatItalic)
        + Applicable.Strikethrough(Formatters.FormatStike)
        + Applicable.Underline(Formatters.FormatUnderline)
        + Applicable.FormatCode(Formatters.FormatCode)
        + Applicable.FormatColor(Formatters.FormatColor)
        + Applicable.Link(Formatters.FormatLink));

    private ConverterSettings? Settings { get; } = default;

    private IEqualityComparer<Option<List<string?>>> Comparer { get; } = new OptionComparer<List<string>>(new ListSequenceComparer<string>());


    [Theory]
    [MemberData(nameof(Texts))]
    public void Text_Convert_Succeds(RichText richText, string expectedString)
    {
        var actualResult = Converter
            .Convert(richText, Settings);

        var expectedResult = new List<string?> { expectedString }.ToOption();

        Assert.Equal(expectedResult, actualResult, Comparer);
    }

    public static TheoryData<RichText, string> Texts { get; } = new()
    {
        { 
            new RichText.Text 
            { 
                Content = "Some text here and there" ,
                Link = new Link 
                { 
                    Url = new System.Uri("https://google.com") 
                },
                PlainText = "Some text here and there",
                Href = new System.Uri("https://google.com")
            }, 
            "[Some text here and there](https://google.com/)" 
        },
        { 
            new RichText.Text 
            { 
                Content = "Some text here and there", 
                PlainText = "Some text here and there" 
            }, 
            "Some text here and there" 
        },
        { new RichText.Text { Content = "Some text here and there", Annotations = new Annotations
            {
                Bold = true
            },
            PlainText = "Some text here and there"
        }, "*Some text here and there*" },
        {
            new RichText.Text
            {
                Content = "Some text here and there",
                Annotations = new Annotations
                {
                    Strikethrough = true
                },
                PlainText = "Some text here and there",
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
                },
                PlainText = "Some text here and there",
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
                },
                PlainText = "Some text here and there",
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
                },
                PlainText = "Some text here and there",
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
                },
                PlainText = "Some text here and there",
            },
            "***Some text here and there***"
        },
        {
            new RichText.UserMention
            {
                User = new User.Person
                {
                    Name = "Cervac Petru",
                    Email = "petru.cervac@gmail.com"
                },
                PlainText = "@Cervac Petru"
            },
            "@Cervac Petru"
        },
        {
            new RichText.DateMention
            {
                Start = new System.DateTimeOffset(new System.DateTime(2021, 10, 26)),
                PlainText = "2021-10-26 →"
            },
            "2021-10-26 →"
        }
    };
}
