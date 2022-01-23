namespace Pevac;

using System;
using System.Text.Json;

public static partial class Parser
{
    /// <summary>
    /// Take the result of parsing, and project it ont a different domain.
    /// </summary>
    /// <typeparam name="TSource">The type of the element of <paramref name="source"/>.</typeparam>
    /// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
    /// <param name="source">A parser to invoke a transform function on.</param>
    /// <param name="selector">A transform function to apply to the value of parser.</param>
    /// <returns>A Parser whose result is the result of invokiung the transform function on the result of <paramref name="source"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
    public static Parser<TResult> Select<TSource, TResult>(this Parser<TSource> source, Func<TSource, TResult> selector) => (source, selector) switch
    {
        (null, _) => throw new ArgumentNullException(nameof(source)),
        (_, null) => throw new ArgumentNullException(nameof(selector)),
        _ => source.Then(t => Return(selector(t)))
    };

    /// <summary>
    /// Monadic combinator Then, adapted for Linq comprehension syntax
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <param name="parser"></param>
    /// <param name="selector"></param>
    /// <param name="projector"></param>
    /// <returns></returns>
    public static Parser<V> SelectMany<T, U, V>(this Parser<T> parser, Func<T, Parser<U>> selector, Func<T, U, V> projector) => (parser, selector, projector) switch
    {
        (null, _, _) => throw new ArgumentNullException(nameof(parser)),
        (_, null, _) => throw new ArgumentNullException(nameof(selector)),
        (_, _, null) => throw new ArgumentNullException(nameof(projector)),
        _ => parser.Then(t => selector(t).Select(u => projector(t, u)))
    };

    /// <summary>
    /// Succeds if the parsed value matches predicate
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parser"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static Parser<T?> Where<T>(this Parser<T?> parser, Func<T?, bool> predicate) => (parser, predicate) switch
    {
        (null, _) => throw new ArgumentNullException(nameof(parser)),
        (_, null) => throw new ArgumentNullException(nameof(predicate)),
        _ => (ref Utf8JsonReader reader, JsonSerializerOptions? options) => parser(ref reader, options) switch
        {
            Success<T?> success when !predicate(success.Value) => Result
            .Failure<T?>($"Unnexpected value {success.Value}"),
            var result => result,
        }
    };
}
