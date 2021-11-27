namespace MarkdownExporter;

public partial class Converter
{
    public static Option<string> Convert<T>(T item, ConverterSettings? settings) => settings switch
    {
        { Converter: var converter and not null } => converter.Convert(item, settings),
        _ => item switch
        {
            not null => new Option<string>(item.ToString()),
            _ => new Option<string>()
        }
    };

    public static string? Convert<T>(T item, ConverterSettings? settings, string? defaultValue) => settings switch
    {
        { Converter: var converter and not null } => converter.Convert(item, settings).ValueOrDefault(defaultValue),
        _ => item switch
        {
            not null => item.ToString(),
            _ => default
        }
    };
}
