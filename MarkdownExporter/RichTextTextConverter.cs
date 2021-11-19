using Notion.Model;

namespace MarkdownExporter;

public class RichTextTextConverter : IConverter<RichText>
{
    public IAplicable Applier { get; }

    public RichTextTextConverter(IAplicable aplicable) => Applier = aplicable ?? throw new ArgumentNullException(nameof(aplicable));

    public Option<string> Convert(RichText richText) => richText switch
    {
        RichText.Text text => Applier.Apply(richText, text.Content).ToOption(),
        _ => default(string).ToOption()
    };
}
