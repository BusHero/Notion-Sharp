using Notion;
using Notion.Model;

namespace MarkdownExporter;

public class ParagraphConverter : BlockConverter<Block.Paragraph>
{
    private INotion Notion { get; }


    public ParagraphConverter(INotion notion) => Notion = notion ?? throw new ArgumentNullException(nameof(notion));

    public override string Format(string text) => text;

    public override string FormatChild(string text) => $"&nbsp;&nbsp;&nbsp;&nbsp;{text}";

    public override Block[] GetChildren(Block.Paragraph paragraph) => paragraph.HasChildren switch
    {
        true => Notion.GetBlocksChildrenAsync(paragraph.Id).Result.Results,
        false => Array.Empty<Block>(),
    };

    public override RichText[] GetText(Block.Paragraph paragraph) => paragraph.Text;
}
