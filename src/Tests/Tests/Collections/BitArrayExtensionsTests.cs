// -----------------------------------------------------------------------
// <copyright file="BitArrayExtensionsTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Collections;
public class BitArrayExtensionsTests
{
    [Test]
    public async Task GetByte()
    {
        const byte Value = 0x03;
        var bitConverter = new System.Collections.BitArray([Value]);
        await Assert.That(bitConverter.GetByte()).IsEqualTo(Value);
    }

    [Test]
    public async Task GetBytes()
    {
        const byte First = 0x03;
        const byte Second = 0x05;
        var bitConverter = new System.Collections.BitArray([First, Second]);
        await Assert.That(bitConverter.GetBytes()).IsEquivalentTo([First, Second]);
    }

    [Test]
    public async Task GetByteWithLength()
    {
        const byte Value = 0x03;
        var bitConverter = new System.Collections.BitArray(new byte[] { 0x00, Value, 0x00, 0x00 });
        await Assert.That(bitConverter.GetByte(8, 3)).IsEqualTo(Value);
    }

    [Test]
    [Arguments(null, null, true)]
    [Arguments(0x52, 0x52, true)]
    [Arguments(0x00, null, false)]
    [Arguments(null, 0x00, false)]
    [Arguments(0x01, 0x02, false)]
    public async Task EqualsExtension(byte? first, byte? second, bool expected)
    {
        var firstBitArray = first.HasValue
            ? new System.Collections.BitArray([first.Value])
            : null;

        var secondBitArray = second.HasValue
            ? new System.Collections.BitArray([second.Value])
            : null;

        await Assert.That(BitArrayExtensions.Equals(firstBitArray!, secondBitArray!)).IsEqualTo(expected);
    }

    [Test]
    [Arguments(0, "0")]
    [Arguments(128, "80")]
    [Arguments(254, "fe")]
    [Arguments(1065357908, "3f 80 12 54")]
    public async Task HexString(int bits, string expected)
    {
        var bytes = bits <= byte.MaxValue
            ? [(byte)bits]
            : BitConverter.GetBytes(bits);
        var bitArray = new System.Collections.BitArray(bytes);
        await Assert.That(bitArray.ToHexString(default)).IsEqualTo(expected);
    }
}