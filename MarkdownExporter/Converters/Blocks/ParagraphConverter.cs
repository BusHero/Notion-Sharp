using Notion;
using Notion.Model;

using System.Text;

namespace MarkdownExporter;

public class ParagraphConverter : Converter<Block.Paragraph>
{
    public ParagraphConverter(INotion notion) => Notion = notion ?? throw new ArgumentNullException(nameof(notion));

    public INotion Notion { get; }

    public override Option<string> Convert(Block.Paragraph paragraph, ConverterSettings? settings)
    {
        Option<string> option = paragraph.Text
                    .Select(text => Converter.Convert(text, settings))
                    .Aggregate(
                    new StringBuilder().ToOption(),
                    (builder, text) => builder.SelectMany(builder => text.Select(builder.Append)))
                    .Select(builder => builder.Append('\n'))
                    .Select(builder => builder.ToString());

        if (!paragraph.HasChildren)
            return option;

        option = Notion.GetBlocksChildrenAsync(paragraph.Id)
                    .Result
                    .Results
                    .Select(x => Converter.Convert(x, settings))
                    .Select(x => x.Select(y => "&nbsp;&nbsp;&nbsp;&nbsp;" + y))
                    .Aggregate((x, y) => x.SelectMany(x1 => y.Select(y1 => x1 + y1)))
                    .SelectMany(x1 => option.Select(y2 => y2 + x1));
        return option;
    }
}
