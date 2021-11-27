using Notion.Model;

using System.Text;

namespace MarkdownExporter;

public class Heading1Converter : Converter<Block.Heading1>
{
    public override Option<string> Convert(Block.Heading1 input, ConverterSettings? settings) => input.Text
        .Cast<RichText.Text>()
        .Select(text => Converter.Convert(text, settings))
        .Aggregate(
            new StringBuilder().ToOption(),
            (builder, text) => builder.SelectMany(builder => text.Select(builder.Append)))
        .Select(builder => builder.ToString())
        .Select(text => $"# {text}")
        .Select(text => text + "\n");
}
