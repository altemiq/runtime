using Altemiq.Protobuf.Converters;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Jobs;

var config =
    DefaultConfig.Instance
        .AddJob(Job.Default.WithRuntime(CoreRuntime.Core10_0))
        .AddJob(Job.Default.WithRuntime(CoreRuntime.Core90))
        .AddDiagnoser(MemoryDiagnoser.Default)
        .CreateImmutableConfig();

BenchmarkRunner.Run(
    [
        typeof(StructConverterBenchmarks.Serialize),
        typeof(StructConverterBenchmarks.Deserialize),
    ],
    config);