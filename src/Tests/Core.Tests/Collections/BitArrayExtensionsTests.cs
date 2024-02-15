// -----------------------------------------------------------------------
// <copyright file="BitArrayExtensionsTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Collections;
public class BitArrayExtensionsTests
{
    [Fact]
    public void GetByte()
    {
        var bitConverter = new System.Collections.BitArray(new byte[] { 0x03 });
        bitConverter.GetByte().Should().Be(0x03);
    }

    [Fact]
    public void GetByteWithLength()
    {
        var bitConverter = new System.Collections.BitArray(new byte[] { 0x00, 0x03, 0x00, 0x00 });
        bitConverter.GetByte(8, 3).Should().Be(0x03);
    }
}
