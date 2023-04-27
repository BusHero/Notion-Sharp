namespace MarkdownExporter;

public class CompositeConverter : Converter
{
    public CompositeConverter(params Converter[] converters)
    {
        Converters = converters?.ToList() ?? throw new ArgumentNullException(nameof(converters));
    }

    public CompositeConverter(List<Converter> converters)
    {
        Converters = converters?.ToList() ?? throw new ArgumentNullException(nameof(converters));
    }

    public List<Converter> Converters { get; }

    public override Option<List<string?>> Convert(object? value, ConverterSettings? settings)
    {
        foreach (var converter in Converters)
        {
            var result = converter.Convert(value, settings);
            if (result is Some<List<string>>)
                return result;
        }
        return Option.None<List<string>>();
    }
}
