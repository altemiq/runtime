using Altemiq.Buffers.Compression;
using Altemiq.Buffers.Compression.Differential;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

var config =
    DefaultConfig.Instance
        .AddJob(Job.Default)
        .AddDiagnoser(MemoryDiagnoser.Default)
        .WithOptions(ConfigOptions.DisableOptimizationsValidator)
        .CreateImmutableConfig();

BenchmarkSwitcher.FromTypes(
[
    typeof(DeltaBenchmarks.DeltaForward),
    typeof(DeltaBenchmarks.DeltaReverse),
    typeof(BitPackingBenchmarks),
    typeof(XorBinaryPackingBenchmarks),
]).Run(args, config);