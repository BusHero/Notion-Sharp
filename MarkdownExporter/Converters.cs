namespace MarkdownExporter;

public static class Converters
{
    public static IConverter<object> GetConverter<T>(Func<T, string> converter) => new RelayConverter<T, object>(converter);

    internal record RelayConverter<T, U>(Func<T, string> Converter) : IConverter<U>
    {
        public Option<string> Convert(U input) => input switch
        {
            T t => Converter(t).ToOption(),
            _ => default(string).ToOption()
        };
    }
}
