// -----------------------------------------------------------------------
// <copyright file="ListTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Linq;

using TUnit.Assertions.AssertConditions.Throws;

public partial class ListTests
{
    [Test]
    public async Task Cast()
    {
        IList<ISecond> list = [default(Third), default(Third), default(Third), default(Third), default(Third)];
        await Assert.That(list.Cast<ISecond, IFirst>()).IsEquivalentTo(list);
    }

    [Test]
    public async Task CastList()
    {
        List<ISecond> list = [default(Third), default(Third), default(Third), default(Third), default(Third)];
        await Assert.That(list.Cast<ISecond, IFirst>()).IsEquivalentTo(list);
    }

    [Test]
    public async Task CastListWithNoConstructors()
    {
        ListWithNoConstructors<ISecond> list = [default(Third), default(Third), default(Third), default(Third), default(Third)];
        await Assert.That(list.Cast<ISecond, IFirst>()).IsTypeOf<ListWithNoConstructors<IFirst>>().And.IsEquivalentTo(list);
    }

    [Test]
    public async Task CastNonListWithNoConstructors()
    {
        NonListWithNoConstructors<ISecond> list = [default(Third), default(Third), default(Third), default(Third), default(Third)];
        await Assert.That(list.Cast<ISecond, IFirst>()).IsTypeOf<NonListWithNoConstructors<IFirst>>().And.IsEquivalentTo(list);
    }

    [Test]
    public async Task CastNested()
    {
        NestedNonGenericList list = [default(Third), default(Third), default(Third), default(Third), default(Third)];
        await Assert.That(list.Cast<ISecond, IFirst>()).IsTypeOf<List<IFirst>>().And.IsEquivalentTo(list);
    }

    [Test]
    public async Task CastArray()
    {
        ISecond[] array = [default(Third), default(Third), default(Third), default(Third), default(Third)];
        await Assert.That(array.Cast<ISecond, IFirst>()).IsTypeOf<IFirst[]>().And.IsEquivalentTo(array);
    }

    [Test]
    public async Task CastWithNull()
    {
        IList<ISecond>? list = default;
        await Assert.That(list!.Cast<ISecond, IFirst>()).IsNull();
    }

    [Test]
    public async Task CastWithNonGeneric()
    {
        NoBaseClass list = [default(Third), default(Third), default(Third), default(Third), default(Third)];
        await Assert.That(() => list.Cast<ISecond, IFirst>()).Throws<InvalidOperationException>();
    }

    public static IEnumerable<Func<IReadOnlyList<int>>> GetInt32ReadOnlyLists() => CreateFunc(CreateReadOnlyLists(1, 5, 10, 15, 20));

    public static IEnumerable<Func<(IEnumerable<int>, IEnumerable<int>)>> GetLists()
    {
        foreach (var first in CreateListsCore(1, 2, 3, 4, 5))
        {
            foreach (var second in CreateListsCore(2, 3, 4))
            {
                yield return () => (first, second);
            }
        }

        foreach (var first in CreateListsCore(1, 2, 3, 4, 5))
        {
            foreach (var second in CreateReadOnlyListsCore(2, 3, 4))
            {
                yield return () => (first, second);
            }
        }

        foreach (var first in CreateReadOnlyListsCore(1, 2, 3, 4, 5))
        {
            foreach (var second in CreateListsCore(2, 3, 4))
            {
                yield return () => (first, second);
            }
        }

        foreach (var first in CreateReadOnlyListsCore(1, 2, 3, 4, 5))
        {
            foreach (var second in CreateReadOnlyListsCore(2, 3, 4))
            {
                yield return () => (first, second);
            }
        }
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
        yield return new System.Collections.ObjectModel.Collection<T>([.. a]);
    }

    private static IEnumerable<Func<T>> CreateFunc<T>(IEnumerable<T> source) => source.Select<T, Func<T>>(x => () => x);

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

        int ICollection<ISecond>.Count { get; } = 0;
        bool ICollection<ISecond>.IsReadOnly { get; } = false;

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