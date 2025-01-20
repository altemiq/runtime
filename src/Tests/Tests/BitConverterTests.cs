// -----------------------------------------------------------------------
// <copyright file="BitConverterTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

public partial class BitConverterTests
{
    private const byte PositiveByte = 0x7F;

    private const byte FloatByte = 0xEF;

    private readonly static ByteOrder DefaultByteOrder = BitConverter.IsLittleEndian ? ByteOrder.LittleEndian : ByteOrder.BigEndian;

    [Fact]
    public void LittleEndianessShouldEqualDefault() => Assert.Equal(System.BitConverter.IsLittleEndian, BitConverter.IsLittleEndian);

    [Theory]
    [InlineData(true, 1)]
    [InlineData(false, 0)]
    public void GetBooleanBytes(bool value, byte expected) =>  Assert.Equal([expected], BitConverter.GetBytes(value));

    [Theory]
    [InlineData(true, 1, ByteOrder.LittleEndian)]
    [InlineData(false, 0, ByteOrder.LittleEndian)]
    [InlineData(true, 1, ByteOrder.BigEndian)]
    [InlineData(false, 0, ByteOrder.BigEndian)]
    public void GetBooleanBytesWithByteOrder(bool value, byte expected, ByteOrder byteOrder) => Assert.Equal([expected], BitConverter.GetBytes(value, byteOrder));

    [Theory]
    [InlineData(true, 1)]
    [InlineData(false, 0)]
    public void TryWriteBooleanBytes(bool value, byte expected)
    {
        Span<byte> span = stackalloc byte[sizeof(byte)];
        Assert.True( BitConverter.TryWriteBytes(span, value));
        Assert.Equal([expected], span.ToArray());
    }

    [Theory]
    [InlineData(true, 1, ByteOrder.LittleEndian)]
    [InlineData(false, 0, ByteOrder.LittleEndian)]
    [InlineData(true, 1, ByteOrder.BigEndian)]
    [InlineData(false, 0, ByteOrder.BigEndian)]
    public void TryWriteBooleanBytesWithByteOrder(bool value, byte expected, ByteOrder byteOrder)
    {
        Span<byte> span = stackalloc byte[sizeof(byte)];
        Assert.True(BitConverter.TryWriteBytes(span, value, byteOrder));
        Assert.Equal([expected], span.ToArray());
    }

    [Fact]
    public void TryWriteBooleanBytesWithSmallSpan() => Assert.False( BitConverter.TryWriteBytes(default, default(bool)));

    [Theory]
    [InlineData(1, true)]
    [InlineData(0, false)]
    public void ToBooleanBytes(byte value, bool expected) => Assert.Equal(expected, BitConverter.ToBoolean(new byte[] { value }, 0));

    [Theory]
    [InlineData(1, true, ByteOrder.LittleEndian)]
    [InlineData(0, false, ByteOrder.LittleEndian)]
    [InlineData(1, true, ByteOrder.BigEndian)]
    [InlineData(0, false, ByteOrder.BigEndian)]
    public void ToBooleanBytesWithByteOrder(byte value, bool expected, ByteOrder byteOrder) => Assert.Equal(expected, BitConverter.ToBoolean([value], 0, byteOrder));

    [Theory]
    [InlineData(1, true)]
    [InlineData(0, false)]
    public void ToBooleanSpan(byte value, bool expected) => Assert.Equal(expected, BitConverter.ToBoolean([value]));

    [Theory]
    [InlineData(1, true, ByteOrder.LittleEndian)]
    [InlineData(0, false, ByteOrder.LittleEndian)]
    [InlineData(1, true, ByteOrder.BigEndian)]
    [InlineData(0, false, ByteOrder.BigEndian)]
    public void ToBooleanSpanWithByteOrder(byte value, bool expected, ByteOrder byteOrder) => Assert.Equal(expected, BitConverter.ToBoolean([value], byteOrder));


    [Fact]
    public void ToBooleanSpanWithSmallSpan()
    {
        Assert.Throws<ArgumentOutOfRangeException>(static () => BitConverter.ToBoolean(default));
    }

    [Theory]
    [InlineData('A', 0x00, 0x41)]
    [InlineData('﴾', 0xFD, 0x3E)]
    public void GetCharBytes(char value, byte first, byte second) => Assert.Equal(GetBytes(first, second, byteOrder: DefaultByteOrder), BitConverter.GetBytes(value));

    [Theory]
    [InlineData('A', 0x00, 0x41, ByteOrder.LittleEndian)]
    [InlineData('﴾', 0xFD, 0x3E, ByteOrder.LittleEndian)]
    [InlineData('A', 0x00, 0x41, ByteOrder.BigEndian)]
    [InlineData('﴾', 0xFD, 0x3E, ByteOrder.BigEndian)]
    public void GetCharBytesWithByteOrder(char value, byte first, byte second, ByteOrder byteOrder) => Assert.Equal(GetBytes(first, second, byteOrder), BitConverter.GetBytes(value, byteOrder));

    [Theory]
    [InlineData('A', 0x00, 0x41)]
    [InlineData('﴾', 0xFD, 0x3E)]
    public void TryWriteCharBytes(char value, byte first, byte second)
    {
        Span<byte> span = stackalloc byte[sizeof(char)];
        Assert.True( BitConverter.TryWriteBytes(span, value));
        Assert.Equal(GetBytes(first, second, byteOrder: DefaultByteOrder), span.ToArray());
    }

    [Theory]
    [InlineData('A', 0x00, 0x41, ByteOrder.LittleEndian)]
    [InlineData('﴾', 0xFD, 0x3E, ByteOrder.LittleEndian)]
    [InlineData('A', 0x00, 0x41, ByteOrder.BigEndian)]
    [InlineData('﴾', 0xFD, 0x3E, ByteOrder.BigEndian)]
    public void TryWriteCharBytesWithByteOrder(char value, byte first, byte second, ByteOrder byteOrder)
    {
        Span<byte> span = stackalloc byte[sizeof(char)];
        Assert.True(BitConverter.TryWriteBytes(span, value, byteOrder));
        Assert.Equal(GetBytes(first, second, byteOrder), span.ToArray());
    }

    [Theory]
    [InlineData(0x00, 0x41, 'A')]
    [InlineData(0xFD, 0x3E, '﴾')]
    public void ToCharBytes(byte first, byte second, char expected) => Assert.Equal(expected, BitConverter.ToChar(GetBytes(first, second, byteOrder: DefaultByteOrder), 0));

    [Theory]
    [InlineData(0x00, 0x41, 'A', ByteOrder.LittleEndian)]
    [InlineData(0xFD, 0x3E, '﴾', ByteOrder.LittleEndian)]
    [InlineData(0x00, 0x41, 'A', ByteOrder.BigEndian)]
    [InlineData(0xFD, 0x3E, '﴾', ByteOrder.BigEndian)]
    public void ToCharBytesWithByteOrder(byte first, byte second, char expected, ByteOrder byteOrder) => Assert.Equal(expected, BitConverter.ToChar(GetBytes(first, second, byteOrder), 0, byteOrder));

    [Theory]
    [InlineData(0x00, 0x41, 'A')]
    [InlineData(0xFD, 0x3E, '﴾')]
    public void ToCharSpan(byte first, byte second, char expected) => Assert.Equal(expected, BitConverter.ToChar(GetBytes(first, second, byteOrder: DefaultByteOrder)));

    [Theory]
    [InlineData(0x00, 0x41, 'A', ByteOrder.LittleEndian)]
    [InlineData(0xFD, 0x3E, '﴾', ByteOrder.LittleEndian)]
    [InlineData(0x00, 0x41, 'A', ByteOrder.BigEndian)]
    [InlineData(0xFD, 0x3E, '﴾', ByteOrder.BigEndian)]
    public void ToCharSpanWithByteOrder(byte first, byte second, char expected, ByteOrder byteOrder) => Assert.Equal(expected, BitConverter.ToChar(GetBytes(first, second, byteOrder), byteOrder));

    [Theory]
    [InlineData(short.MaxValue, PositiveByte, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue)]
    public void GetInt16Bytes(short value, byte first, byte second) => Assert.Equal(GetBytes(first, second, byteOrder: DefaultByteOrder), BitConverter.GetBytes(value));

    [Theory]
    [InlineData(short.MaxValue, PositiveByte, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(short.MaxValue, PositiveByte, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void GetInt16BytesWithByteOrder(short value, byte first, byte second, ByteOrder byteOrder) => Assert.Equal(GetBytes(first, second, byteOrder), BitConverter.GetBytes(value, byteOrder));

    [Theory]
    [InlineData(short.MaxValue, PositiveByte, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue)]
    public void TryWriteInt16Bytes(short value, byte first, byte second)
    {
        Span<byte> span = stackalloc byte[sizeof(short)];
        Assert.True(BitConverter.TryWriteBytes(span, value));
        Assert.Equal(GetBytes(first, second, byteOrder: DefaultByteOrder), span.ToArray());
    }

    [Theory]
    [InlineData(short.MaxValue, PositiveByte, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(short.MaxValue, PositiveByte, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void TryWriteInt16BytesWithByteOrder(short value, byte first, byte second, ByteOrder byteOrder)
    {
        Span<byte> span = stackalloc byte[sizeof(short)];
        Assert.True(BitConverter.TryWriteBytes(span, value, byteOrder));
        Assert.Equal(GetBytes(first, second, byteOrder), span.ToArray());
    }

    [Theory]
    [InlineData(PositiveByte, byte.MaxValue, short.MaxValue)]
    [InlineData(byte.MaxValue, byte.MaxValue, -1)]
    public void ToInt16Bytes(byte first, byte second, short expected) => Assert.Equal(expected, BitConverter.ToInt16(GetBytes(first, second, byteOrder: DefaultByteOrder), 0));

    [Theory]
    [InlineData(PositiveByte, byte.MaxValue, short.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, -1, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, byte.MaxValue, short.MaxValue, ByteOrder.BigEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, -1, ByteOrder.BigEndian)]
    public void ToInt16BytesWithByteOrder(byte first, byte second, short expected, ByteOrder byteOrder) => Assert.Equal(expected, BitConverter.ToInt16(GetBytes(first, second, byteOrder), 0, byteOrder));

    [Theory]
    [InlineData(PositiveByte, byte.MaxValue, short.MaxValue)]
    [InlineData(byte.MaxValue, byte.MaxValue, -1)]
    public void ToInt16Span(byte first, byte second, short expected) => Assert.Equal(expected, BitConverter.ToInt16(GetBytes(first, second, byteOrder: DefaultByteOrder)));

    [Theory]
    [InlineData(PositiveByte, byte.MaxValue, short.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, -1, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, byte.MaxValue, short.MaxValue, ByteOrder.BigEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, -1, ByteOrder.BigEndian)]
    public void ToInt16SpanWithByteOrder(byte first, byte second, short expected, ByteOrder byteOrder) => Assert.Equal(expected, BitConverter.ToInt16(GetBytes(first, second, byteOrder), byteOrder));

    [Theory]
    [InlineData(int.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void GetInt32Bytes(int value, byte first, byte second, byte third, byte forth) => Assert.Equal(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder), BitConverter.GetBytes(value));

    [Theory]
    [InlineData(int.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(int.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void GetInt32BytesWithByteOrder(int value, byte first, byte second, byte third, byte forth, ByteOrder byteOrder) => Assert.Equal(GetBytes(first, second, third, forth, byteOrder), BitConverter.GetBytes(value, byteOrder));

    [Theory]
    [InlineData(int.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void TryWriteInt32Bytes(int value, byte first, byte second, byte third, byte forth)
    {
        Span<byte> span = stackalloc byte[sizeof(int)];
        Assert.True(BitConverter.TryWriteBytes(span, value));
        Assert.Equal(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder), span.ToArray());
    }

    [Theory]
    [InlineData(int.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(int.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void TryWriteInt32BytesWithByteOrder(int value, byte first, byte second, byte third, byte forth, ByteOrder byteOrder)
    {
        Span<byte> span = stackalloc byte[sizeof(int)];
        Assert.True(BitConverter.TryWriteBytes(span, value, byteOrder));
        Assert.Equal(GetBytes(first, second, third, forth, byteOrder), span.ToArray());
    }

    [Theory]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, int.MaxValue)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, -1)]
    public void ToInt32Bytes(byte first, byte second, byte third, byte forth, int expected) => Assert.Equal(expected, BitConverter.ToInt32(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder), 0));

    [Theory]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, int.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, -1, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, int.MaxValue, ByteOrder.BigEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, -1, ByteOrder.BigEndian)]
    public void ToInt32BytesWithByteOrder(byte first, byte second, byte third, byte forth, int expected, ByteOrder byteOrder) => Assert.Equal(expected, BitConverter.ToInt32(GetBytes(first, second, third, forth, byteOrder), 0, byteOrder));

    [Theory]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, int.MaxValue)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, -1)]
    public void ToInt32Span(byte first, byte second, byte third, byte forth, int expected) => Assert.Equal(expected, BitConverter.ToInt32(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder)));

    [Theory]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, int.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, -1, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, int.MaxValue, ByteOrder.BigEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, -1, ByteOrder.BigEndian)]
    public void ToInt32SpanWithByteOrder(byte first, byte second, byte third, byte forth, int expected, ByteOrder byteOrder) => Assert.Equal(expected, BitConverter.ToInt32(GetBytes(first, second, third, forth, byteOrder), byteOrder));

    [Theory]
    [InlineData(long.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void GetInt64Bytes(long value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth) => Assert.Equal(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder), BitConverter.GetBytes(value));

    [Theory]
    [InlineData(long.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(long.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void GetInt64BytesWithByteOrder(long value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder) => Assert.Equal(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), BitConverter.GetBytes(value, byteOrder));

    [Theory]
    [InlineData(long.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void TryWriteInt64Bytes(long value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth)
    {
        Span<byte> span = stackalloc byte[sizeof(long)];
        Assert.True(BitConverter.TryWriteBytes(span, value));
        Assert.Equal(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder), span.ToArray());
    }

    [Theory]
    [InlineData(long.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(long.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void TryWriteInt64BytesWithByteOrder(long value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder)
    {
        Span<byte> span = stackalloc byte[sizeof(long)];
        Assert.True(BitConverter.TryWriteBytes(span, value, byteOrder));
        Assert.Equal(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), span.ToArray());
    }

    [Theory]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, long.MaxValue)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, -1)]
    public void ToInt64Bytes(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, long expected) => Assert.Equal(expected, BitConverter.ToInt64(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder), 0));

    [Theory]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, long.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, -1, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, long.MaxValue, ByteOrder.BigEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, -1, ByteOrder.BigEndian)]
    public void ToInt64BytesWithByteOrder(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, long expected, ByteOrder byteOrder) => Assert.Equal(expected, BitConverter.ToInt64(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), 0, byteOrder));

    [Theory]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, long.MaxValue)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, -1)]
    public void ToInt64Span(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, long expected) => Assert.Equal(expected, BitConverter.ToInt64(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder)));

    [Theory]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, long.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, -1, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, long.MaxValue, ByteOrder.BigEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, -1, ByteOrder.BigEndian)]
    public void ToInt64SpanWithByteOrder(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, long expected, ByteOrder byteOrder) => Assert.Equal(expected, BitConverter.ToInt64(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), byteOrder));

    [Theory]
    [InlineData(ushort.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(ushort.MaxValue / 2, PositiveByte, byte.MaxValue)]
    public void GetUInt16Bytes(ushort value, byte first, byte second) => Assert.Equal(GetBytes(first, second, byteOrder: DefaultByteOrder), BitConverter.GetBytes(value));

    [Theory]
    [InlineData(ushort.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(ushort.MaxValue / 2, PositiveByte, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(ushort.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(ushort.MaxValue / 2, PositiveByte, byte.MaxValue, ByteOrder.BigEndian)]
    public void GetUInt16BytesWithByteOrder(ushort value, byte first, byte second, ByteOrder byteOrder) => Assert.Equal(GetBytes(first, second, byteOrder), BitConverter.GetBytes(value, byteOrder));

    [Theory]
    [InlineData(ushort.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(ushort.MaxValue / 2, PositiveByte, byte.MaxValue)]
    public void TryWriteUInt16Bytes(ushort value, byte first, byte second)
    {
        Span<byte> span = stackalloc byte[sizeof(ushort)];
        Assert.True(BitConverter.TryWriteBytes(span, value));
        Assert.Equal(GetBytes(first, second, byteOrder: DefaultByteOrder), span.ToArray());
    }

    [Theory]
    [InlineData(ushort.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(ushort.MaxValue / 2, PositiveByte, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(ushort.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(ushort.MaxValue / 2, PositiveByte, byte.MaxValue, ByteOrder.BigEndian)]
    public void TryWriteUInt16BytesWithByteOrder(ushort value, byte first, byte second, ByteOrder byteOrder)
    {
        Span<byte> span = stackalloc byte[sizeof(ushort)];
        Assert.True(BitConverter.TryWriteBytes(span, value, byteOrder));
        Assert.Equal(GetBytes(first, second, byteOrder), span.ToArray());
    }

    [Theory]
    [InlineData(byte.MaxValue, byte.MaxValue, ushort.MaxValue)]
    [InlineData(PositiveByte, byte.MaxValue, ushort.MaxValue / 2)]
    public void ToUInt16Bytes(byte first, byte second, ushort expected) => Assert.Equal(expected, BitConverter.ToUInt16(GetBytes(first, second, byteOrder: DefaultByteOrder), 0));

    [Theory]
    [InlineData(byte.MaxValue, byte.MaxValue, ushort.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, byte.MaxValue, ushort.MaxValue / 2, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, ushort.MaxValue, ByteOrder.BigEndian)]
    [InlineData(PositiveByte, byte.MaxValue, ushort.MaxValue / 2, ByteOrder.BigEndian)]
    public void ToUInt16BytesWithByteOrder(byte first, byte second, ushort expected, ByteOrder byteOrder) => Assert.Equal(expected, BitConverter.ToUInt16(GetBytes(first, second, byteOrder), 0, byteOrder));

    [Theory]
    [InlineData(byte.MaxValue, byte.MaxValue, ushort.MaxValue)]
    [InlineData(PositiveByte, byte.MaxValue, ushort.MaxValue / 2)]
    public void ToUInt16Span(byte first, byte second, ushort expected) => Assert.Equal(expected, BitConverter.ToUInt16(GetBytes(first, second, byteOrder: DefaultByteOrder)));

    [Theory]
    [InlineData(byte.MaxValue, byte.MaxValue, ushort.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, byte.MaxValue, ushort.MaxValue / 2, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, ushort.MaxValue, ByteOrder.BigEndian)]
    [InlineData(PositiveByte, byte.MaxValue, ushort.MaxValue / 2, ByteOrder.BigEndian)]
    public void ToUInt16SpanWithByteOrder(byte first, byte second, ushort expected, ByteOrder byteOrder) => Assert.Equal(expected, BitConverter.ToUInt16(GetBytes(first, second, byteOrder), byteOrder));

    [Theory]
    [InlineData(uint.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(uint.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void GetUInt32Bytes(uint value, byte first, byte second, byte third, byte forth) => Assert.Equal(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder), BitConverter.GetBytes(value));

    [Theory]
    [InlineData(uint.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(uint.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(uint.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(uint.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void GetUInt32BytesWithByteOrder(uint value, byte first, byte second, byte third, byte forth, ByteOrder byteOrder) => Assert.Equal(GetBytes(first, second, third, forth, byteOrder), BitConverter.GetBytes(value, byteOrder));

    [Theory]
    [InlineData(uint.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(uint.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void TryWriteUInt32Bytes(uint value, byte first, byte second, byte third, byte forth)
    {
        Span<byte> span = stackalloc byte[sizeof(uint)];
        Assert.True(BitConverter.TryWriteBytes(span, value));
        Assert.Equal(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder), span.ToArray());
    }

    [Theory]
    [InlineData(uint.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(uint.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(uint.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(uint.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void TryWriteUInt32BytesWithByteOrder(uint value, byte first, byte second, byte third, byte forth, ByteOrder byteOrder)
    {
        Span<byte> span = stackalloc byte[sizeof(uint)];
        Assert.True(BitConverter.TryWriteBytes(span, value, byteOrder));
        Assert.Equal(GetBytes(first, second, third, forth, byteOrder), span.ToArray());
    }

    [Theory]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, uint.MaxValue)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, uint.MaxValue / 2)]
    public void ToUInt32Bytes(byte first, byte second, byte third, byte forth, uint expected) => Assert.Equal(expected, BitConverter.ToUInt32(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder), 0));

    [Theory]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, uint.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, uint.MaxValue / 2, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, uint.MaxValue, ByteOrder.BigEndian)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, uint.MaxValue / 2, ByteOrder.BigEndian)]
    public void ToUInt32BytesWithByteOrder(byte first, byte second, byte third, byte forth, uint expected, ByteOrder byteOrder) => Assert.Equal(expected, BitConverter.ToUInt32(GetBytes(first, second, third, forth, byteOrder), 0, byteOrder));

    [Theory]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, uint.MaxValue)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, uint.MaxValue / 2)]
    public void ToUInt32Span(byte first, byte second, byte third, byte forth, uint expected) => Assert.Equal(expected, BitConverter.ToUInt32(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder)));

    [Theory]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, uint.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, uint.MaxValue / 2, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, uint.MaxValue, ByteOrder.BigEndian)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, uint.MaxValue / 2, ByteOrder.BigEndian)]
    public void ToUInt32SpanWithByteOrder(byte first, byte second, byte third, byte forth, uint expected, ByteOrder byteOrder) => Assert.Equal(expected, BitConverter.ToUInt32(GetBytes(first, second, third, forth, byteOrder), byteOrder));

    [Theory]
    [InlineData(ulong.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(ulong.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void GetUInt64Bytes(ulong value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth) => Assert.Equal(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder), BitConverter.GetBytes(value));

    [Theory]
    [InlineData(ulong.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(ulong.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(ulong.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(ulong.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void GetUInt64BytesWithByteOrder(ulong value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder) => Assert.Equal(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), BitConverter.GetBytes(value, byteOrder));

    [Theory]
    [InlineData(ulong.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(ulong.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void TryWriteUInt64Bytes(ulong value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth)
    {
        Span<byte> span = stackalloc byte[sizeof(ulong)];
        Assert.True(BitConverter.TryWriteBytes(span, value));
        Assert.Equal(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder), span.ToArray());
    }

    [Theory]
    [InlineData(ulong.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(ulong.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(ulong.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(ulong.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void TryWriteUInt64BytesWithByteOrder(ulong value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder)
    {
        Span<byte> span = stackalloc byte[sizeof(ulong)];
        Assert.True(BitConverter.TryWriteBytes(span, value, byteOrder));
        Assert.Equal(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), span.ToArray());
    }

    [Theory]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ulong.MaxValue)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ulong.MaxValue / 2)]
    public void ToUInt64Bytes(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ulong expected) => Assert.Equal(expected, BitConverter.ToUInt64(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder), 0));

    [Theory]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ulong.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ulong.MaxValue / 2, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ulong.MaxValue, ByteOrder.BigEndian)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ulong.MaxValue / 2, ByteOrder.BigEndian)]
    public void ToUInt64BytesWithByteOrder(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ulong expected, ByteOrder byteOrder) => Assert.Equal(expected, BitConverter.ToUInt64(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), 0, byteOrder));

    [Theory]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ulong.MaxValue)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ulong.MaxValue / 2)]
    public void ToUInt64Span(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ulong expected) => Assert.Equal(expected, BitConverter.ToUInt64(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder)));

    [Theory]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ulong.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ulong.MaxValue / 2, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ulong.MaxValue, ByteOrder.BigEndian)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ulong.MaxValue / 2, ByteOrder.BigEndian)]
    public void ToUInt64SpanWithByteOrder(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ulong expected, ByteOrder byteOrder) => Assert.Equal(expected, BitConverter.ToUInt64(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), byteOrder));

#if NET5_0_OR_GREATER
    [Theory]
    [InlineData(65504D, 0x7B, byte.MaxValue)]
    [InlineData(-65504D, 0xFB, byte.MaxValue)]
    public void GetHalfBytes(Half value, byte first, byte second) => Assert.Equal(GetBytes(first, second, byteOrder: DefaultByteOrder), BitConverter.GetBytes(value));

    [Theory]
    [InlineData(65504D, 0x7B, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(-65504D, 0xFB, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(65504D, 0x7B, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(-65504D, 0xFB, byte.MaxValue, ByteOrder.BigEndian)]
    public void GetHalfBytesWithByteOrder(Half value, byte first, byte second, ByteOrder byteOrder) => Assert.Equal(GetBytes(first, second, byteOrder), BitConverter.GetBytes(value, byteOrder));

    [Theory]
    [InlineData(65504D, 0x7B, byte.MaxValue)]
    [InlineData(-65504D, 0xFB, byte.MaxValue)]
    public void TryWriteHalfBytes(Half value, byte first, byte second)
    {
        Span<byte> span = stackalloc byte[sizeof(short)];
        Assert.True(BitConverter.TryWriteBytes(span, value));
        Assert.Equal(GetBytes(first, second, byteOrder: DefaultByteOrder), span.ToArray());
    }

    [Theory]
    [InlineData(65504D, 0x7B, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(-65504D, 0xFB, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(65504D, 0x7B, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(-65504D, 0xFB, byte.MaxValue, ByteOrder.BigEndian)]
    public void TryWriteHalfBytesWithByteOrder(Half value, byte first, byte second, ByteOrder byteOrder)
    {
        Span<byte> span = stackalloc byte[sizeof(short)];
        Assert.True(BitConverter.TryWriteBytes(span, value, byteOrder));
        Assert.Equal(GetBytes(first, second, byteOrder), span.ToArray());
    }

    [Theory]
    [InlineData(0x7B, byte.MaxValue, 65504D)]
    [InlineData(0xFB, byte.MaxValue, -65504D)]
    public void ToHalfBytes(byte first, byte second, Half expected) => Assert.Equal((Half)expected, BitConverter.ToHalf(GetBytes(first, second, byteOrder: DefaultByteOrder), 0));

    [Theory]
    [InlineData(0x7B, byte.MaxValue, 65504D, ByteOrder.LittleEndian)]
    [InlineData(0xFB, byte.MaxValue, -65504D, ByteOrder.LittleEndian)]
    [InlineData(0x7B, byte.MaxValue, 65504D, ByteOrder.BigEndian)]
    [InlineData(0xFB, byte.MaxValue, -65504D, ByteOrder.BigEndian)]
    public void ToHalfBytesWithByteOrder(byte first, byte second, Half expected, ByteOrder byteOrder) => Assert.Equal((Half)expected, BitConverter.ToHalf(GetBytes(first, second, byteOrder), 0, byteOrder));

    [Theory]
    [InlineData(0x7B, byte.MaxValue, 65504D)]
    [InlineData(0xFB, byte.MaxValue, -65504D)]
    public void ToHalfSpan(byte first, byte second, Half expected) => Assert.Equal((Half)expected, BitConverter.ToHalf(GetBytes(first, second, byteOrder: DefaultByteOrder)));

    [Theory]
    [InlineData(0x7B, byte.MaxValue, 65504D, ByteOrder.LittleEndian)]
    [InlineData(0xFB, byte.MaxValue, -65504D, ByteOrder.LittleEndian)]
    [InlineData(0x7B, byte.MaxValue, 65504D, ByteOrder.BigEndian)]
    [InlineData(0xFB, byte.MaxValue, -65504D, ByteOrder.BigEndian)]
    public void ToHalfSpanWithByteOrder(byte first, byte second, Half expected, ByteOrder byteOrder) => Assert.Equal((Half)expected, BitConverter.ToHalf(GetBytes(first, second, byteOrder), byteOrder));
#endif

    [Theory]
    [InlineData(float.MaxValue, PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue)]
    [InlineData(float.MinValue, byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue)]
    public void GetSingleBytes(float value, byte first, byte second, byte third, byte forth) => Assert.Equal(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder), BitConverter.GetBytes(value));

    [Theory]
    [InlineData(float.MaxValue, PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(float.MinValue, byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(float.MaxValue, PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(float.MinValue, byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void GetSingleBytesWithByteOrder(float value, byte first, byte second, byte third, byte forth, ByteOrder byteOrder) => Assert.Equal(GetBytes(first, second, third, forth, byteOrder), BitConverter.GetBytes(value, byteOrder));

    [Theory]
    [InlineData(float.MaxValue, PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue)]
    [InlineData(float.MinValue, byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue)]
    public void TryWriteSingleBytes(float value, byte first, byte second, byte third, byte forth)
    {
        Span<byte> span = stackalloc byte[sizeof(float)];
        Assert.True(BitConverter.TryWriteBytes(span, value));
        Assert.Equal(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder), span.ToArray());
    }

    [Theory]
    [InlineData(float.MaxValue, PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(float.MinValue, byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(float.MaxValue, PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(float.MinValue, byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void TryWriteSingleBytesWithByteOrder(float value, byte first, byte second, byte third, byte forth, ByteOrder byteOrder)
    {
        Span<byte> span = stackalloc byte[sizeof(float)];
        Assert.True(BitConverter.TryWriteBytes(span, value, byteOrder));
        Assert.Equal(GetBytes(first, second, third, forth, byteOrder), span.ToArray());
    }

    [Theory]
    [InlineData(PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue, float.MaxValue)]
    [InlineData(byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, float.MinValue)]
    public void ToSingleBytes(byte first, byte second, byte third, byte forth, float expected) => Assert.Equal(expected, BitConverter.ToSingle(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder), 0));

    [Theory]
    [InlineData(PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue, float.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, float.MinValue, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue, float.MaxValue, ByteOrder.BigEndian)]
    [InlineData(byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, float.MinValue, ByteOrder.BigEndian)]
    public void ToSingleBytesWithByteOrder(byte first, byte second, byte third, byte forth, float expected, ByteOrder byteOrder) => Assert.Equal(expected, BitConverter.ToSingle(GetBytes(first, second, third, forth, byteOrder), 0, byteOrder));

    [Theory]
    [InlineData(PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue, float.MaxValue)]
    [InlineData(byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, float.MinValue)]
    public void ToSingleSpan(byte first, byte second, byte third, byte forth, float expected) => Assert.Equal(expected, BitConverter.ToSingle(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder)));

    [Theory]
    [InlineData(PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue, float.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, float.MinValue, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue, float.MaxValue, ByteOrder.BigEndian)]
    [InlineData(byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, float.MinValue, ByteOrder.BigEndian)]
    public void ToSingleSpanWithByteOrder(byte first, byte second, byte third, byte forth, float expected, ByteOrder byteOrder) => Assert.Equal(expected, BitConverter.ToSingle(GetBytes(first, second, third, forth, byteOrder), byteOrder));

    [Theory]
    [InlineData(double.MaxValue, PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(double.MinValue, byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void GetDoubleBytes(double value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth) => Assert.Equal(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder), BitConverter.GetBytes(value));

    [Theory]
    [InlineData(double.MaxValue, PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(double.MinValue, byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(double.MaxValue, PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(double.MinValue, byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void GetDoubleBytesWithByteOrder(double value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder) => Assert.Equal(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), BitConverter.GetBytes(value, byteOrder));

    [Theory]
    [InlineData(double.MaxValue, PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(double.MinValue, byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void TryWriteDoubleBytes(double value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth)
    {
        Span<byte> span = stackalloc byte[sizeof(double)];
        Assert.True(BitConverter.TryWriteBytes(span, value));
        Assert.Equal(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder), span.ToArray());
    }

    [Theory]
    [InlineData(double.MaxValue, PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(double.MinValue, byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(double.MaxValue, PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(double.MinValue, byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void TryWriteDoubleBytesWithByteOrder(double value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder)
    {
        Span<byte> span = stackalloc byte[sizeof(double)];
        Assert.True(BitConverter.TryWriteBytes(span, value, byteOrder));
        Assert.Equal(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), span.ToArray());
    }

    [Theory]
    [InlineData(PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, double.MaxValue)]
    [InlineData(byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, double.MinValue)]
    public void ToDoubleBytes(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, double expected) => Assert.Equal(expected, BitConverter.ToDouble(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder), 0));

    [Theory]
    [InlineData(PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, double.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, double.MinValue, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, double.MaxValue, ByteOrder.BigEndian)]
    [InlineData(byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, double.MinValue, ByteOrder.BigEndian)]
    public void ToDoubleBytesWithByteOrder(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, double expected, ByteOrder byteOrder) => Assert.Equal(expected, BitConverter.ToDouble(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), 0, byteOrder));

    [Theory]
    [InlineData(PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, double.MaxValue)]
    [InlineData(byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, double.MinValue)]
    public void ToDoubleSpan(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, double expected) => Assert.Equal(expected, BitConverter.ToDouble(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder)));

    [Theory]
    [InlineData(PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, double.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, double.MinValue, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, double.MaxValue, ByteOrder.BigEndian)]
    [InlineData(byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, double.MinValue, ByteOrder.BigEndian)]
    public void ToDoubleSpanWithByteOrder(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, double expected, ByteOrder byteOrder) => Assert.Equal(expected, BitConverter.ToDouble(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), byteOrder));

    [Fact]
    public void ToStringBytes() => Assert.Equal("74-78-65-54", BitConverter.ToString(GetBytes(0x54, 0x65, 0x78, 0x74, byteOrder: DefaultByteOrder)));

    [Fact]
    public void ToStringBytesWithStart() => Assert.Equal("74-78-65-54", BitConverter.ToString(GetBytes(0x54, 0x65, 0x78, 0x74, byteOrder: DefaultByteOrder), 0));

    [Fact]
    public void ToStringBytesWithStartAndLength() => Assert.Equal("74-78-65-54", BitConverter.ToString(GetBytes(0x54, 0x65, 0x78, 0x74, byteOrder: DefaultByteOrder), 0, 4));

#if NET5_0_OR_GREATER
    [Theory]
    [InlineData(2.0, (short.MaxValue / 2) + 1)]
    public void HalfToInt16(Half input, short expected) => Assert.Equal(expected, BitConverter.HalfToInt16Bits(input));

    [Theory]
    [InlineData(2.0, (ushort.MaxValue / 4) + 1)]
    public void HalfToUInt16(Half input, ushort expected) => Assert.Equal(expected, BitConverter.HalfToUInt16Bits(input));
#endif

    [Theory]
    [InlineData(2.0, (int.MaxValue / 2) + 1)]
    public void SingleToInt32(float input, int expected) => Assert.Equal(expected, BitConverter.SingleToInt32Bits(input));

    [Theory]
    [InlineData(2.0, (uint.MaxValue / 4) + 1)]
    public void SingleToUInt32(float input, uint expected) => Assert.Equal(expected, BitConverter.SingleToUInt32Bits(input));

    [Theory]
    [InlineData(2.0, (long.MaxValue / 2) + 1)]
    public void DoubleToInt64(double input, long expected) => Assert.Equal(expected, BitConverter.DoubleToInt64Bits(input));

    [Theory]
    [InlineData(2.0, (ulong.MaxValue / 4) + 1)]
    public void DoubleToUInt64(double input, ulong expected) => Assert.Equal(expected, BitConverter.DoubleToUInt64Bits(input));

#if NET5_0_OR_GREATER
    [Theory]
    [InlineData((short.MaxValue / 2) + 1, 2.0)]
    public void Int16ToHalf(short input, Half expected) => Assert.Equal((Half)expected, BitConverter.Int16BitsToHalf(input));

    [Theory]
    [InlineData((ushort.MaxValue / 4) + 1, 2.0)]
    public void UInt16ToHalf(ushort input, Half expected) => Assert.Equal((Half)expected, BitConverter.UInt16BitsToHalf(input));
#endif

    [Theory]
    [InlineData((int.MaxValue / 2) + 1, 2.0)]
    public void Int32ToSingle(int input, float expected) => Assert.Equal(expected, BitConverter.Int32BitsToSingle(input));

    [Theory]
    [InlineData((uint.MaxValue / 4) + 1, 2.0)]
    public void UInt32ToSingle(uint input, float expected) => Assert.Equal(expected, BitConverter.UInt32BitsToSingle(input));

    [Theory]
    [InlineData((long.MaxValue / 2) + 1, 2.0)]
    public void Int64ToDouble(long input, double expected) => Assert.Equal(expected, BitConverter.Int64BitsToDouble(input));

    [Theory]
    [InlineData((ulong.MaxValue / 4) + 1, 2.0)]
    public void UInt64ToDouble(ulong input, double expected) => Assert.Equal(expected, BitConverter.UInt64BitsToDouble(input));

    private static byte[] GetBytes(byte first, byte second, ByteOrder byteOrder) => byteOrder == ByteOrder.LittleEndian ? [second, first] : [first, second];

    private static byte[] GetBytes(byte first, byte second, byte third, byte forth, ByteOrder byteOrder) => byteOrder == ByteOrder.LittleEndian ? [forth, third, second, first] : [first, second, third, forth];

    private static byte[] GetBytes(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder) => byteOrder == ByteOrder.LittleEndian ? [eighth, seventh, sixth, fifth, forth, third, second, first] : [first, second, third, forth, fifth, sixth, seventh, eighth];

    private static bool ShouldSwap(ByteOrder byteOrder) => BitConverter.IsLittleEndian
        ? byteOrder is ByteOrder.LittleEndian
        : byteOrder is ByteOrder.BigEndian;
}