namespace MarkdownExporter;

public class Option<T>
{
    private T Value { get; }

    private bool HasValue { get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Option()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        HasValue = false;
    }

    public Option(T value)
    {
        Value = value;
        HasValue = true;
    }

    public T ValueOrDefault(T @default)
    {
        return HasValue ? Value : @default;
    }

    public Option<T> Blah(Func<Option<T>> @default) => HasValue ? this : @default();

    public Option<U> Select<U>(Func<T, U> map) => HasValue switch
    {
        true => new Option<U>(map(Value)),
        false => new Option<U>()
    };
}
