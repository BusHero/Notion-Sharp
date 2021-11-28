using Notion.Model;

namespace MarkdownExporter;

public class RichTextEquationConverter : Converter<RichText>
{
    public override IOption<List<string>> Convert(RichText t, ConverterSettings? settings)
    {
        throw new NotImplementedException();
    }
}
