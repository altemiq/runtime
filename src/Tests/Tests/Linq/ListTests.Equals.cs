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
        [Test]
        [MethodDataSource(typeof(ListTests), nameof(GetLists))]
        public async Task DoesEqual(IEnumerable<int> first, IEnumerable<int> second)
        {
            await Assert.That(TestListList<int, bool>(first, second, static (f, s) => f.Equals(1, s, 0, s.Count), true)).IsTrue();
            await Assert.That(TestListReadOnlyList<int, bool>(first, second, static (f, s) => f.Equals(1, s, 0, s.Count), true)).IsTrue();
            await Assert.That(TestReadOnlyListList<int, bool>(first, second, static (f, s) => f.Equals(1, s, 0, s.Count), true)).IsTrue();
            await Assert.That(TestReadOnlyListReadOnlyList<int, bool>(first, second, static (f, s) => f.Equals(1, s, 0, s.Count), true)).IsTrue();
        }

        [Test]
        [MethodDataSource(typeof(ListTests), nameof(GetLists))]
        public async Task DoesNotEqual(IEnumerable<int> first, IEnumerable<int> second)
        {
            await Assert.That(TestListList<int, bool>(first, second, static (f, s) => f.Equals(2, s, 0, s.Count), false)).IsFalse();
            await Assert.That(TestListReadOnlyList<int, bool>(first, second, static (f, s) => f.Equals(2, s, 0, s.Count), false)).IsFalse();
            await Assert.That(TestReadOnlyListList<int, bool>(first, second, static (f, s) => f.Equals(2, s, 0, s.Count), false)).IsFalse();
            await Assert.That(TestReadOnlyListReadOnlyList<int, bool>(first, second, static (f, s) => f.Equals(2, s, 0, s.Count), false)).IsFalse();
        }

        private static TResult? TestListReadOnlyList<T, TResult>(object first, object second, Func<IList<T>, IReadOnlyList<T>, TResult> func, TResult? defaultResult = default) => first is IList<T> f && second is IReadOnlyList<T> s ? func(f, s) : defaultResult;

        private static TResult? TestReadOnlyListList<T, TResult>(object first, object second, Func<IReadOnlyList<T>, IList<T>, TResult> func, TResult? defaultResult = default) => first is IReadOnlyList<T> f && second is IList<T> s ? func(f, s) : defaultResult;
    }
}