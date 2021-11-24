namespace MarkdownExporter.Tests;

public abstract class ConverterTestsBase
{
    internal ConverterSettings Settings { get; } = new ConverterSettings
    {
        Converter =
                new TextConverter(
                    Applicable.Link(Formatters.FormatLink)
                    + Applicable.Bold(Formatters.FormatBold)
                    + Applicable.Italic(Formatters.FormatItalic)
                    + Applicable.Strikethrough(Formatters.FormatStike)
                    + Applicable.Underline(Formatters.FormatUnderline)
                    + Applicable.FormatCode(Formatters.FormatCode)
                    + Applicable.FormatColor(Formatters.FormatColor))
                + new ParagraphConverter()
                + new UserMentionConverter()
    };
}
