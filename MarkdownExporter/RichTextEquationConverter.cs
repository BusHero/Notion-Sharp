using Notion.Model;

namespace MarkdownExporter;

public class RichTextEquationConverter : Converter<RichText>
{
    public override IOption<string> Convert(RichText richText, ConverterSettings settings) => richText switch
    {
        RichText.Equation equation => $"`{equation.Expression}`".ToOption(),
        _ => default(string).ToOption()
    };

    public override IOption<List<string>> Convert2(RichText t, ConverterSettings? settings)
    {
        throw new NotImplementedException();
    }
}
