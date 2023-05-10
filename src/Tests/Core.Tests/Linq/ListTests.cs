// -----------------------------------------------------------------------
// <copyright file="ListTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Linq;
public class ListTests
{
    [Fact]
    public void IndexOfClosestInt32()
    {
        IReadOnlyList<int> list = new[] { 1, 5, 10, 15, 20 };
        _ = list.IndexOfClosest(7).Should().Be(1);
    }

    [Fact]
    public void IndexOfClosestDouble()
    {
        IReadOnlyList<double> list = new[] { 1D, 5D, 10D, 15D, 20D };
        _ = list.IndexOfClosest(7D).Should().Be(1);
    }

    [Fact]
    public void QuickSortWithNoItems()
    {
        var list = new List<int>();
        list.QuickSort();
        _ = list.Should().BeEmpty();
    }

    [Fact]
    public void QuickSortWithOneItem()
    {
        var list = new List<int> { 1 };
        list.QuickSort();
        _ = list.Should().ContainSingle().And.HaveElementAt(0, 1);
    }

    [Fact]
    public void QuickSortWithTwoItemsInOrder()
    {
        var list = new List<int> { 1, 2 };
        list.QuickSort();
        _ = list.Should().HaveCount(2).And.HaveElementAt(0, 1).And.HaveElementAt(1, 2);
    }

    [Fact]
    public void QuickSortWithTwoItemsNotInOrder()
    {
        var list = new List<int> { 2, 1 };
        list.QuickSort();
        _ = list.Should().HaveCount(2).And.HaveElementAt(0, 1).And.HaveElementAt(1, 2);
    }

    [Fact]
    public void QuickSortWithThreeItemsNotInOrder()
    {
        var list = new List<int> { 1, 2, 0 };
        list.QuickSort();
        _ = list.Should().HaveCount(3).And.HaveElementAt(0, 0).And.HaveElementAt(1, 1).And.HaveElementAt(2, 2);
    }

    [Fact]
    public void QuickSortWithItems()
    {
        var first = new SimpleStruct(0, 0D);
        var second = new SimpleStruct(1, 1D);
        var third = new SimpleStruct(2, 2D);
        var forth = new SimpleStruct(3, 3D);
        var list = new List<SimpleStruct> { third, forth, second, first };
        list.QuickSort();
        _ = list.Should().HaveCount(4)
            .And.HaveElementAt(0, first)
            .And.HaveElementAt(1, second)
            .And.HaveElementAt(2, third)
            .And.HaveElementAt(3, forth);
    }

    [Fact]
    public void QuickSortWithTwoIdenticalValues()
    {
        var first = 0;
        var second = 0;
        var list = new List<int> { first, second };
        list.QuickSort();
        _ = list.Should().HaveCount(2)
            .And.HaveElementAt(0, first)
            .And.HaveElementAt(1, second);
    }

    [Fact]
    public void QuickSortWithThreeIdenticalValues()
    {
        var first = 0;
        var second = 0;
        var third = 0;
        var list = new List<int> { first, second, third };
        list.QuickSort();
        _ = list.Should().HaveCount(3)
            .And.HaveElementAt(0, first)
            .And.HaveElementAt(1, second)
            .And.HaveElementAt(2, third);
    }

    [Fact]
    public void QuickSortWithThreeIdenticalItems()
    {
        var first = new SimpleStruct(0, 0D);
        var second = new SimpleStruct(0, 0D);
        var third = new SimpleStruct(0, 0D);
        var list = new List<SimpleStruct> { first, second, third };
        list.QuickSort();
        _ = list.Should().HaveCount(3)
            .And.HaveElementAt(0, first)
            .And.HaveElementAt(1, second)
            .And.HaveElementAt(2, third);
    }

    [Fact]
    public void QuickSortWithNullsAndComparer()
    {
        var first = new SimpleClass(0, 0D);
        var second = default(SimpleClass);
        var third = new SimpleClass(1, 1D);
        var list = new List<SimpleClass?> { first, second, third };
        list.QuickSort((x, y) => (x, y) switch
            {
                (null, null) => 0,
                (null, not null) => 1,
                (not null, null) => -1,
                _ => x.Index.CompareTo(y.Index),
            });

        _ = list.Should().HaveCount(3)
            .And.HaveElementAt(0, first)
            .And.HaveElementAt(1, third)
            .And.HaveElementAt(2, second);
    }

    private readonly struct SimpleStruct : IComparable<SimpleStruct>
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
    }
}