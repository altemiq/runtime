// -----------------------------------------------------------------------
// <copyright file="ListTests.IndexOf.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Linq;

public partial class ListTests
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public class IndexOf
    {
        [Test]
        [MethodDataSource(typeof(ListTests), nameof(GetLists))]
        public async Task List(IEnumerable<int> first, IEnumerable<int> second)
        {
            _ = await Assert.That(first).HasIndexOf(second).EqualTo(1);
            _ = await Assert.That(first).HasIndexOf(second).EqualTo(1, true);
        }

        [Test]
        [MethodDataSource(typeof(ListTests), nameof(GetInt32ReadOnlyLists))]
        public async Task Value(IReadOnlyList<int> list)
        {
            _ = await Assert.That(list.IndexOf(10, 1)).IsEqualTo(2);
        }

        [Test]
        [MethodDataSource(typeof(ListTests), nameof(GetLists))]
        public async Task Any(IEnumerable<int> first, IEnumerable<int> second)
        {
            _ = await Assert.That(first).HasIndexOfAny(second).EqualTo(1);
            _ = await Assert.That(first).HasIndexOfAny(second).EqualTo(1, true);
        }

        [Test]
        [MethodDataSource(typeof(ListTests), nameof(GetLists))]
        public async Task Lists(IEnumerable<int> first, IEnumerable<int> second)
        {
            switch (first)
            {
                case IList<int> firstList:
                    _ = await Assert.That(firstList.IndexOf(2, 0)).IsEqualTo(1);
                    break;
                case IReadOnlyList<int> firstReadOnlyList:
                    _ = await Assert.That(firstReadOnlyList.IndexOf(2, 0)).IsEqualTo(1);
                    break;
                default:
                    break;
            }

            switch (second)
            {
                case IList<int> secondList:
                    _ = await Assert.That(secondList.IndexOf(2, 0)).IsEqualTo(0);
                    break;
                case IReadOnlyList<int> secondReadOnlyList:
                    _ = await Assert.That(secondReadOnlyList.IndexOf(2, 0)).IsEqualTo(0);
                    break;
                default:
                    break;
            }
        }
    }
}