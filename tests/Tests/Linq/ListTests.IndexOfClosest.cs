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
        public static IEnumerable<Func<IList<int>>> GetInt32Lists() => CreateFunc(CreateLists(1, 5, 10, 15, 20));

        public static IEnumerable<Func<IReadOnlyList<double>>> GetDoubleReadOnlyLists() => CreateFunc(CreateReadOnlyLists(1D, 5D, 10D, 15D, 20D));

        [Test]
        [MethodDataSource(typeof(ListTests), nameof(GetInt32ReadOnlyLists))]
        public async Task Int32ReadOnly(IReadOnlyList<int> list) => await Assert.That(list.IndexOfClosest(7)).IsEqualTo(1);

        [Test]
        [MethodDataSource(nameof(GetInt32Lists))]
        public async Task Int32(IList<int> list) => await Assert.That(list.IndexOfClosest(7)).IsEqualTo(1);

        [Test]
        [MethodDataSource(nameof(GetDoubleReadOnlyLists))]
        public async Task Double(IReadOnlyList<double> list) => await Assert.That(list.IndexOfClosest(7D)).IsEqualTo(1);
    }
}