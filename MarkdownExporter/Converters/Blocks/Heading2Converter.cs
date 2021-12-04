using Notion.Model;

namespace MarkdownExporter;

public class Heading2Converter : Converter<Block.Heading2>
{
    private Converter<Block.Heading2> Converter { get; }

    public Heading2Converter(Func<string, string> formatter) =>
        Converter = new HeadingConverter<Block.Heading2>(
            h2 => h2.Text, 
            formatter);

    public Heading2Converter() : this(text => $"## {text}") { }

    public override Option<List<string>> Convert(Block.Heading2 heading2, ConverterSettings? settings) => 
        Converter
            .Convert(heading2, settings);
}
