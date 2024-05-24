// -----------------------------------------------------------------------
// <copyright file="ListTests.IndexOf.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Linq;

public partial class ListTests
{
    public class IndexOf
    {
        public static TheoryData<IEnumerable<int>, IEnumerable<int>> GetLists() => ListTests.GetLists();
        public static TheoryData<System.Collections.IEnumerable?, System.Collections.IEnumerable?> CreateNulls() => ListTests.CreateNulls();
        public static TheoryData<IReadOnlyList<int>> GetInt32ReadOnlyLists() => ListTests.GetInt32ReadOnlyLists();

        [Theory]
        [MemberData(nameof(GetLists))]
        public void Int32(IEnumerable<int> first, IEnumerable<int> second)
        {
            _ = TestListList<int, int>(first, second, (f, s) => f.IndexOf(s), 1).Should().Be(1);
            _ = TestReadOnlyListReadOnlyList<int, int>(first, second, (f, s) => f.IndexOf(s), 1).Should().Be(1);
        }

        [Theory]
        [MemberData(nameof(CreateNulls))]
        public void WithNull(System.Collections.IEnumerable? first, System.Collections.IEnumerable? second)
        {
            ((IList<int>)first!).IndexOf((IList<int>)second!).Should().Be(-1);
            ((IReadOnlyList<int>)first!).IndexOf((IReadOnlyList<int>)second!).Should().Be(-1);
        }

        [Theory]
        [MemberData(nameof(GetInt32ReadOnlyLists))]
        public void Value(IReadOnlyList<int> list) => list.IndexOf(10, 1).Should().Be(2);

        [Theory]
        [MemberData(nameof(GetLists))]
        public void Any(IEnumerable<int> first, IEnumerable<int> second)
        {
            _ = TestListList<int, int>(first, second, (f, s) => f.IndexOfAny(s), 1).Should().Be(1);
            _ = TestReadOnlyListReadOnlyList<int, int>(first, second, (f, s) => f.IndexOfAny(s), 1).Should().Be(1);
        }

        [Theory]
        [MemberData(nameof(GetLists))]
        public void Lists(IEnumerable<int> first, IEnumerable<int> second)
        {
            if (first is IList<int> firstList)
            {
                firstList.IndexOf(2, 0).Should().Be(1);
            }
            else if (first is IReadOnlyList<int> firstReadOnlyList)
            {
                firstReadOnlyList.IndexOf(2, 0).Should().Be(1);
            }

            if (second is IList<int> secondList)
            {
                secondList.IndexOf(2, 0).Should().Be(0);
            }
            else if (second is IReadOnlyList<int> secondReadOnlyList)
            {
                secondReadOnlyList.IndexOf(2, 0).Should().Be(0);
            }
        }
    }
}