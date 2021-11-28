using Notion;
using Notion.Model;

using System.Text;
using System.Linq;

namespace MarkdownExporter;

public class ParagraphConverter : Converter<Block.Paragraph>
{
    public ParagraphConverter(INotion notion) => Notion = notion ?? throw new ArgumentNullException(nameof(notion));

    public INotion Notion { get; }

    public override IOption<string> Convert(Block.Paragraph paragraph, ConverterSettings? settings)
    {
        IOption<string> option = paragraph.Text
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

    public override IOption<List<string>> Convert2(Block.Paragraph paragraph, ConverterSettings? settings)
    {
        var result = paragraph
            .Text
            .Select(text => Converter.Convert2(text, settings))
            .Aggregate((first, second) => first.Map2(second, ListExtensions.Add))
            .Select(list => new List<string> { string.Join("", list) });

        if (!paragraph.HasChildren)
            return result;

        var foo = Notion.GetBlocksChildrenAsync(paragraph.Id)
            .Result
            .Results;
        var bar = foo
            .Select(block => Converter.Convert2(block, settings))
            .ToList();
        var result2 = bar
            .Aggregate((first, second) => first.Map2(second, ListExtensions.Add))
            .Select(list => list.Select(item => $"&nbsp;&nbsp;&nbsp;&nbsp;{item}").ToList());

        return result.Map2(result2, ListExtensions.Add);
            //.SelectMany(list => result.Select(list2 => list2.Concat(list).ToList()));
    }

    //public override IOption<List<string>> Convert2(Block.Paragraph paragraph, ConverterSettings? settings)
    //{
    //    var option = paragraph.Text
    //        .Select(text => Converter.Convert2(text, settings))
    //        .Aggregate((first, second) => first + second);
    //                //.SelectMany(text => Converter.Convert2(text, settings))

    //                //.Aggregate(
    //                //new StringBuilder().ToOption(),
    //                //(builder, text) => builder.SelectMany(builder => text.Select(builder.Append)))
    //                //.Select(builder => builder.Append('\n'))
    //                //.Select(builder => builder.ToString())
    //                ;

    //    //if (!paragraph.HasChildren)
    //    //    return option;

    //    //option = Notion.GetBlocksChildrenAsync(paragraph.Id)
    //    //            .Result
    //    //            .Results
    //    //            .Select(x => Converter.Convert(x, settings))
    //    //            .Select(x => x.Select(y => "&nbsp;&nbsp;&nbsp;&nbsp;" + y))
    //    //            .Aggregate((x, y) => x.SelectMany(x1 => y.Select(y1 => x1 + y1)))
    //    //            .SelectMany(x1 => option.Select(y2 => y2 + x1));
    //    //return option;
    //}
}


public static class ListExtensions
{
    public static List<T> Add<T>(this List<T> first, List<T> second) => first.Concat(second).ToList();
}