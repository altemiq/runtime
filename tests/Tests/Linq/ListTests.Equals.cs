// -----------------------------------------------------------------------
// <copyright file="ListTests.Equals.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Linq;

using System.Diagnostics.CodeAnalysis;

public partial class ListTests
{
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public partial class Equal
    {
        [Test]
        [MethodDataSource(typeof(ListTests), nameof(GetLists))]
        public async Task DoesEqual(IEnumerable<int> first, IEnumerable<int> second)
        {
            _ = await Assert.That(first).IsEqualTo(1, second);
            _ = await Assert.That(first).IsEqualTo(1, second, secondReadOnly: true);
            _ = await Assert.That(first).IsEqualTo(1, second, firstReadOnly: true);
            _ = await Assert.That(first).IsEqualTo(1, second, firstReadOnly: true, secondReadOnly: true);
        }

        [Test]
        [MethodDataSource(typeof(ListTests), nameof(GetLists))]
        public async Task DoesNotEqual(IEnumerable<int> first, IEnumerable<int> second)
        {
            _ = await Assert.That(first).IsNotEqualTo(2, second);
            _ = await Assert.That(first).IsNotEqualTo(2, second, secondReadOnly: true);
            _ = await Assert.That(first).IsNotEqualTo(2, second, firstReadOnly: true);
            _ = await Assert.That(first).IsNotEqualTo(2, second, firstReadOnly: true, secondReadOnly: true);
        }
    }
}