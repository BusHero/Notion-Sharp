using Notion;
using Notion.Model;

namespace MarkdownExporter;

public class MarkdownExporter
{
    private INotion Client { get; }

    public MarkdownExporter(string notionKey)
    {
        Client = Notion.Notion.NewClient(notionKey);
    }

    public string Convert(Block.Heading1 heading)
    {
        return $"# {(heading.Text[0] as RichText.Text).Content}";
    }

    public string Convert(RichText richText)
    {
        return $"{(richText as RichText.Text).Content}";
    }
}
