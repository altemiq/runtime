// -----------------------------------------------------------------------
// <copyright file="ListTests.IndexOf.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Linq;

public partial class ListTests
{
    [Theory]
    [MemberData(nameof(GetLists))]
    public void IndexOf(IEnumerable<int> first, IEnumerable<int> second)
    {
        _ = TestListList<int, int>(first, second, (f, s) => f.IndexOf(s), 1).Should().Be(1);
        _ = TestReadOnlyListReadOnlyList<int, int>(first, second, (f, s) => f.IndexOf(s), 1).Should().Be(1);
    }

    [Theory]
    [MemberData(nameof(CreateNulls))]
    public void IndexOfWithNull(System.Collections.IEnumerable? first, System.Collections.IEnumerable? second)
    {
        ((IList<int>)first!).IndexOf((IList<int>)second!).Should().Be(-1);
        ((IReadOnlyList<int>)first!).IndexOf((IReadOnlyList<int>)second!).Should().Be(-1);
    }

    [Theory]
    [MemberData(nameof(GetInt32ReadOnlyLists))]
    public void IndexOfValue(IReadOnlyList<int> list) => list.IndexOf(10, 1).Should().Be(2);

    [Theory]
    [MemberData(nameof(GetLists))]
    public void IndexOfAny(IEnumerable<int> first, IEnumerable<int> second)
    {
        _ = TestListList<int, int>(first, second, (f, s) => f.IndexOfAny(s), 1).Should().Be(1);
        _ = TestReadOnlyListReadOnlyList<int, int>(first, second, (f, s) => f.IndexOfAny(s), 1).Should().Be(1);
    }

    [Theory]
    [MemberData(nameof(GetLists))]
    public void IndexOfLists(IEnumerable<int> first, IEnumerable<int> second)
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