// -----------------------------------------------------------------------
// <copyright file="ListTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Linq;

public partial class ListTests
{
    public static IEnumerable<object?[]> CreateNulls(int count)
    {
        yield return Enumerable.Range(0, count).Select<int, object?>(_ => null).ToArray();
    }

    public static IEnumerable<object[]> GetLists()
    {
        foreach (var first in CreateListsCore(1, 2, 3, 4, 5))
        {
            foreach (var second in CreateListsCore(2, 3, 4))
            {
                yield return new object[] { first, second };
            }
        }

        foreach (var first in CreateListsCore(1, 2, 3, 4, 5))
        {
            foreach (var second in CreateReadOnlyListsCore(2, 3, 4))
            {
                yield return new object[] { first, second };
            }
        }

        foreach (var first in CreateReadOnlyListsCore(1, 2, 3, 4, 5))
        {
            foreach (var second in CreateListsCore(2, 3, 4))
            {
                yield return new object[] { first, second };
            }
        }

        foreach (var first in CreateReadOnlyListsCore(1, 2, 3, 4, 5))
        {
            foreach (var second in CreateReadOnlyListsCore(2, 3, 4))
            {
                yield return new object[] { first, second };
            }
        }
    }

    private static IEnumerable<object[]> CreateReadOnlyLists<T>(params T[] a) => CreateReadOnlyListsCore(a).Select(x => new object[] { x });

    private static IEnumerable<object[]> CreateLists<T>(params T[] a) => CreateListsCore(a).Select(x => new object[] { x });

    private static IEnumerable<IReadOnlyList<T>> CreateReadOnlyListsCore<T>(params T[] a)
    {
        yield return a;
        yield return new List<T>(a);
        yield return new List<T>(a).AsReadOnly();
    }

    private static IEnumerable<IList<T>> CreateListsCore<T>(params T[] a)
    {
        yield return a;
        yield return new List<T>(a);
        yield return new System.Collections.ObjectModel.Collection<T>(new List<T>(a));
    }
}