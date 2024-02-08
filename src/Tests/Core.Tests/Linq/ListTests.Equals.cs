// -----------------------------------------------------------------------
// <copyright file="ListTests.Equals.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Linq;

public partial class ListTests
{
    [Theory]
    [MemberData(nameof(GetLists))]
    public void DoesEquals(IEnumerable<int> first, IEnumerable<int> second)
    {
        _ = TestListList<int, bool>(first, second, (f, s) => f.Equals(1, s, 0, s.Count), true).Should().BeTrue();
        _ = TestListReadOnlyList<int, bool>(first, second, (f, s) => f.Equals(1, s, 0, s.Count), true).Should().BeTrue();
        _ = TestReadOnlyListList<int, bool>(first, second, (f, s) => f.Equals(1, s, 0, s.Count), true).Should().BeTrue();
        _ = TestReadOnlyListReadOnlyList<int, bool>(first, second, (f, s) => f.Equals(1, s, 0, s.Count), true).Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetLists))]
    public void DoesNotEquals(IEnumerable<int> first, IEnumerable<int> second)
    {
        _ = TestListList<int, bool>(first, second, (f, s) => f.Equals(2, s, 0, s.Count), false).Should().BeFalse();
        _ = TestListReadOnlyList<int, bool>(first, second, (f, s) => f.Equals(2, s, 0, s.Count), false).Should().BeFalse();
        _ = TestReadOnlyListList<int, bool>(first, second, (f, s) => f.Equals(2, s, 0, s.Count), false).Should().BeFalse();
        _ = TestReadOnlyListReadOnlyList<int, bool>(first, second, (f, s) => f.Equals(2, s, 0, s.Count), false).Should().BeFalse();
    }

    private static TResult? TestListList<T, TResult>(object first, object second, Func<IList<T>, IList<T>, TResult> func, TResult? defaultResult = default) => first is IList<T> f && second is IList<T> s ? func(f, s) : defaultResult;

    private static TResult? TestListReadOnlyList<T, TResult>(object first, object second, Func<IList<T>, IReadOnlyList<T>, TResult> func, TResult? defaultResult = default) => first is IList<T> f && second is IReadOnlyList<T> s ? func(f, s) : defaultResult;

    private static TResult? TestReadOnlyListList<T, TResult>(object first, object second, Func<IReadOnlyList<T>, IList<T>, TResult> func, TResult? defaultResult = default) => first is IReadOnlyList<T> f && second is IList<T> s ? func(f, s) : defaultResult;

    private static TResult? TestReadOnlyListReadOnlyList<T, TResult>(object first, object second, Func<IReadOnlyList<T>, IReadOnlyList<T>, TResult> func, TResult? defaultResult = default) => first is IReadOnlyList<T> f && second is IReadOnlyList<T> s ? func(f, s) : defaultResult;
}