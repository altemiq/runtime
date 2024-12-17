namespace Altemiq.Buffers.Compression;

using Altemiq.Buffers.Compression;
using Altemiq.Buffers.Compression.Differential;

internal class CodecRunner<T1, T2>
    where T1 : IInt32Codec, new()
    where T2 : Genbox.CSharpFastPFOR.IntegerCODEC, new()
{
    private readonly T1 altemiqCodec = new();
    private readonly T2 genboxCodec = new();

    public static IEnumerable<object[]> Data()
    {
        var cdg = new ClusteredDataGenerator();
        IEnumerable<int> nbrs = [18];

        foreach (var nbr in nbrs)
        {
            var maxSparsity = 31 - nbr;
            for (var sparsity = 0; sparsity <= maxSparsity; sparsity += 8)
            {
                var dataSize = 1 << (nbr + sparsity);
                var data = cdg.GenerateClustered(1 << nbr, dataSize);

                if (typeof(T1).IsAssignableTo(typeof(Buffers.Compression.Differential.IDifferentialInt32Codec)))
                {
                    Delta.Forward(data);
                }

                yield return new object[] { data, sparsity };
            }
        }
    }

    public void BenchmarkAltemiq(int[] data)
    {
        // 4x + 1024 to account for the possibility of some negative compression.
        var compressBuffer = new int[(4 * data.Length) + 1024];
        var decompressBuffer = new int[data.Length + 1024];
        var backupData = new int[data.Length];
        System.Array.Copy(data, backupData, data.Length);

        var input = 1;
        var output = 0;
        this.altemiqCodec.Compress(backupData, ref input, compressBuffer, ref output, backupData.Length - input);

        var compressedSize = output;

        input = 0;
        output = 1;
        decompressBuffer[0] = compressBuffer[0];
        this.altemiqCodec.Decompress(compressBuffer, ref input, decompressBuffer, ref output, compressedSize);
    }

    public void BenchmarkGenbox(int[] data)
    {
        // 4x + 1024 to account for the possibility of some negative compression.
        var compressBuffer = new int[(4 * data.Length) + 1024];
        var decompressBuffer = new int[data.Length + 1024];
        var backupData = new int[data.Length];
        System.Array.Copy(data, backupData, data.Length);

        var inpos = new Genbox.CSharpFastPFOR.IntWrapper();
        var outpos = new Genbox.CSharpFastPFOR.IntWrapper();

        inpos.set(1);
        outpos.set(0);
        this.genboxCodec.compress(backupData, inpos, backupData.Length - inpos.get(), compressBuffer, outpos);

        var thiscompsize = outpos.get();

        inpos.set(0);
        outpos.set(1);
        decompressBuffer[0] = compressBuffer[0];
        this.genboxCodec.uncompress(compressBuffer, inpos, thiscompsize, decompressBuffer, outpos);
    }
}