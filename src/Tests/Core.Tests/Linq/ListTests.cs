// -----------------------------------------------------------------------
// <copyright file="ListTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Linq;

public partial class ListTests
{
    private static IEnumerable<object[]> CreateReadOnlyLists<T>(params T[] a)
    {
        return CreateReadOnlyListsCore(a).Select(x => new object[] { x });

        static IEnumerable<IReadOnlyList<T>> CreateReadOnlyListsCore(T[] a)
        {
            yield return a;
            yield return new List<T>(a);
            yield return new List<T>(a).AsReadOnly();
        }
    }

    private static IEnumerable<object[]> CreateLists<T>(params T[] a) => CreateListsCore(a).Select(x => new object[] { x });

    private static IEnumerable<IList<T>> CreateListsCore<T>(params T[] a)
    {
        yield return a;
        yield return new List<T>(a);
        yield return new System.Collections.ObjectModel.Collection<T>(new List<T>(a));
    }
}