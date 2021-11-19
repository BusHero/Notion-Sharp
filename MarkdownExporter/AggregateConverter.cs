namespace MarkdownExporter;

public class AggregateConverter<T> : IConverter<T>
{
    private IConverter<T> First { get; }
    private IConverter<T> Second { get; }

    public AggregateConverter(IConverter<T> first, IConverter<T> second)
    {
        First = first;
        Second = second;
    }

    public Option<string> Convert(T input) => First.Convert(input).Blah(() => Second.Convert(input));
}
