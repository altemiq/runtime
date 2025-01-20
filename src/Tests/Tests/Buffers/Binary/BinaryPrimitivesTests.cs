// -----------------------------------------------------------------------
// <copyright file="BinaryPrimitivesTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Binary;

public class BinaryPrimitivesTests
{
    private const byte PositiveByte = 0x7F;

    private const byte FloatByte = 0xEF;

    [Theory]
    [InlineData(short.MaxValue, PositiveByte, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue)]
    public void WriteInt16BigEndian(short value, byte first, byte second)
    {
        Span<byte> bytes = stackalloc byte[sizeof(short)];
        BinaryPrimitives.WriteInt16BigEndian(bytes, value);
        Assert.Equal([first, second], bytes.ToArray());
    }

    [Theory]
    [InlineData(short.MaxValue, PositiveByte, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue)]
    public void WriteInt16LittleEndian(short value, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(short)];
        BinaryPrimitives.WriteInt16LittleEndian(bytes, value);
        Assert.Equal([first, second], bytes.ToArray());
    }

    [Theory]
    [InlineData(int.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void WriteInt32BigEndian(int value, byte first, byte second, byte third, byte forth)
    {
        Span<byte> bytes = stackalloc byte[sizeof(int)];
        BinaryPrimitives.WriteInt32BigEndian(bytes, value);
        Assert.Equal([first, second, third, forth], bytes.ToArray());
    }

    [Theory]
    [InlineData(int.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void WriteInt32LittleEndian(int value, byte forth, byte third, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(int)];
        BinaryPrimitives.WriteInt32LittleEndian(bytes, value);
        Assert.Equal([first, second, third, forth], bytes.ToArray());
    }

    [Theory]
    [InlineData(long.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void WriteInt64BigEndian(long value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth)
    {
        Span<byte> bytes = stackalloc byte[sizeof(long)];
        BinaryPrimitives.WriteInt64BigEndian(bytes, value);
        Assert.Equal([first, second, third, forth, fifth, sixth, seventh, eighth], bytes.ToArray());
    }

    [Theory]
    [InlineData(long.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void WriteInt64LittleEndian(long value, byte eighth, byte seventh, byte sixth, byte fifth, byte forth, byte third, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(long)];
        BinaryPrimitives.WriteInt64LittleEndian(bytes, value);
        Assert.Equal([first, second, third, forth, fifth, sixth, seventh, eighth], bytes.ToArray());
    }

    [Theory]
    [InlineData(ushort.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(ushort.MaxValue / 2, PositiveByte, byte.MaxValue)]
    public void WriteUInt16BigEndian(ushort value, byte first, byte second)
    {
        Span<byte> bytes = stackalloc byte[sizeof(ushort)];
        BinaryPrimitives.WriteUInt16BigEndian(bytes, value);
        Assert.Equal([first, second], bytes.ToArray());
    }

    [Theory]
    [InlineData(ushort.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(ushort.MaxValue / 2, PositiveByte, byte.MaxValue)]
    public void WriteUInt16LittleEndian(ushort value, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(ushort)];
        BinaryPrimitives.WriteUInt16LittleEndian(bytes, value);
        Assert.Equal([first, second], bytes.ToArray());
    }

    [Theory]
    [InlineData(uint.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(uint.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void WriteUInt32BigEndian(uint value, byte first, byte second, byte third, byte forth)
    {
        Span<byte> bytes = stackalloc byte[sizeof(uint)];
        BinaryPrimitives.WriteUInt32BigEndian(bytes, value);
        Assert.Equal([first, second, third, forth], bytes.ToArray());
    }

    [Theory]
    [InlineData(uint.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(uint.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void WriteUInt32LittleEndian(uint value, byte forth, byte third, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(uint)];
        BinaryPrimitives.WriteUInt32LittleEndian(bytes, value);
        Assert.Equal([first, second, third, forth], bytes.ToArray());
    }

    [Theory]
    [InlineData(ulong.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(ulong.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void WriteUInt64BigEndian(ulong value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth)
    {
        Span<byte> bytes = stackalloc byte[sizeof(ulong)];
        BinaryPrimitives.WriteUInt64BigEndian(bytes, value);
        Assert.Equal([first, second, third, forth, fifth, sixth, seventh, eighth], bytes.ToArray());
    }

    [Theory]
    [InlineData(ulong.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(ulong.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void WriteUInt64LittleEndian(ulong value, byte eighth, byte seventh, byte sixth, byte fifth, byte forth, byte third, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(ulong)];
        BinaryPrimitives.WriteUInt64LittleEndian(bytes, value);
        Assert.Equal([first, second, third, forth, fifth, sixth, seventh, eighth], bytes.ToArray());
    }

#if NET5_0_OR_GREATER
    [Theory]
    [InlineData(65504D, 0x7B, byte.MaxValue)]
    [InlineData(-65504D, 0xFB, byte.MaxValue)]
    public void WriteHalfBigEndian(Half value, byte first, byte second)
    {
        Span<byte> bytes = stackalloc byte[sizeof(short)];
        BinaryPrimitives.WriteHalfBigEndian(bytes, value);
        Assert.Equal([first, second], bytes.ToArray());
    }

    [Theory]
    [InlineData(65504D, 0x7B, byte.MaxValue)]
    [InlineData(-65504D, 0xFB, byte.MaxValue)]
    public void WriteHalfLittleEndian(Half value, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(short)];
        BinaryPrimitives.WriteHalfLittleEndian(bytes, value);
        Assert.Equal([first, second], bytes.ToArray());
    }
#endif

    [Theory]
    [InlineData(float.MaxValue, PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue)]
    [InlineData(float.MinValue, byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue)]
    public void WriteSingleBigEndian(float value, byte first, byte second, byte third, byte forth)
    {
        Span<byte> bytes = stackalloc byte[sizeof(float)];
        BinaryPrimitives.WriteSingleBigEndian(bytes, value);
        Assert.Equal([first, second, third, forth], bytes.ToArray());
    }

    [Theory]
    [InlineData(float.MaxValue, PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue)]
    [InlineData(float.MinValue, byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue)]
    public void WriteSingleLittleEndian(float value, byte forth, byte third, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(float)];
        BinaryPrimitives.WriteSingleLittleEndian(bytes, value);
        Assert.Equal([first, second, third, forth], bytes.ToArray());
    }

    [Theory]
    [InlineData(double.MaxValue, PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(double.MinValue, byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void WriteDoubleBigEndian(double value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth)
    {
        Span<byte> bytes = stackalloc byte[sizeof(double)];
        BinaryPrimitives.WriteDoubleBigEndian(bytes, value);
        Assert.Equal([first, second, third, forth, fifth, sixth, seventh, eighth], bytes.ToArray());
    }

    [Theory]
    [InlineData(double.MaxValue, PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(double.MinValue, byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void WriteDoubleLittleEndian(double value, byte eighth, byte seventh, byte sixth, byte fifth, byte forth, byte third, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(double)];
        BinaryPrimitives.WriteDoubleLittleEndian(bytes, value);
        Assert.Equal([first, second, third, forth, fifth, sixth, seventh, eighth], bytes.ToArray());
    }

    [Theory]
    [InlineData(short.MaxValue, PositiveByte, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue)]
    public void TryWriteInt16BigEndian(short value, byte first, byte second)
    {
        Span<byte> bytes = stackalloc byte[sizeof(short)];
        Assert.True(BinaryPrimitives.TryWriteInt16BigEndian(bytes, value));
        Assert.Equal([first, second], bytes.ToArray());
    }

    [Theory]
    [InlineData(short.MaxValue, PositiveByte, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue)]
    public void TryWriteInt16LittleEndian(short value, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(short)];
        Assert.True(BinaryPrimitives.TryWriteInt16LittleEndian(bytes, value));
        Assert.Equal([first, second], bytes.ToArray());
    }

    [Theory]
    [InlineData(int.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void TryWriteInt32BigEndian(int value, byte first, byte second, byte third, byte forth)
    {
        Span<byte> bytes = stackalloc byte[sizeof(int)];
        Assert.True(BinaryPrimitives.TryWriteInt32BigEndian(bytes, value));
        Assert.Equal([first, second, third, forth], bytes.ToArray());
    }

    [Theory]
    [InlineData(int.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void TryWriteInt32LittleEndian(int value, byte forth, byte third, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(int)];
        Assert.True(BinaryPrimitives.TryWriteInt32LittleEndian(bytes, value));
        Assert.Equal([first, second, third, forth], bytes.ToArray());
    }

    [Theory]
    [InlineData(long.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void TryWriteInt64BigEndian(long value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth)
    {
        Span<byte> bytes = stackalloc byte[sizeof(long)];
        Assert.True(BinaryPrimitives.TryWriteInt64BigEndian(bytes, value));
        Assert.Equal([first, second, third, forth, fifth, sixth, seventh, eighth], bytes.ToArray());
    }

    [Theory]
    [InlineData(long.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void TryWriteInt64LittleEndian(long value, byte eighth, byte seventh, byte sixth, byte fifth, byte forth, byte third, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(long)];
        Assert.True(BinaryPrimitives.TryWriteInt64LittleEndian(bytes, value));
        Assert.Equal([first, second, third, forth, fifth, sixth, seventh, eighth], bytes.ToArray());
    }

    [Theory]
    [InlineData(ushort.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(ushort.MaxValue / 2, PositiveByte, byte.MaxValue)]
    public void TryWriteUInt16BigEndian(ushort value, byte first, byte second)
    {
        Span<byte> bytes = stackalloc byte[sizeof(ushort)];
        Assert.True(BinaryPrimitives.TryWriteUInt16BigEndian(bytes, value));
        Assert.Equal([first, second], bytes.ToArray());
    }

    [Theory]
    [InlineData(ushort.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(ushort.MaxValue / 2, PositiveByte, byte.MaxValue)]
    public void TryWriteUInt16LittleEndian(ushort value, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(ushort)];
        Assert.True(BinaryPrimitives.TryWriteUInt16LittleEndian(bytes, value));
        Assert.Equal([first, second], bytes.ToArray());
    }

    [Theory]
    [InlineData(uint.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(uint.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void TryWriteUInt32BigEndian(uint value, byte first, byte second, byte third, byte forth)
    {
        Span<byte> bytes = stackalloc byte[sizeof(uint)];
        Assert.True(BinaryPrimitives.TryWriteUInt32BigEndian(bytes, value));
        Assert.Equal([first, second, third, forth], bytes.ToArray());
    }

    [Theory]
    [InlineData(uint.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(uint.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void TryWriteUInt32LittleEndian(uint value, byte forth, byte third, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(uint)];
        Assert.True(BinaryPrimitives.TryWriteUInt32LittleEndian(bytes, value));
        Assert.Equal([first, second, third, forth], bytes.ToArray());
    }

    [Theory]
    [InlineData(ulong.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(ulong.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void TryWriteUInt64BigEndian(ulong value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth)
    {
        Span<byte> bytes = stackalloc byte[sizeof(ulong)];
        Assert.True(BinaryPrimitives.TryWriteUInt64BigEndian(bytes, value));
        Assert.Equal([first, second, third, forth, fifth, sixth, seventh, eighth], bytes.ToArray());
    }

    [Theory]
    [InlineData(ulong.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(ulong.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void TryWriteUInt64LittleEndian(ulong value, byte eighth, byte seventh, byte sixth, byte fifth, byte forth, byte third, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(ulong)];
        Assert.True(BinaryPrimitives.TryWriteUInt64LittleEndian(bytes, value));
        Assert.Equal([first, second, third, forth, fifth, sixth, seventh, eighth], bytes.ToArray());
    }

#if NET5_0_OR_GREATER
    [Theory]
    [InlineData(65504D, 0x7B, byte.MaxValue)]
    [InlineData(-65504D, 0xFB, byte.MaxValue)]
    public void TryWriteHalfBigEndian(Half value, byte first, byte second)
    {
        Span<byte> bytes = stackalloc byte[sizeof(short)];
        Assert.True(BinaryPrimitives.TryWriteHalfBigEndian(bytes, value));
        Assert.Equal([first, second], bytes.ToArray());
    }

    [Theory]
    [InlineData(65504D, 0x7B, byte.MaxValue)]
    [InlineData(-65504D, 0xFB, byte.MaxValue)]
    public void TryWriteHalfLittleEndian(Half value, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(short)];
        Assert.True(BinaryPrimitives.TryWriteHalfLittleEndian(bytes, value));
        Assert.Equal([first, second], bytes.ToArray());
    }
#endif

    [Theory]
    [InlineData(float.MaxValue, PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue)]
    [InlineData(float.MinValue, byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue)]
    public void TryWriteSingleBigEndian(float value, byte first, byte second, byte third, byte forth)
    {
        Span<byte> bytes = stackalloc byte[sizeof(float)];
        Assert.True(BinaryPrimitives.TryWriteSingleBigEndian(bytes, value));
        Assert.Equal([first, second, third, forth], bytes.ToArray());
    }

    [Theory]
    [InlineData(float.MaxValue, PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue)]
    [InlineData(float.MinValue, byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue)]
    public void TryWriteSingleLittleEndian(float value, byte forth, byte third, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(float)];
        Assert.True(BinaryPrimitives.TryWriteSingleLittleEndian(bytes, value));
        Assert.Equal([first, second, third, forth], bytes.ToArray());
    }

    [Theory]
    [InlineData(double.MaxValue, PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(double.MinValue, byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void TryWriteDoubleBigEndian(double value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth)
    {
        Span<byte> bytes = stackalloc byte[sizeof(double)];
        Assert.True(BinaryPrimitives.TryWriteDoubleBigEndian(bytes, value));
        Assert.Equal([first, second, third, forth, fifth, sixth, seventh, eighth], bytes.ToArray());
    }

    [Theory]
    [InlineData(double.MaxValue, PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(double.MinValue, byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void TryWriteDoubleLittleEndian(double value, byte eighth, byte seventh, byte sixth, byte fifth, byte forth, byte third, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(double)];
        Assert.True(BinaryPrimitives.TryWriteDoubleLittleEndian(bytes, value));
        Assert.Equal([first, second, third, forth, fifth, sixth, seventh, eighth], bytes.ToArray());
    }
}