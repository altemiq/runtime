// -----------------------------------------------------------------------
// <copyright file="BinaryPrimitivesTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Binary;

public class BinaryPrimitivesTests
{
    private const int PositiveByte = 0x7F;

    private const int FloatByte = 0xEF;

    private const int Int16Negative = -1;

    private const int Int16MaxValue = short.MaxValue;

    private const int ByteMaxValue = byte.MaxValue;

    [Test]
    [Arguments(Int16MaxValue, PositiveByte, ByteMaxValue)]
    [Arguments(Int16Negative, ByteMaxValue, ByteMaxValue)]
    public async Task WriteInt16BigEndian(short value, byte first, byte second)
    {
        byte[] bytes = new byte[sizeof(short)];
        BinaryPrimitives.WriteInt16BigEndian(bytes, value);
        await Assert.That(bytes).IsEquivalentTo([first, second]);
    }

    [Test]
    [Arguments(Int16MaxValue, PositiveByte, ByteMaxValue)]
    [Arguments(Int16Negative, ByteMaxValue, ByteMaxValue)]
    public async Task WriteInt16LittleEndian(short value, byte second, byte first)
    {
        byte[] bytes = new byte[sizeof(short)];
        BinaryPrimitives.WriteInt16LittleEndian(bytes, value);
        await Assert.That(bytes).IsEquivalentTo([first, second]);
    }

    [Test]
    [Arguments(int.MaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    public async Task WriteInt32BigEndian(int value, byte first, byte second, byte third, byte forth)
    {
        byte[] bytes = new byte[sizeof(int)];
        BinaryPrimitives.WriteInt32BigEndian(bytes, value);
        await Assert.That(bytes).IsEquivalentTo([first, second, third, forth]);
    }

    [Test]
    [Arguments(int.MaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    public async Task WriteInt32LittleEndian(int value, byte forth, byte third, byte second, byte first)
    {
        byte[] bytes = new byte[sizeof(int)];
        BinaryPrimitives.WriteInt32LittleEndian(bytes, value);
        await Assert.That(bytes).IsEquivalentTo([first, second, third, forth]);
    }

    [Test]
    [Arguments(long.MaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    public async Task WriteInt64BigEndian(long value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth)
    {
        byte[] bytes = new byte[sizeof(long)];
        BinaryPrimitives.WriteInt64BigEndian(bytes, value);
        await Assert.That(bytes).IsEquivalentTo([first, second, third, forth, fifth, sixth, seventh, eighth]);
    }

    [Test]
    [Arguments(long.MaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    public async Task WriteInt64LittleEndian(long value, byte eighth, byte seventh, byte sixth, byte fifth, byte forth, byte third, byte second, byte first)
    {
        byte[] bytes = new byte[sizeof(long)];
        BinaryPrimitives.WriteInt64LittleEndian(bytes, value);
        await Assert.That(bytes).IsEquivalentTo([first, second, third, forth, fifth, sixth, seventh, eighth]);
    }

    [Test]
    [Arguments(ushort.MaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(ushort.MaxValue / 2, PositiveByte, ByteMaxValue)]
    public async Task WriteUInt16BigEndian(ushort value, byte first, byte second)
    {
        byte[] bytes = new byte[sizeof(ushort)];
        BinaryPrimitives.WriteUInt16BigEndian(bytes, value);
        await Assert.That(bytes).IsEquivalentTo([first, second]);
    }

    [Test]
    [Arguments(ushort.MaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(ushort.MaxValue / 2, PositiveByte, ByteMaxValue)]
    public async Task WriteUInt16LittleEndian(ushort value, byte second, byte first)
    {
        byte[] bytes = new byte[sizeof(ushort)];
        BinaryPrimitives.WriteUInt16LittleEndian(bytes, value);
        await Assert.That(bytes).IsEquivalentTo([first, second]);
    }

    [Test]
    [Arguments(uint.MaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(uint.MaxValue / 2, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    public async Task WriteUInt32BigEndian(uint value, byte first, byte second, byte third, byte forth)
    {
        byte[] bytes = new byte[sizeof(uint)];
        BinaryPrimitives.WriteUInt32BigEndian(bytes, value);
        await Assert.That(bytes).IsEquivalentTo([first, second, third, forth]);
    }

    [Test]
    [Arguments(uint.MaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(uint.MaxValue / 2, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    public async Task WriteUInt32LittleEndian(uint value, byte forth, byte third, byte second, byte first)
    {
        byte[] bytes = new byte[sizeof(uint)];
        BinaryPrimitives.WriteUInt32LittleEndian(bytes, value);
        await Assert.That(bytes).IsEquivalentTo([first, second, third, forth]);
    }

    [Test]
    [Arguments(ulong.MaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(ulong.MaxValue / 2, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    public async Task WriteUInt64BigEndian(ulong value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth)
    {
        byte[] bytes = new byte[sizeof(ulong)];
        BinaryPrimitives.WriteUInt64BigEndian(bytes, value);
        await Assert.That(bytes).IsEquivalentTo([first, second, third, forth, fifth, sixth, seventh, eighth]);
    }

    [Test]
    [Arguments(ulong.MaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(ulong.MaxValue / 2, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    public async Task WriteUInt64LittleEndian(ulong value, byte eighth, byte seventh, byte sixth, byte fifth, byte forth, byte third, byte second, byte first)
    {
        byte[] bytes = new byte[sizeof(ulong)];
        BinaryPrimitives.WriteUInt64LittleEndian(bytes, value);
        await Assert.That(bytes).IsEquivalentTo([first, second, third, forth, fifth, sixth, seventh, eighth]);
    }

#if NET5_0_OR_GREATER
    [Test]
    [Arguments(65504D, 0x7B, ByteMaxValue)]
    [Arguments(-65504D, 0xFB, ByteMaxValue)]
    public async Task WriteHalfBigEndian(Half value, byte first, byte second)
    {
        byte[] bytes = new byte[sizeof(short)];
        BinaryPrimitives.WriteHalfBigEndian(bytes, value);
        await Assert.That(bytes).IsEquivalentTo([first, second]);
    }

    [Test]
    [Arguments(65504D, 0x7B, ByteMaxValue)]
    [Arguments(-65504D, 0xFB, ByteMaxValue)]
    public async Task WriteHalfLittleEndian(Half value, byte second, byte first)
    {
        byte[] bytes = new byte[sizeof(short)];
        BinaryPrimitives.WriteHalfLittleEndian(bytes, value);
        await Assert.That(bytes).IsEquivalentTo([first, second]);
    }
#endif

    [Test]
    [Arguments(float.MaxValue, PositiveByte, PositiveByte, ByteMaxValue, ByteMaxValue)]
    [Arguments(float.MinValue, ByteMaxValue, PositiveByte, ByteMaxValue, ByteMaxValue)]
    public async Task WriteSingleBigEndian(float value, byte first, byte second, byte third, byte forth)
    {
        byte[] bytes = new byte[sizeof(float)];
        BinaryPrimitives.WriteSingleBigEndian(bytes, value);
        await Assert.That(bytes).IsEquivalentTo([first, second, third, forth]);
    }

    [Test]
    [Arguments(float.MaxValue, PositiveByte, PositiveByte, ByteMaxValue, ByteMaxValue)]
    [Arguments(float.MinValue, ByteMaxValue, PositiveByte, ByteMaxValue, ByteMaxValue)]
    public async Task WriteSingleLittleEndian(float value, byte forth, byte third, byte second, byte first)
    {
        byte[] bytes = new byte[sizeof(float)];
        BinaryPrimitives.WriteSingleLittleEndian(bytes, value);
        await Assert.That(bytes).IsEquivalentTo([first, second, third, forth]);
    }

    [Test]
    [Arguments(double.MaxValue, PositiveByte, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(double.MinValue, ByteMaxValue, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    public async Task WriteDoubleBigEndian(double value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth)
    {
        byte[] bytes = new byte[sizeof(double)];
        BinaryPrimitives.WriteDoubleBigEndian(bytes, value);
        await Assert.That(bytes).IsEquivalentTo([first, second, third, forth, fifth, sixth, seventh, eighth]);
    }

    [Test]
    [Arguments(double.MaxValue, PositiveByte, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(double.MinValue, ByteMaxValue, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    public async Task WriteDoubleLittleEndian(double value, byte eighth, byte seventh, byte sixth, byte fifth, byte forth, byte third, byte second, byte first)
    {
        byte[] bytes = new byte[sizeof(double)];
        BinaryPrimitives.WriteDoubleLittleEndian(bytes, value);
        await Assert.That(bytes).IsEquivalentTo([first, second, third, forth, fifth, sixth, seventh, eighth]);
    }

    [Test]
    [Arguments(Int16MaxValue, PositiveByte, ByteMaxValue)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue)]
    public async Task TryWriteInt16BigEndian(short value, byte first, byte second)
    {
        byte[] bytes = new byte[sizeof(short)];
        await Assert.That(BinaryPrimitives.TryWriteInt16BigEndian(bytes, value)).IsTrue();
        await Assert.That(bytes).IsEquivalentTo([first, second]);
    }

    [Test]
    [Arguments(Int16MaxValue, PositiveByte, ByteMaxValue)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue)]
    public async Task TryWriteInt16LittleEndian(short value, byte second, byte first)
    {
        byte[] bytes = new byte[sizeof(short)];
        await Assert.That(BinaryPrimitives.TryWriteInt16LittleEndian(bytes, value)).IsTrue();
        await Assert.That(bytes).IsEquivalentTo([first, second]);
    }

    [Test]
    [Arguments(int.MaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    public async Task TryWriteInt32BigEndian(int value, byte first, byte second, byte third, byte forth)
    {
        byte[] bytes = new byte[sizeof(int)];
        await Assert.That(BinaryPrimitives.TryWriteInt32BigEndian(bytes, value)).IsTrue();
        await Assert.That(bytes).IsEquivalentTo([first, second, third, forth]);
    }

    [Test]
    [Arguments(int.MaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    public async Task TryWriteInt32LittleEndian(int value, byte forth, byte third, byte second, byte first)
    {
        byte[] bytes = new byte[sizeof(int)];
        await Assert.That(BinaryPrimitives.TryWriteInt32LittleEndian(bytes, value)).IsTrue();
        await Assert.That(bytes).IsEquivalentTo([first, second, third, forth]);
    }

    [Test]
    [Arguments(long.MaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    public async Task TryWriteInt64BigEndian(long value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth)
    {
        byte[] bytes = new byte[sizeof(long)];
        await Assert.That(BinaryPrimitives.TryWriteInt64BigEndian(bytes, value)).IsTrue();
        await Assert.That(bytes).IsEquivalentTo([first, second, third, forth, fifth, sixth, seventh, eighth]);
    }

    [Test]
    [Arguments(long.MaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    public async Task TryWriteInt64LittleEndian(long value, byte eighth, byte seventh, byte sixth, byte fifth, byte forth, byte third, byte second, byte first)
    {
        byte[] bytes = new byte[sizeof(long)];
        await Assert.That(BinaryPrimitives.TryWriteInt64LittleEndian(bytes, value)).IsTrue();
        await Assert.That(bytes).IsEquivalentTo([first, second, third, forth, fifth, sixth, seventh, eighth]);
    }

    [Test]
    [Arguments(ushort.MaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(ushort.MaxValue / 2, PositiveByte, ByteMaxValue)]
    public async Task TryWriteUInt16BigEndian(ushort value, byte first, byte second)
    {
        byte[] bytes = new byte[sizeof(ushort)];
        await Assert.That(BinaryPrimitives.TryWriteUInt16BigEndian(bytes, value)).IsTrue();
        await Assert.That(bytes).IsEquivalentTo([first, second]);
    }

    [Test]
    [Arguments(ushort.MaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(ushort.MaxValue / 2, PositiveByte, ByteMaxValue)]
    public async Task TryWriteUInt16LittleEndian(ushort value, byte second, byte first)
    {
        byte[] bytes = new byte[sizeof(ushort)];
        await Assert.That(BinaryPrimitives.TryWriteUInt16LittleEndian(bytes, value)).IsTrue();
        await Assert.That(bytes).IsEquivalentTo([first, second]);
    }

    [Test]
    [Arguments(uint.MaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(uint.MaxValue / 2, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    public async Task TryWriteUInt32BigEndian(uint value, byte first, byte second, byte third, byte forth)
    {
        byte[] bytes = new byte[sizeof(uint)];
        await Assert.That(BinaryPrimitives.TryWriteUInt32BigEndian(bytes, value)).IsTrue();
        await Assert.That(bytes).IsEquivalentTo([first, second, third, forth]);
    }

    [Test]
    [Arguments(uint.MaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(uint.MaxValue / 2, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    public async Task TryWriteUInt32LittleEndian(uint value, byte forth, byte third, byte second, byte first)
    {
        byte[] bytes = new byte[sizeof(uint)];
        await Assert.That(BinaryPrimitives.TryWriteUInt32LittleEndian(bytes, value)).IsTrue();
        await Assert.That(bytes).IsEquivalentTo([first, second, third, forth]);
    }

    [Test]
    [Arguments(ulong.MaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(ulong.MaxValue / 2, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    public async Task TryWriteUInt64BigEndian(ulong value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth)
    {
        byte[] bytes = new byte[sizeof(ulong)];
        await Assert.That(BinaryPrimitives.TryWriteUInt64BigEndian(bytes, value)).IsTrue();
        await Assert.That(bytes).IsEquivalentTo([first, second, third, forth, fifth, sixth, seventh, eighth]);
    }

    [Test]
    [Arguments(ulong.MaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(ulong.MaxValue / 2, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    public async Task TryWriteUInt64LittleEndian(ulong value, byte eighth, byte seventh, byte sixth, byte fifth, byte forth, byte third, byte second, byte first)
    {
        byte[] bytes = new byte[sizeof(ulong)];
        await Assert.That(BinaryPrimitives.TryWriteUInt64LittleEndian(bytes, value)).IsTrue();
        await Assert.That(bytes).IsEquivalentTo([first, second, third, forth, fifth, sixth, seventh, eighth]);
    }

#if NET5_0_OR_GREATER
    [Test]
    [Arguments(65504D, 0x7B, ByteMaxValue)]
    [Arguments(-65504D, 0xFB, ByteMaxValue)]
    public async Task TryWriteHalfBigEndian(Half value, byte first, byte second)
    {
        byte[] bytes = new byte[sizeof(short)];
        await Assert.That(BinaryPrimitives.TryWriteHalfBigEndian(bytes, value)).IsTrue();
        await Assert.That(bytes).IsEquivalentTo([first, second]);
    }

    [Test]
    [Arguments(65504D, 0x7B, ByteMaxValue)]
    [Arguments(-65504D, 0xFB, ByteMaxValue)]
    public async Task TryWriteHalfLittleEndian(Half value, byte second, byte first)
    {
        byte[] bytes = new byte[sizeof(short)];
        await Assert.That(BinaryPrimitives.TryWriteHalfLittleEndian(bytes, value)).IsTrue();
        await Assert.That(bytes).IsEquivalentTo([first, second]);
    }
#endif

    [Test]
    [Arguments(float.MaxValue, PositiveByte, PositiveByte, ByteMaxValue, ByteMaxValue)]
    [Arguments(float.MinValue, ByteMaxValue, PositiveByte, ByteMaxValue, ByteMaxValue)]
    public async Task TryWriteSingleBigEndian(float value, byte first, byte second, byte third, byte forth)
    {
        byte[] bytes = new byte[sizeof(float)];
        await Assert.That(BinaryPrimitives.TryWriteSingleBigEndian(bytes, value)).IsTrue();
        await Assert.That(bytes).IsEquivalentTo([first, second, third, forth]);
    }

    [Test]
    [Arguments(float.MaxValue, PositiveByte, PositiveByte, ByteMaxValue, ByteMaxValue)]
    [Arguments(float.MinValue, ByteMaxValue, PositiveByte, ByteMaxValue, ByteMaxValue)]
    public async Task TryWriteSingleLittleEndian(float value, byte forth, byte third, byte second, byte first)
    {
        byte[] bytes = new byte[sizeof(float)];
        await Assert.That(BinaryPrimitives.TryWriteSingleLittleEndian(bytes, value)).IsTrue();
        await Assert.That(bytes).IsEquivalentTo([first, second, third, forth]);
    }

    [Test]
    [Arguments(double.MaxValue, PositiveByte, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(double.MinValue, ByteMaxValue, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    public async Task TryWriteDoubleBigEndian(double value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth)
    {
        byte[] bytes = new byte[sizeof(double)];
        await Assert.That(BinaryPrimitives.TryWriteDoubleBigEndian(bytes, value)).IsTrue();
        await Assert.That(bytes).IsEquivalentTo([first, second, third, forth, fifth, sixth, seventh, eighth]);
    }

    [Test]
    [Arguments(double.MaxValue, PositiveByte, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(double.MinValue, ByteMaxValue, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    public async Task TryWriteDoubleLittleEndian(double value, byte eighth, byte seventh, byte sixth, byte fifth, byte forth, byte third, byte second, byte first)
    {
        byte[] bytes = new byte[sizeof(double)];
        await Assert.That(BinaryPrimitives.TryWriteDoubleLittleEndian(bytes, value)).IsTrue();
        await Assert.That(bytes).IsEquivalentTo([first, second, third, forth, fifth, sixth, seventh, eighth]);
    }
}