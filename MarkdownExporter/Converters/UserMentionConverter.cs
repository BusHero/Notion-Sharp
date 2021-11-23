using Notion.Model;

namespace MarkdownExporter;

public class UserMentionConverter: Converter<RichText.UserMention>
{
    public override Option<string> Convert(RichText.UserMention userMention, ConverterSettings? settings) => userMention switch
    {
        { 
            User: User.Person 
            { 
                Name: var name and not null, 
                Email: var email and not null 
            } 
        } => $"[@{name}](mailto:{email})".ToOption(),
        _ => new Option<string>()
    };
}
