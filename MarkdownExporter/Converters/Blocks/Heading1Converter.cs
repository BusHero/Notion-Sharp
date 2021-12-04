using Notion.Model;

namespace MarkdownExporter;

public class Heading1Converter : Converter<Block.Heading1>
{
    public Converter<RichText[]> RichTextArrayConverter { get; }

    public Heading1Converter(Func<string, string> formatter) => RichTextArrayConverter = new RichTextArrayConverter(formatter);

    public Heading1Converter(): this(text => $"# {text}") { }

    public override Option<List<string>> Convert(Block.Heading1 heading1, ConverterSettings? settings) =>
        RichTextArrayConverter.Convert(heading1.Text, settings);
}
