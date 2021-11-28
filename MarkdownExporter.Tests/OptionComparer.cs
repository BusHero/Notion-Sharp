
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MarkdownExporter.Tests;

public class OptionComparer<T> : IEqualityComparer<IOption<T>>
{
    public OptionComparer(IEqualityComparer<T> equalityComparer) => EqualityComparer = equalityComparer ?? throw new ArgumentNullException(nameof(equalityComparer));

    public OptionComparer() : this(EqualityComparer<T>.Default) { }

    public IEqualityComparer<T> EqualityComparer { get; }

    public bool Equals(IOption<T>? x, IOption<T>? y) => (x, y) switch
    {
        (null, null) or (INone<T>, INone<T>) => true,
        (ISome<T> { Value: var first }, ISome<T> { Value: var second }) => EqualityComparer.Equals(first, second),
        _ => false
    };

    public int GetHashCode([DisallowNull] IOption<T> obj)
    {
        throw new NotImplementedException();
    }
}
