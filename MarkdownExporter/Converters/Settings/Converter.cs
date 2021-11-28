namespace MarkdownExporter;

public partial class Converter
{
    public static Option<List<string>> Convert<T>(T item, ConverterSettings? settings) => settings switch
    {
        { Converter: var converter and not null } => converter.Convert2(item, settings),
        _ => item?.ToString() switch
        {
            var text when text != null => new List<string> { text }.ToOption(),
            _ => Option.None<List<string>>()
        }
    };
}
