namespace Altemiq.Buffers.Compression;

public class BitPackingBenchmarks
{
    private const int Size = 32;

    public static IEnumerable<object[]> Data()
    {
        var r = new Random(0);
        IEnumerable<int> bits = [1, 8, 16, 24, 30];
        foreach (var bit in bits)
        {
            var data = new int[Size];
            for (var k = 0; k < Size; ++k)
            {
                data[k] = r.Next(1 << bit);
            }

            yield return new object[] { data, bit };
        }
    }

    [Benchmark]
    [ArgumentsSource(nameof(Data))]
    public void Altemiq(int[] data, int bit)
    {
        Span<int> compressed = stackalloc int[Size];
        Span<int> uncompressed = stackalloc int[Size];

        BitPacking.Pack(data, compressed, bit);
        BitPacking.PackWithoutMask(data, compressed, bit);
        BitPacking.Unpack(compressed, uncompressed, bit);
    }

    [Benchmark(Baseline = true)]
    [ArgumentsSource(nameof(Data))]
    public void CSharpFastPFOR(int[] data, int bit)
    {
        var compressed = new int[Size];
        var uncompressed = new int[Size];

        Genbox.CSharpFastPFOR.BitPacking.fastpack(data, 0, compressed, 0, bit);
        Genbox.CSharpFastPFOR.BitPacking.fastpackwithoutmask(data, 0, compressed, 0, bit);
        Genbox.CSharpFastPFOR.BitPacking.fastunpack(compressed, 0, uncompressed, 0, bit);
    }
}