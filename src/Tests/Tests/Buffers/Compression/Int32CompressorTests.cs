namespace Altemiq.Buffers.Compression;

public class Int32CompressorTests
{
    private static readonly IEnumerable<Func<Int32Compressor>> IntegerCompressors =
        [
            () => new Int32Compressor<VariableByte>(),
            () => new Int32Compressor<HeadlessComposition<BinaryPacking, VariableByte>>()
        ];

    public static IEnumerable<Func<IntegerCompressor>> Data()
    {
        return IntegerCompressors.Select(i => new Func<IntegerCompressor>(() => new IntegerCompressor { Compressor = i() }));
    }

    [Test]
    [MethodDataSource(nameof(Data))]
    public async Task BasicTest(IntegerCompressor compressor)
    {
        var i = compressor.Compressor;

        for (var n = 1; n <= 10000; n *= 10)
        {
            var orig = new int[n];
            for (var k = 0; k < n; k++)
            {
                orig[k] = 3 * k + 5;
            }

            var comp = i.Compress(orig);
            var back = i.Decompress(comp);

            await Assert.That(back).IsEquivalentTo(orig);
        }
    }

    public sealed class IntegerCompressor
    {
        internal Int32Compressor Compressor { get; init; } = null!;

        public override string? ToString()
        {
            return Compressor.ToString();
        }
    }
}