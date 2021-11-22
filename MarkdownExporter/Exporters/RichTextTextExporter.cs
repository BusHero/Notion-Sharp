using Notion.Model;

namespace MarkdownExporter;

public class RichTextTextExporter
{
    public string? Convert(RichText.Text text)
    {
        return text switch
        {
            null => null,
            _ => text.Annotations switch
            {
                { Bold: true } => $"*{text.Content}*",
                _ => text.Content,
            }
        };
    }
}
