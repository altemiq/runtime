namespace Altemiq.Buffers.Compression.Differential;

public class DifferentialInt32CompressorTests
{
    private readonly IEnumerable<Func<DifferentialInt32Compressor>> iic =
    [
        () => new(new DifferentialVariableByte()),
        () => new(new HeadlessDifferentialComposition(new DifferentialBinaryPacking(), new DifferentialVariableByte()))
    ];

    [Test]
    public async Task SuperSimpleExample()
    {
        var iic2 = new DifferentialInt32Compressor();
        var data = new int[2342351];
        for (var k = 0; k < data.Length; ++k)
        {
            data[k] = k;
        }

        await Assert.That(iic2.Decompress(iic2.Compress(data))).HasCount().EqualTo(data.Length).And.IsEquivalentTo(data);
    }

    [Test]
    public async Task BasicIntegratedTest()
    {
        for (var n = 1; n <= 10000; n *= 10)
        {
            var orig = new int[n];
            for (var k = 0; k < n; k++)
            {
                orig[k] = 3 * k + 5;
            }

            foreach (var i in iic.Select(c => c()))
            {
                await Assert.That(i.Decompress(i.Compress(orig))).IsEquivalentTo(orig);
            }
        }
    }
}