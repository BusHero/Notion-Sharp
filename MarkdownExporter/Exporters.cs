using Notion.Model;

namespace MarkdownExporter;

public static class Exporters
{
    //public static Exporter MarkdownExporter(string notionKey) => new(notionKey,
    //    Converters.ToConverter<Block, Block.Heading1>(heading1 =>
    //    {
    //        if (heading1.Text[0] is RichText.Text text)
    //            return Formatters.FormatHeading1(text.Content).ToOption();
    //        return new Option<string>();
    //    }) + Converters.ToConverter<Block, Block.Heading2>(heading1 =>
    //    {
    //        if (heading1.Text[0] is RichText.Text text)
    //            return Formatters.FormatHeading2(text.Content).ToOption();
    //        return new Option<string>();
    //    }) + Converters.ToConverter<Block, Block.Paragraph>(heading1 =>
    //    {
    //        if (heading1.Text[0] is RichText.Text text)
    //            return Formatters.FormatParagraph(text.Content).ToOption();
    //        return new Option<string>();
    //    })
    //    //new RichTextTextConverter(
    //    //    Applicable.Bold(Formatters.FormatBold)
    //    //    + Applicable.Italic(Formatters.FormatItalic)
    //    //    + Applicable.Strikethrough(Formatters.FormatStike)
    //    //    + Applicable.Underline(Formatters.FormatUnderline)
    //    //    + Applicable.FormatCode(Formatters.FormatCode)
    //    //    + Applicable.FormatColor(Formatters.FormatColor))
    //    //+ Converters.ToConverter<RichText, RichText.Equation>(equation => $"`{equation.Expression}`".ToOption())
    //    //+ Converters.ToConverter((RichText richText) => richText switch
    //    //{
    //    //    RichText.Equation equation => $"`{equation.Expression}`".ToOption(),
    //    //    _ => default(string).ToOption()
    //    //})
    //    //+ new RichTextMentionUserConverter())
    //    ;
}


