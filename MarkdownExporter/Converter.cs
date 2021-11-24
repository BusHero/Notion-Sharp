namespace MarkdownExporter;

public abstract partial class Converter
{
    public abstract Option<string> Convert(object? value, ConverterSettings? settings);

    public static Converter operator +(Converter first, Converter second) => new AggregateConverter(first, second);

    private class AggregateConverter : Converter
    {
        public Converter First { get; }
        public Converter Second { get; }

        public AggregateConverter(Converter first, Converter second)
        {
            First = first;
            Second = second;
        }

        public override Option<string> Convert(object? value, ConverterSettings settings) => First
            .Convert(value, settings)
            .Blah(() => Second.Convert(value, settings));
    }
}

public abstract class Converter<T> : Converter
{
    public override Option<string> Convert(object? value, ConverterSettings settings) => value switch
    {
        T t => Convert(t, settings),
        _ => new Option<string>()
    };

    public abstract Option<string> Convert(T input, ConverterSettings? settings);
}
