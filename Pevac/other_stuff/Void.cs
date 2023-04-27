using System;
using System.Diagnostics.CodeAnalysis;

namespace Pevac;

/// <summary>
/// Represents a type with a single value.
/// </summary>
public readonly struct Void : IEquatable<Void>
{
    /// <summary>
    /// Gets the single <see cref="Void"/> value.
    /// </summary>
    public static Void Default { get; } = default;

    #region Boilerplate

    /// <inheritdoc/>
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
    public static bool operator ==(Void left, Void right) => true;

    /// <inheritdoc/>
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
    public static bool operator !=(Void left, Void right) => false;

    /// <inheritdoc/>
    public bool Equals(Void other) => true;

    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Void;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => "()";

    #endregion
}
