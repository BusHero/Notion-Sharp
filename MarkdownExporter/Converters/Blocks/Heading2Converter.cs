using Notion.Model;

namespace MarkdownExporter;

public class Heading2Converter : BlockConverter<Block.Heading2>
{
    public override string Format(string text) => $"## {text}";

    public override string FormatChild(string text) => throw new InvalidOperationException();

    public override Block[] GetChildren(Block.Heading2 t) => Array.Empty<Block>();

    public override RichText[] GetText(Block.Heading2 heading2) => heading2.Text;
}
