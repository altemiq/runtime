// -----------------------------------------------------------------------
// <copyright file="ListTests.IndexOfClosest.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Linq;

public partial class ListTests
{
    public static IEnumerable<object[]> GetInt32Lists() => CreateReadOnlyLists(1, 5, 10, 15, 20);
    public static IEnumerable<object[]> GetDoubleLists() => CreateReadOnlyLists(1D, 5D, 10D, 15D, 20D);

    [Theory]
    [MemberData(nameof(GetInt32Lists))]
    public void IndexOfClosestInt32(IReadOnlyList<int> list) => list.IndexOfClosest(7).Should().Be(1);

    [Theory]
    [MemberData(nameof(GetDoubleLists))]
    public void IndexOfClosestDouble(IReadOnlyList<double> list) => list.IndexOfClosest(7D).Should().Be(1);
}