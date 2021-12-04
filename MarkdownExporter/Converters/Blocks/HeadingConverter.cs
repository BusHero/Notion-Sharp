using Notion.Model;

namespace MarkdownExporter;

public class HeadingConverter<T> : Converter<T>
{
    public HeadingConverter(Func<T, RichText[]> richTextGetter, Func<string, string> formatter)
    {
        RichTextGetter = richTextGetter ?? throw new ArgumentNullException(nameof(richTextGetter));
        RichTextArrayConverter = new RichTextArrayConverter(formatter);
    }

    private Func<T, RichText[]> RichTextGetter { get; }
    private Converter<RichText[]> RichTextArrayConverter { get; }

    public override Option<List<string>> Convert(T t, ConverterSettings? settings)
    {
        return RichTextArrayConverter.Convert(RichTextGetter(t), settings);
    }
}
