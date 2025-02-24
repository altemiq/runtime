// -----------------------------------------------------------------------
// <copyright file="BitConverterTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

using TUnit.Assertions.AssertConditions.Throws;

public partial class BitConverterTests
{
    private const int PositiveByte = 0x7F;

    private const int FloatByte = 0xEF;

    private const int Int16MaxValue = short.MaxValue;

    private const int ByteMaxValue = byte.MaxValue;

    private readonly static ByteOrder DefaultByteOrder = BitConverter.IsLittleEndian ? ByteOrder.LittleEndian : ByteOrder.BigEndian;

    [Test]
    public async Task LittleEndianessShouldEqualDefault() => await Assert.That(BitConverter.IsLittleEndian).IsEqualTo(System.BitConverter.IsLittleEndian);

    [Test]
    [Arguments(true, 1)]
    [Arguments(false, 0)]
    public async Task GetBooleanBytes(bool value, byte expected) => await Assert.That(BitConverter.GetBytes(value)).IsEquivalentTo([expected]);

    [Test]
    [Arguments(true, 1, ByteOrder.LittleEndian)]
    [Arguments(false, 0, ByteOrder.LittleEndian)]
    [Arguments(true, 1, ByteOrder.BigEndian)]
    [Arguments(false, 0, ByteOrder.BigEndian)]
    public async Task GetBooleanBytesWithByteOrder(bool value, byte expected, ByteOrder byteOrder) => await Assert.That(BitConverter.GetBytes(value, byteOrder)).IsEquivalentTo([expected]);

    [Test]
    [Arguments(true, 1)]
    [Arguments(false, 0)]
    public async Task TryWriteBooleanBytes(bool value, byte expected)
    {
        byte[] span = new byte[sizeof(byte)];
        var result = BitConverter.TryWriteBytes(span, value);
        var spanArray = span;
        await Assert.That(result).IsTrue();
        await Assert.That(spanArray).IsEquivalentTo([expected]);
    }

    [Test]
    [Arguments(true, 1, ByteOrder.LittleEndian)]
    [Arguments(false, 0, ByteOrder.LittleEndian)]
    [Arguments(true, 1, ByteOrder.BigEndian)]
    [Arguments(false, 0, ByteOrder.BigEndian)]
    public async Task TryWriteBooleanBytesWithByteOrder(bool value, byte expected, ByteOrder byteOrder)
    {
        byte[] span = new byte[sizeof(byte)];
        await Assert.That(BitConverter.TryWriteBytes(span, value, byteOrder)).IsTrue();
        await Assert.That(span).IsEquivalentTo([expected]);
    }

    [Test]
    public async Task TryWriteBooleanBytesWithSmallSpan() => await Assert.That(BitConverter.TryWriteBytes(default, default(bool))).IsFalse();

    [Test]
    [Arguments(1, true)]
    [Arguments(0, false)]
    public async Task ToBooleanBytes(byte value, bool expected) => await Assert.That(BitConverter.ToBoolean(new byte[] { value }, 0)).IsEqualTo(expected);

    [Test]
    [Arguments(1, true, ByteOrder.LittleEndian)]
    [Arguments(0, false, ByteOrder.LittleEndian)]
    [Arguments(1, true, ByteOrder.BigEndian)]
    [Arguments(0, false, ByteOrder.BigEndian)]
    public async Task ToBooleanBytesWithByteOrder(byte value, bool expected, ByteOrder byteOrder) => await Assert.That(BitConverter.ToBoolean([value], 0, byteOrder)).IsEqualTo(expected);

    [Test]
    [Arguments(1, true)]
    [Arguments(0, false)]
    public async Task ToBooleanSpan(byte value, bool expected) => await Assert.That(BitConverter.ToBoolean([value])).IsEqualTo(expected);

    [Test]
    [Arguments(1, true, ByteOrder.LittleEndian)]
    [Arguments(0, false, ByteOrder.LittleEndian)]
    [Arguments(1, true, ByteOrder.BigEndian)]
    [Arguments(0, false, ByteOrder.BigEndian)]
    public async Task ToBooleanSpanWithByteOrder(byte value, bool expected, ByteOrder byteOrder) => await Assert.That(BitConverter.ToBoolean([value], byteOrder)).IsEqualTo(expected);


    [Test]
    public async Task ToBooleanSpanWithSmallSpan()
    {
        await Assert.That(static () => BitConverter.ToBoolean(default)).Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    [Arguments('A', 0x00, 0x41)]
    [Arguments('﴾', 0xFD, 0x3E)]
    public async Task GetCharBytes(char value, byte first, byte second) => await Assert.That(BitConverter.GetBytes(value)).IsEquivalentTo(GetBytes(first, second, byteOrder: DefaultByteOrder));

    [Test]
    [Arguments('A', 0x00, 0x41, ByteOrder.LittleEndian)]
    [Arguments('﴾', 0xFD, 0x3E, ByteOrder.LittleEndian)]
    [Arguments('A', 0x00, 0x41, ByteOrder.BigEndian)]
    [Arguments('﴾', 0xFD, 0x3E, ByteOrder.BigEndian)]
    public async Task GetCharBytesWithByteOrder(char value, byte first, byte second, ByteOrder byteOrder) => await Assert.That(BitConverter.GetBytes(value, byteOrder)).IsEquivalentTo(GetBytes(first, second, byteOrder));

    [Test]
    [Arguments('A', 0x00, 0x41)]
    [Arguments('﴾', 0xFD, 0x3E)]
    public async Task TryWriteCharBytes(char value, byte first, byte second)
    {
        byte[] span = new byte[sizeof(char)];
        await Assert.That(BitConverter.TryWriteBytes(span, value)).IsTrue();
        await Assert.That(span).IsEquivalentTo(GetBytes(first, second, byteOrder: DefaultByteOrder));
    }

    [Test]
    [Arguments('A', 0x00, 0x41, ByteOrder.LittleEndian)]
    [Arguments('﴾', 0xFD, 0x3E, ByteOrder.LittleEndian)]
    [Arguments('A', 0x00, 0x41, ByteOrder.BigEndian)]
    [Arguments('﴾', 0xFD, 0x3E, ByteOrder.BigEndian)]
    public async Task TryWriteCharBytesWithByteOrder(char value, byte first, byte second, ByteOrder byteOrder)
    {
        byte[] span = new byte[sizeof(char)];
        await Assert.That(BitConverter.TryWriteBytes(span, value, byteOrder)).IsTrue();
        await Assert.That(span).IsEquivalentTo(GetBytes(first, second, byteOrder));
    }

    [Test]
    [Arguments(0x00, 0x41, 'A')]
    [Arguments(0xFD, 0x3E, '﴾')]
    public async Task ToCharBytes(byte first, byte second, char expected) => await Assert.That(BitConverter.ToChar(GetBytes(first, second, byteOrder: DefaultByteOrder), 0)).IsEqualTo(expected);

    [Test]
    [Arguments(0x00, 0x41, 'A', ByteOrder.LittleEndian)]
    [Arguments(0xFD, 0x3E, '﴾', ByteOrder.LittleEndian)]
    [Arguments(0x00, 0x41, 'A', ByteOrder.BigEndian)]
    [Arguments(0xFD, 0x3E, '﴾', ByteOrder.BigEndian)]
    public async Task ToCharBytesWithByteOrder(byte first, byte second, char expected, ByteOrder byteOrder) => await Assert.That(BitConverter.ToChar(GetBytes(first, second, byteOrder), 0, byteOrder)).IsEqualTo(expected);

    [Test]
    [Arguments(0x00, 0x41, 'A')]
    [Arguments(0xFD, 0x3E, '﴾')]
    public async Task ToCharSpan(byte first, byte second, char expected) => await Assert.That(BitConverter.ToChar(GetBytes(first, second, byteOrder: DefaultByteOrder))).IsEqualTo(expected);

    [Test]
    [Arguments(0x00, 0x41, 'A', ByteOrder.LittleEndian)]
    [Arguments(0xFD, 0x3E, '﴾', ByteOrder.LittleEndian)]
    [Arguments(0x00, 0x41, 'A', ByteOrder.BigEndian)]
    [Arguments(0xFD, 0x3E, '﴾', ByteOrder.BigEndian)]
    public async Task ToCharSpanWithByteOrder(byte first, byte second, char expected, ByteOrder byteOrder) => await Assert.That(BitConverter.ToChar(GetBytes(first, second, byteOrder), byteOrder)).IsEqualTo(expected);

    [Test]
    [Arguments(Int16MaxValue, PositiveByte, ByteMaxValue)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue)]
    public async Task GetInt16Bytes(short value, byte first, byte second) => await Assert.That(BitConverter.GetBytes(value)).IsEquivalentTo(GetBytes(first, second, byteOrder: DefaultByteOrder));

    [Test]
    [Arguments(Int16MaxValue, PositiveByte, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(Int16MaxValue, PositiveByte, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    public async Task GetInt16BytesWithByteOrder(short value, byte first, byte second, ByteOrder byteOrder) => await Assert.That(BitConverter.GetBytes(value, byteOrder)).IsEquivalentTo(GetBytes(first, second, byteOrder));

    [Test]
    [Arguments(Int16MaxValue, PositiveByte, ByteMaxValue)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue)]
    public async Task TryWriteInt16Bytes(short value, byte first, byte second)
    {
        byte[] span = new byte[sizeof(short)];
        await Assert.That(BitConverter.TryWriteBytes(span, value)).IsTrue();
        await Assert.That(span).IsEquivalentTo(GetBytes(first, second, byteOrder: DefaultByteOrder));
    }

    [Test]
    [Arguments(Int16MaxValue, PositiveByte, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(Int16MaxValue, PositiveByte, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    public async Task TryWriteInt16BytesWithByteOrder(short value, byte first, byte second, ByteOrder byteOrder)
    {
        byte[] span = new byte[sizeof(short)];
        await Assert.That(BitConverter.TryWriteBytes(span, value, byteOrder)).IsTrue();
        await Assert.That(span).IsEquivalentTo(GetBytes(first, second, byteOrder));
    }

    [Test]
    [Arguments(PositiveByte, ByteMaxValue, Int16MaxValue)]
    [Arguments(ByteMaxValue, ByteMaxValue, -1)]
    public async Task ToInt16Bytes(byte first, byte second, short expected) => await Assert.That(BitConverter.ToInt16(GetBytes(first, second, byteOrder: DefaultByteOrder), 0)).IsEqualTo(expected);

    [Test]
    [Arguments(PositiveByte, ByteMaxValue, Int16MaxValue, ByteOrder.LittleEndian)]
    [Arguments(ByteMaxValue, ByteMaxValue, -1, ByteOrder.LittleEndian)]
    [Arguments(PositiveByte, ByteMaxValue, Int16MaxValue, ByteOrder.BigEndian)]
    [Arguments(ByteMaxValue, ByteMaxValue, -1, ByteOrder.BigEndian)]
    public async Task ToInt16BytesWithByteOrder(byte first, byte second, short expected, ByteOrder byteOrder) => await Assert.That(BitConverter.ToInt16(GetBytes(first, second, byteOrder), 0, byteOrder)).IsEqualTo(expected);

    [Test]
    [Arguments(PositiveByte, ByteMaxValue, Int16MaxValue)]
    [Arguments(ByteMaxValue, ByteMaxValue, -1)]
    public async Task ToInt16Span(byte first, byte second, short expected) => await Assert.That(BitConverter.ToInt16(GetBytes(first, second, byteOrder: DefaultByteOrder))).IsEqualTo(expected);

    [Test]
    [Arguments(PositiveByte, ByteMaxValue, Int16MaxValue, ByteOrder.LittleEndian)]
    [Arguments(ByteMaxValue, ByteMaxValue, -1, ByteOrder.LittleEndian)]
    [Arguments(PositiveByte, ByteMaxValue, Int16MaxValue, ByteOrder.BigEndian)]
    [Arguments(ByteMaxValue, ByteMaxValue, -1, ByteOrder.BigEndian)]
    public async Task ToInt16SpanWithByteOrder(byte first, byte second, short expected, ByteOrder byteOrder) => await Assert.That(BitConverter.ToInt16(GetBytes(first, second, byteOrder), byteOrder)).IsEqualTo(expected);

    [Test]
    [Arguments(int.MaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    public async Task GetInt32Bytes(int value, byte first, byte second, byte third, byte forth) => await Assert.That(BitConverter.GetBytes(value)).IsEquivalentTo(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder));

    [Test]
    [Arguments(int.MaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(int.MaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    public async Task GetInt32BytesWithByteOrder(int value, byte first, byte second, byte third, byte forth, ByteOrder byteOrder) => await Assert.That(BitConverter.GetBytes(value, byteOrder)).IsEquivalentTo(GetBytes(first, second, third, forth, byteOrder));

    [Test]
    [Arguments(int.MaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    public async Task TryWriteInt32Bytes(int value, byte first, byte second, byte third, byte forth)
    {
        byte[] span = new byte[sizeof(int)];
        await Assert.That(BitConverter.TryWriteBytes(span, value)).IsTrue();
        await Assert.That(span).IsEquivalentTo(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder));
    }

    [Test]
    [Arguments(int.MaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(int.MaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    public async Task TryWriteInt32BytesWithByteOrder(int value, byte first, byte second, byte third, byte forth, ByteOrder byteOrder)
    {
        byte[] span = new byte[sizeof(int)];
        await Assert.That(BitConverter.TryWriteBytes(span, value, byteOrder)).IsTrue();
        await Assert.That(span).IsEquivalentTo(GetBytes(first, second, third, forth, byteOrder));
    }

    [Test]
    [Arguments(PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, int.MaxValue)]
    [Arguments(ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, -1)]
    public async Task ToInt32Bytes(byte first, byte second, byte third, byte forth, int expected) => await Assert.That(BitConverter.ToInt32(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder), 0)).IsEqualTo(expected);

    [Test]
    [Arguments(PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, int.MaxValue, ByteOrder.LittleEndian)]
    [Arguments(ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, -1, ByteOrder.LittleEndian)]
    [Arguments(PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, int.MaxValue, ByteOrder.BigEndian)]
    [Arguments(ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, -1, ByteOrder.BigEndian)]
    public async Task ToInt32BytesWithByteOrder(byte first, byte second, byte third, byte forth, int expected, ByteOrder byteOrder) => await Assert.That(BitConverter.ToInt32(GetBytes(first, second, third, forth, byteOrder), 0, byteOrder)).IsEqualTo(expected);

    [Test]
    [Arguments(PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, int.MaxValue)]
    [Arguments(ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, -1)]
    public async Task ToInt32Span(byte first, byte second, byte third, byte forth, int expected) => await Assert.That(BitConverter.ToInt32(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder))).IsEqualTo(expected);

    [Test]
    [Arguments(PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, int.MaxValue, ByteOrder.LittleEndian)]
    [Arguments(ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, -1, ByteOrder.LittleEndian)]
    [Arguments(PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, int.MaxValue, ByteOrder.BigEndian)]
    [Arguments(ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, -1, ByteOrder.BigEndian)]
    public async Task ToInt32SpanWithByteOrder(byte first, byte second, byte third, byte forth, int expected, ByteOrder byteOrder) => await Assert.That(BitConverter.ToInt32(GetBytes(first, second, third, forth, byteOrder), byteOrder)).IsEqualTo(expected);

    [Test]
    [Arguments(long.MaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    public async Task GetInt64Bytes(long value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth) => await Assert.That(BitConverter.GetBytes(value)).IsEquivalentTo(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder));

    [Test]
    [Arguments(long.MaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(long.MaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    public async Task GetInt64BytesWithByteOrder(long value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder) => await Assert.That(BitConverter.GetBytes(value, byteOrder)).IsEquivalentTo(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder));

    [Test]
    [Arguments(long.MaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    public async Task TryWriteInt64Bytes(long value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth)
    {
        byte[] span = new byte[sizeof(long)];
        await Assert.That(BitConverter.TryWriteBytes(span, value)).IsTrue();
        await Assert.That(span).IsEquivalentTo(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder));
    }

    [Test]
    [Arguments(long.MaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(long.MaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    public async Task TryWriteInt64BytesWithByteOrder(long value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder)
    {
        byte[] span = new byte[sizeof(long)];
        await Assert.That(BitConverter.TryWriteBytes(span, value, byteOrder)).IsTrue();
        await Assert.That(span).IsEquivalentTo(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder));
    }

    [Test]
    [Arguments(PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, long.MaxValue)]
    [Arguments(ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, -1)]
    public async Task ToInt64Bytes(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, long expected) => await Assert.That(BitConverter.ToInt64(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder), 0)).IsEqualTo(expected);

    [Test]
    [Arguments(PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, long.MaxValue, ByteOrder.LittleEndian)]
    [Arguments(ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, -1, ByteOrder.LittleEndian)]
    [Arguments(PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, long.MaxValue, ByteOrder.BigEndian)]
    [Arguments(ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, -1, ByteOrder.BigEndian)]
    public async Task ToInt64BytesWithByteOrder(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, long expected, ByteOrder byteOrder) => await Assert.That(BitConverter.ToInt64(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), 0, byteOrder)).IsEqualTo(expected);

    [Test]
    [Arguments(PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, long.MaxValue)]
    [Arguments(ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, -1)]
    public async Task ToInt64Span(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, long expected) => await Assert.That(BitConverter.ToInt64(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder))).IsEqualTo(expected);

    [Test]
    [Arguments(PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, long.MaxValue, ByteOrder.LittleEndian)]
    [Arguments(ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, -1, ByteOrder.LittleEndian)]
    [Arguments(PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, long.MaxValue, ByteOrder.BigEndian)]
    [Arguments(ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, -1, ByteOrder.BigEndian)]
    public async Task ToInt64SpanWithByteOrder(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, long expected, ByteOrder byteOrder) => await Assert.That(BitConverter.ToInt64(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), byteOrder)).IsEqualTo(expected);

    [Test]
    [Arguments(ushort.MaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(ushort.MaxValue / 2, PositiveByte, ByteMaxValue)]
    public async Task GetUInt16Bytes(ushort value, byte first, byte second) => await Assert.That(BitConverter.GetBytes(value)).IsEquivalentTo(GetBytes(first, second, byteOrder: DefaultByteOrder));

    [Test]
    [Arguments(ushort.MaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(ushort.MaxValue / 2, PositiveByte, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(ushort.MaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(ushort.MaxValue / 2, PositiveByte, ByteMaxValue, ByteOrder.BigEndian)]
    public async Task GetUInt16BytesWithByteOrder(ushort value, byte first, byte second, ByteOrder byteOrder) => await Assert.That(BitConverter.GetBytes(value, byteOrder)).IsEquivalentTo(GetBytes(first, second, byteOrder));

    [Test]
    [Arguments(ushort.MaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(ushort.MaxValue / 2, PositiveByte, ByteMaxValue)]
    public async Task TryWriteUInt16Bytes(ushort value, byte first, byte second)
    {
        byte[] span = new byte[sizeof(ushort)];
        await Assert.That(BitConverter.TryWriteBytes(span, value)).IsTrue();
        await Assert.That(span).IsEquivalentTo(GetBytes(first, second, byteOrder: DefaultByteOrder));
    }

    [Test]
    [Arguments(ushort.MaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(ushort.MaxValue / 2, PositiveByte, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(ushort.MaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(ushort.MaxValue / 2, PositiveByte, ByteMaxValue, ByteOrder.BigEndian)]
    public async Task TryWriteUInt16BytesWithByteOrder(ushort value, byte first, byte second, ByteOrder byteOrder)
    {
        byte[] span = new byte[sizeof(ushort)];
        await Assert.That(BitConverter.TryWriteBytes(span, value, byteOrder)).IsTrue();
        await Assert.That(span).IsEquivalentTo(GetBytes(first, second, byteOrder));
    }

    [Test]
    [Arguments(ByteMaxValue, ByteMaxValue, ushort.MaxValue)]
    [Arguments(PositiveByte, ByteMaxValue, ushort.MaxValue / 2)]
    public async Task ToUInt16Bytes(byte first, byte second, ushort expected) => await Assert.That(BitConverter.ToUInt16(GetBytes(first, second, byteOrder: DefaultByteOrder), 0)).IsEqualTo(expected);

    [Test]
    [Arguments(ByteMaxValue, ByteMaxValue, ushort.MaxValue, ByteOrder.LittleEndian)]
    [Arguments(PositiveByte, ByteMaxValue, ushort.MaxValue / 2, ByteOrder.LittleEndian)]
    [Arguments(ByteMaxValue, ByteMaxValue, ushort.MaxValue, ByteOrder.BigEndian)]
    [Arguments(PositiveByte, ByteMaxValue, ushort.MaxValue / 2, ByteOrder.BigEndian)]
    public async Task ToUInt16BytesWithByteOrder(byte first, byte second, ushort expected, ByteOrder byteOrder) => await Assert.That(BitConverter.ToUInt16(GetBytes(first, second, byteOrder), 0, byteOrder)).IsEqualTo(expected);

    [Test]
    [Arguments(ByteMaxValue, ByteMaxValue, ushort.MaxValue)]
    [Arguments(PositiveByte, ByteMaxValue, ushort.MaxValue / 2)]
    public async Task ToUInt16Span(byte first, byte second, ushort expected) => await Assert.That(BitConverter.ToUInt16(GetBytes(first, second, byteOrder: DefaultByteOrder))).IsEqualTo(expected);

    [Test]
    [Arguments(ByteMaxValue, ByteMaxValue, ushort.MaxValue, ByteOrder.LittleEndian)]
    [Arguments(PositiveByte, ByteMaxValue, ushort.MaxValue / 2, ByteOrder.LittleEndian)]
    [Arguments(ByteMaxValue, ByteMaxValue, ushort.MaxValue, ByteOrder.BigEndian)]
    [Arguments(PositiveByte, ByteMaxValue, ushort.MaxValue / 2, ByteOrder.BigEndian)]
    public async Task ToUInt16SpanWithByteOrder(byte first, byte second, ushort expected, ByteOrder byteOrder) => await Assert.That(BitConverter.ToUInt16(GetBytes(first, second, byteOrder), byteOrder)).IsEqualTo(expected);

    [Test]
    [Arguments(uint.MaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(uint.MaxValue / 2, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    public async Task GetUInt32Bytes(uint value, byte first, byte second, byte third, byte forth) => await Assert.That(BitConverter.GetBytes(value)).IsEquivalentTo(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder));

    [Test]
    [Arguments(uint.MaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(uint.MaxValue / 2, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(uint.MaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(uint.MaxValue / 2, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    public async Task GetUInt32BytesWithByteOrder(uint value, byte first, byte second, byte third, byte forth, ByteOrder byteOrder) => await Assert.That(BitConverter.GetBytes(value, byteOrder)).IsEquivalentTo(GetBytes(first, second, third, forth, byteOrder));

    [Test]
    [Arguments(uint.MaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(uint.MaxValue / 2, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    public async Task TryWriteUInt32Bytes(uint value, byte first, byte second, byte third, byte forth)
    {
        byte[] span = new byte[sizeof(uint)];
        await Assert.That(BitConverter.TryWriteBytes(span, value)).IsTrue();
        await Assert.That(span).IsEquivalentTo(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder));
    }

    [Test]
    [Arguments(uint.MaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(uint.MaxValue / 2, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(uint.MaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(uint.MaxValue / 2, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    public async Task TryWriteUInt32BytesWithByteOrder(uint value, byte first, byte second, byte third, byte forth, ByteOrder byteOrder)
    {
        byte[] span = new byte[sizeof(uint)];
        await Assert.That(BitConverter.TryWriteBytes(span, value, byteOrder)).IsTrue();
        await Assert.That(GetBytes(first, second, third, forth, byteOrder)).IsEquivalentTo(GetBytes(first, second, third, forth, byteOrder));
    }

    [Test]
    [Arguments(ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, uint.MaxValue)]
    [Arguments(PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, uint.MaxValue / 2)]
    public async Task ToUInt32Bytes(byte first, byte second, byte third, byte forth, uint expected) => await Assert.That(BitConverter.ToUInt32(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder), 0)).IsEqualTo(expected);

    [Test]
    [Arguments(ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, uint.MaxValue, ByteOrder.LittleEndian)]
    [Arguments(PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, uint.MaxValue / 2, ByteOrder.LittleEndian)]
    [Arguments(ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, uint.MaxValue, ByteOrder.BigEndian)]
    [Arguments(PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, uint.MaxValue / 2, ByteOrder.BigEndian)]
    public async Task ToUInt32BytesWithByteOrder(byte first, byte second, byte third, byte forth, uint expected, ByteOrder byteOrder) => await Assert.That(BitConverter.ToUInt32(GetBytes(first, second, third, forth, byteOrder), 0, byteOrder)).IsEqualTo(expected);

    [Test]
    [Arguments(ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, uint.MaxValue)]
    [Arguments(PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, uint.MaxValue / 2)]
    public async Task ToUInt32Span(byte first, byte second, byte third, byte forth, uint expected) => await Assert.That(BitConverter.ToUInt32(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder))).IsEqualTo(expected);

    [Test]
    [Arguments(ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, uint.MaxValue, ByteOrder.LittleEndian)]
    [Arguments(PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, uint.MaxValue / 2, ByteOrder.LittleEndian)]
    [Arguments(ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, uint.MaxValue, ByteOrder.BigEndian)]
    [Arguments(PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, uint.MaxValue / 2, ByteOrder.BigEndian)]
    public async Task ToUInt32SpanWithByteOrder(byte first, byte second, byte third, byte forth, uint expected, ByteOrder byteOrder) => await Assert.That(BitConverter.ToUInt32(GetBytes(first, second, third, forth, byteOrder), byteOrder)).IsEqualTo(expected);

    [Test]
    [Arguments(ulong.MaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(ulong.MaxValue / 2, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    public async Task GetUInt64Bytes(ulong value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth) => await Assert.That(BitConverter.GetBytes(value)).IsEquivalentTo(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder));

    [Test]
    [Arguments(ulong.MaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(ulong.MaxValue / 2, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(ulong.MaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(ulong.MaxValue / 2, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    public async Task GetUInt64BytesWithByteOrder(ulong value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder) => await Assert.That(BitConverter.GetBytes(value, byteOrder)).IsEquivalentTo(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder));

    [Test]
    [Arguments(ulong.MaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(ulong.MaxValue / 2, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    public async Task TryWriteUInt64Bytes(ulong value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth)
    {
        byte[] span = new byte[sizeof(ulong)];
        await Assert.That(BitConverter.TryWriteBytes(span, value)).IsTrue();
        await Assert.That(span).IsEquivalentTo(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder));
    }

    [Test]
    [Arguments(ulong.MaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(ulong.MaxValue / 2, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(ulong.MaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(ulong.MaxValue / 2, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    public async Task TryWriteUInt64BytesWithByteOrder(ulong value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder)
    {
        byte[] span = new byte[sizeof(ulong)];
        await Assert.That(BitConverter.TryWriteBytes(span, value, byteOrder)).IsTrue();
        await Assert.That(span).IsEquivalentTo(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder));
    }

    [Test]
    [Arguments(ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ulong.MaxValue)]
    [Arguments(PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ulong.MaxValue / 2)]
    public async Task ToUInt64Bytes(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ulong expected) => await Assert.That(BitConverter.ToUInt64(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder), 0)).IsEqualTo(expected);

    [Test]
    [Arguments(ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ulong.MaxValue, ByteOrder.LittleEndian)]
    [Arguments(PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ulong.MaxValue / 2, ByteOrder.LittleEndian)]
    [Arguments(ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ulong.MaxValue, ByteOrder.BigEndian)]
    [Arguments(PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ulong.MaxValue / 2, ByteOrder.BigEndian)]
    public async Task ToUInt64BytesWithByteOrder(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ulong expected, ByteOrder byteOrder) => await Assert.That(BitConverter.ToUInt64(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), 0, byteOrder)).IsEqualTo(expected);

    [Test]
    [Arguments(ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ulong.MaxValue)]
    [Arguments(PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ulong.MaxValue / 2)]
    public async Task ToUInt64Span(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ulong expected) => await Assert.That(BitConverter.ToUInt64(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder))).IsEqualTo(expected);

    [Test]
    [Arguments(ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ulong.MaxValue, ByteOrder.LittleEndian)]
    [Arguments(PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ulong.MaxValue / 2, ByteOrder.LittleEndian)]
    [Arguments(ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ulong.MaxValue, ByteOrder.BigEndian)]
    [Arguments(PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ulong.MaxValue / 2, ByteOrder.BigEndian)]
    public async Task ToUInt64SpanWithByteOrder(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ulong expected, ByteOrder byteOrder) => await Assert.That(BitConverter.ToUInt64(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), byteOrder)).IsEqualTo(expected);

#if NET5_0_OR_GREATER
    [Test]
    [Arguments(65504D, 0x7B, ByteMaxValue)]
    [Arguments(-65504D, 0xFB, ByteMaxValue)]
    public async Task GetHalfBytes(Half value, byte first, byte second) => await Assert.That(BitConverter.GetBytes(value)).IsEquivalentTo(GetBytes(first, second, byteOrder: DefaultByteOrder));

    [Test]
    [Arguments(65504D, 0x7B, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(-65504D, 0xFB, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(65504D, 0x7B, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(-65504D, 0xFB, ByteMaxValue, ByteOrder.BigEndian)]
    public async Task GetHalfBytesWithByteOrder(Half value, byte first, byte second, ByteOrder byteOrder) => await Assert.That(BitConverter.GetBytes(value, byteOrder)).IsEquivalentTo(GetBytes(first, second, byteOrder));

    [Test]
    [Arguments(65504D, 0x7B, ByteMaxValue)]
    [Arguments(-65504D, 0xFB, ByteMaxValue)]
    public async Task TryWriteHalfBytes(Half value, byte first, byte second)
    {
        byte[] span = new byte[sizeof(short)];
        await Assert.That(BitConverter.TryWriteBytes(span, value)).IsTrue();
        await Assert.That(span).IsEquivalentTo(GetBytes(first, second, byteOrder: DefaultByteOrder));
    }

    [Test]
    [Arguments(65504D, 0x7B, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(-65504D, 0xFB, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(65504D, 0x7B, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(-65504D, 0xFB, ByteMaxValue, ByteOrder.BigEndian)]
    public async Task TryWriteHalfBytesWithByteOrder(Half value, byte first, byte second, ByteOrder byteOrder)
    {
        byte[] span = new byte[sizeof(short)];
        await Assert.That(BitConverter.TryWriteBytes(span, value, byteOrder)).IsTrue();
        await Assert.That(span).IsEquivalentTo(GetBytes(first, second, byteOrder));
    }

    [Test]
    [Arguments(0x7B, ByteMaxValue, 65504D)]
    [Arguments(0xFB, ByteMaxValue, -65504D)]
    public async Task ToHalfBytes(byte first, byte second, Half expected) => await Assert.That(BitConverter.ToHalf(GetBytes(first, second, byteOrder: DefaultByteOrder), 0)).IsEqualTo(expected);

    [Test]
    [Arguments(0x7B, ByteMaxValue, 65504D, ByteOrder.LittleEndian)]
    [Arguments(0xFB, ByteMaxValue, -65504D, ByteOrder.LittleEndian)]
    [Arguments(0x7B, ByteMaxValue, 65504D, ByteOrder.BigEndian)]
    [Arguments(0xFB, ByteMaxValue, -65504D, ByteOrder.BigEndian)]
    public async Task ToHalfBytesWithByteOrder(byte first, byte second, Half expected, ByteOrder byteOrder) => await Assert.That(BitConverter.ToHalf(GetBytes(first, second, byteOrder), 0, byteOrder)).IsEqualTo(expected);

    [Test]
    [Arguments(0x7B, ByteMaxValue, 65504D)]
    [Arguments(0xFB, ByteMaxValue, -65504D)]
    public async Task ToHalfSpan(byte first, byte second, Half expected) => await Assert.That(BitConverter.ToHalf(GetBytes(first, second, byteOrder: DefaultByteOrder))).IsEqualTo(expected);

    [Test]
    [Arguments(0x7B, ByteMaxValue, 65504D, ByteOrder.LittleEndian)]
    [Arguments(0xFB, ByteMaxValue, -65504D, ByteOrder.LittleEndian)]
    [Arguments(0x7B, ByteMaxValue, 65504D, ByteOrder.BigEndian)]
    [Arguments(0xFB, ByteMaxValue, -65504D, ByteOrder.BigEndian)]
    public async Task ToHalfSpanWithByteOrder(byte first, byte second, Half expected, ByteOrder byteOrder) => await Assert.That(BitConverter.ToHalf(GetBytes(first, second, byteOrder), byteOrder)).IsEqualTo(expected);
#endif

    [Test]
    [Arguments(float.MaxValue, PositiveByte, PositiveByte, ByteMaxValue, ByteMaxValue)]
    [Arguments(float.MinValue, ByteMaxValue, PositiveByte, ByteMaxValue, ByteMaxValue)]
    public async Task GetSingleBytes(float value, byte first, byte second, byte third, byte forth) => await Assert.That(BitConverter.GetBytes(value)).IsEquivalentTo(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder));

    [Test]
    [Arguments(float.MaxValue, PositiveByte, PositiveByte, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(float.MinValue, ByteMaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(float.MaxValue, PositiveByte, PositiveByte, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(float.MinValue, ByteMaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    public async Task GetSingleBytesWithByteOrder(float value, byte first, byte second, byte third, byte forth, ByteOrder byteOrder) => await Assert.That(BitConverter.GetBytes(value, byteOrder)).IsEquivalentTo(GetBytes(first, second, third, forth, byteOrder));

    [Test]
    [Arguments(float.MaxValue, PositiveByte, PositiveByte, ByteMaxValue, ByteMaxValue)]
    [Arguments(float.MinValue, ByteMaxValue, PositiveByte, ByteMaxValue, ByteMaxValue)]
    public async Task TryWriteSingleBytes(float value, byte first, byte second, byte third, byte forth)
    {
        byte[] span = new byte[sizeof(float)];
        await Assert.That(BitConverter.TryWriteBytes(span, value)).IsTrue();
        await Assert.That(span).IsEquivalentTo(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder));
    }

    [Test]
    [Arguments(float.MaxValue, PositiveByte, PositiveByte, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(float.MinValue, ByteMaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(float.MaxValue, PositiveByte, PositiveByte, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(float.MinValue, ByteMaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    public async Task TryWriteSingleBytesWithByteOrder(float value, byte first, byte second, byte third, byte forth, ByteOrder byteOrder)
    {
        byte[] span = new byte[sizeof(float)];
        await Assert.That(BitConverter.TryWriteBytes(span, value, byteOrder)).IsTrue();
        await Assert.That(span).IsEquivalentTo(GetBytes(first, second, third, forth, byteOrder));
    }

    [Test]
    [Arguments(PositiveByte, PositiveByte, ByteMaxValue, ByteMaxValue, float.MaxValue)]
    [Arguments(ByteMaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, float.MinValue)]
    public async Task ToSingleBytes(byte first, byte second, byte third, byte forth, float expected) => await Assert.That(BitConverter.ToSingle(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder), 0)).IsEqualTo(expected);

    [Test]
    [Arguments(PositiveByte, PositiveByte, ByteMaxValue, ByteMaxValue, float.MaxValue, ByteOrder.LittleEndian)]
    [Arguments(ByteMaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, float.MinValue, ByteOrder.LittleEndian)]
    [Arguments(PositiveByte, PositiveByte, ByteMaxValue, ByteMaxValue, float.MaxValue, ByteOrder.BigEndian)]
    [Arguments(ByteMaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, float.MinValue, ByteOrder.BigEndian)]
    public async Task ToSingleBytesWithByteOrder(byte first, byte second, byte third, byte forth, float expected, ByteOrder byteOrder) => await Assert.That(BitConverter.ToSingle(GetBytes(first, second, third, forth, byteOrder), 0, byteOrder)).IsEqualTo(expected);

    [Test]
    [Arguments(PositiveByte, PositiveByte, ByteMaxValue, ByteMaxValue, float.MaxValue)]
    [Arguments(ByteMaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, float.MinValue)]
    public async Task ToSingleSpan(byte first, byte second, byte third, byte forth, float expected) => await Assert.That(BitConverter.ToSingle(GetBytes(first, second, third, forth, byteOrder: DefaultByteOrder))).IsEqualTo(expected);

    [Test]
    [Arguments(PositiveByte, PositiveByte, ByteMaxValue, ByteMaxValue, float.MaxValue, ByteOrder.LittleEndian)]
    [Arguments(ByteMaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, float.MinValue, ByteOrder.LittleEndian)]
    [Arguments(PositiveByte, PositiveByte, ByteMaxValue, ByteMaxValue, float.MaxValue, ByteOrder.BigEndian)]
    [Arguments(ByteMaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, float.MinValue, ByteOrder.BigEndian)]
    public async Task ToSingleSpanWithByteOrder(byte first, byte second, byte third, byte forth, float expected, ByteOrder byteOrder) => await Assert.That(BitConverter.ToSingle(GetBytes(first, second, third, forth, byteOrder), byteOrder)).IsEqualTo(expected);

    [Test]
    [Arguments(double.MaxValue, PositiveByte, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(double.MinValue, ByteMaxValue, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    public async Task GetDoubleBytes(double value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth) => await Assert.That(BitConverter.GetBytes(value)).IsEquivalentTo(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder));

    [Test]
    [Arguments(double.MaxValue, PositiveByte, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(double.MinValue, ByteMaxValue, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(double.MaxValue, PositiveByte, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(double.MinValue, ByteMaxValue, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    public async Task GetDoubleBytesWithByteOrder(double value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder) => await Assert.That(BitConverter.GetBytes(value, byteOrder)).IsEquivalentTo(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder));

    [Test]
    [Arguments(double.MaxValue, PositiveByte, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    [Arguments(double.MinValue, ByteMaxValue, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue)]
    public async Task TryWriteDoubleBytes(double value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth)
    {
        byte[] span = new byte[sizeof(double)];
        await Assert.That(BitConverter.TryWriteBytes(span, value)).IsTrue();
        await Assert.That(span).IsEquivalentTo(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder));
    }

    [Test]
    [Arguments(double.MaxValue, PositiveByte, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(double.MinValue, ByteMaxValue, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(double.MaxValue, PositiveByte, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(double.MinValue, ByteMaxValue, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    public async Task TryWriteDoubleBytesWithByteOrder(double value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder)
    {
        byte[] span = new byte[sizeof(double)];
        await Assert.That(BitConverter.TryWriteBytes(span, value, byteOrder)).IsTrue();
        await Assert.That(span).IsEquivalentTo(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder));
    }

    [Test]
    [Arguments(PositiveByte, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, double.MaxValue)]
    [Arguments(ByteMaxValue, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, double.MinValue)]
    public async Task ToDoubleBytes(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, double expected) => await Assert.That(BitConverter.ToDouble(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder), 0)).IsEqualTo(expected);

    [Test]
    [Arguments(PositiveByte, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, double.MaxValue, ByteOrder.LittleEndian)]
    [Arguments(ByteMaxValue, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, double.MinValue, ByteOrder.LittleEndian)]
    [Arguments(PositiveByte, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, double.MaxValue, ByteOrder.BigEndian)]
    [Arguments(ByteMaxValue, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, double.MinValue, ByteOrder.BigEndian)]
    public async Task ToDoubleBytesWithByteOrder(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, double expected, ByteOrder byteOrder) => await Assert.That(BitConverter.ToDouble(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), 0, byteOrder)).IsEqualTo(expected);

    [Test]
    [Arguments(PositiveByte, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, double.MaxValue)]
    [Arguments(ByteMaxValue, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, double.MinValue)]
    public async Task ToDoubleSpan(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, double expected) => await Assert.That(BitConverter.ToDouble(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder: DefaultByteOrder))).IsEqualTo(expected);

    [Test]
    [Arguments(PositiveByte, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, double.MaxValue, ByteOrder.LittleEndian)]
    [Arguments(ByteMaxValue, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, double.MinValue, ByteOrder.LittleEndian)]
    [Arguments(PositiveByte, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, double.MaxValue, ByteOrder.BigEndian)]
    [Arguments(ByteMaxValue, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, double.MinValue, ByteOrder.BigEndian)]
    public async Task ToDoubleSpanWithByteOrder(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, double expected, ByteOrder byteOrder) => await Assert.That(BitConverter.ToDouble(GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), byteOrder)).IsEqualTo(expected);

    [Test]
    public async Task ToStringBytes() => await Assert.That(BitConverter.ToString(GetBytes(0x54, 0x65, 0x78, 0x74, byteOrder: DefaultByteOrder))).IsEqualTo("74-78-65-54");

    [Test]
    public async Task ToStringBytesWithStart() => await Assert.That(BitConverter.ToString(GetBytes(0x54, 0x65, 0x78, 0x74, byteOrder: DefaultByteOrder), 0)).IsEqualTo("74-78-65-54");

    [Test]
    public async Task ToStringBytesWithStartAndLength() => await Assert.That(BitConverter.ToString(GetBytes(0x54, 0x65, 0x78, 0x74, byteOrder: DefaultByteOrder), 0, 4)).IsEqualTo("74-78-65-54");

#if NET5_0_OR_GREATER
    [Test]
    [Arguments(2.0, (Int16MaxValue / 2) + 1)]
    public async Task HalfToInt16(Half input, short expected) => await Assert.That(BitConverter.HalfToInt16Bits(input)).IsEqualTo(expected);

    [Test]
    [Arguments(2.0, (ushort.MaxValue / 4) + 1)]
    public async Task HalfToUInt16(Half input, ushort expected) => await Assert.That(BitConverter.HalfToUInt16Bits(input)).IsEqualTo(expected);
#endif

    [Test]
    [Arguments(2.0, (int.MaxValue / 2) + 1)]
    public async Task SingleToInt32(float input, int expected) => await Assert.That(BitConverter.SingleToInt32Bits(input)).IsEqualTo(expected);

    [Test]
    [Arguments(2.0, (uint.MaxValue / 4) + 1)]
    public async Task SingleToUInt32(float input, uint expected) => await Assert.That(BitConverter.SingleToUInt32Bits(input)).IsEqualTo(expected);

    [Test]
    [Arguments(2.0, (long.MaxValue / 2) + 1)]
    public async Task DoubleToInt64(double input, long expected) => await Assert.That(BitConverter.DoubleToInt64Bits(input)).IsEqualTo(expected);

    [Test]
    [Arguments(2.0, (ulong.MaxValue / 4) + 1)]
    public async Task DoubleToUInt64(double input, ulong expected) => await Assert.That(BitConverter.DoubleToUInt64Bits(input)).IsEqualTo(expected);

#if NET5_0_OR_GREATER
    [Test]
    [Arguments((Int16MaxValue / 2) + 1, 2.0)]
    public async Task Int16ToHalf(short input, Half expected) => await Assert.That(BitConverter.Int16BitsToHalf(input)).IsEqualTo(expected);

    [Test]
    [Arguments((ushort.MaxValue / 4) + 1, 2.0)]
    public async Task UInt16ToHalf(ushort input, Half expected) => await Assert.That(BitConverter.UInt16BitsToHalf(input)).IsEqualTo(expected);
#endif

    [Test]
    [Arguments((int.MaxValue / 2) + 1, 2.0)]
    public async Task Int32ToSingle(int input, float expected) => await Assert.That(BitConverter.Int32BitsToSingle(input)).IsEqualTo(expected);

    [Test]
    [Arguments((uint.MaxValue / 4) + 1, 2.0)]
    public async Task UInt32ToSingle(uint input, float expected) => await Assert.That(BitConverter.UInt32BitsToSingle(input)).IsEqualTo(expected);

    [Test]
    [Arguments((long.MaxValue / 2) + 1, 2.0)]
    public async Task Int64ToDouble(long input, double expected) => await Assert.That(BitConverter.Int64BitsToDouble(input)).IsEqualTo(expected);

    [Test]
    [Arguments((ulong.MaxValue / 4) + 1, 2.0)]
    public async Task UInt64ToDouble(ulong input, double expected) => await Assert.That(BitConverter.UInt64BitsToDouble(input)).IsEqualTo(expected);

    private static byte[] GetBytes(byte first, byte second, ByteOrder byteOrder) => byteOrder == ByteOrder.LittleEndian ? [second, first] : [first, second];

    private static byte[] GetBytes(byte first, byte second, byte third, byte forth, ByteOrder byteOrder) => byteOrder == ByteOrder.LittleEndian ? [forth, third, second, first] : [first, second, third, forth];

    private static byte[] GetBytes(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder) => byteOrder == ByteOrder.LittleEndian ? [eighth, seventh, sixth, fifth, forth, third, second, first] : [first, second, third, forth, fifth, sixth, seventh, eighth];

    private static bool ShouldSwap(ByteOrder byteOrder) => BitConverter.IsLittleEndian
        ? byteOrder is ByteOrder.LittleEndian
        : byteOrder is ByteOrder.BigEndian;
}