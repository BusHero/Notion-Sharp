using Notion;
using Notion.Model;

namespace MarkdownExporter;

public class BlockConverter<T> : Converter<T> where T : Block
{
    public BlockConverter(
        Func<T, RichText[]> richTextGetter,
        Func<T, Block[]> childrenGetter,
        Func<string, string> formatter,
        Func<string, string> childFormatter)
    {
        RichTextGetter = richTextGetter ?? throw new ArgumentNullException(nameof(richTextGetter));
        ChildrenGetter = childrenGetter ?? throw new ArgumentNullException(nameof(childrenGetter));
        Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
        ChildFormatter = childFormatter ?? throw new ArgumentNullException(nameof(childFormatter));
    }

    private Func<T, RichText[]> RichTextGetter { get; }
    private Func<T, Block[]> ChildrenGetter { get; }
    private Func<string, string> Formatter { get; }
    private Func<string, string> ChildFormatter { get; }

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

        var result2 = ChildrenGetter(block)
            .Select(block => Converter.Convert(block, settings))
            .Aggregate((first, second) => first.Map2(second, Lists.Add))
            .Select(list => list.Select(ChildFormatter))
            .Select(Enumerable.ToList);

        return result.Map2(result2, Lists.Add);
    }
}
