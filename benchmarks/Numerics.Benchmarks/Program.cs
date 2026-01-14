using Altemiq.Numerics;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

var config = new ManualConfig();
config.Add(DefaultConfig.Instance);
config.AddDiagnoser(MemoryDiagnoser.Default);
config.AddJob(
     Job.Default.WithRuntime(CoreRuntime.Core70),
     Job.Default.WithRuntime(CoreRuntime.Core80),
     Job.Default.WithRuntime(CoreRuntime.Core90),
     Job.Default.WithRuntime(CoreRuntime.Core10_0));

BenchmarkRunner.Run([typeof(Matrix4x4Benchmarks)], config);