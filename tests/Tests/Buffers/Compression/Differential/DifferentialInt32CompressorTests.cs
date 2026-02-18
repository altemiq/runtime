using Enumerable = System.Linq.Enumerable;

namespace Altemiq.Buffers.Compression.Differential;

public class DifferentialInt32CompressorTests
{
    private readonly IEnumerable<Func<DifferentialInt32Compressor>> iic =
    [
        () => new(new DifferentialVariableByte()),
        () => new(new HeadlessDifferentialComposition(new DifferentialBinaryPacking(), new DifferentialVariableByte())),
    ];

    [Test]
    public async Task SuperSimpleExample()
    {
        var iic2 = new DifferentialInt32Compressor();
        var data = System.Buffers.ArrayPool<int>.Shared.Rent(ushort.MaxValue);
        await Assert.That(iic2.Decompress(iic2.Compress(data))).HasSameSequenceAs(data);
        System.Buffers.ArrayPool<int>.Shared.Return(data);
    }

#if !NETFRAMEWORK
    [Test]
    [MatrixDataSource]
    public async Task BasicIntegratedTest([Matrix(1, 10, 100, 1000, 10000)] int n, [MatrixMethod<DifferentialInt32CompressorTests>(nameof(GetCompressors))] Wrapper i)
    {
        var orig = new int[n];
        for (var k = 0; k < n; k++)
        {
            orig[k] = 3 * k + 5;
        }

        await Assert.That(i.Decompress(i.Compress(orig))).HasSameSequenceAs(orig);
    }

    public IEnumerable<Wrapper> GetCompressors()
    {
        return this.iic.Select(c => new Wrapper(c()));
    }

    public sealed class Wrapper
    {
        private readonly DifferentialInt32Compressor compressor;

        internal Wrapper(DifferentialInt32Compressor compressor) => this.compressor = compressor;

        internal int[] Compress(int[] input) => this.compressor.Compress(input);

        internal int[] Decompress(int[] input) => this.compressor.Decompress(input);

        public override string ToString() => this.compressor.ToString();
    }
#endif
}