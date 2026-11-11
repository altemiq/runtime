namespace Altemiq;

public partial class BitConverterTests
{
    public class VarInt
    {
        public class Array
        {
            [Test]
            [Arguments(sbyte.MaxValue)]
            [Arguments(sbyte.MinValue)]
            [Arguments(default(sbyte))]
            [Arguments(sbyte.MaxValue / 2)]
            public async Task EncodeAndDecodeSByte(sbyte number)
            {
                await EncodeAndDecode(number, BitConverter.GetVarBytes, BitConverter.ToSByte);
            }

            [Test]
            [Arguments(byte.MaxValue)]
            [Arguments(byte.MinValue)]
            [Arguments(byte.MaxValue / 2)]
            public async Task EncodeAndDecodeByte(byte number)
            {
                await EncodeAndDecode(number, BitConverter.GetVarBytes, BitConverter.ToByte);
            }

            [Test]
            [Arguments(short.MaxValue)]
            [Arguments(short.MinValue)]
            [Arguments(default(short))]
            [Arguments(short.MaxValue / 2)]
            public async Task EncodeAndDecodeInt16(short number)
            {
                await EncodeAndDecode(number, BitConverter.GetVarBytes, BitConverter.ToInt16);
            }

            [Test]
            [Arguments(ushort.MaxValue)]
            [Arguments(ushort.MinValue)]
            [Arguments(ushort.MaxValue / 2)]
            public async Task EncodeAndDecodeUInt16(ushort number)
            {
                await EncodeAndDecode(number, BitConverter.GetVarBytes, BitConverter.ToUInt16);
            }

            [Test]
            [Arguments(int.MaxValue)]
            [Arguments(int.MinValue)]
            [Arguments(default(int))]
            [Arguments(int.MaxValue / 2)]
            public async Task EncodeAndDecodeInt32(int number)
            {
                await EncodeAndDecode(number, BitConverter.GetVarBytes, BitConverter.ToInt32);
            }

            [Test]
            [Arguments(uint.MaxValue)]
            [Arguments(uint.MinValue)]
            [Arguments(uint.MaxValue / 2)]
            public async Task EncodeAndDecodeUInt32(uint number)
            {
                await EncodeAndDecode(number, BitConverter.GetVarBytes, BitConverter.ToUInt32);
            }

            [Test]
            [Arguments(long.MaxValue)]
            [Arguments(long.MinValue)]
            [Arguments(default(long))]
            [Arguments(long.MaxValue / 2)]
            public async Task EncodeAndDecodeInt64(long number)
            {
                await EncodeAndDecode(number, BitConverter.GetVarBytes, BitConverter.ToInt64);
            }

            [Test]
            [Arguments(ulong.MaxValue)]
            [Arguments(ulong.MinValue)]
            [Arguments(ulong.MaxValue / 2)]
            public async Task EncodeAndDecodeUInt64(ulong number)
            {
                await EncodeAndDecode(number, BitConverter.GetVarBytes, BitConverter.ToUInt64);
            }

#if NET7_0_OR_GREATER
            [Test]
            [MethodDataSource(typeof(VarInt), nameof(GetInt128Data))]
            public async Task EncodeAndDecodeInt128(Int128 number)
            {
                await EncodeAndDecode(number, BitConverter.GetVarBytes, BitConverter.ToInt128);
            }

            [Test]
            [MethodDataSource(typeof(VarInt), nameof(GetUInt128Data))]
            public async Task EncodeAndDecodeUInt128(UInt128 number)
            {
                await EncodeAndDecode(number, BitConverter.GetVarBytes, BitConverter.ToUInt128);
            }
#endif

            [Test]
            public async Task ToSmall()
            {
                var encoded = BitConverter.GetVarBytes(ulong.MaxValue / 2);
                System.Array.Resize(ref encoded, encoded.Length / 2);
                _ = await Assert.That(() => BitConverter.ToUInt64(encoded, 0, out _)).Throws<ArgumentException>();
            }

            [Test]
            public async Task ToBig()
            {
                const ulong Number = ulong.MaxValue / 2;
                var encoded = BitConverter.GetVarBytes(Number);
                var length = encoded.Length;
                System.Array.Resize(ref encoded, length * 2);
                var decoded = BitConverter.ToUInt64(encoded, 0, out var bytesRead);
                _ = await Assert.That(decoded).IsEqualTo(Number);
                _ = await Assert.That(bytesRead).IsEqualTo(length);
            }

            private static async Task EncodeAndDecode<T>(
                T number,
                GetVarBytes<T> encode,
                ToValue<T> decode)
            {
                var encoded = encode(number);
                var decoded = decode(encoded, 0, out var bytesRead);
                _ = await Assert.That(decoded).IsEqualTo(number);
                _ = await Assert.That(bytesRead).IsEqualTo(encoded.Length);
            }

            private delegate byte[] GetVarBytes<in T>(T value);

            private delegate T ToValue<out T>(byte[] bytes, int startIndex, out int bytesRead);
        }

        public class Span
        {
            [Test]
            [Arguments(sbyte.MaxValue)]
            [Arguments(sbyte.MinValue)]
            [Arguments(default(short))]
            [Arguments(sbyte.MaxValue / 2)]
            public async Task EncodeAndDecodeSByte(sbyte number)
            {
                await EncodeAndDecode(number, BitConverter.TryWriteBytes, BitConverter.ToSByte);
            }

            [Test]
            [Arguments(byte.MaxValue)]
            [Arguments(byte.MinValue)]
            [Arguments(byte.MaxValue / 2)]
            public async Task EncodeAndDecodeByte(byte number)
            {
                await EncodeAndDecode(number, BitConverter.TryWriteBytes, BitConverter.ToByte);
            }

            [Test]
            [Arguments(short.MaxValue)]
            [Arguments(short.MinValue)]
            [Arguments(default(short))]
            [Arguments(short.MaxValue / 2)]
            public async Task EncodeAndDecodeInt16(short number)
            {
                await EncodeAndDecode(number, BitConverter.TryWriteBytes, BitConverter.ToInt16);
            }

            [Test]
            [Arguments(ushort.MaxValue)]
            [Arguments(ushort.MinValue)]
            [Arguments(ushort.MaxValue / 2)]
            public async Task EncodeAndDecodeUInt16(ushort number)
            {
                await EncodeAndDecode(number, BitConverter.TryWriteBytes, BitConverter.ToUInt16);
            }

            [Test]
            [Arguments(int.MaxValue)]
            [Arguments(int.MinValue)]
            [Arguments(default(int))]
            [Arguments(int.MaxValue / 2)]
            public async Task EncodeAndDecodeInt32(int number)
            {
                await EncodeAndDecode(number, BitConverter.TryWriteBytes, BitConverter.ToInt32);
            }

            [Test]
            [Arguments(uint.MaxValue)]
            [Arguments(uint.MinValue)]
            [Arguments(uint.MaxValue / 2)]
            public async Task EncodeAndDecodeUInt32(uint number)
            {
                await EncodeAndDecode(number, BitConverter.TryWriteBytes, BitConverter.ToUInt32);
            }

            [Test]
            [Arguments(long.MaxValue)]
            [Arguments(long.MinValue)]
            [Arguments(default(long))]
            [Arguments(long.MaxValue / 2)]
            public async Task EncodeAndDecodeInt64(long number)
            {
                await EncodeAndDecode(number, BitConverter.TryWriteBytes, BitConverter.ToInt64);
            }

            [Test]
            [Arguments(ulong.MaxValue)]
            [Arguments(ulong.MinValue)]
            [Arguments(ulong.MaxValue / 2)]
            public async Task EncodeAndDecodeUInt64(ulong number)
            {
                await EncodeAndDecode(number, BitConverter.TryWriteBytes, BitConverter.ToUInt64);
            }

#if NET7_0_OR_GREATER
            [Test]
            [MethodDataSource(typeof(VarInt), nameof(GetInt128Data))]
            public async Task EncodeAndDecodeInt128(Int128 number)
            {
                await EncodeAndDecode(number, BitConverter.TryWriteBytes, BitConverter.ToInt128);
            }

            [Test]
            [MethodDataSource(typeof(VarInt), nameof(GetUInt128Data))]
            public async Task EncodeAndDecodeUInt128(UInt128 number)
            {
                await EncodeAndDecode(number, BitConverter.TryWriteBytes, BitConverter.ToUInt128);
            }
#endif

            [Test]
            public async Task ToSmall()
            {
                var encoded = BitConverter.GetVarBytes(ulong.MaxValue / 2);
                _ = await Assert.That(() => BitConverter.ToUInt64(encoded.AsSpan(0, encoded.Length / 2), out _)).Throws<ArgumentException>();
            }

            [Test]
            public async Task ToBig()
            {
                const ulong Number = ulong.MaxValue / 2;
                var encoded = BitConverter.GetVarBytes(Number);
                var length = encoded.Length;
                System.Array.Resize(ref encoded, length * 2);
                var decoded = BitConverter.ToUInt64(encoded.AsSpan(), out var bytesRead);
                _ = await Assert.That(decoded).IsEqualTo(Number);
                _ = await Assert.That(bytesRead).IsEqualTo(length);
            }

            private static async Task EncodeAndDecode<T>(
                T number,
                TryWriteBytes<T> encode,
                ToValue<T> decode)
            {
                var encoded = new byte[20];
                _ = await Assert.That(encode(encoded, number, out var bytesWritten)).IsTrue();
                _ = await Assert.That(decode(encoded.AsSpan(0, bytesWritten), out var bytesRead)).IsEqualTo(number);
                _ = await Assert.That(bytesRead).IsEqualTo(bytesWritten);
            }

            private delegate bool TryWriteBytes<in T>(Span<byte> destination, T value, out int bytesWritten);

            private delegate T ToValue<out T>(ReadOnlySpan<byte> source, out int bytesRead);
        }

#if NET7_0_OR_GREATER
        public static IEnumerable<Func<Int128>> GetInt128Data()
        {
            yield return static () => Int128.MinValue;
            yield return static () => Int128.MaxValue;
            yield return static () => default;
            yield return static () => Int128.MaxValue / 2;
        }

        public static IEnumerable<Func<UInt128>> GetUInt128Data()
        {
            yield return static () => UInt128.MinValue;
            yield return static () => UInt128.MaxValue;
            yield return static () => UInt128.MaxValue / 2;
        }
#endif
    }
}