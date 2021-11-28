
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MarkdownExporter.Tests;

public class ListSequenceComparer<T> : IEqualityComparer<List<T>>
{
    public bool Equals(List<T>? x, List<T>? y)
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);

        return x.Zip(y).All(tuple => tuple.First.Equals(tuple.Second));
    }

    public int GetHashCode([DisallowNull] List<T> obj)
    {
        throw new System.NotImplementedException();
    }
}
