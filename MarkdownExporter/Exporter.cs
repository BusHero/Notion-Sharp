using Notion;
using Notion.Model;

namespace MarkdownExporter;

public class Exporter
{
    private INotion Client { get; }
    public IConverter<RichText> RichTextConverter { get; }

    public IConverter<Block> BlockConverter { get; }

    public Exporter(string notionKey, 
        IConverter<Block> blockConverter,
        IConverter<RichText> richTextConverter)
    {
        Client = Notion.Notion.NewClient(notionKey);
        BlockConverter = blockConverter ?? throw new ArgumentNullException(nameof(blockConverter));
        RichTextConverter = richTextConverter ?? throw new ArgumentNullException(nameof(richTextConverter));
    }

    public string Convert(Block block) => BlockConverter.Convert(block).ValueOrDefault(string.Empty);

    public string? Convert(RichText richText) => RichTextConverter.Convert(richText).ValueOrDefault(string.Empty);
}

public static class Expressions
{
    public static T Invoke<T>(Func<T> func) => func.Invoke();
}