namespace Pevac;

using System;

/// <summary>
/// An exception that will be thrown in unexpected situations(aka. What a fuck?).
/// </summary>
public sealed class ParseException : Exception
{
    internal ParseException(string? message = default) : base(message) { }
}
