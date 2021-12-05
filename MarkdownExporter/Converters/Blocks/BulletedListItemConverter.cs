using Notion;
using Notion.Model;

namespace MarkdownExporter;

public class BulletedListItemConverter : BlockConverter<Block.BulletedListItem>
{
    public BulletedListItemConverter(INotion notion) => Notion = notion ?? throw new ArgumentNullException(nameof(notion));

    private INotion Notion { get; }

    public override string Format(string text) => $"* {text}";

    public override string FormatChild(string text) => $"&nbsp;&nbsp;&nbsp;&nbsp;{text}";

    public override Block[] GetChildren(Block.BulletedListItem bulletedListItem) => bulletedListItem.HasChildren switch
    {
        true => Notion.GetBlocksChildrenAsync(bulletedListItem.Id).Result.Results,
        false => Array.Empty<Block>(),
    };

    public override RichText[] GetText(Block.BulletedListItem bulletedListItem) => bulletedListItem.Text;
}
