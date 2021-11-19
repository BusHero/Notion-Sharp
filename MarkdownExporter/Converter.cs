namespace MarkdownExporter;

public abstract class Converter<T> : Converter
{
    public abstract string? Convert(T input);
}
