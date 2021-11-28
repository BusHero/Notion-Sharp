namespace MarkdownExporter;

public abstract partial class Converter
{
    public abstract IOption<List<string>> Convert2(object? value, ConverterSettings? settings);

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

        public override IOption<List<string>> Convert2(object? value, ConverterSettings? settings) => First.Convert2(value, settings) switch
        {
            ISome<List<string>> foo => foo,
            _ => Second.Convert2(value, settings)
        };
    }
}

public abstract class Converter<T> : Converter
{
    public override IOption<List<string>> Convert2(object? value, ConverterSettings? settings) => value switch
    {
        T t => Convert(t, settings),
        _ => Option.None<List<string>>()
    };
    public abstract IOption<List<string>> Convert(T t, ConverterSettings? settings);
}
