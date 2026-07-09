namespace Altemiq.Buffers.Compression.Differential;

public class XorBinaryPackingTest
{
    [Test]
    [MethodDataSource(nameof(Data))]
    public async Task CheckCompressAndDecompress(int[] data)
    {
        var codec = new XorBinaryPacking();
        _ = await Assert.That(TestUtils.Decompress(codec, TestUtils.Compress(codec, data), data.Length)).HasSameSequenceAs(data);
    }

    public static IEnumerable<Func<int[]>> Data()
    {
        yield return static () => System.Linq.Enumerable
            .Range(0, 128)
            .Select(static i => i switch
            {
                >= 0 and < 31 => 1,
                >= 32 and < 63 => 2,
                >= 64 and < 95 => 4,
                >= 96 and < 127 => 8,
                _ => 0,
            }).ToArray();
        yield return static () => System.Linq.Enumerable.Range(0, 128).Select(static i => i).ToArray();
        yield return static () => System.Linq.Enumerable.Range(0, 128).Select(static i => i * (i + 1) / 2).ToArray();
        yield return static () => System.Linq.Enumerable
            .Range(0, 256)
            .Select(static i => i switch
            {
                < 127 => 2,
                > 127 => 3,
                _ => 0,
            }).ToArray();
        yield return static () => System.Linq.Enumerable
            .Range(0, 256)
            .Select(static i => i switch
            {
                < 127 => 3,
                > 127 => 2,
                _ => 0,
            }).ToArray();
        yield return static () => System.Linq.Enumerable.Range(0, 256).Select(static i => i).ToArray();
    }
}