using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Pevac;

public static partial class Parser
{
    /// <summary>
    /// Creates an updater.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <returns></returns>
    public static Parser<Func<U, U>> Updater<T, U>(this Parser<T> parser, Func<T, U, U> func) =>
        from t in parser
        select new Func<U, U>(u => func(t, u));

    /// <summary>
    /// Creates an updater.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="parser"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static Parser<Func<U, U>> Updater<T, U>(this Parser<T> parser, Action<T, U> action) =>
        from t in parser
        select new Func<U, U>(u =>
        {
            action(t, u);
            return u;
        });

    /// <summary>
    /// Creates an updater.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="parser"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static Parser<Func<U, U>> Updater<T, U>(this Parser<T> parser, Func<U, U> func) =>
        from _ in parser
        select func;

    /// <summary>
    /// Creates an updater.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="parser"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static Parser<Func<U, U>> Updater<T, U>(this Parser<T> parser, Action<U> action) =>
        from _ in parser
        select new Func<U, U>(u =>
        {
            action(u);
            return u;
        });

    /// <summary>
    /// Creates an identity updater
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="parser"></param>
    /// <returns></returns>
    public static Parser<Func<U, U>> Updater<T, U>(this Parser<T> parser) =>
        from _ in parser
        select new Func<U, U>(u => u);

    public static Parser<Func<T, T>> Updater<T>(this Parser<object> parser) => parser.Then(Return((T t) => t));

    /// <summary>
    /// Parses an object with fields
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parserSelector"></param>
    /// <param name="default"></param>
    /// <returns></returns>
    public static Parser<T> ParseObject<T>(Func<string, Parser<Func<T, T>>> parserSelector, T @default) => parserSelector switch
    {
        null => throw new ArgumentNullException(nameof(parserSelector)),
        not null => from updater in ParseObjectProperties(parserSelector)
                    select updater(@default)
    };

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parserSelector"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Parser<T> ParseObject<T>(Func<string, Parser<Func<T, T>>> parserSelector) where T : new() => parserSelector switch
    {
        null => throw new ArgumentNullException(nameof(parserSelector)),
        not null => from updater in ParseObjectProperties(parserSelector)
                    select updater(new T())
    };


    /// <summary>
    /// Parses an object with fields
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Parser<Func<T, T>> ParseObjectProperties<T>(Func<string, Parser<Func<T, T>>> parserSelector)
    {
        ArgumentNullException.ThrowIfNull(parserSelector);
        return (ref Utf8JsonReader reader, JsonSerializerOptions? options) =>
        {
            if (ParseCurrentToken(JsonTokenType.StartObject).Or(StartObjectToken)(ref reader, options) is IFailure<Func<T, T>> failure)
                return failure;
            var updaters = new List<Func<T, T>>();
            
            while (PropertyName(ref reader, options) is ISuccess<string>{Value: var propertyName})
            {
                switch (parserSelector(propertyName)(ref reader, options))
                {
                    case ISuccess<Func<T, T>> { Value: var value }:
                        updaters.Add(value);
                        break;
                    case IFailure<Func<T, T>> failure1:
                        return failure1;
                }
            }
            return (ParseCurrentToken(JsonTokenType.EndObject)(ref reader, options) switch
            {
                IFailure<Void> { Message: var message } => Result.Failure<Func<T, T>>(message),
                ISuccess<Void> => Result.Success((Func<T, T>?)(t => updaters.Aggregate(t, (data, updater) => updater(data)))),
                _ => throw new Exception("You should not see this")
            })!;
        };
    }

    /// <summary>
    /// Parses an object with fields.
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    /// <typeparam name="TChild"></typeparam>
    /// <param name="parserSelector"></param>
    /// <param name="cast"></param>
    /// <returns></returns>
    public static Parser<Func<TParent, TParent>> ParseObject<TParent, TChild>(
        Func<string, Parser<Func<TChild, TChild>>> parserSelector,
        Func<TParent, TChild> cast) where TChild : TParent =>
        from updater in ParseObjectProperties(parserSelector)
        select new Func<TParent, TParent>(parent => updater(cast(parent)));

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="message"></param>
    /// <returns></returns>
    public static Parser<Func<T, T>?> FailUpdate<T>(string message = "Some default message") => Failure<Func<T, T>>(message);

}
