﻿using System;

namespace Pevac;

/// <summary>
/// Provides a set of static methods to simplify the interaction with the <see cref="Result{T}"/> class.
/// </summary>
public static partial class Result

{
    /// <summary>
    /// Generates a failed <see cref="Result{T}"/> with the message specified by the <paramref name="message"/>.
    /// </summary>
    /// <typeparam name="T">The type of the resulting <see cref="Result{T}"/>.</typeparam>
    /// <param name="message">The message of the result.</param>
    /// <returns>A result.</returns>
    public static IResult<T> Failure<T>(string message = "Some default message") => new Failure<T>(message);

    /// <summary>
    /// Creates a successful <see cref="Result{T}"/> with the <paramref name="value"/> as the result.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="value">The value of the result.</param>
    /// <returns>A result.</returns>
    public static IResult<T> Success<T>(T value) => new Success<T>(value);

    /// <summary>
    /// Matches a result
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <typeparam name="TU">The output value.</typeparam>
    /// <param name="result">The result on which to perform the match.</param>
    /// <param name="success">Function to invoke when result is successful.</param>
    /// <param name="failure">Function to invoke when result is a failure.</param>
    /// <returns></returns>
    /// <example>
    /// How to use the function
    /// <code>
    ///     string foo = result.Match(success: (int value) => $"{value}",
    ///                               failure: (string message) => "");
    /// </code>
    /// </example>
    public static TU? Match<T, TU>(this IResult<T?> result, Func<T?, TU?> success, Func<string, TU?> failure) => (result, success, failure) switch
    {
        (null, _, _) => throw new ArgumentNullException(nameof(result)),
        (_, null, _) => throw new ArgumentNullException(nameof(success)),
        (_, _, null) => throw new ArgumentNullException(nameof(failure)),
        (Success<T?> success1, _, _) => success(success1.Value),
        (Failure<T?> failure1, _, _) => failure(failure1.Message),
        _ => throw new ParseException("You shouldn't see this"),
    };

    /// <summary>
    /// Will try to get an alternative value in case the result is a failure
    /// </summary>
    /// <typeparam name="T">The type of the result</typeparam>
    /// <param name="result">The result.</param>
    /// <param name="alternative">A method that takes as input the error message and returns back an alternative value.</param>
    /// <returns>The value of the result, in case the result is successful, otherwise the result of invoking <paramref name="alternative"/> function.</returns>
    private static T? IfFailure<T>(this IResult<T?> result, Func<string, T?> alternative) => (result, alternative) switch
    {
        (null, _) => throw new ArgumentNullException(nameof(result)),
        (_, null) => throw new ArgumentNullException(nameof(alternative)),
        (Success<T?> success, _) => success.Value,
        (Failure<T?> failure, _) => alternative(failure.Message),
        _ => throw new ParseException("You shouldn't see this"),
    };


#pragma warning disable CS1574 // XML comment has cref attribute that could not be resolved
    /// <param name="alternative">A method that returns an alternative value.</param>
    /// <inheritdoc cref="IfFailure{T}(Result{T?}, Func{string, T?})"/>
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
    public static T? IfFailure<T>(this IResult<T?> result, Func<T?> alternative) => IfFailure(result, _ => alternative());
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
#pragma warning restore CS1574 // XML comment has cref attribute that could not be resolved


#pragma warning disable CS1574 // XML comment has cref attribute that could not be resolved
    /// <param name="alternative">The alternative value.</param>
    /// <returns>The value of the result, in case the result is successful, otherwise the <paramref name="alternative"/>.</returns>
    /// <inheritdoc cref="IfFailure{T}(Result{T?}, Func{string, T?})"/>
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
    public static T? IfFailure<T>(this IResult<T?> result, T alternative) => result.IfFailure(_ => alternative);
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
#pragma warning restore CS1574 // XML comment has cref attribute that could not be resolved

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TU"></typeparam>
    /// <param name="result"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public static IResult<TU?> IfSuccess<T, TU>(this IResult<T?> result, Func<T?, IResult<TU?>> next) => (result, next) switch
    {
        (null, _) => throw new ArgumentNullException(nameof(result)),
        (_, null) => throw new ArgumentNullException(nameof(next)),
        (ISuccess<T?> success, _) => next(success.Value),
        (IFailure<T?> failure, _) => Failure<TU>(failure.Message),
        _ => throw new ParseException("You shouldn't see this")
    };
}
