// -----------------------------------------------------------------------
// <copyright file="ListTests.IndexOfClosest.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Linq;

public partial class ListTests
{
    public static TheoryData<IReadOnlyList<double>> GetDoubleReadOnlyLists() => new(CreateReadOnlyLists(1D, 5D, 10D, 15D, 20D));

    public static TheoryData<IList<int>> GetInt32Lists() => new(CreateLists(1, 5, 10, 15, 20));

    [Theory]
    [MemberData(nameof(GetInt32ReadOnlyLists))]
    public void IndexOfClosestInt32ReadOnly(IReadOnlyList<int> list) => list.IndexOfClosest(7).Should().Be(1);


    [Theory]
    [MemberData(nameof(GetInt32Lists))]
    public void IndexOfClosestInt32(IList<int> list) => list.IndexOfClosest(7).Should().Be(1);

    [Theory]
    [MemberData(nameof(GetDoubleReadOnlyLists))]
    public void IndexOfClosestDouble(IReadOnlyList<double> list) => list.IndexOfClosest(7D).Should().Be(1);
}