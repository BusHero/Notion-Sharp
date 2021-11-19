using Notion.Model;

namespace MarkdownExporter;

public class RichTextEquationConverter : IConverter<RichText>
{
    public Option<string> Convert(RichText richText) => richText switch
    {
        RichText.Equation equation => $"`{equation.Expression}`".ToOption(),
        _ => default(string).ToOption()
    };
}
