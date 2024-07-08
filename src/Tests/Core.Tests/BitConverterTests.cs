// -----------------------------------------------------------------------
// <copyright file="BitConverterTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

public class BitConverterTests
{
    private const byte PositiveByte = 0x7F;

    private const byte FloatByte = 0xEF;

    private readonly static ByteOrder DefaultByteOrder = BitConverter.IsLittleEndian ? ByteOrder.LittleEndian : ByteOrder.BigEndian;

    [Fact]
    public void LittleEndianessShouldEqualDefault() => BitConverter.IsLittleEndian.Should().Be(System.BitConverter.IsLittleEndian);

    [Theory]
    [InlineData(true, 1)]
    [InlineData(false, 0)]
    public void GetBooleanBytes(bool value, byte expected) => BitConverter.GetBytes(value).Should().BeEquivalentTo([expected]);

    [Theory]
    [InlineData(true, 1, ByteOrder.LittleEndian)]
    [InlineData(false, 0, ByteOrder.LittleEndian)]
    [InlineData(true, 1, ByteOrder.BigEndian)]
    [InlineData(false, 0, ByteOrder.BigEndian)]
    public void GetBooleanBytesWithByteOrder(bool value, byte expected, ByteOrder byteOrder) => BitConverter.GetBytes(value, byteOrder).Should().BeEquivalentTo([expected]);

    [Theory]
    [InlineData(true, 1)]
    [InlineData(false, 0)]
    public void TryWriteBooleanBytes(bool value, byte expected)
    {
        Span<byte> span = stackalloc byte[sizeof(byte)];
        BitConverter.TryWriteBytes(span, value).Should().BeTrue();
        span.ToArray().Should().BeEquivalentTo([expected]);
    }

    [Theory]
    [InlineData(true, 1, ByteOrder.LittleEndian)]
    [InlineData(false, 0, ByteOrder.LittleEndian)]
    [InlineData(true, 1, ByteOrder.BigEndian)]
    [InlineData(false, 0, ByteOrder.BigEndian)]
    public void TryWriteBooleanBytesWithByteOrder(bool value, byte expected, ByteOrder byteOrder)
    {
        Span<byte> span = stackalloc byte[sizeof(byte)];
        BitConverter.TryWriteBytes(span, value, byteOrder).Should().BeTrue();
        span.ToArray().Should().BeEquivalentTo([expected]);
    }

    [Fact]
    public void TryWriteBooleanBytesWithSmallSpan() => BitConverter.TryWriteBytes(default, default(bool)).Should().BeFalse();

    [Theory]
    [InlineData(1, true)]
    [InlineData(0, false)]
    public void ToBooleanBytes(byte value, bool expected) => BitConverter.ToBoolean(new byte[] { value }, 0).Should().Be(expected);

    [Theory]
    [InlineData(1, true, ByteOrder.LittleEndian)]
    [InlineData(0, false, ByteOrder.LittleEndian)]
    [InlineData(1, true, ByteOrder.BigEndian)]
    [InlineData(0, false, ByteOrder.BigEndian)]
    public void ToBooleanBytesWithByteOrder(byte value, bool expected, ByteOrder byteOrder) => BitConverter.ToBoolean([value], 0, byteOrder).Should().Be(expected);

    [Theory]
    [InlineData(1, true)]
    [InlineData(0, false)]
    public void ToBooleanSpan(byte value, bool expected) => BitConverter.ToBoolean([value]).Should().Be(expected);

    [Theory]
    [InlineData(1, true, ByteOrder.LittleEndian)]
    [InlineData(0, false, ByteOrder.LittleEndian)]
    [InlineData(1, true, ByteOrder.BigEndian)]
    [InlineData(0, false, ByteOrder.BigEndian)]
    public void ToBooleanSpanWithByteOrder(byte value, bool expected, ByteOrder byteOrder) => BitConverter.ToBoolean([value], byteOrder).Should().Be(expected);


    [Fact]
    public void ToBooleanSpanWithSmallSpan()
    {
        Action act = () => BitConverter.ToBoolean(default);
        act.Should().ThrowExactly<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData('A', 0x00, 0x41)]
    [InlineData('﴾', 0xFD, 0x3E)]
    public void GetCharBytes(char value, byte first, byte second) => BitConverter.GetBytes(value).Should().BeEquivalentTo(GetBytes(first, second, byteOrder: DefaultByteOrder));

    [Theory]
    [InlineData('A', 0x00, 0x41, ByteOrder.LittleEndian)]
    [InlineData('﴾', 0xFD, 0x3E, ByteOrder.LittleEndian)]
    [InlineData('A', 0x00, 0x41, ByteOrder.BigEndian)]
    [InlineData('﴾', 0xFD, 0x3E, ByteOrder.BigEndian)]
    public void GetCharBytesWithByteOrder(char value, byte first, byte second, ByteOrder byteOrder) => BitConverter.GetBytes(value, byteOrder).Should().BeEquivalentTo(GetBytes(first, second, byteOrder));

    [Theory]
    [InlineData('A', 0x00, 0x41)]
    [InlineData('﴾', 0xFD, 0x3E)]
    public void TryWriteCharBytes(char value, byte first, byte second)
    {
        Span<byte> span = stackalloc byte[sizeof(char)];
        BitConverter.TryWriteBytes(span, value).Should().BeTrue();
        span.ToArray().Should().BeEquivalentTo(GetBytes(first, second, byteOrder: DefaultByteOrder));
    }

    [Theory]
    [InlineData('A', 0x00, 0x41, ByteOrder.LittleEndian)]
    [InlineData('﴾', 0xFD, 0x3E, ByteOrder.LittleEndian)]
    [InlineData('A', 0x00, 0x41, ByteOrder.BigEndian)]
    [InlineData('﴾', 0xFD, 0x3E, ByteOrder.BigEndian)]
    public void TryWriteCharBytesWithByteOrder(char value, byte first, byte second, ByteOrder byteOrder)
    {
        Span<byte> span = stackalloc byte[sizeof(char)];
        BitConverter.TryWriteBytes(span, value, byteOrder).Should().BeTrue();
        span.ToArray().Should().BeEquivalentTo(GetBytes(first, second, byteOrder));
    }

    [Theory]
    [InlineData(0x00, 0x41, 'A')]
    [InlineData(0xFD, 0x3E, '﴾')]
    public void ToCharBytes(byte first, byte second, char expected) => BitConverter.ToChar(GetBytes(first, second, byteOrder: DefaultByteOrder), 0).Should().Be(expected);

    [Theory]
    [InlineData(0x00, 0x41, 'A', ByteOrder.LittleEndian)]
    [InlineData(0xFD, 0x3E, '﴾', ByteOrder.LittleEndian)]
    [InlineData(0x00, 0x41, 'A', ByteOrder.BigEndian)]
    [InlineData(0xFD, 0x3E, '﴾', ByteOrder.BigEndian)]
    public void ToCharBytesWithByteOrder(byte first, byte second, char expected, ByteOrder byteOrder) => BitConverter.ToChar(GetBytes(first, second, byteOrder), 0, byteOrder).Should().Be(expected);

    [Theory]
    [InlineData(0x00, 0x41, 'A')]
    [InlineData(0xFD, 0x3E, '﴾')]
    public void ToCharSpan(byte first, byte second, char expected) => BitConverter.ToChar(GetBytes(first, second, byteOrder: DefaultByteOrder)).Should().Be(expected);

    [Theory]
    [InlineData(0x00, 0x41, 'A', ByteOrder.LittleEndian)]
    [InlineData(0xFD, 0x3E, '﴾', ByteOrder.LittleEndian)]
    [InlineData(0x00, 0x41, 'A', ByteOrder.BigEndian)]
    [InlineData(0xFD, 0x3E, '﴾', ByteOrder.BigEndian)]
    public void ToCharSpanWithByteOrder(byte first, byte second, char expected, ByteOrder byteOrder) => BitConverter.ToChar(GetBytes(first, second, byteOrder), byteOrder).Should().Be(expected);

    [Theory]
    [InlineData(short.MaxValue, PositiveByte, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue)]
    public void GetInt16Bytes(short value, byte first, byte second) => BitConverter.GetBytes(value).Should().BeEquivalentTo(GetBytes(first, second, byteOrder: DefaultByteOrder));

    [Theory]
    [InlineData(short.MaxValue, PositiveByte, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(short.MaxValue, PositiveByte, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void GetInt16BytesWithByteOrder(short value, byte first, byte second, ByteOrder byteOrder) => BitConverter.GetBytes(value, byteOrder).Should().BeEquivalentTo(GetBytes(first, second, byteOrder));

    [Theory]
    [InlineData(short.MaxValue, PositiveByte, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue)]
    public void TryWriteInt16Bytes(short value, byte first, byte second)
    {
        Span<byte> span = stackalloc byte[sizeof(short)];
        BitConverter.TryWriteBytes(span, value).Should().BeTrue();
        span.ToArray().Should().BeEquivalentTo(GetBytes(first, second, byteOrder: DefaultByteOrder));
    }

    [Theory]
    [InlineData(short.MaxValue, PositiveByte, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(short.MaxValue, PositiveByte, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void TryWriteInt16BytesWithByteOrder(short value, byte first, byte second, ByteOrder byteOrder)
    {
        Span<byte> span = stackalloc byte[sizeof(short)];
        BitConverter.TryWriteBytes(span, value, byteOrder).Should().BeTrue();
        span.ToArray().Should().BeEquivalentTo(GetBytes(first, second, byteOrder));
    }

    [Theory]
    [InlineData(PositiveByte, byte.MaxValue, short.MaxValue)]
    [InlineData(byte.MaxValue, byte.MaxValue, -1)]
    public void ToInt16Bytes(byte first, byte second, short expected) => BitConverter.ToInt16(GetBytes(first, second, byteOrder: DefaultByteOrder), 0).Should().Be(expected);

    [Theory]
    [InlineData(PositiveByte, byte.MaxValue, short.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, -1, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, byte.MaxValue, short.MaxValue, ByteOrder.BigEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, -1, ByteOrder.BigEndian)]
    public void ToInt16BytesWithByteOrder(byte first, byte second, short expected, ByteOrder byteOrder) => BitConverter.ToInt16(GetBytes(first, second, byteOrder), 0, byteOrder).Should().Be(expected);

    [Theory]
    [InlineData(PositiveByte, byte.MaxValue, short.MaxValue)]
    [InlineData(byte.MaxValue, byte.MaxValue, -1)]
    public void ToInt16Span(byte first, byte second, short expected) => BitConverter.ToInt16(GetBytes(first, second, byteOrder: DefaultByteOrder)).Should().Be(expected);

    [Theory]
    [InlineData(PositiveByte, byte.MaxValue, short.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, -1, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, byte.MaxValue, short.MaxValue, ByteOrder.BigEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, -1, ByteOrder.BigEndian)]
    public void ToInt16SpanWithByteOrder(byte first, byte second, short expected, ByteOrder byteOrder) => BitConverter.ToInt16(GetBytes(first, second, byteOrder), byteOrder).Should().Be(expected);

    [Theory]
    [InlineData(int.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void GetInt32Bytes(int value, byte first, byte second, byte third, byte forth) => BitConverter.GetBytes(value).Should().BeEquivalentTo(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder));

    [Theory]
    [InlineData(int.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(int.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void GetInt32BytesWithByteOrder(int value, byte first, byte second, byte third, byte forth, ByteOrder byteOrder) => BitConverter.GetBytes(value, byteOrder).Should().BeEquivalentTo(GetBytes(first, second, third, forth, byteOrder));

    [Theory]
    [InlineData(int.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void TryWriteInt32Bytes(int value, byte first, byte second, byte third, byte forth)
    {
        Span<byte> span = stackalloc byte[sizeof(int)];
        BitConverter.TryWriteBytes(span, value).Should().BeTrue();
        span.ToArray().Should().BeEquivalentTo(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder));
    }

    [Theory]
    [InlineData(int.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(int.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void TryWriteInt32BytesWithByteOrder(int value, byte first, byte second, byte third, byte forth, ByteOrder byteOrder)
    {
        Span<byte> span = stackalloc byte[sizeof(int)];
        BitConverter.TryWriteBytes(span, value, byteOrder).Should().BeTrue();
        span.ToArray().Should().BeEquivalentTo(GetBytes(first, second, third, forth, byteOrder));
    }

    [Theory]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, int.MaxValue)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, -1)]
    public void ToInt32Bytes(byte first, byte second, byte third, byte forth, int expected) => BitConverter.ToInt32(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder), 0).Should().Be(expected);

    [Theory]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, int.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, -1, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, int.MaxValue, ByteOrder.BigEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, -1, ByteOrder.BigEndian)]
    public void ToInt32BytesWithByteOrder(byte first, byte second, byte third, byte forth, int expected, ByteOrder byteOrder) => BitConverter.ToInt32(GetBytes(first, second, third, forth, byteOrder), 0, byteOrder).Should().Be(expected);

    [Theory]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, int.MaxValue)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, -1)]
    public void ToInt32Span(byte first, byte second, byte third, byte forth, int expected) => BitConverter.ToInt32(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder)).Should().Be(expected);

    [Theory]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, int.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, -1, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, int.MaxValue, ByteOrder.BigEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, -1, ByteOrder.BigEndian)]
    public void ToInt32SpanWithByteOrder(byte first, byte second, byte third, byte forth, int expected, ByteOrder byteOrder) => BitConverter.ToInt32(GetBytes(first, second, third, forth, byteOrder), byteOrder).Should().Be(expected);

    [Theory]
    [InlineData(long.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void GetInt64Bytes(long value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth) => BitConverter.GetBytes(value).Should().BeEquivalentTo(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder));

    [Theory]
    [InlineData(long.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(long.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void GetInt64BytesWithByteOrder(long value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder) => BitConverter.GetBytes(value, byteOrder).Should().BeEquivalentTo(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder));

    [Theory]
    [InlineData(long.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void TryWriteInt64Bytes(long value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth)
    {
        Span<byte> span = stackalloc byte[sizeof(long)];
        BitConverter.TryWriteBytes(span, value).Should().BeTrue();
        span.ToArray().Should().BeEquivalentTo(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder));
    }

    [Theory]
    [InlineData(long.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(long.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void TryWriteInt64BytesWithByteOrder(long value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder)
    {
        Span<byte> span = stackalloc byte[sizeof(long)];
        BitConverter.TryWriteBytes(span, value, byteOrder).Should().BeTrue();
        span.ToArray().Should().BeEquivalentTo(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder));
    }

    [Theory]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, long.MaxValue)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, -1)]
    public void ToInt64Bytes(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, long expected) => BitConverter.ToInt64(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder), 0).Should().Be(expected);

    [Theory]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, long.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, -1, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, long.MaxValue, ByteOrder.BigEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, -1, ByteOrder.BigEndian)]
    public void ToInt64BytesWithByteOrder(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, long expected, ByteOrder byteOrder) => BitConverter.ToInt64(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), 0, byteOrder).Should().Be(expected);

    [Theory]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, long.MaxValue)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, -1)]
    public void ToInt64Span(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, long expected) => BitConverter.ToInt64(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder)).Should().Be(expected);

    [Theory]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, long.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, -1, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, long.MaxValue, ByteOrder.BigEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, -1, ByteOrder.BigEndian)]
    public void ToInt64SpanWithByteOrder(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, long expected, ByteOrder byteOrder) => BitConverter.ToInt64(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), byteOrder).Should().Be(expected);

    [Theory]
    [InlineData(ushort.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(ushort.MaxValue / 2, PositiveByte, byte.MaxValue)]
    public void GetUInt16Bytes(ushort value, byte first, byte second) => BitConverter.GetBytes(value).Should().BeEquivalentTo(GetBytes(first, second, byteOrder: DefaultByteOrder));

    [Theory]
    [InlineData(ushort.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(ushort.MaxValue / 2, PositiveByte, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(ushort.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(ushort.MaxValue / 2, PositiveByte, byte.MaxValue, ByteOrder.BigEndian)]
    public void GetUInt16BytesWithByteOrder(ushort value, byte first, byte second, ByteOrder byteOrder) => BitConverter.GetBytes(value, byteOrder).Should().BeEquivalentTo(GetBytes(first, second, byteOrder));

    [Theory]
    [InlineData(ushort.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(ushort.MaxValue / 2, PositiveByte, byte.MaxValue)]
    public void TryWriteUInt16Bytes(ushort value, byte first, byte second)
    {
        Span<byte> span = stackalloc byte[sizeof(ushort)];
        BitConverter.TryWriteBytes(span, value).Should().BeTrue();
        span.ToArray().Should().BeEquivalentTo(GetBytes(first, second, byteOrder: DefaultByteOrder));
    }

    [Theory]
    [InlineData(ushort.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(ushort.MaxValue / 2, PositiveByte, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(ushort.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(ushort.MaxValue / 2, PositiveByte, byte.MaxValue, ByteOrder.BigEndian)]
    public void TryWriteUInt16BytesWithByteOrder(ushort value, byte first, byte second, ByteOrder byteOrder)
    {
        Span<byte> span = stackalloc byte[sizeof(ushort)];
        BitConverter.TryWriteBytes(span, value, byteOrder).Should().BeTrue();
        span.ToArray().Should().BeEquivalentTo(GetBytes(first, second, byteOrder));
    }

    [Theory]
    [InlineData(byte.MaxValue, byte.MaxValue, ushort.MaxValue)]
    [InlineData(PositiveByte, byte.MaxValue, ushort.MaxValue / 2)]
    public void ToUInt16Bytes(byte first, byte second, ushort expected) => BitConverter.ToUInt16(GetBytes(first, second, byteOrder: DefaultByteOrder), 0).Should().Be(expected);

    [Theory]
    [InlineData(byte.MaxValue, byte.MaxValue, ushort.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, byte.MaxValue, ushort.MaxValue / 2, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, ushort.MaxValue, ByteOrder.BigEndian)]
    [InlineData(PositiveByte, byte.MaxValue, ushort.MaxValue / 2, ByteOrder.BigEndian)]
    public void ToUInt16BytesWithByteOrder(byte first, byte second, ushort expected, ByteOrder byteOrder) => BitConverter.ToUInt16(GetBytes(first, second, byteOrder), 0, byteOrder).Should().Be(expected);

    [Theory]
    [InlineData(byte.MaxValue, byte.MaxValue, ushort.MaxValue)]
    [InlineData(PositiveByte, byte.MaxValue, ushort.MaxValue / 2)]
    public void ToUInt16Span(byte first, byte second, ushort expected) => BitConverter.ToUInt16(GetBytes(first, second, byteOrder: DefaultByteOrder)).Should().Be(expected);

    [Theory]
    [InlineData(byte.MaxValue, byte.MaxValue, ushort.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, byte.MaxValue, ushort.MaxValue / 2, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, ushort.MaxValue, ByteOrder.BigEndian)]
    [InlineData(PositiveByte, byte.MaxValue, ushort.MaxValue / 2, ByteOrder.BigEndian)]
    public void ToUInt16SpanWithByteOrder(byte first, byte second, ushort expected, ByteOrder byteOrder) => BitConverter.ToUInt16(GetBytes(first, second, byteOrder), byteOrder).Should().Be(expected);

    [Theory]
    [InlineData(uint.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(uint.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void GetUInt32Bytes(uint value, byte first, byte second, byte third, byte forth) => BitConverter.GetBytes(value).Should().BeEquivalentTo(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder));

    [Theory]
    [InlineData(uint.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(uint.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(uint.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(uint.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void GetUInt32BytesWithByteOrder(uint value, byte first, byte second, byte third, byte forth, ByteOrder byteOrder) => BitConverter.GetBytes(value, byteOrder).Should().BeEquivalentTo(GetBytes(first, second, third, forth, byteOrder));

    [Theory]
    [InlineData(uint.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(uint.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void TryWriteUInt32Bytes(uint value, byte first, byte second, byte third, byte forth)
    {
        Span<byte> span = stackalloc byte[sizeof(uint)];
        BitConverter.TryWriteBytes(span, value).Should().BeTrue();
        span.ToArray().Should().BeEquivalentTo(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder));
    }

    [Theory]
    [InlineData(uint.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(uint.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(uint.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(uint.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void TryWriteUInt32BytesWithByteOrder(uint value, byte first, byte second, byte third, byte forth, ByteOrder byteOrder)
    {
        Span<byte> span = stackalloc byte[sizeof(uint)];
        BitConverter.TryWriteBytes(span, value, byteOrder).Should().BeTrue();
        span.ToArray().Should().BeEquivalentTo(GetBytes(first, second, third, forth, byteOrder));
    }

    [Theory]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, uint.MaxValue)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, uint.MaxValue / 2)]
    public void ToUInt32Bytes(byte first, byte second, byte third, byte forth, uint expected) => BitConverter.ToUInt32(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder), 0).Should().Be(expected);

    [Theory]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, uint.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, uint.MaxValue / 2, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, uint.MaxValue, ByteOrder.BigEndian)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, uint.MaxValue / 2, ByteOrder.BigEndian)]
    public void ToUInt32BytesWithByteOrder(byte first, byte second, byte third, byte forth, uint expected, ByteOrder byteOrder) => BitConverter.ToUInt32(GetBytes(first, second, third, forth, byteOrder), 0, byteOrder).Should().Be(expected);

    [Theory]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, uint.MaxValue)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, uint.MaxValue / 2)]
    public void ToUInt32Span(byte first, byte second, byte third, byte forth, uint expected) => BitConverter.ToUInt32(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder)).Should().Be(expected);

    [Theory]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, uint.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, uint.MaxValue / 2, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, uint.MaxValue, ByteOrder.BigEndian)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, uint.MaxValue / 2, ByteOrder.BigEndian)]
    public void ToUInt32SpanWithByteOrder(byte first, byte second, byte third, byte forth, uint expected, ByteOrder byteOrder) => BitConverter.ToUInt32(GetBytes(first, second, third, forth, byteOrder), byteOrder).Should().Be(expected);

    [Theory]
    [InlineData(ulong.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(ulong.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void GetUInt64Bytes(ulong value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth) => BitConverter.GetBytes(value).Should().BeEquivalentTo(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder));

    [Theory]
    [InlineData(ulong.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(ulong.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(ulong.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(ulong.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void GetUInt64BytesWithByteOrder(ulong value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder) => BitConverter.GetBytes(value, byteOrder).Should().BeEquivalentTo(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder));

    [Theory]
    [InlineData(ulong.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(ulong.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void TryWriteUInt64Bytes(ulong value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth)
    {
        Span<byte> span = stackalloc byte[sizeof(ulong)];
        BitConverter.TryWriteBytes(span, value).Should().BeTrue();
        span.ToArray().Should().BeEquivalentTo(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder));
    }

    [Theory]
    [InlineData(ulong.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(ulong.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(ulong.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(ulong.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void TryWriteUInt64BytesWithByteOrder(ulong value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder)
    {
        Span<byte> span = stackalloc byte[sizeof(ulong)];
        BitConverter.TryWriteBytes(span, value, byteOrder).Should().BeTrue();
        span.ToArray().Should().BeEquivalentTo(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder));
    }

    [Theory]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ulong.MaxValue)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ulong.MaxValue / 2)]
    public void ToUInt64Bytes(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ulong expected) => BitConverter.ToUInt64(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder), 0).Should().Be(expected);

    [Theory]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ulong.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ulong.MaxValue / 2, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ulong.MaxValue, ByteOrder.BigEndian)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ulong.MaxValue / 2, ByteOrder.BigEndian)]
    public void ToUInt64BytesWithByteOrder(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ulong expected, ByteOrder byteOrder) => BitConverter.ToUInt64(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), 0, byteOrder).Should().Be(expected);

    [Theory]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ulong.MaxValue)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ulong.MaxValue / 2)]
    public void ToUInt64Span(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ulong expected) => BitConverter.ToUInt64(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder)).Should().Be(expected);

    [Theory]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ulong.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ulong.MaxValue / 2, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ulong.MaxValue, ByteOrder.BigEndian)]
    [InlineData(PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ulong.MaxValue / 2, ByteOrder.BigEndian)]
    public void ToUInt64SpanWithByteOrder(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ulong expected, ByteOrder byteOrder) => BitConverter.ToUInt64(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), byteOrder).Should().Be(expected);

#if NET5_0_OR_GREATER
    [Theory]
    [InlineData(65504D, 0x7B, byte.MaxValue)]
    [InlineData(-65504D, 0xFB, byte.MaxValue)]
    public void GetHalfBytes(SerializableHalf value, byte first, byte second) => BitConverter.GetBytes(value).Should().BeEquivalentTo(GetBytes(first, second, byteOrder: DefaultByteOrder));

    [Theory]
    [InlineData(65504D, 0x7B, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(-65504D, 0xFB, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(65504D, 0x7B, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(-65504D, 0xFB, byte.MaxValue, ByteOrder.BigEndian)]
    public void GetHalfBytesWithByteOrder(SerializableHalf value, byte first, byte second, ByteOrder byteOrder) => BitConverter.GetBytes(value, byteOrder).Should().BeEquivalentTo(GetBytes(first, second, byteOrder));

    [Theory]
    [InlineData(65504D, 0x7B, byte.MaxValue)]
    [InlineData(-65504D, 0xFB, byte.MaxValue)]
    public void TryWriteHalfBytes(SerializableHalf value, byte first, byte second)
    {
        Span<byte> span = stackalloc byte[sizeof(short)];
        BitConverter.TryWriteBytes(span, value).Should().BeTrue();
        span.ToArray().Should().BeEquivalentTo(GetBytes(first, second, byteOrder: DefaultByteOrder));
    }

    [Theory]
    [InlineData(65504D, 0x7B, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(-65504D, 0xFB, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(65504D, 0x7B, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(-65504D, 0xFB, byte.MaxValue, ByteOrder.BigEndian)]
    public void TryWriteHalfBytesWithByteOrder(SerializableHalf value, byte first, byte second, ByteOrder byteOrder)
    {
        Span<byte> span = stackalloc byte[sizeof(short)];
        BitConverter.TryWriteBytes(span, value, byteOrder).Should().BeTrue();
        span.ToArray().Should().BeEquivalentTo(GetBytes(first, second, byteOrder));
    }

    [Theory]
    [InlineData(0x7B, byte.MaxValue, 65504D)]
    [InlineData(0xFB, byte.MaxValue, -65504D)]
    public void ToHalfBytes(byte first, byte second, SerializableHalf expected) => BitConverter.ToHalf(GetBytes(first, second, byteOrder: DefaultByteOrder), 0).Should().Be(expected);

    [Theory]
    [InlineData(0x7B, byte.MaxValue, 65504D, ByteOrder.LittleEndian)]
    [InlineData(0xFB, byte.MaxValue, -65504D, ByteOrder.LittleEndian)]
    [InlineData(0x7B, byte.MaxValue, 65504D, ByteOrder.BigEndian)]
    [InlineData(0xFB, byte.MaxValue, -65504D, ByteOrder.BigEndian)]
    public void ToHalfBytesWithByteOrder(byte first, byte second, SerializableHalf expected, ByteOrder byteOrder) => BitConverter.ToHalf(GetBytes(first, second, byteOrder), 0, byteOrder).Should().Be(expected);

    [Theory]
    [InlineData(0x7B, byte.MaxValue, 65504D)]
    [InlineData(0xFB, byte.MaxValue, -65504D)]
    public void ToHalfSpan(byte first, byte second, SerializableHalf expected) => BitConverter.ToHalf(GetBytes(first, second, byteOrder: DefaultByteOrder)).Should().Be(expected);

    [Theory]
    [InlineData(0x7B, byte.MaxValue, 65504D, ByteOrder.LittleEndian)]
    [InlineData(0xFB, byte.MaxValue, -65504D, ByteOrder.LittleEndian)]
    [InlineData(0x7B, byte.MaxValue, 65504D, ByteOrder.BigEndian)]
    [InlineData(0xFB, byte.MaxValue, -65504D, ByteOrder.BigEndian)]
    public void ToHalfSpanWithByteOrder(byte first, byte second, SerializableHalf expected, ByteOrder byteOrder) => BitConverter.ToHalf(GetBytes(first, second, byteOrder), byteOrder).Should().Be(expected);
#endif

    [Theory]
    [InlineData(float.MaxValue, PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue)]
    [InlineData(float.MinValue, byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue)]
    public void GetSingleBytes(float value, byte first, byte second, byte third, byte forth) => BitConverter.GetBytes(value).Should().BeEquivalentTo(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder));

    [Theory]
    [InlineData(float.MaxValue, PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(float.MinValue, byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(float.MaxValue, PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(float.MinValue, byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void GetSingleBytesWithByteOrder(float value, byte first, byte second, byte third, byte forth, ByteOrder byteOrder) => BitConverter.GetBytes(value, byteOrder).Should().BeEquivalentTo(GetBytes(first, second, third, forth, byteOrder));

    [Theory]
    [InlineData(float.MaxValue, PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue)]
    [InlineData(float.MinValue, byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue)]
    public void TryWriteSingleBytes(float value, byte first, byte second, byte third, byte forth)
    {
        Span<byte> span = stackalloc byte[sizeof(float)];
        BitConverter.TryWriteBytes(span, value).Should().BeTrue();
        span.ToArray().Should().BeEquivalentTo(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder));
    }

    [Theory]
    [InlineData(float.MaxValue, PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(float.MinValue, byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(float.MaxValue, PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(float.MinValue, byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void TryWriteSingleBytesWithByteOrder(float value, byte first, byte second, byte third, byte forth, ByteOrder byteOrder)
    {
        Span<byte> span = stackalloc byte[sizeof(float)];
        BitConverter.TryWriteBytes(span, value, byteOrder).Should().BeTrue();
        span.ToArray().Should().BeEquivalentTo(GetBytes(first, second, third, forth, byteOrder));
    }

    [Theory]
    [InlineData(PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue, float.MaxValue)]
    [InlineData(byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, float.MinValue)]
    public void ToSingleBytes(byte first, byte second, byte third, byte forth, float expected) => BitConverter.ToSingle(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder), 0).Should().Be(expected);

    [Theory]
    [InlineData(PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue, float.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, float.MinValue, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue, float.MaxValue, ByteOrder.BigEndian)]
    [InlineData(byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, float.MinValue, ByteOrder.BigEndian)]
    public void ToSingleBytesWithByteOrder(byte first, byte second, byte third, byte forth, float expected, ByteOrder byteOrder) => BitConverter.ToSingle(GetBytes(first, second, third, forth, byteOrder), 0, byteOrder).Should().Be(expected);

    [Theory]
    [InlineData(PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue, float.MaxValue)]
    [InlineData(byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, float.MinValue)]
    public void ToSingleSpan(byte first, byte second, byte third, byte forth, float expected) => BitConverter.ToSingle(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder)).Should().Be(expected);

    [Theory]
    [InlineData(PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue, float.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, float.MinValue, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue, float.MaxValue, ByteOrder.BigEndian)]
    [InlineData(byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, float.MinValue, ByteOrder.BigEndian)]
    public void ToSingleSpanWithByteOrder(byte first, byte second, byte third, byte forth, float expected, ByteOrder byteOrder) => BitConverter.ToSingle(GetBytes(first, second, third, forth, byteOrder), byteOrder).Should().Be(expected);

    [Theory]
    [InlineData(double.MaxValue, PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(double.MinValue, byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void GetDoubleBytes(double value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth) => BitConverter.GetBytes(value).Should().BeEquivalentTo(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder));

    [Theory]
    [InlineData(double.MaxValue, PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(double.MinValue, byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(double.MaxValue, PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(double.MinValue, byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void GetDoubleBytesWithByteOrder(double value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder) => BitConverter.GetBytes(value, byteOrder).Should().BeEquivalentTo(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder));

    [Theory]
    [InlineData(double.MaxValue, PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    [InlineData(double.MinValue, byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
    public void TryWriteDoubleBytes(double value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth)
    {
        Span<byte> span = stackalloc byte[sizeof(double)];
        BitConverter.TryWriteBytes(span, value).Should().BeTrue();
        span.ToArray().Should().BeEquivalentTo(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder));
    }

    [Theory]
    [InlineData(double.MaxValue, PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(double.MinValue, byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(double.MaxValue, PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(double.MinValue, byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void TryWriteDoubleBytesWithByteOrder(double value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder)
    {
        Span<byte> span = stackalloc byte[sizeof(double)];
        BitConverter.TryWriteBytes(span, value, byteOrder).Should().BeTrue();
        span.ToArray().Should().BeEquivalentTo(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder));
    }

    [Theory]
    [InlineData(PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, double.MaxValue)]
    [InlineData(byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, double.MinValue)]
    public void ToDoubleBytes(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, double expected) => BitConverter.ToDouble(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder), 0).Should().Be(expected);

    [Theory]
    [InlineData(PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, double.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, double.MinValue, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, double.MaxValue, ByteOrder.BigEndian)]
    [InlineData(byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, double.MinValue, ByteOrder.BigEndian)]
    public void ToDoubleBytesWithByteOrder(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, double expected, ByteOrder byteOrder) => BitConverter.ToDouble(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), 0, byteOrder).Should().Be(expected);

    [Theory]
    [InlineData(PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, double.MaxValue)]
    [InlineData(byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, double.MinValue)]
    public void ToDoubleSpan(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, double expected) => BitConverter.ToDouble(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder)).Should().Be(expected);

    [Theory]
    [InlineData(PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, double.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, double.MinValue, ByteOrder.LittleEndian)]
    [InlineData(PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, double.MaxValue, ByteOrder.BigEndian)]
    [InlineData(byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, double.MinValue, ByteOrder.BigEndian)]
    public void ToDoubleSpanWithByteOrder(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, double expected, ByteOrder byteOrder) => BitConverter.ToDouble(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), byteOrder).Should().Be(expected);

    [Fact]
    public void ToStringBytes() => BitConverter.ToString(GetBytes(0x54, 0x65, 0x78, 0x74, byteOrder: DefaultByteOrder)).Should().Be("74-78-65-54");

    [Fact]
    public void ToStringBytesWithStart() => BitConverter.ToString(GetBytes(0x54, 0x65, 0x78, 0x74, byteOrder: DefaultByteOrder), 0).Should().Be("74-78-65-54");

    [Fact]
    public void ToStringBytesWithStartAndLength() => BitConverter.ToString(GetBytes(0x54, 0x65, 0x78, 0x74, byteOrder: DefaultByteOrder), 0, 4).Should().Be("74-78-65-54");

#if NET5_0_OR_GREATER
    [Theory]
    [InlineData(2.0, (short.MaxValue / 2) + 1)]
    public void HalfToInt16(SerializableHalf input, short expected) => BitConverter.HalfToInt16Bits(input).Should().Be(expected);

    [Theory]
    [InlineData(2.0, (ushort.MaxValue / 4) + 1)]
    public void HalfToUInt16(SerializableHalf input, ushort expected) => BitConverter.HalfToUInt16Bits(input).Should().Be(expected);
#endif

    [Theory]
    [InlineData(2.0, (int.MaxValue / 2) + 1)]
    public void SingleToInt32(float input, int expected) => BitConverter.SingleToInt32Bits(input).Should().Be(expected);

    [Theory]
    [InlineData(2.0, (uint.MaxValue / 4) + 1)]
    public void SingleToUInt32(float input, uint expected) => BitConverter.SingleToUInt32Bits(input).Should().Be(expected);

    [Theory]
    [InlineData(2.0, (long.MaxValue / 2) + 1)]
    public void DoubleToInt64(double input, long expected) => BitConverter.DoubleToInt64Bits(input).Should().Be(expected);

    [Theory]
    [InlineData(2.0, (ulong.MaxValue / 4) + 1)]
    public void DoubleToUInt64(double input, ulong expected) => BitConverter.DoubleToUInt64Bits(input).Should().Be(expected);

#if NET5_0_OR_GREATER
    [Theory]
    [InlineData((short.MaxValue / 2) + 1, 2.0)]
    public void Int16ToHalf(short input, SerializableHalf expected) => BitConverter.Int16BitsToHalf(input).Should().Be(expected);

    [Theory]
    [InlineData((ushort.MaxValue / 4) + 1, 2.0)]
    public void UInt16ToHalf(ushort input, SerializableHalf expected) => BitConverter.UInt16BitsToHalf(input).Should().Be(expected);
#endif

    [Theory]
    [InlineData((int.MaxValue / 2) + 1, 2.0)]
    public void Int32ToSingle(int input, float expected) => BitConverter.Int32BitsToSingle(input).Should().Be(expected);

    [Theory]
    [InlineData((uint.MaxValue / 4) + 1, 2.0)]
    public void UInt32ToSingle(uint input, float expected) => BitConverter.UInt32BitsToSingle(input).Should().Be(expected);

    [Theory]
    [InlineData((long.MaxValue / 2) + 1, 2.0)]
    public void Int64ToDouble(long input, double expected) => BitConverter.Int64BitsToDouble(input).Should().Be(expected);

    [Theory]
    [InlineData((ulong.MaxValue / 4) + 1, 2.0)]
    public void UInt64ToDouble(ulong input, double expected) => BitConverter.UInt64BitsToDouble(input).Should().Be(expected);

    private static byte[] GetBytes(byte first, byte second, ByteOrder byteOrder) => byteOrder == ByteOrder.LittleEndian ? [second, first] : [first, second];

    private static byte[] GetBytes(byte first, byte second, byte third, byte forth, ByteOrder byteOrder) => byteOrder == ByteOrder.LittleEndian ? [forth, third, second, first] : [first, second, third, forth];

    private static byte[] GetBytes(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder) => byteOrder == ByteOrder.LittleEndian ? [eighth, seventh, sixth, fifth, forth, third, second, first] : [first, second, third, forth, fifth, sixth, seventh, eighth];

    private static bool ShouldSwap(ByteOrder byteOrder) => BitConverter.IsLittleEndian
        ? byteOrder is ByteOrder.LittleEndian
        : byteOrder is ByteOrder.BigEndian;

#if NET5_0_OR_GREATER
    public class SerializableHalf : Xunit.Abstractions.IXunitSerializable
    {
        private Half value;

        public SerializableHalf()
        {
        }

        public SerializableHalf(Half value) => this.value = value;

        public void Deserialize(Xunit.Abstractions.IXunitSerializationInfo info) => this.value = System.BitConverter.Int16BitsToHalf(info.GetValue<short>($"{nameof(Half)}.{nameof(this.value)}"));

        public void Serialize(Xunit.Abstractions.IXunitSerializationInfo info) => info.AddValue($"{nameof(Half)}.{nameof(this.value)}", System.BitConverter.HalfToInt16Bits(this.value), typeof(short));

        public static implicit operator Half(SerializableHalf value) => value.value;

        public static implicit operator SerializableHalf(Half value) => new(value);

        public static implicit operator SerializableHalf(double value)
        {
            if (value <= (double)Half.MinValue)
            {
                return new(Half.MinValue);
            }
            else if (value >= (double)Half.MaxValue)
            {
                return new(Half.MaxValue);
            }

            return new((Half)value);
        }

        public override string ToString() => this.value.ToString();

        public override bool Equals(object? obj) => obj switch
        {
            SerializableHalf half => HalfEqualityComparer.Instance.Equals(this.value, half.value),
            Half half => HalfEqualityComparer.Instance.Equals(this.value, half),
            _ => base.Equals(obj),
        };

        public override int GetHashCode() => HalfEqualityComparer.Instance.GetHashCode(this);
    }

    private class HalfEqualityComparer : IEqualityComparer<Half>
    {
        public static readonly HalfEqualityComparer Instance = new();

        public bool Equals(Half x, Half y) => x.Equals(y);

        public int GetHashCode([System.Diagnostics.CodeAnalysis.DisallowNull] Half obj) => obj.GetHashCode();
    }
#endif
}
