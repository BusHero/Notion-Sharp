using Notion.Model;

namespace MarkdownExporter;

public class Heading2Converter : Converter<Block.Heading2>
{
    public Heading2Converter(Func<string, string> formatter) => Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));

    public Heading2Converter() : this(text => $"## {text}") { }

    private Func<string, string> Formatter { get; }

    public override Option<List<string>> Convert(Block.Heading2 heading1, ConverterSettings? settings) => heading1
        .Text
        .Select(text => Converter.Convert(text, settings))
        .Aggregate(Option.Binary<List<string>>(Lists.Add))
        .Select(Strings.Join)
        .Select(Formatter)
        .Select(Lists.Of);
}
