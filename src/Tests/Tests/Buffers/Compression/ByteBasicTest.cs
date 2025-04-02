namespace Altemiq.Buffers.Compression;

public class ByteBasicTest
{
    private static readonly IEnumerable<Func<ISByteCodec>> codecs = [
        () => new VariableByte(),
        () => new Differential.DifferentialVariableByte(),
    ];

    [Test]
    [MethodDataSource(nameof(Data))]
    public async Task SaulTest(ByteIntegerCodec codec)
    {
        var C = codec.Codec!;
        for (var x = 0; x < 50 * 4; ++x)
        {
            int[] a = [2, 3, 4, 5];
            var b = new sbyte[90 * 4];
            var c = new int[a.Length];

            var aOffset = 0;
            var bOffset = x;
            C.Compress(a, ref aOffset, b, ref bOffset, a.Length);
            var len = bOffset - x;

            bOffset = x;
            var cOffset = 0;
            C.Decompress(b, ref bOffset, c, ref cOffset, len);
            await Assert.That(c).IsEquivalentTo(a);
        }
    }

    [Test]
    [MethodDataSource(nameof(Data))]
    public async Task VaryingLengthTest(ByteIntegerCodec codec)
    {
        const int N = 4096;
        var data = System.Linq.Enumerable.Range(0, N).ToArray();

        var c = codec.Codec!;
        for (var l = 1; l <= 128; l++)
        {
            var comp = TestUtils.Compress(c, TestUtils.CopyArray(data, l));
            var answer = TestUtils.Uncompress(c, comp, l);
            await Assert.That(answer).IsEquivalentTo(data.Take(l));
        }

        for (var l = 128; l <= N; l *= 2)
        {
            var comp = TestUtils.Compress(c, TestUtils.CopyArray(data, l));
            var answer = TestUtils.Uncompress(c, comp, l);
            await Assert.That(answer).IsEquivalentTo(data.Take(l));
        }
    }

    [Test]
    [MethodDataSource(nameof(Data))]
    public async Task VaryingLengthTest2(ByteIntegerCodec codec)
    {
        const int N = 128;
        var data = new int[N];
        data[127] = -1;
        var c = codec.Codec!;

        for (var l = 1; l <= 128; l++)
        {
            var comp = TestUtils.Compress(c, TestUtils.CopyArray(data, l));
            var answer = TestUtils.Uncompress(c, comp, l);
            await Assert.That(answer).IsEquivalentTo(data.Take(l));
        }

        for (var l = 128; l <= N; l *= 2)
        {
            var comp = TestUtils.Compress(c, TestUtils.CopyArray(data, l));
            var answer = TestUtils.Uncompress(c, comp, l);
            await Assert.That(answer).IsEquivalentTo(data.Take(l));
        }
    }

    public static IEnumerable<Func<ByteIntegerCodec>> Data()
    {
        return codecs.Select(c => new Func<ByteIntegerCodec>(() => new() { Codec = c() }));
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