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
        public static IEnumerable<Func<IList<int>>> GetInt32Lists()
        {
            return CreateFunc(CreateLists(1, 5, 10, 15, 20));
        }

        public static IEnumerable<Func<IReadOnlyList<double>>> GetDoubleReadOnlyLists()
        {
            return CreateFunc(CreateReadOnlyLists(1D, 5D, 10D, 15D, 20D));
        }

        [Test]
        [MethodDataSource(typeof(ListTests), nameof(GetInt32ReadOnlyLists))]
        public async Task ReadOnlyInt32(IReadOnlyList<int> list)
        {
            _ = await Assert.That(list.IndexOfClosest(7)).IsEqualTo(1);
        }

        [Test]
        [MethodDataSource(nameof(GetInt32Lists))]
        public async Task List(IList<int> list)
        {
            _ = await Assert.That(list.IndexOfClosest(7)).IsEqualTo(1);
        }

        [Test]
        [MethodDataSource(nameof(GetDoubleReadOnlyLists))]
        public async Task ReadOnlyDouble(IReadOnlyList<double> list)
        {
            _ = await Assert.That(list.IndexOfClosest(7D)).IsEqualTo(1);
        }
    }
}