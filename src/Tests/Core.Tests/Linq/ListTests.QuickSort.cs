// -----------------------------------------------------------------------
// <copyright file="ListTests.QuickSort.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Linq;

public partial class ListTests
{
    public static IEnumerable<object[]> GetEmptyLists() => CreateLists<int>();
    public static IEnumerable<object[]> GetSingleItemLists() => CreateLists(1);
    public static IEnumerable<object[]> GetTwoItemsInOrderLists() => CreateLists(1, 2);
    public static IEnumerable<object[]> GetTwoItemsNotInOrderLists() => CreateLists(2, 1);
    public static IEnumerable<object[]> GetThreeItemsNotInOrderLists() => CreateLists(1, 2, 0);
    public static IEnumerable<object[]> GetItemLists()
    {
        var first = new SimpleStruct(0, 0D);
        var second = new SimpleStruct(1, 1D);
        var third = new SimpleStruct(2, 2D);
        var forth = new SimpleStruct(3, 3D);

        foreach (var list in CreateListsCore(third, forth, second, first))
        {
            yield return new object[] { list, first, second, third, forth };
        }
    }

    public static IEnumerable<object[]> GetTwoIdenticalValuesLists() => CreateLists(0, 0);
    public static IEnumerable<object[]> GetThreeIdenticalValuesLists() => CreateLists(0, 0, 0);
    public static IEnumerable<object[]> GetThreeIdenticalItemsList()
    {
        var first = new SimpleStruct(0, 0D);
        var second = new SimpleStruct(0, 0D);
        var third = new SimpleStruct(0, 0D);

        foreach (var list in CreateListsCore(first, second, third))
        {
            yield return new object[] { list, first, second, third };
        }
    }

    public static IEnumerable<object?[]> GetsItemsWithNulls()
    {
        var first = new SimpleClass(0, 0D);
        var second = default(SimpleClass);
        var third = new SimpleClass(1, 1D);

        foreach (var list in CreateListsCore(first, second, third))
        {
            yield return new object?[] { list, first, second, third };
        }
    }


    [Theory]
    [MemberData(nameof(GetEmptyLists))]
    public void QuickSortWithNoItems(IList<int> list) => QuickSort(list).Should().BeEmpty();

    [Theory]
    [MemberData(nameof(GetSingleItemLists))]
    public void QuickSortWithOneItem(IList<int> list) => QuickSort(list).Should()
        .ContainSingle().And
        .HaveElementAt(0, 1);

    [Theory]
    [MemberData(nameof(GetTwoItemsInOrderLists))]
    public void QuickSortWithTwoItemsInOrder(IList<int> list) => QuickSort(list).Should()
        .HaveCount(2).And
        .HaveElementAt(0, 1).And
        .HaveElementAt(1, 2);

    [Theory]
    [MemberData(nameof(GetTwoItemsNotInOrderLists))]
    public void QuickSortWithTwoItemsNotInOrder(IList<int> list) => QuickSort(list).Should()
        .HaveCount(2).And
        .HaveElementAt(0, 1).And
        .HaveElementAt(1, 2);

    [Theory]
    [MemberData(nameof(GetThreeItemsNotInOrderLists))]
    public void QuickSortWithThreeItemsNotInOrder(IList<int> list) => QuickSort(list).Should()
        .HaveCount(3).And
        .HaveElementAt(0, 0).And
        .HaveElementAt(1, 1).And
        .HaveElementAt(2, 2);

    [Theory]
    [MemberData(nameof(GetItemLists))]
    public void QuickSortWithItems(IList<SimpleStruct> list, SimpleStruct first, SimpleStruct second, SimpleStruct third, SimpleStruct forth) => QuickSort(list).Should()
        .HaveCount(4).And
        .HaveElementAt(0, first).And
        .HaveElementAt(1, second).And
        .HaveElementAt(2, third).And
        .HaveElementAt(3, forth);

    [Theory]
    [MemberData(nameof(GetTwoIdenticalValuesLists))]
    public void QuickSortWithTwoIdenticalValues(IList<int> list) => QuickSort(list).Should()
        .HaveCount(2).And
        .HaveElementAt(0, 0).And
        .HaveElementAt(1, 0);

    [Theory]
    [MemberData(nameof(GetThreeIdenticalValuesLists))]
    public void QuickSortWithThreeIdenticalValues(IList<int> list) => QuickSort(list).Should()
        .HaveCount(3).And
        .HaveElementAt(0, 0).And
        .HaveElementAt(1, 0).And
        .HaveElementAt(2, 0);

    [Theory]
    [MemberData(nameof(GetThreeIdenticalItemsList))]
    public void QuickSortWithThreeIdenticalItems(IList<SimpleStruct> list, SimpleStruct first, SimpleStruct second, SimpleStruct third) => QuickSort(list).Should()
        .HaveCount(3).And
        .HaveElementAt(0, first).And
        .HaveElementAt(1, second).And
        .HaveElementAt(2, third);

    [Theory]
    [MemberData(nameof(GetsItemsWithNulls))]
    public void QuickSortWithNullsAndComparer(IList<SimpleClass?> list, SimpleClass? first, SimpleClass? second, SimpleClass? third) => QuickSort(list, SimpleClass.Comparison).Should()
        .HaveCount(3).And
        .HaveElementAt(0, first).And
        .HaveElementAt(1, third).And
        .HaveElementAt(2, second);

    private static IList<T> QuickSort<T>(IList<T> source)
        where T : IComparable<T>
    {
        source.QuickSort();
        return source;
    }

    private static IList<T> QuickSort<T>(IList<T> source, Comparison<T> comparison)
    {
        source.QuickSort(comparison);
        return source;
    }

    public readonly struct SimpleStruct : IComparable<SimpleStruct>
    {
        public SimpleStruct(int index, double value) => (this.Index, this.Value) = (index, value);

        public int Index { get; }

        public double Value { get; }

        public int CompareTo(SimpleStruct other) => this.Index.CompareTo(other.Index);
    }

    public class SimpleClass
    {
        public SimpleClass(int index, double value) => (this.Index, this.Value) = (index, value);

        public int Index { get; }

        public double Value { get; }

        public static int Comparison(SimpleClass? a, SimpleClass? b) => (a, b) switch
        {
            (null, null) => 0,
            (null, not null) => 1,
            (not null, null) => -1,
            _ => a.Index.CompareTo(b.Index),
        };
    }
}