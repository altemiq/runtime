// -----------------------------------------------------------------------
// <copyright file="ListTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Linq;

public partial class ListTests
{
    [Fact]
    public void Cast()
    {
        IList<ISecond> list = [default(Third), default(Third), default(Third), default(Third), default(Third)];
        var cast = List.Cast<ISecond, IFirst>(list);
        Assert.Equal(list, cast);
    }

    [Fact]
    public void CastList()
    {
        List<ISecond> list = [default(Third), default(Third), default(Third), default(Third), default(Third)];
        var cast = List.Cast<ISecond, IFirst>(list);
        Assert.Equal(list, cast);
    }

    [Fact]
    public void CastListWithNoConstructors()
    {
        ListWithNoConstructors<ISecond> list = [default(Third), default(Third), default(Third), default(Third), default(Third)];
        var cast = List.Cast<ISecond, IFirst>(list);
        Assert.Equal(list, Assert.IsType<ListWithNoConstructors<IFirst>>(cast));
    }

    [Fact]
    public void CastNonListWithNoConstructors()
    {
        NonListWithNoConstructors<ISecond> list = [default(Third), default(Third), default(Third), default(Third), default(Third)];
        var cast = List.Cast<ISecond, IFirst>(list);
        Assert.Equal(list, Assert.IsType<NonListWithNoConstructors<IFirst>>(cast));
    }

    [Fact]
    public void CastNested()
    {
        NestedNonGenericList list = [default(Third), default(Third), default(Third), default(Third), default(Third)];
        var cast = List.Cast<ISecond, IFirst>(list);
        Assert.Equal(list, Assert.IsType<List<IFirst>>(cast));
    }

    [Fact]
    public void CastArray()
    {
        ISecond[] array = [default(Third), default(Third), default(Third), default(Third), default(Third)];
        var cast = List.Cast<ISecond, IFirst>(array);
        Assert.Equal(array, Assert.IsType<IFirst[]>(cast));
    }

    [Fact]
    public void CastWithNull()
    {
        IList<ISecond>? list = default;
        Assert.Null(List.Cast<ISecond, IFirst>(list!));
    }

    [Fact]
    public void CastWithNonGeneric()
    {
        NoBaseClass list = [default(Third), default(Third), default(Third), default(Third), default(Third)];
        Assert.Throws<InvalidOperationException>(() => list.Cast<ISecond, IFirst>());
    }

    public static TheoryData<IReadOnlyList<int>> GetInt32ReadOnlyLists() => new(CreateReadOnlyLists(1, 5, 10, 15, 20));

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

    private interface IFirst;

    private interface ISecond : IFirst;

    private readonly struct Third : ISecond;

    private static TResult? TestListList<T, TResult>(object first, object second, Func<IList<T>, IList<T>, TResult> func, TResult? defaultResult = default) => first is IList<T> f && second is IList<T> s ? func(f, s) : defaultResult;

    private static TResult? TestReadOnlyListReadOnlyList<T, TResult>(object first, object second, Func<IReadOnlyList<T>, IReadOnlyList<T>, TResult> func, TResult? defaultResult = default) => first is IReadOnlyList<T> f && second is IReadOnlyList<T> s ? func(f, s) : defaultResult;

    private sealed class ListWithNoConstructors<T> : List<T>;

    private sealed class NonListWithNoConstructors<T> : System.Collections.ObjectModel.Collection<T>;

    private sealed class NestedNonGenericList : List<ISecond>;

    private sealed class NoBaseClass : IList<ISecond>
    {
        ISecond IList<ISecond>.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        int ICollection<ISecond>.Count { get; }
        bool ICollection<ISecond>.IsReadOnly { get; }

        public void Add(ISecond item) { }
        void ICollection<ISecond>.Clear() => throw new NotImplementedException();
        bool ICollection<ISecond>.Contains(ISecond item) => throw new NotImplementedException();
        void ICollection<ISecond>.CopyTo(ISecond[] array, int arrayIndex) => throw new NotImplementedException();
        IEnumerator<ISecond> IEnumerable<ISecond>.GetEnumerator() => throw new NotImplementedException();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => throw new NotImplementedException();
        int IList<ISecond>.IndexOf(ISecond item) => throw new NotImplementedException();
        void IList<ISecond>.Insert(int index, ISecond item) => throw new NotImplementedException();
        bool ICollection<ISecond>.Remove(ISecond item) => throw new NotImplementedException();
        void IList<ISecond>.RemoveAt(int index) => throw new NotImplementedException();
    }
}