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
    public void IndexOf(object first, object second)
    {
        _ = TestListList<int, int>(first, second, (f, s) => f.IndexOf(s), 1).Should().Be(1);
        _ = TestReadOnlyListReadOnlyList<int, int>(first, second, (f, s) => f.IndexOf(s), 1).Should().Be(1);
    }

    [Theory]
    [MemberData(nameof(CreateNulls), 2)]
    public void IndexOfWithNull(object first, object second)
    {
        ((IList<int>)first).IndexOf((IList<int>)second).Should().Be(-1);
        ((IReadOnlyList<int>)first).IndexOf((IReadOnlyList<int>)second).Should().Be(-1);
    }

    [Theory]
    [MemberData(nameof(GetInt32Lists))]
    public void IndexOfValue(IReadOnlyList<int> list) => list.IndexOf(10, 1).Should().Be(2);

    [Theory]
    [MemberData(nameof(GetLists))]
    public void IndexOfAny(object first, object second)
    {
        _ = TestListList<int, int>(first, second, (f, s) => f.IndexOfAny(s), 1).Should().Be(1);
        _ = TestReadOnlyListReadOnlyList<int, int>(first, second, (f, s) => f.IndexOfAny(s), 1).Should().Be(1);
    }
}