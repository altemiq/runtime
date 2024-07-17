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
        public static TheoryData<IList<int>> GetEmptyLists() => new(CreateLists<int>());
        public static TheoryData<IList<int>> GetSingleItemLists() => new(CreateLists(1));
        public static TheoryData<IList<int>> GetTwoItemsInOrderLists() => new(CreateLists(1, 2));
        public static TheoryData<IList<int>> GetTwoItemsNotInOrderLists() => new(CreateLists(2, 1));
        public static TheoryData<IList<int>> GetThreeItemsNotInOrderLists() => new(CreateLists(1, 2, 0));
        public static TheoryData<IList<SimpleStruct>, SimpleStruct, SimpleStruct, SimpleStruct, SimpleStruct> GetItemLists()
        {
            var first = new SimpleStruct(0, 0D);
            var second = new SimpleStruct(1, 1D);
            var third = new SimpleStruct(2, 2D);
            var forth = new SimpleStruct(3, 3D);

            var theoryData = new TheoryData<IList<SimpleStruct>, SimpleStruct, SimpleStruct, SimpleStruct, SimpleStruct>();
            foreach (var list in CreateListsCore(third, forth, second, first))
            {
                theoryData.Add(list, first, second, third, forth);
            }

            return theoryData;
        }

        public static TheoryData<IList<int>> GetTwoIdenticalValuesLists() => new(CreateLists(0, 0));
        public static TheoryData<IList<int>> GetThreeIdenticalValuesLists() => new(CreateLists(0, 0, 0));
        public static TheoryData<IList<SimpleStruct>, SimpleStruct, SimpleStruct, SimpleStruct> GetThreeIdenticalItemsList()
        {
            var first = new SimpleStruct(0, 0D);
            var second = new SimpleStruct(0, 0D);
            var third = new SimpleStruct(0, 0D);

            var theoryData = new TheoryData<IList<SimpleStruct>, SimpleStruct, SimpleStruct, SimpleStruct>();
            foreach (var list in CreateListsCore(first, second, third))
            {
                theoryData.Add(list, first, second, third);
            }

            return theoryData;
        }

        public static TheoryData<IList<SimpleClass?>, SimpleClass, SimpleClass?, SimpleClass> GetsItemsWithNulls()
        {
            var first = new SimpleClass(0, 0D);
            var second = default(SimpleClass);
            var third = new SimpleClass(1, 1D);

            var theoryData = new TheoryData<IList<SimpleClass?>, SimpleClass, SimpleClass?, SimpleClass>();
            foreach (var list in CreateListsCore(first, second, third))
            {
                theoryData.Add(list, first, second, third);
            }

            return theoryData;
        }

        [Theory]
        [MemberData(nameof(GetEmptyLists))]
        public void WithNoItems(IList<int> list)
        {
            WithNoParameters(list).Should().BeEmpty();
            WithComparer(list).Should().BeEmpty();
        }

        [Theory]
        [MemberData(nameof(GetSingleItemLists))]
        public void WithOneItem(IList<int> list) => WithNoParameters(list).Should()
            .ContainSingle().And
            .HaveElementAt(0, 1);

        [Theory]
        [MemberData(nameof(GetTwoItemsInOrderLists))]
        public void WithTwoItemsInOrder(IList<int> list) => WithNoParameters(list).Should()
            .HaveCount(2).And
            .HaveElementAt(0, 1).And
            .HaveElementAt(1, 2);

        [Theory]
        [MemberData(nameof(GetTwoItemsNotInOrderLists))]
        public void WithTwoItemsNotInOrder(IList<int> list) => WithNoParameters(list).Should()
            .HaveCount(2).And
            .HaveElementAt(0, 1).And
            .HaveElementAt(1, 2);

        [Theory]
        [MemberData(nameof(GetThreeItemsNotInOrderLists))]
        public void WithThreeItemsNotInOrder(IList<int> list) => WithNoParameters(list).Should()
            .HaveCount(3).And
            .HaveElementAt(0, 0).And
            .HaveElementAt(1, 1).And
            .HaveElementAt(2, 2);

        [Theory]
        [MemberData(nameof(GetThreeItemsNotInOrderLists))]
        public void WithThreeItemsNotInOrderWithSize(IList<int> list) => WithLength(list).Should()
            .HaveCount(3).And
            .HaveElementAt(0, 0).And
            .HaveElementAt(1, 1).And
            .HaveElementAt(2, 2);

        [Theory]
        [MemberData(nameof(GetThreeItemsNotInOrderLists))]
        public void WithThreeItemsNotInOrderWithComparison(IList<int> list) => WithComparison(list, Comparer<int>.Default.Compare).Should()
            .HaveCount(3).And
            .HaveElementAt(0, 0).And
            .HaveElementAt(1, 1).And
            .HaveElementAt(2, 2);

        [Theory]
        [MemberData(nameof(GetThreeItemsNotInOrderLists))]
        public void WithThreeItemsNotInOrderWithComparer(IList<int> list) => WithComparer(list).Should()
            .HaveCount(3).And
            .HaveElementAt(0, 0).And
            .HaveElementAt(1, 1).And
            .HaveElementAt(2, 2);

        [Theory]
        [MemberData(nameof(GetThreeItemsNotInOrderLists))]
        public void WithThreeItemsNotInOrderWithComparisonAndSize(IList<int> list) => WithStartLengthAndComparison(list, 0, list.Count, Comparer<int>.Default.Compare).Should()
            .HaveCount(3).And
            .HaveElementAt(0, 0).And
            .HaveElementAt(1, 1).And
            .HaveElementAt(2, 2);

        [Theory]
        [MemberData(nameof(GetItemLists))]
        public void WithItems(IList<SimpleStruct> list, SimpleStruct first, SimpleStruct second, SimpleStruct third, SimpleStruct forth) => WithNoParameters(list).Should()
            .HaveCount(4).And
            .HaveElementAt(0, first).And
            .HaveElementAt(1, second).And
            .HaveElementAt(2, third).And
            .HaveElementAt(3, forth);

        [Theory]
        [MemberData(nameof(GetTwoIdenticalValuesLists))]
        public void WithTwoIdenticalValues(IList<int> list) => WithNoParameters(list).Should()
            .HaveCount(2).And
            .HaveElementAt(0, 0).And
            .HaveElementAt(1, 0);

        [Theory]
        [MemberData(nameof(GetThreeIdenticalValuesLists))]
        public void WithThreeIdenticalValues(IList<int> list) => WithNoParameters(list).Should()
            .HaveCount(3).And
            .HaveElementAt(0, 0).And
            .HaveElementAt(1, 0).And
            .HaveElementAt(2, 0);

        [Theory]
        [MemberData(nameof(GetThreeIdenticalItemsList))]
        public void QuickSortWithThreeIdenticalItems(IList<SimpleStruct> list, SimpleStruct first, SimpleStruct second, SimpleStruct third) => WithNoParameters(list).Should()
            .HaveCount(3).And
            .HaveElementAt(0, first).And
            .HaveElementAt(1, second).And
            .HaveElementAt(2, third);

        [Theory]
        [MemberData(nameof(GetsItemsWithNulls))]
        public void WithNullsAndComparer(IList<SimpleClass?> list, SimpleClass first, SimpleClass? second, SimpleClass third) => WithComparison(list, SimpleClass.Comparison).Should()
            .HaveCount(3).And
            .HaveElementAt(0, first).And
            .HaveElementAt(1, third).And
            .HaveElementAt(2, second);

        [Theory]
        [MemberData(nameof(GetsItemsWithNulls))]
        public void WithNullsAndComparerAndSize(IList<SimpleClass?> list, SimpleClass first, SimpleClass? second, SimpleClass third) => WithStartLengthAndComparison(list, 0, list.Count, SimpleClass.Comparison).Should()
            .HaveCount(3).And
            .HaveElementAt(0, first).And
            .HaveElementAt(1, third).And
            .HaveElementAt(2, second);

        private static IList<T> WithNoParameters<T>(IList<T> source)
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

        public struct SimpleStruct : IComparable<SimpleStruct>, Xunit.Abstractions.IXunitSerializable
        {
            public SimpleStruct(int index, double value) => (this.Index, this.Value) = (index, value);

            public int Index { get; private set; }

            public double Value { get; private set; }

            public readonly int CompareTo(SimpleStruct other) => this.Index.CompareTo(other.Index);

            void Xunit.Abstractions.IXunitSerializable.Deserialize(Xunit.Abstractions.IXunitSerializationInfo info)
            {
                this.Index = info.GetValue<int>(nameof(this.Index));
                this.Value = info.GetValue<double>(nameof(this.Value));
            }

            readonly void Xunit.Abstractions.IXunitSerializable.Serialize(Xunit.Abstractions.IXunitSerializationInfo info)
            {
                info.AddValue(nameof(this.Index), this.Index, typeof(int));
                info.AddValue(nameof(this.Value), this.Value, typeof(double));
            }
        }

        public class SimpleClass : Xunit.Abstractions.IXunitSerializable
        {
            public SimpleClass()
            {
            }

            public SimpleClass(int index, double value) => (this.Index, this.Value) = (index, value);

            public int Index { get; private set; }

            public double Value { get; private set; }

            public static int Comparison(SimpleClass? a, SimpleClass? b) => (a, b) switch
            {
                (null, null) => 0,
                (null, not null) => 1,
                (not null, null) => -1,
                _ => a.Index.CompareTo(b.Index),
            };

            void Xunit.Abstractions.IXunitSerializable.Deserialize(Xunit.Abstractions.IXunitSerializationInfo info)
            {
                this.Index = info.GetValue<int>(nameof(this.Index));
                this.Value = info.GetValue<double>(nameof(this.Value));
            }

            void Xunit.Abstractions.IXunitSerializable.Serialize(Xunit.Abstractions.IXunitSerializationInfo info)
            {
                info.AddValue(nameof(this.Index), this.Index, typeof(int));
                info.AddValue(nameof(this.Value), this.Value, typeof(double));
            }
        }
    }
}