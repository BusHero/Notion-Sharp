using Notion.Model;

namespace MarkdownExporter;

public class Heading1Converter : Converter<Block.Heading1>
{
    private Converter<Block.Heading1> Converter { get; }

    public Heading1Converter(Func<string, string> formatter) =>
        Converter = new HeadingConverter<Block.Heading1>(h1 => h1.Text, formatter);

    public Heading1Converter(): this(text => $"# {text}") { }

    public override Option<List<string>> Convert(Block.Heading1 heading1, ConverterSettings? settings) =>
        Converter.Convert(heading1, settings);
}
