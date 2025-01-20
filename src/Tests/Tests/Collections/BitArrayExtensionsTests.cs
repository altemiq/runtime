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
        Assert.Equal(0x03, bitConverter.GetByte());
    }

    [Fact]
    public void GetBytes()
    {
        var bitConverter = new System.Collections.BitArray(new byte[] { 0x03, 0x05 });
        Assert.Equal([0x03, 0x05], bitConverter.GetBytes());
    }

    [Fact]
    public void GetByteWithLength()
    {
        var bitConverter = new System.Collections.BitArray(new byte[] { 0x00, 0x03, 0x00, 0x00 });
        Assert.Equal(0x03, bitConverter.GetByte(8, 3));
    }

    [Theory]
    [InlineData(null, null, true)]
    [InlineData((byte)0x52, (byte)0x52, true)]
    [InlineData((byte)0x00, null, false)]
    [InlineData(null, (byte)0x00, false)]
    [InlineData((byte)0x01, (byte)0x02, false)]
    public void EqualsExtension(byte? first, byte? second, bool expected)
    {
        var firstBitArray = first.HasValue
            ? new System.Collections.BitArray([first.Value])
            : null;

        var secondBitArray = second.HasValue
            ? new System.Collections.BitArray([second.Value])
            : null;

        Assert.Equal(expected, BitArrayExtensions.Equals(firstBitArray!, secondBitArray!));
    }

    [Theory]
    [InlineData(0, "0")]
    [InlineData(128, "80")]
    [InlineData(254, "fe")]
    [InlineData(1065357908, "3f 80 12 54")]
    public void HexString(int bits, string expected)
    {
        var bytes = bits <= byte.MaxValue
            ? [(byte)bits]
            : BitConverter.GetBytes(bits);
        var bitArray = new System.Collections.BitArray(bytes);
        Assert.Equal(expected, bitArray.ToHexString(default));
    }
}