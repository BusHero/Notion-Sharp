using Notion.Model;

namespace MarkdownExporter;

public class Heading1Converter : Converter<Block.Heading1>
{
    public Heading1Converter(Func<string, string> formatter) => Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));

    public Heading1Converter(): this(text => $"# {text}") { }

    private Func<string, string> Formatter { get; }

    public override Option<List<string>> Convert(Block.Heading1 heading1, ConverterSettings? settings) => heading1
        .Text
        .Select(text => Converter.Convert(text, settings))
        .Aggregate(Option.Binary<List<string>>(Lists.Add))
        .Select(Strings.Join)
        .Select(Formatter)
        .Select(Lists.Of);
}

public static class Funcs
{
    public static Func<T, U, V> AsFunc<T, U, V>(Func<T, U, V> func) => func;
}