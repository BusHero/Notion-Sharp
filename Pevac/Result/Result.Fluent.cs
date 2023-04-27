using System;

namespace Pevac;

public static partial class Result

{
    /// <summary>
    /// Specifies a transform function that will be invoked if the result is succesful. It's a part of the fluent interface. 
    /// </summary>
    /// <typeparam name="T">The type of the source.</typeparam>
    /// <typeparam name="U">The type of the result.</typeparam>
    /// <param name="result">The result object on which to invoke the the transform function.</param>
    /// <param name="success">A transform function to be invoked if the result is succesfull.</param>
    /// <example>
    /// This shows how the method is supposed to be used.
    /// <code>
    ///     string foo = bar
    ///         .IsSuccess((int number) => $"{number}")
    ///         .IsFailure((string message) => "");
    /// </code>
    /// </example>
    public static ISuccess<T?, U?> Success<T, U>(this Result<T?> result, Func<T?, U?> success) => (result, success) switch
    {
        (null, _) => throw new ArgumentNullException(nameof(result)),
        (_, null) => throw new ArgumentNullException(nameof(success)),
        _ => new IntermediateResult<T?, U?>(Flag.Success, result, success: success)
    };

    /// <summary>
    /// Specifies a transform function that will be invoked if the result is failure. It's a part of the fluent interface. 
    /// </summary>
    /// <typeparam name="T">The type of the source.</typeparam>
    /// <typeparam name="U">The type of the result.</typeparam>
    /// <param name="result">The result object on which to invoke the the transform function.</param>
    /// <param name="failure">A transform function to be invoked if the result is a failure.</param>
    /// <example>
    /// This shows how the method is supposed to be used.
    /// <code>
    ///     string foo = bar
    ///         .IsFailure((string message) => "");
    ///         .IsSuccess((int number) => $"{number}")
    /// </code>
    /// </example>
    public static IFailure<T?, U?> Failure<T, U>(this Result<T?> result, Func<string, U?> failure) => (result, failure) switch
    {
        (null, _) => throw new ArgumentNullException(nameof(result)),
        (_, null) => throw new ArgumentNullException(nameof(failure)),
        _ => new IntermediateResult<T?, U?>(Flag.Failure, result, failure: failure)
    };


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public interface ISuccess<T, U>
    {
        /// <summary>
        /// A method to specify a failure method
        /// </summary>
        /// <param name="failure">A method that will be invoked if the result is a failure</param>
        /// <returns>The result of the evaluation.</returns>
        U? Failure(Func<string, U?> failure);
    }

    /// <summary>
    /// Allows to specify the function to be invoked when the result is a success.
    /// </summary>
    /// <typeparam name="T">Type of the input.</typeparam>
    /// <typeparam name="U">Type of the output.</typeparam>
    public interface IFailure<T, U>
    {
        /// <summary>
        /// Specifies a function to be invoked when the result is a success.
        /// </summary>
        /// <param name="success">The function to be invoked when the result is a success.</param>
        /// <returns></returns>
        U? Success(Func<T?, U?> success);
    }

    #region Implementation Details

    private enum Flag
    {
        Success, Failure
    }

    private class IntermediateResult<T, U> : ISuccess<T?, U?>, IFailure<T?, U?>
    {
        private Flag Flag { get; }
        private Result<T?> Result { get; }
        private Func<string, U?>? Failure { get; }
        private Func<T?, U?>? Success { get; }

        public IntermediateResult(Flag flag, Result<T?> result, Func<T?, U?>? success = default, Func<string, U?>? failure = default)
        {
            (Flag, Result, Success, Failure) = (flag, result, success, failure) switch
            {
                (_, null, _, _) => throw new ArgumentNullException(nameof(result)),
                (Flag.Success, _, null, _) => throw new ArgumentNullException(nameof(success)),
                (Flag.Success, _, _, _) => (flag, result, success, default(Func<string, U?>)),
                (Flag.Failure, _, _, null) => throw new ArgumentNullException(nameof(failure)),
                (Flag.Failure, _, _, _) => (flag, result, default(Func<T?, U?>), failure),
                _ => throw new ArgumentException("Unexpected flag", nameof(flag))
            };
        }

        U? ISuccess<T?, U?>.Failure(Func<string, U?> failure) => (Flag, failure) switch
        {
            (_, null) => throw new ArgumentNullException(nameof(failure)),
#pragma warning disable CS8604 // Possible null reference argument.
            (Flag.Success, _) => Result.Match(success: Success, failure: failure),
#pragma warning restore CS8604 // Possible null reference argument.
            _ => throw new InvalidOperationException("Cannot perform this operation")
        };

        U? IFailure<T?, U?>.Success(Func<T?, U?> success) => (Flag, success) switch
        {
            (_, null) => throw new ArgumentNullException(nameof(success)),
#pragma warning disable CS8604 // Possible null reference argument.
            (Flag.Failure, _) => Result.Match(success: success, failure: Failure),
#pragma warning restore CS8604 // Possible null reference argument.
            _ => throw new InvalidOperationException("Cannot perform this operation")
        };
    }

    #endregion
}
