namespace Altemiq.Buffers.Compression.Differential;

public class XorBinaryPackingBenchmarks
{
    private readonly CodecRunner<XorBinaryPacking, Genbox.CSharpFastPFOR.Differential.XorBinaryPacking> runner = new();

    [Benchmark]
    [ArgumentsSource(nameof(Data))]
    public void Altemiq(int[] data, int sparsity) => runner.BenchmarkAltemiq(data);

    [Benchmark(Baseline = true)]
    [ArgumentsSource(nameof(Data))]
    public void Genbox(int[] data, int sparsity) => runner.BenchmarkGenbox(data);

    public IEnumerable<object[]> Data() => CodecRunner<XorBinaryPacking, Genbox.CSharpFastPFOR.Differential.XorBinaryPacking>.Data();
}