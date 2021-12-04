using Notion.Model;

namespace MarkdownExporter;

public class RichTextArrayConverter : Converter<RichText[]>
{
    public RichTextArrayConverter(Func<string, string> formatter) => Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));

    public Func<string, string> Formatter { get; }

    public override Option<List<string>> Convert(RichText[] text, ConverterSettings? settings) =>
        text
        .Select(text => Converter.Convert(text, settings))
        .Aggregate(Option.Binary<List<string>>(Lists.Add))
        .Select(Strings.Join)
        .Select(Formatter)
        .Select(Lists.Of);

}