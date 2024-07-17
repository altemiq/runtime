// -----------------------------------------------------------------------
// <copyright file="ListTests.IndexOfClosest.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Linq;

public partial class ListTests
{
    public class IndexOfClosest
    {
        public static TheoryData<IReadOnlyList<int>> GetInt32ReadOnlyLists() => ListTests.GetInt32ReadOnlyLists();

        public static TheoryData<IList<int>> GetInt32Lists() => new(CreateLists(1, 5, 10, 15, 20));

        public static TheoryData<IReadOnlyList<double>> GetDoubleReadOnlyLists() => new(CreateReadOnlyLists(1D, 5D, 10D, 15D, 20D));

        [Theory]
        [MemberData(nameof(GetInt32ReadOnlyLists))]
        public void Int32ReadOnly(IReadOnlyList<int> list) => list.IndexOfClosest(7).Should().Be(1);

        [Theory]
        [MemberData(nameof(GetInt32Lists))]
        public void Int32(IList<int> list) => list.IndexOfClosest(7).Should().Be(1);

        [Theory]
        [MemberData(nameof(GetDoubleReadOnlyLists))]
        public void Double(IReadOnlyList<double> list) => list.IndexOfClosest(7D).Should().Be(1);
    }
}