using Notion.Model;

namespace MarkdownExporter;

public class TextConverter : Converter<RichText.Text>
{
    public IAplicable Applier { get; }

    public TextConverter(IAplicable aplicable) => Applier = aplicable ?? throw new ArgumentNullException(nameof(aplicable));

    public override Option<string> Convert(RichText.Text text, ConverterSettings? settings) => text switch
    {
        not null => Applier.Apply(text, text.Content).ToOption(),
        _ => default(string).ToOption()
    };
}
