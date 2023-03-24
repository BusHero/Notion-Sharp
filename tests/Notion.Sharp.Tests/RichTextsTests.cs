namespace Notion.Sharp.Tests;

public class RichTextsTests : NotionTestsBase
{
    [Theory(Skip = "It's broken")]
    [InlineData("3bbf6ded7f2d467c85d36bc284fcaef1")]
    [InlineData("83de0d65771d4281b92a9e7a88097259")]
    [InlineData("3321337b0208402db5541969f0cb6251")]
    [InlineData("67f26dd5e73c452e82308c289d8e3996")]
    [InlineData("9ad3666614394d2fbd661a6ee184f254")]
    [InlineData("30e286a392b0413992ea355facf9e195")]
    [InlineData("fea9c0130e8743cc9608c227b6bca5a9")]
    [InlineData("25c3c8f17ef047a3b3a6da6e99d255bf")]
    public async Task GetRichText(string guid)
    {
        var block = await SUT.GetBlockAsync(new Guid(guid));
        block.Should().NotBeNull();
    }

    [Theory(Skip = "It's broken")]
    [MemberData(nameof(RichTexts))]
    public async Task AppendRichText_Succed(RichText richText) => await RetryAsync(async () =>
    {
        var result = await SUT.AppendBlockChildrenAsync(ValidPageId, new List<Block>
        {
            new Block.Paragraph
            {
                Text = new RichText[]
                {
                    richText
                }
            }
        });
        var block = await SUT.DeleteBlockAsync(result.Results[0].Id);
    }, 3);

    public static TheoryData<RichText> RichTexts { get; } = new TheoryData<RichText>
    {
        new RichText.Text { Content = "Some text here and there" },
        new RichText.Text { Content = "Some text here and there", Link = new Link { Url = new Uri("https://google.com") } },
        new RichText.DatabaseMention { Id = new Guid("a6dbd20bafdb4b3e857bcf04a77ee3b5") },
        new RichText.PageMention { Id = new Guid("72437ca5b34c484f901a6f2368f5199e") },
        new RichText.DateMention { Start = DateTime.Today, End = DateTime.Today },
        new RichText.UserMention { User = new User.Person { Id = new Guid("6591a4d4-ca53-4b54-a3ca-7d2420d6b902") } },
        new RichText.Equation { Expression = "1 + 1" },
    };
}
