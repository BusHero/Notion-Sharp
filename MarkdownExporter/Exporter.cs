using Notion;
using Notion.Model;

namespace MarkdownExporter;

public class Exporter
{
    private INotion Client { get; }
    public IConverter<RichText> RichTextConverter { get; }

    public Exporter(string notionKey, IConverter<RichText> richTextConverter)
    {
        Client = Notion.Notion.NewClient(notionKey);
        RichTextConverter = richTextConverter ?? throw new ArgumentNullException(nameof(richTextConverter));
    }

    public string Convert(Block.Heading1 heading)
    {
        return $"# {(heading.Text[0] as RichText.Text).Content}";
    }

    public string? Convert(RichText richText)
    {
        return RichTextConverter.Convert(richText).ValueOrDefault(string.Empty);
    }
}
