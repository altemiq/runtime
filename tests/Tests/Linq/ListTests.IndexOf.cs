// -----------------------------------------------------------------------
// <copyright file="ListTests.IndexOf.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace Altemiq.Linq;

public partial class ListTests
{
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public class IndexOf
    {
        [Test]
        [MethodDataSource(typeof(ListTests), nameof(GetLists))]
        public async Task Int32(IEnumerable<int> first, IEnumerable<int> second)
        {
            await Assert.That(first).HasIndexOf(second).EqualTo(1);
            await Assert.That(first).HasIndexOf(second).EqualTo(1, true);
        }

        [Test]
        [MethodDataSource(typeof(ListTests), nameof(GetInt32ReadOnlyLists))]
        public async Task Value(IReadOnlyList<int> list) => await Assert.That(list.IndexOf(10, 1)).IsEqualTo(2);

        [Test]
        [MethodDataSource(typeof(ListTests), nameof(GetLists))]
        public async Task Any(IEnumerable<int> first, IEnumerable<int> second)
        {
            await Assert.That(first).HasIndexOfAny(second).EqualTo(1);
            await Assert.That(first).HasIndexOfAny(second).EqualTo(1, true);
        }

        [Test]
        [MethodDataSource(typeof(ListTests), nameof(GetLists))]
        public async Task Lists(IEnumerable<int> first, IEnumerable<int> second)
        {
            switch (first)
            {
                case IList<int> firstList:
                    await Assert.That(firstList.IndexOf(2, 0)).IsEqualTo(1);
                    break;
                case IReadOnlyList<int> firstReadOnlyList:
                    await Assert.That(firstReadOnlyList.IndexOf(2, 0)).IsEqualTo(1);
                    break;
            }

            switch (second)
            {
                case IList<int> secondList:
                    await Assert.That(secondList.IndexOf(2, 0)).IsEqualTo(0);
                    break;
                case IReadOnlyList<int> secondReadOnlyList:
                    await Assert.That(secondReadOnlyList.IndexOf(2, 0)).IsEqualTo(0);
                    break;
            }
        }
    }
}