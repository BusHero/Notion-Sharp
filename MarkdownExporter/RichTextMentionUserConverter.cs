using Notion.Model;

namespace MarkdownExporter;

public class RichTextMentionUserConverter: IConverter<RichText>
{
    public Option<string> Convert(RichText richText) => richText switch
    {
        RichText.UserMention userMention => $"**@{userMention.User.Name}**".ToOption(),
        _ => default(string).ToOption()
    };
}
