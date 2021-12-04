using Notion.Model;

namespace MarkdownExporter;

public class Heading3Converter : Converter<Block.Heading3>
{
    private Converter<Block.Heading3> Converter { get; }

    public Heading3Converter(Func<string, string> formatter) =>
        Converter = new HeadingConverter<Block.Heading3>(
            h3 => h3.Text,
            formatter);

    public Heading3Converter() : this(text => $"### {text}") { }

    public override Option<List<string>> Convert(Block.Heading3 heading3, ConverterSettings? settings) =>
            Converter.Convert(heading3, settings);
}
