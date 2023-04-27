using MarkdownExporter;

using Notion.Model;

namespace MarkdownExporter;

public abstract class BlockConverter<T> : Converter<T> where T : Block
{
    public override Option<List<string?>> Convert(T block, ConverterSettings? settings)
    {
        Option<List<string?>> result = GetText(block)
            .Select(text => Converter.Convert(text, settings))
            .Aggregate(Option.Binary<List<string>>(Lists.Add))
            .Select(Strings.Join)
            .Select(Format)
            .Select(Lists.Of);

        Option<List<string?>> children = GetChildren(block)
            .Select(block => Converter.Convert(block, settings))
            .Aggregate(new List<string>().ToOption(), (first, second) => first.Map2(second, Lists.Add))
            .Select(list => list.Select(FormatChild))
            .Select(Enumerable.ToList);

        return result.Combine(children, Lists.Add);
    }

    public abstract string Format(string text);
    public abstract string FormatChild(string text);
    public abstract Block[] GetChildren(T t);
    public abstract RichText[] GetText(T t);
}
