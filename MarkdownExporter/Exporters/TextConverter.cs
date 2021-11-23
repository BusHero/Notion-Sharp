using Notion.Model;

namespace MarkdownExporter;

public class TextConverter : Converter<RichText>
{
    public IAplicable Applier { get; }

    public TextConverter(IAplicable aplicable) => Applier = aplicable ?? throw new ArgumentNullException(nameof(aplicable));

    public override Option<string> Convert(RichText richText, ConverterSettings? settings) => richText switch
    {
        RichText.Text text => Applier.Apply(richText, text.Content).ToOption(),
        _ => default(string).ToOption()
    };
}
