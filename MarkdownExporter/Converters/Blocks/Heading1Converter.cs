using Notion.Model;

namespace MarkdownExporter;

public class Heading1Converter : BlockConverter<Block.Heading1>
{
    public override string Format(string text) => $"# {text}";

    public override string FormatChild(string text) => text;

    public override Block[] GetChildren(Block.Heading1 t) => Array.Empty<Block>();

    public override RichText[] GetText(Block.Heading1 heading1) => heading1.Text;
}
