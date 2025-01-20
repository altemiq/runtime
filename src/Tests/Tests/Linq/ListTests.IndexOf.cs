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
            Assert.Equal(1, TestListList<int, int>(first, second, static (f, s) => f.IndexOf(s), 1));
            Assert.Equal(1, TestReadOnlyListReadOnlyList<int, int>(first, second, static (f, s) => f.IndexOf(s), 1));
        }

        [Theory]
        [MemberData(nameof(CreateNulls))]
        public void WithNull(System.Collections.IEnumerable? first, System.Collections.IEnumerable? second)
        {
            Assert.Equal(-1, ((IList<int>)first!).IndexOf((IList<int>)second!));
            Assert.Equal(-1, ((IReadOnlyList<int>)first!).IndexOf((IReadOnlyList<int>)second!));
        }

        [Theory]
        [MemberData(nameof(GetInt32ReadOnlyLists))]
        public void Value(IReadOnlyList<int> list) => Assert.Equal(2, list.IndexOf(10, 1));

        [Theory]
        [MemberData(nameof(GetLists))]
        public void Any(IEnumerable<int> first, IEnumerable<int> second)
        {
            Assert.Equal(1,  TestListList<int, int>(first, second, static (f, s) => f.IndexOfAny(s), 1));
            Assert.Equal(1, TestReadOnlyListReadOnlyList<int, int>(first, second, static (f, s) => f.IndexOfAny(s), 1));
        }

        [Theory]
        [MemberData(nameof(GetLists))]
        public void Lists(IEnumerable<int> first, IEnumerable<int> second)
        {
            if (first is IList<int> firstList)
            {
                Assert.Equal(1, firstList.IndexOf(2, 0));
            }
            else if (first is IReadOnlyList<int> firstReadOnlyList)
            {
                Assert.Equal(1, firstReadOnlyList.IndexOf(2, 0));
            }

            if (second is IList<int> secondList)
            {
                Assert.Equal(0, secondList.IndexOf(2, 0));
            }
            else if (second is IReadOnlyList<int> secondReadOnlyList)
            {
                Assert.Equal(0, secondReadOnlyList.IndexOf(2, 0));
            }
        }
    }
}