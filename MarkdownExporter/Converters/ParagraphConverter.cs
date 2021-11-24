using Notion.Model;

using System.Text;

namespace MarkdownExporter;

public class ParagraphConverter : Converter<Block.Paragraph>
{
    public override Option<string> Convert(Block.Paragraph input, ConverterSettings? settings) => input.Text.Cast<RichText.Text>()
        .Select(text => Converter.Convert(text, settings))
        .Aggregate(
            new StringBuilder().ToOption(),
            (builder, text) => builder.SelectMany(builder => text.Select(builder.Append)))
        .Select(builder => builder.Append('\n'))
        .Select(builder => builder.ToString());
}
