namespace MarkdownExporter;

public partial class Converter
{
    public static IOption<string> Convert<T>(T item, ConverterSettings? settings) => settings switch
    {
        { Converter: var converter and not null } => converter.Convert(item, settings),
        _ => item switch
        {
            not null => Option.Some(item.ToString()),
            _ => Option.None<string>()
        }
    };

    public static IOption<List<string>> Convert2<T>(T item, ConverterSettings? settings) => settings switch
    {
        { Converter: var converter and not null } => converter.Convert2(item, settings),
        _ => item?.ToString() switch
        {
            var text when text != null => new List<string> { text }.ToOption(),
            _ => Option.None<List<string>>()
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
