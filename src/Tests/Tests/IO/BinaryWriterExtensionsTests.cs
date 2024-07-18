// -----------------------------------------------------------------------
// <copyright file="BinaryWriterExtensionsTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO;

public class BinaryWriterExtensionsTests
{
    private const byte PositiveByte = 0x7F;

    private const byte FloatByte = 0xEF;

    [Theory]
    [InlineData(127, ByteOrder.LittleEndian)]
    [InlineData(127, ByteOrder.BigEndian)]
    public void WriteByte(byte value, ByteOrder byteOrder) => TestValue(BinaryWriterExtensions.Write, byteOrder, [value], value);

    [Theory]
    [InlineData(127, ByteOrder.LittleEndian)]
    [InlineData(127, ByteOrder.BigEndian)]
    public void WriteBytes(byte value, ByteOrder byteOrder) => TestValue(BinaryWriterExtensions.Write, byteOrder, [value], new byte[] { value });

    [Theory]
    [InlineData('A', 0x00, 0x41, ByteOrder.LittleEndian, null)]
    [InlineData('A', 0x41, 0x00, ByteOrder.BigEndian, null)]
    [InlineData('A', 0x00, 0x41, ByteOrder.LittleEndian, "unicode")]
    [InlineData('﴾', 0xFD, 0x3E, ByteOrder.LittleEndian, "unicode")]
    [InlineData('A', 0x00, 0x41, ByteOrder.BigEndian, "unicode")]
    [InlineData('﴾', 0xFD, 0x3E, ByteOrder.BigEndian, "unicode")]
    public void WriteChar(char value, byte first, byte second, ByteOrder byteOrder, string? encoding) => TestValue(BinaryWriterExtensions.Write, byteOrder, GetBytes(first, second, byteOrder), value, encoding);

    [Theory]
    [InlineData('A', 0x00, 0x41, ByteOrder.LittleEndian, null)]
    [InlineData('A', 0x41, 0x00, ByteOrder.BigEndian, null)]
    [InlineData('A', 0x00, 0x41, ByteOrder.LittleEndian, "unicode")]
    [InlineData('﴾', 0xFD, 0x3E, ByteOrder.LittleEndian, "unicode")]
    [InlineData('A', 0x00, 0x41, ByteOrder.BigEndian, "unicode")]
    [InlineData('﴾', 0xFD, 0x3E, ByteOrder.BigEndian, "unicode")]
    public void WriteChars(char value, byte first, byte second, ByteOrder byteOrder, string? encoding) => TestValue(BinaryWriterExtensions.Write, byteOrder, GetBytes(first, second, byteOrder), new[] { value }, encoding);

    [Theory]
    [InlineData(short.MaxValue, PositiveByte, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(short.MaxValue, PositiveByte, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void WriteInt16(short value, byte first, byte second, ByteOrder byteOrder) => TestValue(BinaryWriterExtensions.Write, byteOrder, GetBytes(first, second, byteOrder), value);

    [Theory]
    [InlineData(int.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(int.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void WriteInt32(int value, byte first, byte second, byte third, byte forth, ByteOrder byteOrder) => TestValue(BinaryWriterExtensions.Write, byteOrder, GetBytes(first, second, third, forth, byteOrder), value);

    [Theory]
    [InlineData(long.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(long.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void WriteInt64(long value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder) => TestValue(BinaryWriterExtensions.Write, byteOrder, GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), value);

    [Theory]
    [InlineData(-80, ByteOrder.LittleEndian)]
    [InlineData(-80, ByteOrder.BigEndian)]
    public void WriteSByte(sbyte value, ByteOrder byteOrder) => TestValue(BinaryWriterExtensions.Write, byteOrder, [(byte)value], value);

    [Theory]
    [InlineData(ushort.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(ushort.MaxValue / 2, PositiveByte, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(ushort.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(ushort.MaxValue / 2, PositiveByte, byte.MaxValue, ByteOrder.BigEndian)]
    public void WriteUInt16(ushort value, byte first, byte second, ByteOrder byteOrder) => TestValue(BinaryWriterExtensions.Write, byteOrder, GetBytes(first, second, byteOrder), value);

    [Theory]
    [InlineData(uint.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(uint.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(uint.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(uint.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void WriteUInt32(uint value, byte first, byte second, byte third, byte forth, ByteOrder byteOrder) => TestValue(BinaryWriterExtensions.Write, byteOrder, GetBytes(first, second, third, forth, byteOrder), value);

    [Theory]
    [InlineData(ulong.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(ulong.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(ulong.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(ulong.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void WriteUInt64(ulong value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder) => TestValue(BinaryWriterExtensions.Write, byteOrder, GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), value);

#if NET5_0_OR_GREATER
    [Theory]
    [InlineData(65504D, 0x7B, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(-65504D, 0xFB, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(65504D, 0x7B, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(-65504D, 0xFB, byte.MaxValue, ByteOrder.BigEndian)]
    public void WriteHalf(SerializableHalf value, byte first, byte second, ByteOrder byteOrder) => TestValue(BinaryWriterExtensions.Write, byteOrder, GetBytes(first, second, byteOrder), (Half)value);
#endif

    [Theory]
    [InlineData(float.MaxValue, PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(float.MinValue, byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(float.MaxValue, PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(float.MinValue, byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void WriteSingle(float value, byte first, byte second, byte third, byte forth, ByteOrder byteOrder) => TestValue(BinaryWriterExtensions.Write, byteOrder, GetBytes(first, second, third, forth, byteOrder), value);

    [Theory]
    [InlineData(double.MaxValue, PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(double.MinValue, byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(double.MaxValue, PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(double.MinValue, byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void WriteDouble(double value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder) => TestValue(BinaryWriterExtensions.Write, byteOrder, GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), value);

    private static void TestValue<T>(Action<BinaryWriter, T, ByteOrder> writeValue, ByteOrder byteOrder, byte[] buffer, T expected, string? encoding = default)
    {
        using var stream = new MemoryStream();

        var writer = encoding is { } e
            ? new BinaryWriter(stream, System.Text.Encoding.GetEncoding(e), true)
            : new BinaryWriter(stream, System.Text.Encoding.UTF8, true);
        using (writer)
        {
            writeValue(writer, expected, byteOrder);
        }

        stream.Position = 0;

        var streamArray = stream.ToArray();
        if (streamArray.Length < buffer.Length)
        {
            System.Array.Resize(ref streamArray, buffer.Length);
        }

        streamArray.ToArray().Should().BeEquivalentTo(buffer);
    }

    private static byte[] GetBytes(byte first, byte second, ByteOrder byteOrder) => byteOrder == ByteOrder.LittleEndian ? [second, first] : [first, second];

    private static byte[] GetBytes(byte first, byte second, byte third, byte forth, ByteOrder byteOrder) => byteOrder == ByteOrder.LittleEndian ? [forth, third, second, first] : [first, second, third, forth];

    private static byte[] GetBytes(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder) => byteOrder == ByteOrder.LittleEndian ? [eighth, seventh, sixth, fifth, forth, third, second, first] : [first, second, third, forth, fifth, sixth, seventh, eighth];
}