// -----------------------------------------------------------------------
// <copyright file="BinaryReaderExtensionsTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO;

public class BinaryReaderExtensionsTests
{
    private const byte PositiveByte = 0x7F;

    private const byte FloatByte = 0xEF;

    [Fact]
    public void ReadString()
    {
        const string value = "This is a string";
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(value));
        stream.Position = 0;

        using var reader = new BinaryReader(stream);
        Assert.Equal(value, reader.ReadString(value.Length));
    }

    [Theory]
    [InlineData(127, ByteOrder.LittleEndian)]
    [InlineData(127, ByteOrder.BigEndian)]
    public void ReadByte(byte value, ByteOrder byteOrder) => TestValue(BinaryReaderExtensions.ReadByte, byteOrder, [value], value);

    [Theory]
    [InlineData('A', 0x00, 0x41, ByteOrder.LittleEndian, null)]
    [InlineData('A', 0x41, 0x00, ByteOrder.BigEndian, null)]
    [InlineData('A', 0x00, 0x41, ByteOrder.LittleEndian, "unicode")]
    [InlineData('﴾', 0xFD, 0x3E, ByteOrder.LittleEndian, "unicode")]
    [InlineData('A', 0x00, 0x41, ByteOrder.BigEndian, "unicode")]
    [InlineData('﴾', 0xFD, 0x3E, ByteOrder.BigEndian, "unicode")]
    public void ReadChar(char value, byte first, byte second, ByteOrder byteOrder, string? encoding) => TestValue(BinaryReaderExtensions.ReadChar, byteOrder, GetBytes(first, second, byteOrder), value, encoding);

    [Theory]
    [InlineData(short.MaxValue, PositiveByte, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(short.MaxValue, PositiveByte, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void ReadInt16(short value, byte first, byte second, ByteOrder byteOrder) => TestValue(BinaryReaderExtensions.ReadInt16, byteOrder, GetBytes(first, second, byteOrder), value);

    [Theory]
    [InlineData(int.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(int.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void ReadInt32(int value, byte first, byte second, byte third, byte forth, ByteOrder byteOrder) => TestValue(BinaryReaderExtensions.ReadInt32, byteOrder, GetBytes(first, second, third, forth, byteOrder), value);

    [Theory]
    [InlineData(long.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(long.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(-1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void ReadInt64(long value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder) => TestValue(BinaryReaderExtensions.ReadInt64, byteOrder, GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), value);

    [Theory]
    [InlineData(ushort.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(ushort.MaxValue / 2, PositiveByte, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(ushort.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(ushort.MaxValue / 2, PositiveByte, byte.MaxValue, ByteOrder.BigEndian)]
    public void ReadUInt16(ushort value, byte first, byte second, ByteOrder byteOrder) => TestValue(BinaryReaderExtensions.ReadUInt16, byteOrder, GetBytes(first, second, byteOrder), value);

    [Theory]
    [InlineData(uint.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(uint.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(uint.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(uint.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void ReadUInt32(uint value, byte first, byte second, byte third, byte forth, ByteOrder byteOrder) => TestValue(BinaryReaderExtensions.ReadUInt32, byteOrder, GetBytes(first, second, third, forth, byteOrder), value);

    [Theory]
    [InlineData(ulong.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(ulong.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(ulong.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(ulong.MaxValue / 2, PositiveByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void ReadUInt64(ulong value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder) => TestValue(BinaryReaderExtensions.ReadUInt64, byteOrder, GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), value);

#if NET5_0_OR_GREATER
    [Theory]
    [InlineData(65504D, 0x7B, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(-65504D, 0xFB, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(65504D, 0x7B, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(-65504D, 0xFB, byte.MaxValue, ByteOrder.BigEndian)]
    public void ReadHalf(Half value, byte first, byte second, ByteOrder byteOrder) => TestValue(BinaryReaderExtensions.ReadHalf, byteOrder, GetBytes(first, second, byteOrder), (Half)value);
#endif

    [Theory]
    [InlineData(float.MaxValue, PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(float.MinValue, byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(float.MaxValue, PositiveByte, PositiveByte, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(float.MinValue, byte.MaxValue, PositiveByte, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void ReadSingle(float value, byte first, byte second, byte third, byte forth, ByteOrder byteOrder) => TestValue(BinaryReaderExtensions.ReadSingle, byteOrder, GetBytes(first, second, third, forth, byteOrder), value);

    [Theory]
    [InlineData(double.MaxValue, PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(double.MinValue, byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.LittleEndian)]
    [InlineData(double.MaxValue, PositiveByte, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    [InlineData(double.MinValue, byte.MaxValue, FloatByte, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, ByteOrder.BigEndian)]
    public void ReadDouble(double value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder) => TestValue(BinaryReaderExtensions.ReadDouble, byteOrder, GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), value);

    private static void TestValue<T>(Func<BinaryReader, ByteOrder, T> getValue, ByteOrder byteOrder, byte[] buffer, T expected, string? encoding = default)
    {
        using var stream = new MemoryStream(buffer);
        stream.Position = 0;

        var reader = encoding is { } e
            ? new BinaryReader(stream, System.Text.Encoding.GetEncoding(e))
            : new BinaryReader(stream);
        using (reader)
        {
            Assert.Equal(expected, getValue(reader, byteOrder));
        }
    }

    private static byte[] GetBytes(byte first, byte second, ByteOrder byteOrder) => byteOrder == ByteOrder.LittleEndian ? [second, first] : [first, second];

    private static byte[] GetBytes(byte first, byte second, byte third, byte forth, ByteOrder byteOrder) => byteOrder == ByteOrder.LittleEndian ? [forth, third, second, first] : [first, second, third, forth];

    private static byte[] GetBytes(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder) => byteOrder == ByteOrder.LittleEndian ? [eighth, seventh, sixth, fifth, forth, third, second, first] : [first, second, third, forth, fifth, sixth, seventh, eighth];
}