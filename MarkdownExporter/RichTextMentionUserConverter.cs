using Notion.Model;

namespace MarkdownExporter;

public class RichTextMentionUserConverter: Converter<RichText>
{
    public override Option<string> Convert(RichText richText, ConverterSettings settings) => richText switch
    {
        RichText.UserMention userMention => $"**@{userMention.User.Name}**".ToOption(),
        _ => default(string).ToOption()
    };
}
