using Notion;
using Notion.Model;

namespace MarkdownExporter;

public class NumberedListConverter : Converter<Block.NumberedListItem>
{
    private INotion Notion { get; }
    private Func<string, string> Formatter { get; }
    private Func<string, string> ChildFormatter { get; }

    public NumberedListConverter(INotion notion, 
        Func<string, string> formatter,
        Func<string, string> childFormatter)
    {
        Notion = notion ?? throw new ArgumentNullException(nameof(notion));
        Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
        ChildFormatter = childFormatter ?? throw new ArgumentNullException(nameof(childFormatter));
    }

    public NumberedListConverter(INotion notion) : this(notion, 
        text => $"1. {text}",
        text => $"&nbsp;&nbsp;&nbsp;&nbsp;{text}")
    { }

    public override Option<List<string>> Convert(Block.NumberedListItem numberedListItem, ConverterSettings? settings)
    {
        var result = numberedListItem
            .Text
            .Select(text => Converter.Convert(text, settings))
            .Aggregate((first, second) => first.Map2(second, Lists.Add))
            .Select(list => string.Join("", list))
            .Select(Formatter)
            .Select(Lists.Of);

        if (!numberedListItem.HasChildren)
            return result;

        var result2 = Notion.GetBlocksChildrenAsync(numberedListItem.Id)
            .Result
            .Results
            .Select(block => Converter.Convert(block, settings))
            .Aggregate((first, second) => first.Map2(second, Lists.Add))
            .Select(list => list.Select(ChildFormatter).ToList());

        return result.Map2(result2, Lists.Add);
    }
}
