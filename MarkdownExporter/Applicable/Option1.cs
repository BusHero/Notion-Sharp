namespace MarkdownExporter;

public static class Option
{
    public static Option<T> ToOption<T>(this T? item) => item switch
    {
        null => new Option<T>(),
        _ => new Option<T>(item)
    };
}