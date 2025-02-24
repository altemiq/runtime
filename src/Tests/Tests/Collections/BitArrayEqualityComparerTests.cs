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
        var firstBitArray = new System.Collections.BitArray(BitConverter.GetBytes(Value));
        var secondBitArray = new System.Collections.BitArray(BitConverter.GetBytes(Value));

        await Assert
            .That(BitArrayEqualityComparer.Instance.GetHashCode(firstBitArray))
            .IsEqualTo(BitArrayEqualityComparer.Instance.GetHashCode(secondBitArray));
    }
}