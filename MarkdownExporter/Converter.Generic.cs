namespace MarkdownExporter;

public abstract class Converter<T> : Converter
{
    public override Option<List<string?>> Convert(object? value, ConverterSettings? settings) => value switch
    {
        T t => Convert(t, settings),
        _ => Option.None<List<string>>()
    };
    public abstract Option<List<string?>> Convert(T t, ConverterSettings? settings);
}