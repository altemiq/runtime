// -----------------------------------------------------------------------
// <copyright file="BitArrayEqualityComparerTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Collections;

public class BitArrayEqualityComparerTests
{
    [Fact]
    public void ConsistentHashCode()
    {
        const int Value = 12345;
        var firstBitArray = new System.Collections.BitArray(BitConverter.GetBytes(Value));
        var secondBitArray = new System.Collections.BitArray(BitConverter.GetBytes(Value));

        BitArrayEqualityComparer.Instance.GetHashCode(firstBitArray).Should().Be(BitArrayEqualityComparer.Instance.GetHashCode(secondBitArray));
    }
}