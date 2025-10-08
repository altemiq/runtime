using Altemiq.Protobuf.Converters;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;

//var options = config ConfigOptions.
var config = new ManualConfig();
config.Add(DefaultConfig.Instance);
config.AddDiagnoser(MemoryDiagnoser.Default);

BenchmarkRunner.Run(
    [
        typeof(StructConverterBenchmarks.Serialize),
        typeof(StructConverterBenchmarks.Deserialize),
    ],
    config);