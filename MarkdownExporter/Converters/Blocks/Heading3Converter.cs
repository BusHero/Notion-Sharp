using Notion.Model;

namespace MarkdownExporter;

public class Heading3Converter : Converter<Block.Heading3>
{
    private Converter<RichText[]> RichTextArrayConverter { get; }

    public Heading3Converter(Func<string, string> formatter) => RichTextArrayConverter = new RichTextArrayConverter(formatter);

    public Heading3Converter() : this(text => $"### {text}") { }

    public override Option<List<string>> Convert(Block.Heading3 heading1, ConverterSettings? settings) =>
        RichTextArrayConverter.Convert(heading1.Text, settings);
}
