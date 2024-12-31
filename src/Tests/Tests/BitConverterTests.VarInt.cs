namespace Altemiq;

public partial class BitConverterTests
{
    public class VarInt
    {
        public class Array
        {
            [Theory]
            [InlineData(sbyte.MaxValue)]
            [InlineData(sbyte.MinValue)]
            [InlineData(default(sbyte))]
            [InlineData(sbyte.MaxValue / 2)]
            public void EncodeAndDecodeSByte(sbyte number) => EncodeAndDecode(number, BitConverter.GetVarBytes, BitConverter.ToSByte);

            [Theory]
            [InlineData(byte.MaxValue)]
            [InlineData(byte.MinValue)]
            [InlineData(byte.MaxValue / 2)]
            public void EncodeAndDecodeByte(byte number) => EncodeAndDecode(number, BitConverter.GetVarBytes, BitConverter.ToByte);

            [Theory]
            [InlineData(short.MaxValue)]
            [InlineData(short.MinValue)]
            [InlineData(default(short))]
            [InlineData(short.MaxValue / 2)]
            public void EncodeAndDecodeInt16(short number) => EncodeAndDecode(number, BitConverter.GetVarBytes, BitConverter.ToInt16);

            [Theory]
            [InlineData(ushort.MaxValue)]
            [InlineData(ushort.MinValue)]
            [InlineData(ushort.MaxValue / 2)]
            public void EncodeAndDecodeUInt16(ushort number) => EncodeAndDecode(number, BitConverter.GetVarBytes, BitConverter.ToUInt16);

            [Theory]
            [InlineData(int.MaxValue)]
            [InlineData(int.MinValue)]
            [InlineData(default(int))]
            [InlineData(int.MaxValue / 2)]
            public void EncodeAndDecodeInt32(int number) => EncodeAndDecode(number, BitConverter.GetVarBytes, BitConverter.ToInt32);

            [Theory]
            [InlineData(uint.MaxValue)]
            [InlineData(uint.MinValue)]
            [InlineData(uint.MaxValue / 2)]
            public void EncodeAndDecodeUInt32(uint number) => EncodeAndDecode(number, BitConverter.GetVarBytes, BitConverter.ToUInt32);

            [Theory]
            [InlineData(long.MaxValue)]
            [InlineData(long.MinValue)]
            [InlineData(default(long))]
            [InlineData(long.MaxValue / 2)]
            public void EncodeAndDecodeInt64(long number) => EncodeAndDecode(number, BitConverter.GetVarBytes, BitConverter.ToInt64);

            [Theory]
            [InlineData(ulong.MaxValue)]
            [InlineData(ulong.MinValue)]
            [InlineData(ulong.MaxValue / 2)]
            public void EncodeAndDecodeUInt64(ulong number) => EncodeAndDecode(number, BitConverter.GetVarBytes, BitConverter.ToUInt64);

#if NET7_0_OR_GREATER
            [Theory]
            [MemberData(nameof(GetInt128Data), MemberType = typeof(VarInt))]
            public void EncodeAndDecodeInt128(Int128 number) => EncodeAndDecode(number, BitConverter.GetVarBytes, BitConverter.ToInt128);

            [Theory]
            [MemberData(nameof(GetUInt128Data), MemberType = typeof(VarInt))]
            public void EncodeAndDecodeUInt128(UInt128 number) => EncodeAndDecode(number, BitConverter.GetVarBytes, BitConverter.ToUInt128);
#endif

            [Fact]
            public void ToSmall()
            {
                var encoded = BitConverter.GetVarBytes(ulong.MaxValue / 2);
                System.Array.Resize(ref encoded, encoded.Length / 2);
                var action = () => BitConverter.ToUInt64(encoded, 0, out var bytesRead);
                action.Should().Throw<ArgumentException>();
            }

            [Fact]
            public void ToBig()
            {
                const ulong Number = ulong.MaxValue / 2;
                var encoded = BitConverter.GetVarBytes(Number);
                var length = encoded.Length;
                System.Array.Resize(ref encoded, length * 2);
                var decoded = BitConverter.ToUInt64(encoded, 0, out var bytesRead);
                decoded.Should().Be(Number);
                bytesRead.Should().Be(length);
            }

            private static void EncodeAndDecode<T>(
                T number,
                GetVarBytes<T> encode,
                ToValue<T> decode)
            {
                var encoded = encode(number);
                var decoded = decode(encoded, 0, out var bytesRead);
                decoded.Should().Be(number);
                bytesRead.Should().Be(encoded.Length);
            }

            private delegate byte[] GetVarBytes<T>(T value);

            private delegate T ToValue<T>(byte[] bytes, int startIndex, out int bytesRead);
        }

        public class Span
        {
            [Theory]
            [InlineData(sbyte.MaxValue)]
            [InlineData(sbyte.MinValue)]
            [InlineData(default(sbyte))]
            [InlineData(sbyte.MaxValue / 2)]
            public void EncodeAndDecodeSByte(sbyte number) => EncodeAndDecode(number, BitConverter.TryWriteBytes, BitConverter.ToSByte);

            [Theory]
            [InlineData(byte.MaxValue)]
            [InlineData(byte.MinValue)]
            [InlineData(byte.MaxValue / 2)]
            public void EncodeAndDecodeByte(byte number) => EncodeAndDecode(number, BitConverter.TryWriteBytes, BitConverter.ToByte);

            [Theory]
            [InlineData(short.MaxValue)]
            [InlineData(short.MinValue)]
            [InlineData(default(short))]
            [InlineData(short.MaxValue / 2)]
            public void EncodeAndDecodeInt16(short number) => EncodeAndDecode(number, BitConverter.TryWriteBytes, BitConverter.ToInt16);

            [Theory]
            [InlineData(ushort.MaxValue)]
            [InlineData(ushort.MinValue)]
            [InlineData(ushort.MaxValue / 2)]
            public void EncodeAndDecodeUInt16(ushort number) => EncodeAndDecode(number, BitConverter.TryWriteBytes, BitConverter.ToUInt16);

            [Theory]
            [InlineData(int.MaxValue)]
            [InlineData(int.MinValue)]
            [InlineData(default(int))]
            [InlineData(int.MaxValue / 2)]
            public void EncodeAndDecodeInt32(int number) => EncodeAndDecode(number, BitConverter.TryWriteBytes, BitConverter.ToInt32);

            [Theory]
            [InlineData(uint.MaxValue)]
            [InlineData(uint.MinValue)]
            [InlineData(uint.MaxValue / 2)]
            public void EncodeAndDecodeUInt32(uint number) => EncodeAndDecode(number, BitConverter.TryWriteBytes, BitConverter.ToUInt32);

            [Theory]
            [InlineData(long.MaxValue)]
            [InlineData(long.MinValue)]
            [InlineData(default(long))]
            [InlineData(long.MaxValue / 2)]
            public void EncodeAndDecodeInt64(long number) => EncodeAndDecode(number, BitConverter.TryWriteBytes, BitConverter.ToInt64);

            [Theory]
            [InlineData(ulong.MaxValue)]
            [InlineData(ulong.MinValue)]
            [InlineData(ulong.MaxValue / 2)]
            public void EncodeAndDecodeUInt64(ulong number) => EncodeAndDecode(number, BitConverter.TryWriteBytes, BitConverter.ToUInt64);

#if NET7_0_OR_GREATER
            [Theory]
            [MemberData(nameof(GetInt128Data), MemberType = typeof(VarInt))]
            public void EncodeAndDecodeInt128(Int128 number) => EncodeAndDecode(number, BitConverter.TryWriteBytes, BitConverter.ToInt128);

            [Theory]
            [MemberData(nameof(GetUInt128Data), MemberType = typeof(VarInt))]
            public void EncodeAndDecodeUInt128(UInt128 number) => EncodeAndDecode(number, BitConverter.TryWriteBytes, BitConverter.ToUInt128);
#endif

            [Fact]
            public void ToSmall()
            {
                var encoded = BitConverter.GetVarBytes(ulong.MaxValue / 2);
                var action = () => BitConverter.ToUInt64(encoded.AsSpan(0, encoded.Length / 2), out var bytesRead);
                action.Should().Throw<ArgumentException>();
            }

            [Fact]
            public void ToBig()
            {
                const ulong Number = ulong.MaxValue / 2;
                var encoded = BitConverter.GetVarBytes(Number);
                var length = encoded.Length;
                System.Array.Resize(ref encoded, length * 2);
                var decoded = BitConverter.ToUInt64(encoded.AsSpan(), out var bytesRead);
                decoded.Should().Be(Number);
                bytesRead.Should().Be(length);
            }
            private void EncodeAndDecode<T>(
                T number,
                TryWriteBytes<T> encode,
                ToValue<T> decode)
            {
                Span<byte> encoded = stackalloc byte[20];
                encode(encoded, number, out var bytesWritten).Should().BeTrue();
                var decoded = decode(encoded[..bytesWritten], out var bytesRead);
                decoded.Should().Be(number);
                bytesRead.Should().Be(bytesWritten);
            }

            private delegate bool TryWriteBytes<T>(Span<byte> destination, T value, out int bytesWritten);

            private delegate T ToValue<T>(ReadOnlySpan<byte> source, out int bytesRead);
        }

#if NET7_0_OR_GREATER
        public static TheoryData<Int128> GetInt128Data() =>
        [
            Int128.MinValue,
            Int128.MaxValue,
            default(Int128),
            Int128.MaxValue / 2,
        ];

        public static TheoryData<UInt128> GetUInt128Data() =>
        [
            UInt128.MinValue,
            UInt128.MaxValue,
            UInt128.MaxValue / 2,
        ];
#endif
    }
}