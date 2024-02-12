// -----------------------------------------------------------------------
// <copyright file="ListTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Linq;

public partial class ListTests
{
    public static TheoryData<IReadOnlyList<int>> GetInt32Lists() => new(CreateReadOnlyLists(1, 5, 10, 15, 20));

    public static TheoryData<System.Collections.IEnumerable?, System.Collections.IEnumerable?> CreateNulls() => new() { { null, null } };

    public static TheoryData<IEnumerable<int>, IEnumerable<int>> GetLists()
    {
        var theoryData = new TheoryData<IEnumerable<int>, IEnumerable<int>>();

        foreach (var first in CreateListsCore(1, 2, 3, 4, 5))
        {
            foreach (var second in CreateListsCore(2, 3, 4))
            {
                theoryData.Add(first, second);
            }
        }

        foreach (var first in CreateListsCore(1, 2, 3, 4, 5))
        {
            foreach (var second in CreateReadOnlyListsCore(2, 3, 4))
            {
                theoryData.Add(first, second);
            }
        }

        foreach (var first in CreateReadOnlyListsCore(1, 2, 3, 4, 5))
        {
            foreach (var second in CreateListsCore(2, 3, 4))
            {
                theoryData.Add(first, second);
            }
        }

        foreach (var first in CreateReadOnlyListsCore(1, 2, 3, 4, 5))
        {
            foreach (var second in CreateReadOnlyListsCore(2, 3, 4))
            {
                theoryData.Add(first, second);
            }
        }

        return theoryData;
    }

    private static IEnumerable<IReadOnlyList<T>> CreateReadOnlyLists<T>(params T[] a) => CreateReadOnlyListsCore(a);

    private static IEnumerable<IList<T>> CreateLists<T>(params T[] a) => CreateListsCore(a);

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