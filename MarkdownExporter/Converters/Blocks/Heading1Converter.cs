using Notion.Model;

using System.Text;

namespace MarkdownExporter;

public class Heading1Converter : Converter<Block.Heading1>
{
    public Heading1Converter(Func<string, string> formatter) => Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));

    public Heading1Converter(): this(text => $"# {text}") { }

    private Func<string, string> Formatter { get; }

    public override IOption<List<string>> Convert(Block.Heading1 heading1, ConverterSettings? settings) => heading1
        .Text
        .Select(text => Converter.Convert(text, settings))
        .Aggregate((first, second) => first.Map2(second, ListExtensions.Add))
        .Select(list => new List<string> { "# " + string.Join("", list) });
}
