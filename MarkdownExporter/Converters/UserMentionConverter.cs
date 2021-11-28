//using Notion.Model;

//namespace MarkdownExporter;

//public class UserMentionConverter: Converter<RichText.UserMention>
//{
//    public override IOption<string> Convert(RichText.UserMention userMention, ConverterSettings? settings) => userMention switch
//    {
//        { 
//            User: User.Person 
//            { 
//                Name: var name and not null, 
//                Email: var email and not null 
//            } 
//        } => $"[@{name}](mailto:{email})".ToOption(),
//        _ => Option.None<string>()
//    };

//    public override IOption<List<string>> Convert2(RichText.UserMention userMention, ConverterSettings? settings) => (userMention switch
//    {
//        {
//            User: User.Person
//            {
//                Name: var name and not null,
//                Email: var email and not null
//            }
//        } => new List<string> { $"[@{name}](mailto:{email})" },
//        _ => default
//    }).ToOption();
//}
