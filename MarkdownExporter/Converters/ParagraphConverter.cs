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
        .Select(builder => builder.ToString());
}

public static class Options
{
    public static void Foo(Option<StringBuilder> first, Option<string> second)
    {
        var foo = first.Select(builder => second.Select(text => builder.Append(text)));
    }
}