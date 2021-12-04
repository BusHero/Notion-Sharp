using Notion.Model;

namespace MarkdownExporter;

public class RelayBlockConverter<T> : BlockConverter<T> where T : Block
{
    public RelayBlockConverter(
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

    public override RichText[] GetText(T t) => RichTextGetter(t);
    public override Block[] GetChildren(T t) => ChildrenGetter(t);
    public override string Format(string text) => Formatter(text);
    public override string FormatChild(string text) => ChildFormatter(text);
}
