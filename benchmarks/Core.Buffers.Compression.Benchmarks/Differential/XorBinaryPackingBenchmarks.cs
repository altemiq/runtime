namespace Altemiq.Buffers.Compression.Differential;

public class XorBinaryPackingBenchmarks
{
    private readonly CodecRunner<XorBinaryPacking, Genbox.CSharpFastPFOR.Differential.XorBinaryPacking> runner = new();

    [Benchmark]
    [ArgumentsSource(nameof(Data))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This is so it's displayed.")]
    public void Altemiq(int[] data, int sparsity)
    {
        this.runner.BenchmarkAltemiq(data);
    }

    [Benchmark(Baseline = true)]
    [ArgumentsSource(nameof(Data))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This is so it's displayed.")]
    public void Genbox(int[] data, int sparsity)
    {
        this.runner.BenchmarkGenbox(data);
    }

    public static IEnumerable<object[]> Data()
    {
        return CodecRunner<XorBinaryPacking, Genbox.CSharpFastPFOR.Differential.XorBinaryPacking>.Data();
    }
}