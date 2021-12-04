using Notion;
using Notion.Model;

namespace MarkdownExporter;

public class BlockConverter<T> : Converter<T> where T : Block
{
    public BlockConverter(
        INotion notion,
        Func<T, RichText[]> richTextGetter, 
        Func<string, string> formatter,
        Func<string, string> childFormatter)
    {
        Notion = notion ?? throw new ArgumentNullException(nameof(notion));
        RichTextGetter = richTextGetter ?? throw new ArgumentNullException(nameof(richTextGetter));
        Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
        ChildFormatter = childFormatter ?? throw new ArgumentNullException(nameof(childFormatter));
    }

    public INotion Notion { get; }
    private Func<T, RichText[]> RichTextGetter { get; }
    public Func<string, string> Formatter { get; }
    public Func<string, string> ChildFormatter { get; }

    public override Option<List<string>> Convert(T block, ConverterSettings? settings)
    {
        var result = RichTextGetter(block)
            .Select(text => Converter.Convert(text, settings))
            .Aggregate(Option.Binary<List<string>>(Lists.Add))
            .Select(Strings.Join)
            .Select(Formatter)
            .Select(Lists.Of);

        if (!block.HasChildren)
            return result;

        var result2 = Notion
            .GetBlocksChildrenAsync(block.Id)
            .Result
            .Results
            .Select(block => Converter.Convert(block, settings))
            .Aggregate((first, second) => first.Map2(second, Lists.Add))
            .Select(list => list.Select(ChildFormatter))
            .Select(Enumerable.ToList);

        return result.Map2(result2, Lists.Add);
    }
}
