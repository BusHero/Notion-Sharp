using Notion;
using Notion.Model;

namespace MarkdownExporter;

public class NumberedListItemConverter : BlockConverter<Block.NumberedListItem>
{
    private INotion Notion { get; }

    public NumberedListItemConverter(INotion notion) => Notion = notion ?? throw new ArgumentNullException(nameof(notion));

    public override string Format(string text) => $"1. {text}";

    public override string FormatChild(string text) => $"&nbsp;&nbsp;&nbsp;&nbsp;{text}";

    public override Block[] GetChildren(Block.NumberedListItem numberedListItem) => numberedListItem.HasChildren switch
    {
        true => Notion.GetBlocksChildrenAsync(numberedListItem.Id).Result.Results,
        false => Array.Empty<Block>(),
    };

    public override RichText[] GetText(Block.NumberedListItem numberedListItem) => numberedListItem.Text;
}
