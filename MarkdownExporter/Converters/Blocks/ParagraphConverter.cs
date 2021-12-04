using Notion;
using Notion.Model;

namespace MarkdownExporter;

public class ParagraphConverter : Converter<Block.Paragraph>
{
    public ParagraphConverter(INotion notion, Func<string, string> childFormatter)
    {
        Notion = notion ?? throw new ArgumentNullException(nameof(notion));
        ChildFormatter = childFormatter ?? throw new ArgumentNullException(nameof(childFormatter));
    }

    public ParagraphConverter(INotion notion): this(notion, text => $"&nbsp;&nbsp;&nbsp;&nbsp;{text}")
    { }

    public INotion Notion { get; }
    public Func<string, string> ChildFormatter { get; }

    public override Option<List<string>> Convert(Block.Paragraph paragraph, ConverterSettings? settings)
    {
        var result = paragraph
            .Text
            .Select(text => Converter.Convert(text, settings))
            .Aggregate((first, second) => first.Map2(second, Lists.Add))
            .Select(list => new List<string> { string.Join("", list) });

        if (!paragraph.HasChildren)
            return result;

        var foo = Notion.GetBlocksChildrenAsync(paragraph.Id)
            .Result
            .Results;
        var bar = foo
            .Select(block => Converter.Convert(block, settings))
            .ToList();
        var result2 = bar
            .Aggregate((first, second) => first.Map2(second, Lists.Add))
            .Select(list => list.Select(ChildFormatter).ToList());

        return result.Map2(result2, Lists.Add);
    }
}
