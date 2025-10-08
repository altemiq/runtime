// -----------------------------------------------------------------------
// <copyright file="BinaryReaderExtensionsTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO;

public class BinaryReaderExtensionsTests
{
    private const int PositiveByte = 0x7F;

    private const int FloatByte = 0xEF;

    private const int Int16MaxValue = short.MaxValue;

    private const int ByteMaxValue = byte.MaxValue;

    [Test]
    public async Task ReadString()
    {
        const string Value = "This is a string";
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(Value));
        stream.Position = 0;

        using var reader = new BinaryReader(stream);
        await Assert.That(reader.ReadString(Value.Length)).IsEqualTo(Value);
    }

    [Test]
    [Arguments(127, ByteOrder.LittleEndian)]
    [Arguments(127, ByteOrder.BigEndian)]
    public Task ReadByte(byte value, ByteOrder byteOrder) => TestValue(BinaryReaderExtensions.ReadByte, byteOrder, [value], value);

    [Test]
    [Arguments('A', 0x00, 0x41, ByteOrder.LittleEndian, null)]
    [Arguments('A', 0x41, 0x00, ByteOrder.BigEndian, null)]
    [Arguments('A', 0x00, 0x41, ByteOrder.LittleEndian, "unicode")]
    [Arguments('﴾', 0xFD, 0x3E, ByteOrder.LittleEndian, "unicode")]
    [Arguments('A', 0x00, 0x41, ByteOrder.BigEndian, "unicode")]
    [Arguments('﴾', 0xFD, 0x3E, ByteOrder.BigEndian, "unicode")]
    public Task ReadChar(char value, byte first, byte second, ByteOrder byteOrder, string? encoding) => TestValue(BinaryReaderExtensions.ReadChar, byteOrder, GetBytes(first, second, byteOrder), value, encoding);

    [Test]
    [Arguments(Int16MaxValue, PositiveByte, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(Int16MaxValue, PositiveByte, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    public Task ReadInt16(short value, byte first, byte second, ByteOrder byteOrder) => TestValue(BinaryReaderExtensions.ReadInt16, byteOrder, GetBytes(first, second, byteOrder), value);

    [Test]
    [Arguments(int.MaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(int.MaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    public Task ReadInt32(int value, byte first, byte second, byte third, byte forth, ByteOrder byteOrder) => TestValue(BinaryReaderExtensions.ReadInt32, byteOrder, GetBytes(first, second, third, forth, byteOrder), value);

    [Test]
    [Arguments(long.MaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(long.MaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(-1, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    public Task ReadInt64(long value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder) => TestValue(BinaryReaderExtensions.ReadInt64, byteOrder, GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), value);

    [Test]
    [Arguments(ushort.MaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(ushort.MaxValue / 2, PositiveByte, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(ushort.MaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(ushort.MaxValue / 2, PositiveByte, ByteMaxValue, ByteOrder.BigEndian)]
    public Task ReadUInt16(ushort value, byte first, byte second, ByteOrder byteOrder) => TestValue(BinaryReaderExtensions.ReadUInt16, byteOrder, GetBytes(first, second, byteOrder), value);

    [Test]
    [Arguments(uint.MaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(uint.MaxValue / 2, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(uint.MaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(uint.MaxValue / 2, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    public Task ReadUInt32(uint value, byte first, byte second, byte third, byte forth, ByteOrder byteOrder) => TestValue(BinaryReaderExtensions.ReadUInt32, byteOrder, GetBytes(first, second, third, forth, byteOrder), value);

    [Test]
    [Arguments(ulong.MaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(ulong.MaxValue / 2, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(ulong.MaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(ulong.MaxValue / 2, PositiveByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    public Task ReadUInt64(ulong value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder) => TestValue(BinaryReaderExtensions.ReadUInt64, byteOrder, GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), value);

#if NET5_0_OR_GREATER
    [Test]
    [Arguments(65504D, 0x7B, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(-65504D, 0xFB, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(65504D, 0x7B, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(-65504D, 0xFB, ByteMaxValue, ByteOrder.BigEndian)]
    public Task ReadHalf(Half value, byte first, byte second, ByteOrder byteOrder) => TestValue(BinaryReaderExtensions.ReadHalf, byteOrder, GetBytes(first, second, byteOrder), value);
#endif

    [Test]
    [Arguments(float.MaxValue, PositiveByte, PositiveByte, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(float.MinValue, ByteMaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(float.MaxValue, PositiveByte, PositiveByte, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(float.MinValue, ByteMaxValue, PositiveByte, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    public Task ReadSingle(float value, byte first, byte second, byte third, byte forth, ByteOrder byteOrder) => TestValue(BinaryReaderExtensions.ReadSingle, byteOrder, GetBytes(first, second, third, forth, byteOrder), value);

    [Test]
    [Arguments(double.MaxValue, PositiveByte, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(double.MinValue, ByteMaxValue, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.LittleEndian)]
    [Arguments(double.MaxValue, PositiveByte, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    [Arguments(double.MinValue, ByteMaxValue, FloatByte, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteMaxValue, ByteOrder.BigEndian)]
    public Task ReadDouble(double value, byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder) => TestValue(BinaryReaderExtensions.ReadDouble, byteOrder, GetBytes(first, second, third, forth, fifth, sixth, seventh, eighth, byteOrder), value);

    private static async Task TestValue<T>(Func<BinaryReader, ByteOrder, T> getValue, ByteOrder byteOrder, byte[] buffer, T expected, string? encoding = default)
    {
        using var stream = new MemoryStream(buffer);
        stream.Position = 0;

        var reader = encoding is { } e
            ? new BinaryReader(stream, System.Text.Encoding.GetEncoding(e))
            : new BinaryReader(stream);
        using (reader)
        {
            await Assert.That(getValue(reader, byteOrder)).IsEqualTo(expected);
        }
    }

    private static byte[] GetBytes(byte first, byte second, ByteOrder byteOrder) => byteOrder == ByteOrder.LittleEndian ? [second, first] : [first, second];

    private static byte[] GetBytes(byte first, byte second, byte third, byte forth, ByteOrder byteOrder) => byteOrder == ByteOrder.LittleEndian ? [forth, third, second, first] : [first, second, third, forth];

    private static byte[] GetBytes(byte first, byte second, byte third, byte forth, byte fifth, byte sixth, byte seventh, byte eighth, ByteOrder byteOrder) => byteOrder == ByteOrder.LittleEndian ? [eighth, seventh, sixth, fifth, forth, third, second, first] : [first, second, third, forth, fifth, sixth, seventh, eighth];
}