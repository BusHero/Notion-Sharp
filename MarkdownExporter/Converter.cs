namespace MarkdownExporter;

public abstract  class Converter
{
    public abstract Option<List<string?>> Convert(object? value, ConverterSettings? settings);

    public static Converter operator +(Converter first, Converter second) => new AggregateConverter(first, second);

    public static Option<List<string?>> Convert<T>(T item, ConverterSettings? settings) => settings switch
    {
        { Converter: var converter and not null } => converter.Convert(item, settings),
        _ => item?.ToString() switch
        {
            var text when text != null => new List<string?> { text }.ToOption(),
            _ => Option.None<List<string>>()
        }
    };

    public static Converter<T> ToConverter<T>(Func<T, ConverterSettings?, Option<List<string>>> converter) => new RelayConveter<T>(converter);
    
    private class RelayConveter<T> : Converter<T>
    {
        public RelayConveter(Func<T, ConverterSettings?, Option<List<string>>> converter)
        {
            Converter = converter ?? throw new ArgumentNullException(nameof(converter));
        }

        private Func<T, ConverterSettings?, Option<List<string>>> Converter { get; }

        public override Option<List<string?>> Convert(T t, ConverterSettings? settings) => Converter(t, settings);
    }

    private class AggregateConverter : Converter
    {
        public Converter First { get; }
        public Converter Second { get; }

        public AggregateConverter(Converter first, Converter second)
        {
            First = first;
            Second = second;
        }

        public override Option<List<string?>> Convert(object? value, ConverterSettings? settings) => First.Convert(value, settings) switch
        {
            Some<List<string>> foo => foo,
            _ => Second.Convert(value, settings)
        };
    }
}
