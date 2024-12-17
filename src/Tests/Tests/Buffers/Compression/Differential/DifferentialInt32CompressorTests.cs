namespace Altemiq.Buffers.Compression.Differential;

public class DifferentialInt32CompressorTests
{
    private readonly DifferentialInt32Compressor[] iic = [
        new(new DifferentialVariableByte()),
        new(new HeadlessDifferentialComposition(new DifferentialBinaryPacking(),new DifferentialVariableByte())) ];

    [Fact]
    public void SuperSimpleExample()
    {
        var iic2 = new DifferentialInt32Compressor();
        var data = new int[2342351];
        for (var k = 0; k < data.Length; ++k)
        {
            data[k] = k;
        }

        var compressed = iic2.Compress(data);
        var recov = iic2.Uncompress(compressed);

        _ = recov.Should().HaveCount(data.Length);
        for (var k = 0; k < data.Length; ++k)
        {
            _ = recov[k].Should().Be(data[k]);
        }
    }

    [Fact]
    public void BasicIntegratedTest()
    {
        for (var n = 1; n <= 10000; n *= 10)
        {
            var orig = new int[n];
            for (var k = 0; k < n; k++)
            {
                orig[k] = (3 * k) + 5;
            }

            foreach (var i in iic)
            {
                var comp = i.Compress(orig);
                var back = i.Uncompress(comp);
                _ = back.Should().HaveSameElementsAs(orig);
            }
        }
    }
}