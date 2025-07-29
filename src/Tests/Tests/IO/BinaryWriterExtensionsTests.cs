// -----------------------------------------------------------------------
// <copyright file="BinaryWriterExtensionsTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO;

public class BinaryWriterExtensionsTests
{
    private const int PositiveByte = 0x7F;

    private const int FloatByte = 0xEF;

    private const int Int16MaxValue = short.MaxValue;

    private const int ByteMaxValue = byte.MaxValue;

    [Test]
    [Arguments(127, ByteOrder.LittleEndian)]
    [Arguments(127, ByteOrder.BigEndian)]
    public Task WriteByte(byte value, ByteOrder byteOrder) => TestValue(BinaryWriterExtensions.Write, byteOrder, [value], value);

    [Test]
    [Arguments(127, ByteOrder.LittleEndian)]
    [Arguments(127, ByteOrder.BigEndian)]
    public Task WriteBytes(byte value, ByteOrder byteOrder) => TestValue(BinaryWriterExtensions.Write, byteOrder, [value], new[] { value });

    [Test]
    [Arguments('A', 0x00, 0x41, ByteOrder.LittleEndian, null)]
    [Arguments('A', 0x41, 0x00, ByteOrder.BigEndian, null)]
    [Arguments('A', 0x00, 0x41, ByteOrder.LittleEndian, "unicode")]
    [Arguments('﴾', 0xFD, 0x3E, ByteOrder.LittleEndian, "unicode")]
    [Arguments('A', 0x00, 0x41, ByteOrder.BigEndian, "unicode")]
    [Arguments('﴾', 0xFD, 0x3E, ByteOrder.BigEndian, "unicode")]
    public Task WriteChar(char value, byte first, byte second, ByteOrder byteOrder, string? encoding) => TestValue(BinaryWriterExtensions.Write, byteOrder, GetBytes(first, second, byteOrder), value, encoding);

    [Test]
    [Arguments('A', 0x00, 0x41, ByteOrder.LittleEndian, null)]
    [Arguments('A', 0x41, 0x00, ByteOrder.BigEndian, null)]
    [Arguments('A', 0x00, 0x41, ByteOrder.LittleEndian, "unicode")]
    [Arguments('﴾', 0xFD, 0x3E, ByteOrder.LittleEndian, "unicode")]
    [Arguments('A', 0x00, 0x41, ByteOrder.BigEndian, "unicode")]
    [Arguments('﴾', 0xFD, 0x3E, ByteOrder.BigEndian, "unicode")]
    public Task WriteChars(char value, byte first, byte second, ByteOrder byteOrder, string? encoding) => TestValue(BinaryWriterExtensions.Write, byteOrder, GetBytes(first, second, byteOrder), new[] { value }, encoding);

    [Test]
    [Arguments(Int16MaxValue, PositiveByte, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(Int16MaxValue, PositiveByte, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    public Task WriteInt16(short value, byte first, byte second, ByteOrder byteOrder) => TestValue(BinaryWriterExtensions.Write, byteOrder, GetBytes(first, second, byteOrder), value);

    [Test]
    [Arguments(int.MaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(int.MaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    public Task WriteInt32(int value, byte first, byte second, byte third, byte forth, ByteOrder byteOrder) => TestValue(BinaryWriterExtensions.Write, byteOrder, GetBytes(first, second, third, forth, byteOrder), value);

    [Test]
    [Arguments(long.MaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(long.MaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    public Task WriteInt64(long value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder) => TestValue(BinaryWriterExtensions.Write, byteOrder, GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), value);

    [Test]
    [Arguments(-80, ByteOrder.LittleEndian)]
    [Arguments(-80, ByteOrder.BigEndian)]
    public Task WriteSByte(sbyte value, ByteOrder byteOrder) => TestValue(BinaryWriterExtensions.Write, byteOrder, [(byte)value], value);

    [Test]
    [Arguments(ushort.MaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(ushort.MaxValue / 2, PositiveByte, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(ushort.MaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(ushort.MaxValue / 2, PositiveByte, ByteMaxValue, ByteOrder.BigEndian)]
    public Task WriteUInt16(ushort value, byte first, byte second, ByteOrder byteOrder) => TestValue(BinaryWriterExtensions.Write, byteOrder, GetBytes(first, second, byteOrder), value);

    [Test]
    [Arguments(uint.MaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(uint.MaxValue / 2, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(uint.MaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(uint.MaxValue / 2, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    public Task WriteUInt32(uint value, byte first, byte second, byte third, byte forth, ByteOrder byteOrder) => TestValue(BinaryWriterExtensions.Write, byteOrder, GetBytes(first, second, third, forth, byteOrder), value);

    [Test]
    [Arguments(ulong.MaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(ulong.MaxValue / 2, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(ulong.MaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(ulong.MaxValue / 2, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    public Task WriteUInt64(ulong value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder) => TestValue(BinaryWriterExtensions.Write, byteOrder, GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), value);

#if NET5_0_OR_GREATER
    [Test]
    [Arguments(65504D, 0x7B, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(-65504D, 0xFB, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(65504D, 0x7B, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(-65504D, 0xFB, ByteMaxValue, ByteOrder.BigEndian)]
    public Task WriteHalf(Half value, byte first, byte second, ByteOrder byteOrder) => TestValue(BinaryWriterExtensions.Write, byteOrder, GetBytes(first, second, byteOrder), value);
#endif

    [Test]
    [Arguments(float.MaxValue, PositiveByte, PositiveByte, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(float.MinValue, ByteMaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(float.MaxValue, PositiveByte, PositiveByte, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(float.MinValue, ByteMaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    public Task WriteSingle(float value, byte first, byte second, byte third, byte forth, ByteOrder byteOrder) => TestValue(BinaryWriterExtensions.Write, byteOrder, GetBytes(first, second, third, forth, byteOrder), value);

    [Test]
    [Arguments(double.MaxValue, PositiveByte, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(double.MinValue, ByteMaxValue, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(double.MaxValue, PositiveByte, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(double.MinValue, ByteMaxValue, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    public Task WriteDouble(double value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder) => TestValue(BinaryWriterExtensions.Write, byteOrder, GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), value);

    private static async Task TestValue<T>(Action<BinaryWriter, T, ByteOrder> writeValue, ByteOrder byteOrder, byte[] buffer, T expected, string? encoding = default)
    {
        using var stream = new MemoryStream();

        var writer = encoding is { } e
            ? new BinaryWriter(stream, System.Text.Encoding.GetEncoding(e), true)
            : new BinaryWriter(stream, System.Text.Encoding.UTF8, true);
#if NET
        await
#endif
        using (writer)
        {
            writeValue(writer, expected, byteOrder);
        }

        stream.Position = 0;

        var streamArray = stream.ToArray();
        if (streamArray.Length < buffer.Length)
        {
            Array.Resize(ref streamArray, buffer.Length);
        }

        await Assert.That(streamArray).IsEquivalentTo(buffer);
    }

    private static byte[] GetBytes(byte first, byte second, ByteOrder byteOrder)
    {
        return byteOrder switch
        {
            ByteOrder.LittleEndian => [second, first],
            _ => [first, second],
        };
    }

    private static byte[] GetBytes(byte first, byte second, byte third, byte forth, ByteOrder byteOrder)
    {
        return byteOrder switch
        {
            ByteOrder.LittleEndian => [forth, third, second, first],
            _ => [first, second, third, forth],
        };
    }

    private static byte[] GetBytes(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder)
    {
        return byteOrder switch
        {
            ByteOrder.LittleEndian => [eighth, seventh, sixth, fifth, forth, third, second, first],
            _ => [first, second, third, forth, fifth, sixth, seventh, eighth],
        };
    }
}