namespace Altemiq.Buffers.Compression.Differential;

public class XorBinaryPackingTest
{
    [Test]
    [MethodDataSource(nameof(Data))]
    public async Task CheckCompressAndDecompress(int[] data)
    {
        var codec = new XorBinaryPacking();
        var compressed = TestUtils.Compress(codec, data);
        var decompressed = TestUtils.Decompress(codec, compressed, data.Length);
        await Assert.That(decompressed).IsEquivalentTo(data);
    }

    public static IEnumerable<Func<int[]>> Data()
    {
        yield return () => System.Linq.Enumerable
            .Range(0, 128)
            .Select(i => i switch
            {
                >= 0 and < 31 => 1,
                >= 32 and < 63 => 2,
                >= 64 and < 95 => 4,
                >= 96 and < 127 => 8,
                _ => 0,
            }).ToArray();
        yield return () => System.Linq.Enumerable.Range(0, 128).Select(i => i).ToArray();
        yield return () => System.Linq.Enumerable.Range(0, 128).Select(i => i * (i + 1) / 2).ToArray();
        yield return () => System.Linq.Enumerable
            .Range(0, 256)
            .Select(i => i switch
            {
                < 127 => 2,
                > 127 => 3,
                _ => 0,
            }).ToArray();
        yield return () => System.Linq.Enumerable
            .Range(0, 256)
            .Select(i => i switch
            {
                < 127 => 3,
                > 127 => 2,
                _ => 0,
            }).ToArray();
        yield return () => System.Linq.Enumerable.Range(0, 256).Select(i => i).ToArray();
    }
}