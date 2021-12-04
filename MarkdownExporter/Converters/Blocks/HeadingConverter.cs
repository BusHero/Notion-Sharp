using Notion.Model;

namespace MarkdownExporter;

public class HeadingConverter<T> : Converter<T>
{
    public HeadingConverter(Func<T, RichText[]> richTextGetter, Func<string, string> formatter)
    {
        RichTextGetter = richTextGetter ?? throw new ArgumentNullException(nameof(richTextGetter));
        Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
    }

    private Func<T, RichText[]> RichTextGetter { get; }
    public Func<string, string> Formatter { get; }

    public override Option<List<string>> Convert(T t, ConverterSettings? settings) => RichTextGetter(t)
        .Select(text => Converter.Convert(text, settings))
        .Aggregate(Option.Binary<List<string>>(Lists.Add))
        .Select(Strings.Join)
        .Select(Formatter)
        .Select(Lists.Of);
}
