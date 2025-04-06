// -----------------------------------------------------------------------
// <copyright file="ListTests.QuickSort.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Linq;

public partial class ListTests
{
    public class QuickSort
    {
        public static IEnumerable<Func<IList<int>>> GetEmptyLists() => CreateFunc(CreateLists<int>());
        public static IEnumerable<Func<IList<int>>> GetSingleItemLists() => CreateFunc(CreateLists(1));
        public static IEnumerable<Func<IList<int>>> GetTwoItemsInOrderLists() => CreateFunc(CreateLists(1, 2));
        public static IEnumerable<Func<IList<int>>> GetTwoItemsNotInOrderLists() => CreateFunc(CreateLists(2, 1));
        public static IEnumerable<Func<IList<int>>> GetThreeItemsNotInOrderLists() => CreateFunc(CreateLists(1, 2, 0));
        public static IEnumerable<Func<(IList<SimpleStruct>, SimpleStruct, SimpleStruct, SimpleStruct, SimpleStruct)>> GetItemLists()
        {
            var first = new SimpleStruct(0, 0D);
            var second = new SimpleStruct(1, 1D);
            var third = new SimpleStruct(2, 2D);
            var forth = new SimpleStruct(3, 3D);

            return CreateListsCore(third, forth, second, first).Select(list => CreateListFunc(list, first, second, third, forth));
        }

        public static IEnumerable<Func<IList<int>>> GetTwoIdenticalValuesLists() => CreateFunc(CreateLists(0, 0));
        public static IEnumerable<Func<IList<int>>> GetThreeIdenticalValuesLists() => CreateFunc(CreateLists(0, 0, 0));
        public static IEnumerable<Func<(IList<SimpleStruct>, SimpleStruct, SimpleStruct, SimpleStruct)>> GetThreeIdenticalItemsList()
        {
            var first = new SimpleStruct(0, 0D);
            var second = new SimpleStruct(0, 0D);
            var third = new SimpleStruct(0, 0D);

            return CreateListsCore(first, second, third).Select(list => CreateListFunc(list, first, second, third));
        }

        public static IEnumerable<Func<(IList<SimpleClass?>, SimpleClass, SimpleClass?, SimpleClass)>> GetsItemsWithNulls()
        {
            var first = new SimpleClass(0, 0D);
            var second = default(SimpleClass);
            var third = new SimpleClass(1, 1D);

            return CreateListsCore(first, second, third).Select(list => CreateListFunc(list, first, second, third));
        }

        [Test]
        [MethodDataSource(nameof(GetEmptyLists))]
        public async Task WithNoItems(IList<int> list)
        {
            await Assert.That(WithNoParameters(list)).IsEmpty();
            await Assert.That(WithComparer(list)).IsEmpty();
        }

        [Test]
        [MethodDataSource(nameof(GetSingleItemLists))]
        public async Task WithOneItem(IList<int> list) => await Assert.That(WithNoParameters(list)).HasSingleItem();

        [Test]
        [MethodDataSource(nameof(GetTwoItemsInOrderLists))]
        public async Task WithTwoItemsInOrder(IList<int> list) => await Assert.That(WithNoParameters(list)).IsEquivalentTo([1, 2]);

        [Test]
        [MethodDataSource(nameof(GetTwoItemsNotInOrderLists))]
        public async Task WithTwoItemsNotInOrder(IList<int> list) => await Assert.That(WithNoParameters(list)).IsEquivalentTo([1, 2]);

        [Test]
        [MethodDataSource(nameof(GetThreeItemsNotInOrderLists))]
        public async Task WithThreeItemsNotInOrder(IList<int> list) => await Assert.That(WithNoParameters(list)).IsEquivalentTo([0, 1, 2]);

        [Test]
        [MethodDataSource(nameof(GetThreeItemsNotInOrderLists))]
        public async Task WithThreeItemsNotInOrderWithSize(IList<int> list) => await Assert.That(WithLength(list)).IsEquivalentTo([0, 1, 2]);

        [Test]
        [MethodDataSource(nameof(GetThreeItemsNotInOrderLists))]
        public async Task WithThreeItemsNotInOrderWithComparison(IList<int> list) => await Assert.That(WithComparison(list, Comparer<int>.Default.Compare)).IsEquivalentTo([0, 1, 2]);

        [Test]
        [MethodDataSource(nameof(GetThreeItemsNotInOrderLists))]
        public async Task WithThreeItemsNotInOrderWithComparer(IList<int> list) => await Assert.That(WithComparer(list)).IsEquivalentTo([0, 1, 2]);

        [Test]
        [MethodDataSource(nameof(GetThreeItemsNotInOrderLists))]
        public async Task WithThreeItemsNotInOrderWithComparisonAndSize(IList<int> list) => await Assert.That(WithStartLengthAndComparison(list, 0, list.Count, Comparer<int>.Default.Compare)).IsEquivalentTo([0, 1, 2]);

        [Test]
        [MethodDataSource(nameof(GetItemLists))]
        public async Task WithItems(IList<SimpleStruct> list, SimpleStruct first, SimpleStruct second, SimpleStruct third, SimpleStruct forth) => await Assert.That(WithNoParameters(list)).IsEquivalentTo([first, second, third, forth]);

        [Test]
        [MethodDataSource(nameof(GetTwoIdenticalValuesLists))]
        public async Task WithTwoIdenticalValues(IList<int> list) => await Assert.That(WithNoParameters(list)).IsEquivalentTo([0, 0]);

        [Test]
        [MethodDataSource(nameof(GetThreeIdenticalValuesLists))]
        public async Task WithThreeIdenticalValues(IList<int> list) => await Assert.That(WithNoParameters(list)).IsEquivalentTo([0, 0, 0]);

        [Test]
        [MethodDataSource(nameof(GetThreeIdenticalItemsList))]
        public async Task QuickSortWithThreeIdenticalItems(IList<SimpleStruct> list, SimpleStruct first, SimpleStruct second, SimpleStruct third) => await Assert.That(WithNoParameters(list)).IsEquivalentTo([first, second, third]);

        [Test]
        [MethodDataSource(nameof(GetsItemsWithNulls))]
        public async Task WithNullsAndComparer(IList<SimpleClass?> list, SimpleClass first, SimpleClass? second, SimpleClass third) => await Assert.That(WithComparison(list, SimpleClass.Comparison)).IsEquivalentTo([first, third, second]);

        [Test]
        [MethodDataSource(nameof(GetsItemsWithNulls))]
        public async Task WithNullsAndComparerAndSize(IList<SimpleClass?> list, SimpleClass first, SimpleClass? second, SimpleClass third) => await Assert.That(WithStartLengthAndComparison(list, 0, list.Count, SimpleClass.Comparison)).IsEquivalentTo([first, third, second]);

        private static IList<T?> WithNoParameters<T>(IList<T?> source)
            where T : IComparable<T>
        {
            source.QuickSort();
            return source;
        }

        private static IList<T> WithComparer<T>(IList<T> source)
            where T : IComparable<T>
        {
            source.QuickSort(Comparer<T>.Default);
            return source;
        }

        private static IList<T> WithLength<T>(IList<T> source)
            where T : IComparable<T>
        {
            source.QuickSort(0, source.Count, Comparer<T>.Default);
            return source;
        }

        private static IList<T> WithComparison<T>(IList<T> source, Comparison<T> comparison)
        {
            source.QuickSort(comparison);
            return source;
        }

        private static IList<T> WithStartLengthAndComparison<T>(IList<T> source, int start, int length, Comparison<T> comparison)
        {
            source.QuickSort(start, length, comparison);
            return source;
        }

        private static Func<(IList<TList>, T1, T2, T3)> CreateListFunc<TList, T1, T2, T3>(IList<TList> list, T1 first, T2 second, T3 third) => () => (list, first, second, third);

        private static Func<(IList<TList>, T1, T2, T3, T4)> CreateListFunc<TList, T1, T2, T3, T4>(IList<TList> list, T1 first, T2 second, T3 third, T4 forth) => () => (list, first, second, third, forth);

        public struct SimpleStruct(int index, double value) : IComparable<SimpleStruct>
        {
            private int Index { get; } = index;

            public double Value { get; private set; } = value;

            public readonly int CompareTo(SimpleStruct other) => this.Index.CompareTo(other.Index);
        }

        public class SimpleClass
        {
            public SimpleClass()
            {
            }

            public SimpleClass(int index, double value) => (this.Index, this.Value) = (index, value);

            private int Index { get; }

            public double Value { get; private set; }

            public static int Comparison(SimpleClass? a, SimpleClass? b) => (a, b) switch
            {
                (null, null) => 0,
                (null, not null) => 1,
                (not null, null) => -1,
                _ => a.Index.CompareTo(b.Index),
            };
        }
    }
}