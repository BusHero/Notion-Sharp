using Notion.Model;

namespace MarkdownExporter;

public class RichTextConverter : Converter<RichText>
{
    public IAplicable Applier { get; }

    public RichTextConverter(IAplicable aplicable) => Applier = aplicable ?? throw new ArgumentNullException(nameof(aplicable));

    public override Option<string> Convert(RichText text, ConverterSettings? settings) => text switch
    {
        not null => Applier.Apply(text, text.PlainText).ToOption(),
        _ => default(string).ToOption()
    };
}
