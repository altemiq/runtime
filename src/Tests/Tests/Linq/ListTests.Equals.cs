// -----------------------------------------------------------------------
// <copyright file="ListTests.Equals.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Linq;

public partial class ListTests
{
    public class Equal
    {
        [Theory]
        [MemberData(nameof(GetLists), MemberType = typeof(ListTests))]
        public void DoesEqual(IEnumerable<int> first, IEnumerable<int> second)
        {
            Assert.True(TestListList<int, bool>(first, second, static (f, s) => f.Equals(1, s, 0, s.Count), true));
            Assert.True(TestListReadOnlyList<int, bool>(first, second, static (f, s) => f.Equals(1, s, 0, s.Count), true));
            Assert.True(TestReadOnlyListList<int, bool>(first, second, static (f, s) => f.Equals(1, s, 0, s.Count), true));
            Assert.True(TestReadOnlyListReadOnlyList<int, bool>(first, second, static (f, s) => f.Equals(1, s, 0, s.Count), true));
        }

        [Theory]
        [MemberData(nameof(GetLists), MemberType = typeof(ListTests))]
        public void DoesNotEqual(IEnumerable<int> first, IEnumerable<int> second)
        {
            Assert.False(TestListList<int, bool>(first, second, static (f, s) => f.Equals(2, s, 0, s.Count), false));
            Assert.False(TestListReadOnlyList<int, bool>(first, second, static (f, s) => f.Equals(2, s, 0, s.Count), false));
            Assert.False(TestReadOnlyListList<int, bool>(first, second, static (f, s) => f.Equals(2, s, 0, s.Count), false));
            Assert.False(TestReadOnlyListReadOnlyList<int, bool>(first, second, static (f, s) => f.Equals(2, s, 0, s.Count), false));
        }

        private static TResult? TestListReadOnlyList<T, TResult>(object first, object second, Func<IList<T>, IReadOnlyList<T>, TResult> func, TResult? defaultResult = default) => first is IList<T> f && second is IReadOnlyList<T> s ? func(f, s) : defaultResult;

        private static TResult? TestReadOnlyListList<T, TResult>(object first, object second, Func<IReadOnlyList<T>, IList<T>, TResult> func, TResult? defaultResult = default) => first is IReadOnlyList<T> f && second is IList<T> s ? func(f, s) : defaultResult;
    }
}