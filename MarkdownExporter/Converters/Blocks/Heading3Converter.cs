using Notion.Model;

namespace MarkdownExporter;

public class Heading3Converter : BlockConverter<Block.Heading3>
{
    public override string Format(string text) => $"### {text}";

    public override string FormatChild(string text) => throw new InvalidOperationException();

    public override Block[] GetChildren(Block.Heading3 heading3) => Array.Empty<Block>();

    public override RichText[] GetText(Block.Heading3 heading3) => heading3.Text;
}
