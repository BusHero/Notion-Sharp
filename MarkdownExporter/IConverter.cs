namespace MarkdownExporter;

public interface IConverter<in T>
{
    Option<string> Convert(T input);

    public static IConverter<T> operator +(IConverter<T> first, IConverter<T> second) => new AggregateConverter<T>(first, second);
}
