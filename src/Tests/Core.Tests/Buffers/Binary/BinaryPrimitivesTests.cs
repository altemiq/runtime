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
        bytes.ToArray().Should().BeEquivalentTo([first, second]);
    }

    [Theory]
    [InlineData(short.MaxValue, PositiveByte, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue)]
    public void WriteInt16LittleEndian(short value, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(short)];
        BinaryPrimitives.WriteInt16LittleEndian(bytes, value);
        bytes.ToArray().Should().BeEquivalentTo([first, second]);
    }

    [Theory]
    [InlineData(int.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void WriteInt32BigEndian(int value, byte first, byte second, byte third, byte forth)
    {
        Span<byte> bytes = stackalloc byte[sizeof(int)];
        BinaryPrimitives.WriteInt32BigEndian(bytes, value);
        bytes.ToArray().Should().BeEquivalentTo([first, second, third, forth]);
    }

    [Theory]
    [InlineData(int.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void WriteInt32LittleEndian(int value, byte forth, byte third, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(int)];
        BinaryPrimitives.WriteInt32LittleEndian(bytes, value);
        bytes.ToArray().Should().BeEquivalentTo([first, second, third, forth]);
    }

    [Theory]
    [InlineData(long.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void WriteInt64BigEndian(long value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth)
    {
        Span<byte> bytes = stackalloc byte[sizeof(long)];
        BinaryPrimitives.WriteInt64BigEndian(bytes, value);
        bytes.ToArray().Should().BeEquivalentTo([first, second, third, forth, fifth, sixth, seventh, eighth]);
    }

    [Theory]
    [InlineData(long.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void WriteInt64LittleEndian(long value, byte eighth, byte seventh, byte sixth, byte fifth, byte forth, byte third, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(long)];
        BinaryPrimitives.WriteInt64LittleEndian(bytes, value);
        bytes.ToArray().Should().BeEquivalentTo([first, second, third, forth, fifth, sixth, seventh, eighth]);
    }

    [Theory]
    [InlineData(ushort.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(ushort.MaxValue / 2, PositiveByte, byte.MaxValue)]
    public void WriteUInt16BigEndian(ushort value, byte first, byte second)
    {
        Span<byte> bytes = stackalloc byte[sizeof(ushort)];
        BinaryPrimitives.WriteUInt16BigEndian(bytes, value);
        bytes.ToArray().Should().BeEquivalentTo([first, second]);
    }

    [Theory]
    [InlineData(ushort.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(ushort.MaxValue / 2, PositiveByte, byte.MaxValue)]
    public void WriteUInt16LittleEndian(ushort value, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(ushort)];
        BinaryPrimitives.WriteUInt16LittleEndian(bytes, value);
        bytes.ToArray().Should().BeEquivalentTo([first, second]);
    }

    [Theory]
    [InlineData(uint.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(uint.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void WriteUInt32BigEndian(uint value, byte first, byte second, byte third, byte forth)
    {
        Span<byte> bytes = stackalloc byte[sizeof(uint)];
        BinaryPrimitives.WriteUInt32BigEndian(bytes, value);
        bytes.ToArray().Should().BeEquivalentTo([first, second, third, forth]);
    }

    [Theory]
    [InlineData(uint.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(uint.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void WriteUInt32LittleEndian(uint value, byte forth, byte third, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(uint)];
        BinaryPrimitives.WriteUInt32LittleEndian(bytes, value);
        bytes.ToArray().Should().BeEquivalentTo([first, second, third, forth]);
    }

    [Theory]
    [InlineData(ulong.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(ulong.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void WriteUInt64BigEndian(ulong value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth)
    {
        Span<byte> bytes = stackalloc byte[sizeof(ulong)];
        BinaryPrimitives.WriteUInt64BigEndian(bytes, value);
        bytes.ToArray().Should().BeEquivalentTo([first, second, third, forth, fifth, sixth, seventh, eighth]);
    }

    [Theory]
    [InlineData(ulong.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(ulong.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void WriteUInt64LittleEndian(ulong value, byte eighth, byte seventh, byte sixth, byte fifth, byte forth, byte third, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(ulong)];
        BinaryPrimitives.WriteUInt64LittleEndian(bytes, value);
        bytes.ToArray().Should().BeEquivalentTo([first, second, third, forth, fifth, sixth, seventh, eighth]);
    }

#if NET5_0_OR_GREATER
    [Theory]
    [InlineData(65504D, 0x7B, byte.MaxValue)]
    [InlineData(-65504D, 0xFB, byte.MaxValue)]
    public void WriteHalfBigEndian(SerializableHalf value, byte first, byte second)
    {
        Span<byte> bytes = stackalloc byte[sizeof(short)];
        BinaryPrimitives.WriteHalfBigEndian(bytes, value);
        bytes.ToArray().Should().BeEquivalentTo([first, second]);
    }

    [Theory]
    [InlineData(65504D, 0x7B, byte.MaxValue)]
    [InlineData(-65504D, 0xFB, byte.MaxValue)]
    public void WriteHalfLittleEndian(SerializableHalf value, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(short)];
        BinaryPrimitives.WriteHalfLittleEndian(bytes, value);
        bytes.ToArray().Should().BeEquivalentTo([first, second]);
    }
#endif

    [Theory]
    [InlineData(float.MaxValue, PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue)]
    [InlineData(float.MinValue, byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue)]
    public void WriteSingleBigEndian(float value, byte first, byte second, byte third, byte forth)
    {
        Span<byte> bytes = stackalloc byte[sizeof(float)];
        BinaryPrimitives.WriteSingleBigEndian(bytes, value);
        bytes.ToArray().Should().BeEquivalentTo([first, second, third, forth]);
    }

    [Theory]
    [InlineData(float.MaxValue, PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue)]
    [InlineData(float.MinValue, byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue)]
    public void WriteSingleLittleEndian(float value, byte forth, byte third, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(float)];
        BinaryPrimitives.WriteSingleLittleEndian(bytes, value);
        bytes.ToArray().Should().BeEquivalentTo([first, second, third, forth]);
    }

    [Theory]
    [InlineData(double.MaxValue, PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(double.MinValue, byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void WriteDoubleBigEndian(double value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth)
    {
        Span<byte> bytes = stackalloc byte[sizeof(double)];
        BinaryPrimitives.WriteDoubleBigEndian(bytes, value);
        bytes.ToArray().Should().BeEquivalentTo([first, second, third, forth, fifth, sixth, seventh, eighth]);
    }

    [Theory]
    [InlineData(double.MaxValue, PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(double.MinValue, byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void WriteDoubleLittleEndian(double value, byte eighth, byte seventh, byte sixth, byte fifth, byte forth, byte third, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(double)];
        BinaryPrimitives.WriteDoubleLittleEndian(bytes, value);
        bytes.ToArray().Should().BeEquivalentTo([first, second, third, forth, fifth, sixth, seventh, eighth]);
    }

    [Theory]
    [InlineData(short.MaxValue, PositiveByte, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue)]
    public void TryWriteInt16BigEndian(short value, byte first, byte second)
    {
        Span<byte> bytes = stackalloc byte[sizeof(short)];
        BinaryPrimitives.TryWriteInt16BigEndian(bytes, value).Should().BeTrue();
        bytes.ToArray().Should().BeEquivalentTo([first, second]);
    }

    [Theory]
    [InlineData(short.MaxValue, PositiveByte, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue)]
    public void TryWriteInt16LittleEndian(short value, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(short)];
        BinaryPrimitives.TryWriteInt16LittleEndian(bytes, value).Should().BeTrue();
        bytes.ToArray().Should().BeEquivalentTo([first, second]);
    }

    [Theory]
    [InlineData(int.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void TryWriteInt32BigEndian(int value, byte first, byte second, byte third, byte forth)
    {
        Span<byte> bytes = stackalloc byte[sizeof(int)];
        BinaryPrimitives.TryWriteInt32BigEndian(bytes, value).Should().BeTrue();
        bytes.ToArray().Should().BeEquivalentTo([first, second, third, forth]);
    }

    [Theory]
    [InlineData(int.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void TryWriteInt32LittleEndian(int value, byte forth, byte third, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(int)];
        BinaryPrimitives.TryWriteInt32LittleEndian(bytes, value).Should().BeTrue();
        bytes.ToArray().Should().BeEquivalentTo([first, second, third, forth]);
    }

    [Theory]
    [InlineData(long.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void TryWriteInt64BigEndian(long value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth)
    {
        Span<byte> bytes = stackalloc byte[sizeof(long)];
        BinaryPrimitives.TryWriteInt64BigEndian(bytes, value).Should().BeTrue();
        bytes.ToArray().Should().BeEquivalentTo([first, second, third, forth, fifth, sixth, seventh, eighth]);
    }

    [Theory]
    [InlineData(long.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void TryWriteInt64LittleEndian(long value, byte eighth, byte seventh, byte sixth, byte fifth, byte forth, byte third, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(long)];
        BinaryPrimitives.TryWriteInt64LittleEndian(bytes, value).Should().BeTrue();
        bytes.ToArray().Should().BeEquivalentTo([first, second, third, forth, fifth, sixth, seventh, eighth]);
    }

    [Theory]
    [InlineData(ushort.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(ushort.MaxValue / 2, PositiveByte, byte.MaxValue)]
    public void TryWriteUInt16BigEndian(ushort value, byte first, byte second)
    {
        Span<byte> bytes = stackalloc byte[sizeof(ushort)];
        BinaryPrimitives.TryWriteUInt16BigEndian(bytes, value).Should().BeTrue();
        bytes.ToArray().Should().BeEquivalentTo([first, second]);
    }

    [Theory]
    [InlineData(ushort.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(ushort.MaxValue / 2, PositiveByte, byte.MaxValue)]
    public void TryWriteUInt16LittleEndian(ushort value, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(ushort)];
        BinaryPrimitives.TryWriteUInt16LittleEndian(bytes, value).Should().BeTrue();
        bytes.ToArray().Should().BeEquivalentTo([first, second]);
    }

    [Theory]
    [InlineData(uint.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(uint.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void TryWriteUInt32BigEndian(uint value, byte first, byte second, byte third, byte forth)
    {
        Span<byte> bytes = stackalloc byte[sizeof(uint)];
        BinaryPrimitives.TryWriteUInt32BigEndian(bytes, value).Should().BeTrue();
        bytes.ToArray().Should().BeEquivalentTo([first, second, third, forth]);
    }

    [Theory]
    [InlineData(uint.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(uint.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void TryWriteUInt32LittleEndian(uint value, byte forth, byte third, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(uint)];
        BinaryPrimitives.TryWriteUInt32LittleEndian(bytes, value).Should().BeTrue();
        bytes.ToArray().Should().BeEquivalentTo([first, second, third, forth]);
    }

    [Theory]
    [InlineData(ulong.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(ulong.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void TryWriteUInt64BigEndian(ulong value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth)
    {
        Span<byte> bytes = stackalloc byte[sizeof(ulong)];
        BinaryPrimitives.TryWriteUInt64BigEndian(bytes, value).Should().BeTrue();
        bytes.ToArray().Should().BeEquivalentTo([first, second, third, forth, fifth, sixth, seventh, eighth]);
    }

    [Theory]
    [InlineData(ulong.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(ulong.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void TryWriteUInt64LittleEndian(ulong value, byte eighth, byte seventh, byte sixth, byte fifth, byte forth, byte third, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(ulong)];
        BinaryPrimitives.TryWriteUInt64LittleEndian(bytes, value).Should().BeTrue();
        bytes.ToArray().Should().BeEquivalentTo([first, second, third, forth, fifth, sixth, seventh, eighth]);
    }

#if NET5_0_OR_GREATER
    [Theory]
    [InlineData(65504D, 0x7B, byte.MaxValue)]
    [InlineData(-65504D, 0xFB, byte.MaxValue)]
    public void TryWriteHalfBigEndian(SerializableHalf value, byte first, byte second)
    {
        Span<byte> bytes = stackalloc byte[sizeof(short)];
        BinaryPrimitives.TryWriteHalfBigEndian(bytes, value).Should().BeTrue();
        bytes.ToArray().Should().BeEquivalentTo([first, second]);
    }

    [Theory]
    [InlineData(65504D, 0x7B, byte.MaxValue)]
    [InlineData(-65504D, 0xFB, byte.MaxValue)]
    public void TryWriteHalfLittleEndian(SerializableHalf value, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(short)];
        BinaryPrimitives.TryWriteHalfLittleEndian(bytes, value).Should().BeTrue();
        bytes.ToArray().Should().BeEquivalentTo([first, second]);
    }
#endif

    [Theory]
    [InlineData(float.MaxValue, PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue)]
    [InlineData(float.MinValue, byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue)]
    public void TryWriteSingleBigEndian(float value, byte first, byte second, byte third, byte forth)
    {
        Span<byte> bytes = stackalloc byte[sizeof(float)];
        BinaryPrimitives.TryWriteSingleBigEndian(bytes, value).Should().BeTrue();
        bytes.ToArray().Should().BeEquivalentTo([first, second, third, forth]);
    }

    [Theory]
    [InlineData(float.MaxValue, PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue)]
    [InlineData(float.MinValue, byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue)]
    public void TryWriteSingleLittleEndian(float value, byte forth, byte third, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(float)];
        BinaryPrimitives.TryWriteSingleLittleEndian(bytes, value).Should().BeTrue();
        bytes.ToArray().Should().BeEquivalentTo([first, second, third, forth]);
    }

    [Theory]
    [InlineData(double.MaxValue, PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(double.MinValue, byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void TryWriteDoubleBigEndian(double value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth)
    {
        Span<byte> bytes = stackalloc byte[sizeof(double)];
        BinaryPrimitives.TryWriteDoubleBigEndian(bytes, value).Should().BeTrue();
        bytes.ToArray().Should().BeEquivalentTo([first, second, third, forth, fifth, sixth, seventh, eighth]);
    }

    [Theory]
    [InlineData(double.MaxValue, PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(double.MinValue, byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void TryWriteDoubleLittleEndian(double value, byte eighth, byte seventh, byte sixth, byte fifth, byte forth, byte third, byte second, byte first)
    {
        Span<byte> bytes = stackalloc byte[sizeof(double)];
        BinaryPrimitives.TryWriteDoubleLittleEndian(bytes, value).Should().BeTrue();
        bytes.ToArray().Should().BeEquivalentTo([first, second, third, forth, fifth, sixth, seventh, eighth]);
    }
}