using FluentAssertions;

using Notion.Model;

using Xunit;

namespace MarkdownExporter.Tests;

public class UserMentionConverterTests
{
    private ConverterSettings? Settings { get; } = default;
    private Converter SUT { get; } = new UserMentionConverter();

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
        var actualText = SUT.Convert(userMention, Settings).ValueOrDefault(string.Empty);
        actualText.Should().Be("[@Cervac Petru](mailto:petru.cervac@gmail.com)");
    }
}
