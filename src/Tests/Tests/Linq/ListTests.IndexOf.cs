// -----------------------------------------------------------------------
// <copyright file="ListTests.IndexOf.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Linq;

public partial class ListTests
{
    public class IndexOf
    {
        [Test]
        [MethodDataSource(typeof(ListTests), nameof(GetLists))]
        public async Task Int32(IEnumerable<int> first, IEnumerable<int> second)
        {
            await Assert.That(TestListList<int, int>(first, second, static (f, s) => f.IndexOf(s), 1)).IsEqualTo(1);
            await Assert.That(TestReadOnlyListReadOnlyList<int, int>(first, second, static (f, s) => f.IndexOf(s), 1)).IsEqualTo(1);
        }

        [Test]
        public async Task WithNulls()
        {
            await Assert.That(((IList<int>)null!).IndexOf((IList<int>)null!)).IsEqualTo(-1);
            await Assert.That(((IReadOnlyList<int>)null!).IndexOf((IReadOnlyList<int>)null!)).IsEqualTo(-1);
        }

        [Test]
        [MethodDataSource(typeof(ListTests), nameof(GetInt32ReadOnlyLists))]
        public async Task Value(IReadOnlyList<int> list) => await Assert.That(list.IndexOf(10, 1)).IsEqualTo(2);

        [Test]
        [MethodDataSource(typeof(ListTests), nameof(GetLists))]
        public async Task Any(IEnumerable<int> first, IEnumerable<int> second)
        {
            await Assert.That(TestListList<int, int>(first, second, static (f, s) => f.IndexOfAny(s), 1)).IsEqualTo(1);
            await Assert.That(TestReadOnlyListReadOnlyList<int, int>(first, second, static (f, s) => f.IndexOfAny(s), 1)).IsEqualTo(1);
        }

        [Test]
        [MethodDataSource(typeof(ListTests), nameof(GetLists))]
        public async Task Lists(IEnumerable<int> first, IEnumerable<int> second)
        {
            if (first is IList<int> firstList)
            {
                await Assert.That(firstList.IndexOf(2, 0)).IsEqualTo(1);
            }
            else if (first is IReadOnlyList<int> firstReadOnlyList)
            {
                await Assert.That(firstReadOnlyList.IndexOf(2, 0)).IsEqualTo(1);
            }

            if (second is IList<int> secondList)
            {
                await Assert.That(secondList.IndexOf(2, 0)).IsEqualTo(0);
            }
            else if (second is IReadOnlyList<int> secondReadOnlyList)
            {
                await Assert.That(secondReadOnlyList.IndexOf(2, 0)).IsEqualTo(0);
            }
        }
    }
}