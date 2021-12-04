using Notion.Model;

namespace MarkdownExporter;

public class Heading3Converter : Converter<Block.Heading3>
{
    public Heading3Converter(Func<string, string> formatter) => Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));

    public Heading3Converter() : this(text => $"### {text}") { }

    private Func<string, string> Formatter { get; }

    public override Option<List<string>> Convert(Block.Heading3 heading1, ConverterSettings? settings) => heading1
        .Text
        .Select(text => Converter.Convert(text, settings))
        .Aggregate(Option.Binary<List<string>>(Lists.Add))
        .Select(Strings.Join)
        .Select(Formatter)
        .Select(Lists.Of);
}