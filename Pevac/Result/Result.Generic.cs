namespace Pevac;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IResult<out T> { }

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ISuccess<out T> : IResult<T>
{
    /// <summary>
    /// 
    /// </summary>
    T Value { get; }
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IFailure<out T> : IResult<T>
{
    /// <summary>
    /// 
    /// </summary>
    string Message { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <returns></returns>
    IFailure<U> Repack<U>();
}

/// <summary>
/// Represents a result.
/// </summary>
/// <typeparam name="T">The type of the result.</typeparam>
public abstract record Result<T> : IResult<T>
{
    internal Result() { }
}

/// <summary>
/// Represents a succesfull result.
/// </summary>
/// <typeparam name="T">The type of the result.</typeparam>
public record Success<T>(T Value) : Result<T>, ISuccess<T>;

/// <summary>
/// Represents a failed result.
/// </summary>
/// <typeparam name="T">The type of the result.</typeparam>
public record Failure<T>(string Message) : Result<T>, IFailure<T>
{
    /// <summary>
    /// Repack the failure.
    /// </summary>
    public Failure<U> Repack<U>() => new(Message);

    IFailure<U> IFailure<T>.Repack<U>() => Repack<U>();
}
