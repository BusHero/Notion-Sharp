namespace MarkdownExporter;

public interface IOption<out T> 
{
    public static IOption<T> operator +(IOption<T> first, IOption<T> second) => default;
}

public interface INone<out T> : IOption<T> { }

public interface ISome<out T>: IOption<T> 
{
    public T Value { get; }
}

public static class Option
{
    private record SomeInstance<T>(T Value) : ISome<T> { }
    private record NoneInstance<T>() : INone<T> 
    {
        public static NoneInstance<T> Default { get; } = new NoneInstance<T>();
    }

    public static IOption<T> Some<T>(T item) => new SomeInstance<T>(item);

    public static IOption<T> None<T>() => NoneInstance<T>.Default;

    public static IOption<T> ToOption<T>(this T? item) => item switch
    {
        null => None<T>(),
        _ => Some<T>(item)
    };

    public static IOption<U> Select<T, U>(this IOption<T> option, Func<T, U> map) => option switch
    {
        ISome<T> { Value: var value} => Some(map(value)),
        _ => None<U>()
    };

    public static IOption<U> SelectMany<T, U>(this IOption<T> option, Func<T, IOption<U>> selector) => option switch
    {
        ISome<T> { Value: var value} => selector(value).Select(x => x),
        _ => None<U>()
    };

    public static IOption<V> SelectMany<T, U, V>(this IOption<T> option, Func<T, IOption<U>> selector, Func<T, U, V> projector) => option switch
    {
        ISome<T> { Value: var value} => selector(value).Select(u => projector(value, u)),
        _ => None<V>()
    };

    public static IOption<V> Map2<T, U, V>(this IOption<T> first, IOption<U> second, Func<T, U, V> map) => (first, second, map) switch
    {
        (null, _, _) => throw new ArgumentNullException(nameof(first)),
        (_, null, _) => throw new ArgumentNullException(nameof(second)),
        (_, _, null) => throw new ArgumentNullException(nameof(map)),
        (ISome<T> { Value: var firstValue }, ISome<U> { Value: var secondValue }, _) => map(firstValue, secondValue).ToOption(),
        _ => None<V>()
    };

    public static IOption<T> Binary<T>(this IOption<T> first, IOption<T> second, Func<T, T, T> map) => first.Map2(second, map);

    public static Func<IOption<T>, IOption<U>, IOption<V>> Map2<T, U, V>(Func<T, U, V> map) => (first, second) => first.Map2(second, map);

    public static Func<IOption<T>, IOption<T>, IOption<T>> Binary<T>(Func<T, T, T> map) => Map2(map);

    public static T? ValueOrDefault<T>(this IOption<T> option, T @default) => option switch
    {
        null => throw new ArgumentNullException(nameof(option)),
        ISome<T> { Value: var value } => value,
        _ => @default
    };
    public static IOption<T> Blah<T>(this IOption<T> option, Func<IOption<T>> @default) => option switch
    {
        ISome<T> => option,
        _ => @default()
    };

    public static bool Equals<T>(this Option<T> first, Option<T> second, EqualityComparer<T> comparer) => (first, second) switch
    {
        (ISome<T> { Value: var firstValue }, ISome<T> { Value: var secondValue }) => comparer.Equals(firstValue, secondValue),
        _ => false
    };
}