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
        [Theory]
        [MemberData(nameof(GetLists), MemberType = typeof(ListTests))]
        public void Int32(IEnumerable<int> first, IEnumerable<int> second)
        {
            Assert.Equal(1, TestListList<int, int>(first, second, static (f, s) => f.IndexOf(s), 1));
            Assert.Equal(1, TestReadOnlyListReadOnlyList<int, int>(first, second, static (f, s) => f.IndexOf(s), 1));
        }

        [Fact]
        public void WithNulls()
        {
            Assert.Equal(-1, ((IList<int>)null!).IndexOf((IList<int>)null!));
            Assert.Equal(-1, ((IReadOnlyList<int>)null!).IndexOf((IReadOnlyList<int>)null!));
        }

        [Theory]
        [MemberData(nameof(GetInt32ReadOnlyLists), MemberType = typeof(ListTests))]
        public void Value(IReadOnlyList<int> list) => Assert.Equal(2, list.IndexOf(10, 1));

        [Theory]
        [MemberData(nameof(GetLists), MemberType = typeof(ListTests))]
        public void Any(IEnumerable<int> first, IEnumerable<int> second)
        {
            Assert.Equal(1, TestListList<int, int>(first, second, static (f, s) => f.IndexOfAny(s), 1));
            Assert.Equal(1, TestReadOnlyListReadOnlyList<int, int>(first, second, static (f, s) => f.IndexOfAny(s), 1));
        }

        [Theory]
        [MemberData(nameof(GetLists), MemberType = typeof(ListTests))]
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