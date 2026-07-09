namespace Altemiq.Buffers.Compression.Differential;

public class DifferentialInt32CompressorTests
{
    [Test]
    public async Task SuperSimpleExample()
    {
        var iic2 = new DifferentialInt32Compressor();
        var data = System.Buffers.ArrayPool<int>.Shared.Rent(ushort.MaxValue);
        _ = await Assert.That(iic2.Decompress(iic2.Compress(data))).HasSameSequenceAs(data);
        System.Buffers.ArrayPool<int>.Shared.Return(data);
    }

#if !NETFRAMEWORK
    public static IEnumerable<Wrapper> GetCompressors()
    {
        yield return new(new DifferentialInt32Compressor(new DifferentialVariableByte()));
        yield return new(new DifferentialInt32Compressor(new HeadlessDifferentialComposition(new DifferentialBinaryPacking(), new DifferentialVariableByte())));
    }

    [Test]
    [MatrixDataSource]
    public async Task BasicIntegratedTest([Matrix(1, 10, 100, 1000, 10000)] int n, [MatrixMethod<DifferentialInt32CompressorTests>(nameof(GetCompressors))] Wrapper i)
    {
        var orig = new int[n];
        for (var k = 0; k < n; k++)
        {
            orig[k] = (3 * k) + 5;
        }

        _ = await Assert.That(i.Decompress(i.Compress(orig))).HasSameSequenceAs(orig);
    }

    public sealed class Wrapper
    {
        private readonly DifferentialInt32Compressor compressor;

        internal Wrapper(DifferentialInt32Compressor compressor)
        {
            this.compressor = compressor;
        }

        internal int[] Compress(int[] input)
        {
            return compressor.Compress(input);
        }

        internal int[] Decompress(int[] input)
        {
            return compressor.Decompress(input);
        }

        public override string ToString()
        {
            return compressor.ToString();
        }
    }
#endif
}