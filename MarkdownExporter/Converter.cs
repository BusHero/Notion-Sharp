namespace MarkdownExporter;

public abstract partial class Converter
{
    public abstract Option<List<string>> Convert2(object? value, ConverterSettings? settings);

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

        public override Option<List<string>> Convert2(object? value, ConverterSettings? settings) => First.Convert2(value, settings) switch
        {
            Some<List<string>> foo => foo,
            _ => Second.Convert2(value, settings)
        };
    }
}


