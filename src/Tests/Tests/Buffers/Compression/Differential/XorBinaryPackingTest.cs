namespace Altemiq.Buffers.Compression.Differential;

public class XorBinaryPackingTest
{
    private static void CheckCompressAndUncompress(int[] data)
    {
        var codec = new XorBinaryPacking();
        var compBuf = TestUtils.Compress(codec, data);
        var decompBuf = TestUtils.Uncompress(codec, compBuf, data.Length);
        _ = decompBuf.Should().HaveSameElementsAs(data);
    }

    [Fact]
    public void CompressAndUncompress0()
    {
        CheckCompressAndUncompress(
            System.Linq.Enumerable
            .Range(0, 128)
                .Select(i => i switch
                {
                    >= 0 and < 31 => 1,
                    >= 32 and < 63 => 2,
                    >= 64 and < 95 => 4,
                    >= 96 and < 127 => 8,
                    _ => 0,
                })
                .ToArray());
    }

    [Fact]
    public void CompressAndUncompress1()
    {
        CheckCompressAndUncompress(System.Linq.Enumerable.Range(0, 128).Select(i => i).ToArray());
    }

    [Fact]
    public void CompressAndUncompress2()
    {
        CheckCompressAndUncompress(System.Linq.Enumerable.Range(0, 128).Select(i => i * (i + 1) / 2).ToArray());
    }

    [Fact]
    public void CompressAndUncompress3()
    {
        CheckCompressAndUncompress(
            System.Linq.Enumerable
                .Range(0, 256)
                .Select(i => i switch
                {
                    < 127 => 2,
                    > 127 => 3,
                    _ => 0,
                })
                .ToArray());
    }

    [Fact]
    public void CompressAndUncompress4()
    {
        CheckCompressAndUncompress(
            System.Linq.Enumerable
                .Range(0, 256)
                .Select(i => i switch
                {
                    < 127 => 3,
                    > 127 => 2,
                    _ => 0,
                })
                .ToArray());
    }

    [Fact]
    public void CompressAndUncompress5()
    {
        CheckCompressAndUncompress(System.Linq.Enumerable.Range(0, 256).Select(i => i).ToArray());
    }
}