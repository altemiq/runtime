// -----------------------------------------------------------------------
// <copyright file="ListTests.Equals.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using TUnit.Assertions.Core;

namespace Altemiq.Linq;

public partial class ListTests
{
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public partial class Equal
    {
        [Test]
        [MethodDataSource(typeof(ListTests), nameof(GetLists))]
        public async Task DoesEqual(IEnumerable<int> first, IEnumerable<int> second)
        {
            await Assert.That(first).IsEqualTo(1, second);
            await Assert.That(first).IsEqualTo(1, second, secondReadOnly: true);
            await Assert.That(first).IsEqualTo(1, second, firstReadOnly: true);
            await Assert.That(first).IsEqualTo(1, second, firstReadOnly: true, secondReadOnly: true);
        }

        [Test]
        [MethodDataSource(typeof(ListTests), nameof(GetLists))]
        public async Task DoesNotEqual(IEnumerable<int> first, IEnumerable<int> second)
        {
            await Assert.That(first).IsNotEqualTo(2, second);
            await Assert.That(first).IsNotEqualTo(2, second, secondReadOnly: true);
            await Assert.That(first).IsNotEqualTo(2, second, firstReadOnly: true);
            await Assert.That(first).IsNotEqualTo(2, second, firstReadOnly: true, secondReadOnly: true);
        }
    }
}