using System.Text.Json;
using System.Text.Json.Serialization;

namespace MarkdownExporter;

public abstract class Converter1
{
    public abstract Option<string> Convert(object value);
}

public abstract class Converter1<T> : Converter1
{
    public abstract Option<string> Convert(T value);

    public override Option<string> Convert(object value) => value switch
    {
        T t => Convert(t),
        _ => new()
    };
}

public interface IConverter
{
    public virtual Option<string> Convert(object value) => new();
}

public interface IConverter<in T> : IConverter
{
    public virtual Option<string> Convert(T input, Converter1 backup) => Convert(input);
    Option<string> Convert(T input);

    public static IConverter<T> operator +(IConverter<T> first, IConverter<T> second) => new AggregateConverter<T>(first, second);
}

public static class Converters
{
    public static IConverter<object> GetConverter<T>(Func<T, string> converter) => new RelayConverter<T, object>(converter);

    public static IConverter<T> ToConverter<T>(this Func<T, Option<string>> converter) => new RelayConverter2<T>(converter);

    public static IConverter<T> ToConverter<T>(
        Func<T, bool> Predicate,
        Func<T, Option<string>> WhenTrue,
        Func<T, Option<string>> WhenFalse) => new RelayConverter2<T>(input => Predicate(input) switch
        {
            true => WhenTrue(input),
            false => WhenFalse(input)
        });

    public static IConverter<T> ToConverter<T>(
        Func<T, bool> predicate,
        Func<T, Option<string>> whenTrue) => new RelayConverter2<T>(Foo(predicate, Foo(whenTrue)));

    public static IConverter<Parent> ToConverter<Parent, Child>(
        Func<Child, Option<string>> whenTrue) where Child : class, Parent => new RelayConverter2<Parent>(
            Foo((Parent x) => x is Child, Foo((Parent x) => whenTrue(x as Child))));

    internal record RelayConverter<T, U>(Func<T, string> Converter) : IConverter<U>
    {
        public Option<string> Convert(U input) => input switch
        {
            T t => Converter(t).ToOption(),
            _ => default(string).ToOption()
        };
    }

    public static Func<A, C> Foo<A, B, C>(Func<A, B> brancher, Func<B, Func<A, C>> branchGiver) => x => branchGiver(brancher(x))(x);

    public static Func<bool, Func<B, C>> Foo<B, C>(Func<B, C> whenTrue, Func<B, C> whenFalse) => x => x switch
    {
        true => whenTrue,
        false => whenFalse
    };

    public static Func<bool, Func<B, Option<C>>> Foo<B, C>(Func<B, Option<C>> whenTrue) => Foo<B, Option<C>>(
        whenTrue: whenTrue,
        whenFalse: _ => new Option<C>());

    private record RelayConverter2<T>(Func<T, Option<string>> Converter) : IConverter<T>
    {
        public Option<string> Convert(T input) => Converter(input);
    }

    private record RelayConverter3<T>(
        Func<T, bool> Predicate,
        Func<T, Option<string>> WhenTrue,
        Func<T, Option<string>> WhenFalse
        ) : IConverter<T>
    {
        Option<string> IConverter<T>.Convert(T input) => Predicate(input) switch
        {
            true => WhenTrue(input),
            false => WhenFalse(input)
        };
    }
}
