namespace Altemiq.Buffers.Compression.Differential;

public class XorBinaryPackingTest
{
    private static async Task CheckCompressAndUncompress(int[] data)
    {
        var codec = new XorBinaryPacking();
        var compBuf = TestUtils.Compress(codec, data);
        var decompBuf = TestUtils.Uncompress(codec, compBuf, data.Length);
        await Assert.That(decompBuf).IsEquivalentTo(data);
    }

    [Test]
    public Task CompressAndUncompress0()
    {
        return CheckCompressAndUncompress(
             [.. System.Linq.Enumerable
             .Range(0, 128)
                 .Select(i => i switch
                 {
                     >= 0 and < 31 => 1,
                     >= 32 and < 63 => 2,
                     >= 64 and < 95 => 4,
                     >= 96 and < 127 => 8,
                     _ => 0,
                 })]);
    }

    [Test]
    public Task CompressAndUncompress1()
    {
        return CheckCompressAndUncompress([.. System.Linq.Enumerable.Range(0, 128).Select(i => i)]);
    }

    [Test]
    public Task CompressAndUncompress2()
    {
        return CheckCompressAndUncompress([.. System.Linq.Enumerable.Range(0, 128).Select(i => i * (i + 1) / 2)]);
    }

    [Test]
    public Task CompressAndUncompress3()
    {
        return CheckCompressAndUncompress(
            [.. System.Linq.Enumerable
                .Range(0, 256)
                .Select(i => i switch
                {
                    < 127 => 2,
                    > 127 => 3,
                    _ => 0,
                })]);
    }

    [Test]
    public Task CompressAndUncompress4()
    {
        return CheckCompressAndUncompress(
            [.. System.Linq.Enumerable
                .Range(0, 256)
                .Select(i => i switch
                {
                    < 127 => 3,
                    > 127 => 2,
                    _ => 0,
                })]);
    }

    [Test]
    public Task CompressAndUncompress5()
    {
        return CheckCompressAndUncompress([.. System.Linq.Enumerable.Range(0, 256).Select(i => i)]);
    }
}