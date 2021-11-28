using Notion;
using Notion.Model;

namespace MarkdownExporter;

public class Exporter
{
    private INotion Client { get; }
    public Converter<RichText> RichTextConverter { get; }

    public Converter<Block> BlockConverter { get; }

    public Exporter(string notionKey, 
        Converter<Block> blockConverter,
        Converter<RichText> richTextConverter)
    {
        Client = Notion.Notion.NewClient(notionKey);
        BlockConverter = blockConverter ?? throw new ArgumentNullException(nameof(blockConverter));
        RichTextConverter = richTextConverter ?? throw new ArgumentNullException(nameof(richTextConverter));
    }
}

public static class Expressions
{
    public static T Invoke<T>(Func<T> func) => func.Invoke();
}