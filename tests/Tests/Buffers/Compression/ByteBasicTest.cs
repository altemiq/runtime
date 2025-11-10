namespace Altemiq.Buffers.Compression;

public class ByteBasicTest
{
    private static readonly IEnumerable<Func<ISByteCodec>> Codecs = [
        () => new VariableByte(),
        () => new Differential.DifferentialVariableByte(),
    ];

    [Test]
    [MethodDataSource(nameof(Data))]
    public async Task SaulTest(ByteIntegerCodec codec)
    {
        var c = codec.Codec;
        for (var x = 0; x < 50 * 4; ++x)
        {
            int[] data = [2, 3, 4, 5];
            var compressed = new sbyte[90 * 4];
            var decompressed = new int[data.Length];

            var dataOffset = 0;
            var compressedOffset = x;
            c.Compress(data, ref dataOffset, compressed, ref compressedOffset, data.Length);
            var len = compressedOffset - x;

            compressedOffset = x;
            var decompressedOffset = 0;
            c.Decompress(compressed, ref compressedOffset, decompressed, ref decompressedOffset, len);
            await Assert.That(decompressed).HasSameSequenceAs(data);
        }
    }

    [Test]
    [MethodDataSource(nameof(Data))]
    public async Task VaryingLengthTest(ByteIntegerCodec codec)
    {
        const int N = 4096;
        var data = System.Linq.Enumerable.Range(0, N).ToArray();

        var c = codec.Codec;
        for (var l = 1; l <= 128; l++)
        {
            var comp = TestUtils.Compress(c, TestUtils.CopyArray(data, l));
            var answer = TestUtils.Decompress(c, comp, l);
            await Assert.That(answer).HasSameSequenceAs(data.Take(l));
        }

        for (var l = 128; l <= N; l *= 2)
        {
            var comp = TestUtils.Compress(c, TestUtils.CopyArray(data, l));
            var answer = TestUtils.Decompress(c, comp, l);
            await Assert.That(answer).HasSameSequenceAs(data.Take(l));
        }
    }

    [Test]
    [MethodDataSource(nameof(Data))]
    public async Task VaryingLengthTest2(ByteIntegerCodec codec)
    {
        const int N = 128;
        var data = new int[N];
        data[127] = -1;
        var c = codec.Codec;

        for (var l = 1; l <= 128; l++)
        {
            var comp = TestUtils.Compress(c, TestUtils.CopyArray(data, l));
            var answer = TestUtils.Decompress(c, comp, l);
            await Assert.That(answer).HasSameSequenceAs(data.Take(l));
        }

        for (var l = 128; l <= N; l *= 2)
        {
            var comp = TestUtils.Compress(c, TestUtils.CopyArray(data, l));
            var answer = TestUtils.Decompress(c, comp, l);
            await Assert.That(answer).HasSameSequenceAs(data.Take(l));
        }
    }

    public static IEnumerable<Func<ByteIntegerCodec>> Data()
    {
        return Codecs.Select(c => new Func<ByteIntegerCodec>(() => new() { Codec = c() }));
    }

    public class ByteIntegerCodec
    {
        internal ISByteCodec Codec { get; init; } = null!;

        public override string? ToString()
        {
            return Codec.ToString();
        }
    }
}