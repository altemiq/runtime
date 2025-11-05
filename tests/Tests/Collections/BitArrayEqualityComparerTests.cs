// -----------------------------------------------------------------------
// <copyright file="BitArrayEqualityComparerTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Collections;

public class BitArrayEqualityComparerTests
{
    [Test]
    public async Task ConsistentHashCode()
    {
        const int Value = 12345;
        await Assert
            .That(BitArrayEqualityComparer.Instance.GetHashCode(new(BitConverter.GetBytes(Value))))
            .IsEqualTo(BitArrayEqualityComparer.Instance.GetHashCode(new(BitConverter.GetBytes(Value))));
    }
}