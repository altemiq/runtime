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

    [Theory]
    [InlineData(null, null, true)]
    [InlineData((byte)0x52, (byte)0x52, true)]
    [InlineData((byte)0x00, null, false)]
    [InlineData(null, (byte)0x00, false)]
    [InlineData((byte)0x01, (byte)0x02, false)]
    public void EqualsExtension(byte? first, byte? second, bool result)
    {
        var firstBitArray = first.HasValue
            ? new System.Collections.BitArray(new byte[] { first.Value })
            : null;

        var secondBitArray = second.HasValue
            ? new System.Collections.BitArray(new byte[] { second.Value })
            : null;

        _ = BitArrayExtensions.Equals(firstBitArray!, secondBitArray!).Should().Be(result);
    }
}