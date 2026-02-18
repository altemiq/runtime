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
        IEnumerable<int> numbers = [18];

        foreach (var number in numbers)
        {
            var maxSparsity = 31 - number;
            for (var sparsity = 0; sparsity <= maxSparsity; sparsity += 8)
            {
                var dataSize = 1 << (number + sparsity);
                var data = cdg.GenerateClustered(1 << number, dataSize);

                if (typeof(T1).IsAssignableTo(typeof(IDifferentialInt32Codec)))
                {
                    Delta.Forward(data);
                }

                yield return [data, sparsity];
            }
        }
    }

    public void BenchmarkAltemiq(int[] data)
    {
        // 4x + 1024 to account for the possibility of some negative compression.
        var compressBuffer = new int[(4 * data.Length) + 1024];
        var decompressBuffer = new int[data.Length + 1024];
        var backupData = new int[data.Length];
        Array.Copy(data, backupData, data.Length);
        var (_, output) = this.altemiqCodec.Compress(backupData.AsSpan(1), compressBuffer);
        decompressBuffer[0] = compressBuffer[0];
        this.altemiqCodec.Decompress(compressBuffer.AsSpan(0, output), decompressBuffer.AsSpan(1));
    }

    public void BenchmarkGenbox(int[] data)
    {
        // 4x + 1024 to account for the possibility of some negative compression.
        var compressBuffer = new int[(4 * data.Length) + 1024];
        var decompressBuffer = new int[data.Length + 1024];
        var backupData = new int[data.Length];
        Array.Copy(data, backupData, data.Length);

        var input = new Genbox.CSharpFastPFOR.IntWrapper();
        var output = new Genbox.CSharpFastPFOR.IntWrapper();

        input.set(1);
        output.set(0);
        this.genboxCodec.compress(backupData, input, backupData.Length - input.get(), compressBuffer, output);

        var size = output.get();

        input.set(0);
        output.set(1);
        decompressBuffer[0] = compressBuffer[0];
        this.genboxCodec.uncompress(compressBuffer, input, size, decompressBuffer, output);
    }
}