using Notion.Model;

using System.Text;

namespace MarkdownExporter;

public class Heading1Converter : Converter<Block.Heading1>
{
    public Heading1Converter(Func<string, string> formatter) => Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));

    public Heading1Converter(): this(text => $"# {text}") { }

    private Func<string, string> Formatter { get; }

    public override IOption<string> Convert(Block.Heading1 input, ConverterSettings? settings) => input.Text
        .Cast<RichText.Text>()
        .Select(text => Converter.Convert(text, settings))
        .Aggregate(
            new StringBuilder().ToOption(),
            (builder, text) => builder.SelectMany(builder => text.Select(builder.Append)))
        .Select(builder => builder.ToString())
        .Select(Formatter)
        .Select(text => text + "\n");

    public override IOption<List<string>> Convert2(Block.Heading1 heading1, ConverterSettings? settings) => heading1
        .Text
        .Select(text => Converter.Convert(text, settings))
        .Aggregate(
            new StringBuilder().ToOption(),
            (builder, text) => builder.SelectMany(builder => text.Select(builder.Append)))
        .Select(builder => builder.ToString())
        .Select(Formatter)
        .Select(text => new List<string> { text });
}
