namespace Pevac;

using System;
using System.Linq;
using System.Text.Json;


/// <summary>
/// 
/// </summary>
/// <param name="reader"></param>
/// <returns></returns>
public delegate bool Predicate(Utf8JsonReader reader);

/// <summary>
/// Contains a bunch of utilities.
/// </summary>
public static partial class Parser
{
    /// <summary>
    /// Creates a parser that succeds if the current token is any of <paramref name="tokens"/>, otherwise fails.
    /// </summary>
    /// <param name="tokens"></param>
    /// <returns></returns>
    public static Parser<Void> ParseToken(params JsonTokenType[] tokens) => (ref Utf8JsonReader reader, JsonSerializerOptions? _) =>
    {
        return reader.Read() switch
        {
            false => Result.Failure<Void>("Cannot read the next token. You've probably reached the end of stream"),
            true when tokens.Contains(reader.TokenType) => Result.Success(Void.Default),
            _ => Result.Failure<Void>($"Wrong token! Expected [{string.Join(',', tokens)}]; Actual {reader.TokenType}"),
        };
    };

    /// <summary>
    /// 
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Parser<Void> ParsePredicate(Predicate predicate) => (ref Utf8JsonReader reader, JsonSerializerOptions? _) =>
    {
        var foo = predicate switch
        {
            null => throw new ArgumentNullException(nameof(predicate)),
            not null => predicate(reader) switch
            {
                true => Result.Success(Void.Default),
                false => Result.Failure<Void>("Predicate failed")
            }
        };
        return foo;
    };

    /// <summary>
    /// 
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public static Parser<Void> ParseCurrentToken(JsonTokenType token) => ParsePredicate(reader => reader.TokenType == token);

    /// <summary>
    /// Creates a parser that succeds if a object of type <typeparamref name="T"/> 
    /// can be obtained from the current position of the <see cref="Utf8JsonReader"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Parser<T?> ParseType<T>() => (ref Utf8JsonReader reader, JsonSerializerOptions? options) =>
    {
        try
        {
            var result = JsonSerializer.Deserialize<T>(ref reader, options);
            return Result.Success(result);
        }
        catch (Exception e)
        {
            return Result.Failure<T>(e.Message);
        }
    };

    /// <summary>
    /// Creates a parser that succeds if the current position of the reader contains a value that.
    /// </summary>
    /// <param name="expectedValue"></param>
    /// <returns></returns>
    public static Parser<string?> ParseString(params string[] expectedValue) => (ref Utf8JsonReader reader, JsonSerializerOptions? options) =>
    {
        return String(ref reader, options) switch
        {
            Success<string?> success when !expectedValue.Contains(success.Value) =>
                Result
            .Failure<string>($"\"{expectedValue}\" was expected, but \"{success.Value}\" recieved"),
            var result => result
        };
    };

    private static Parser<Void>? ignored;
    
    
    public static Parser<Void> Ignored => ignored ??= (ref Utf8JsonReader reader, JsonSerializerOptions? options) =>
    {
        return reader.TrySkip() switch
        {
            true => Pevac.Result.Success(Void.Default),
            false => Pevac.Result.Failure<Void>("Cannot skip")
        };
    };
}
