using Notion.Model;

namespace MarkdownExporter;

public static class Exporters
{
    public static Exporter MarkdownExporter(string notionKey) => new(notionKey,
        (new RichTextTextConverter(
            Applicable.Bold(Formatters.FormatBold)
            + Applicable.Italic(Formatters.FormatItalic)
            + Applicable.Strikethrough(Formatters.FormatStike)
            + Applicable.Underline(Formatters.FormatUnderline)
            + Applicable.FormatCode(Formatters.FormatCode)
            + Applicable.FormatColor(Formatters.FormatColor)) as IConverter<RichText>)
        + new RichTextEquationConverter()
        + new RichTextMentionUserConverter());
}
