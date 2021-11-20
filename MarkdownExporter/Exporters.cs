using Notion.Model;

namespace MarkdownExporter;

public static class Exporters
{
    public static Exporter MarkdownExporter(string notionKey) => new(notionKey,
        new RichTextTextConverter(
            Applicable.Bold(Formatters.FormatBold)
            + Applicable.Italic(Formatters.FormatItalic)
            + Applicable.Strikethrough(Formatters.FormatStike)
            + Applicable.Underline(Formatters.FormatUnderline)
            + Applicable.FormatCode(Formatters.FormatCode)
            + Applicable.FormatColor(Formatters.FormatColor))
        + Converters.ToConverter<RichText, RichText.Equation>(equation => $"`{equation.Expression}`".ToOption())
        + Converters.ToConverter((RichText richText) => richText switch
        {
            RichText.Equation equation => $"`{equation.Expression}`".ToOption(),
            _ => default(string).ToOption()
        })
        + new RichTextMentionUserConverter());
}
