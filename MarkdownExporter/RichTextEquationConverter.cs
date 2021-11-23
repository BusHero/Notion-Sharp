using Notion.Model;

namespace MarkdownExporter;

public class RichTextEquationConverter : Converter<RichText>
{
    public override Option<string> Convert(RichText richText, ConverterSettings settings) => richText switch
    {
        RichText.Equation equation => $"`{equation.Expression}`".ToOption(),
        _ => default(string).ToOption()
    };
}
