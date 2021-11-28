
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MarkdownExporter.Tests;

public class OptionComparer<T> : IEqualityComparer<Option<T>>
{
    public OptionComparer(IEqualityComparer<T> equalityComparer) => EqualityComparer = equalityComparer ?? throw new ArgumentNullException(nameof(equalityComparer));

    public OptionComparer() : this(EqualityComparer<T>.Default) { }

    public IEqualityComparer<T> EqualityComparer { get; }

    public bool Equals(Option<T>? x, Option<T>? y) => (x, y) switch
    {
        (null, null) or (None<T>, None<T>) => true,
        (Some<T> { Value: var first }, Some<T> { Value: var second }) => EqualityComparer.Equals(first, second),
        _ => false
    };

    public int GetHashCode([DisallowNull] Option<T> obj)
    {
        throw new NotImplementedException();
    }
}
