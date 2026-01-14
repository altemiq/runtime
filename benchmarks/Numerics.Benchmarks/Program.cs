using Altemiq.Numerics;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

var config = DefaultConfig.Instance
    .AddJob(Job.Default.WithRuntime(CoreRuntime.Core10_0))
    .AddJob(Job.Default.WithRuntime(CoreRuntime.Core90))
    .AddJob(Job.Default.WithRuntime(CoreRuntime.Core80))
    .AddJob(Job.Default.WithRuntime(CoreRuntime.Core70))
    .AddDiagnoser(MemoryDiagnoser.Default)
    .CreateImmutableConfig();

BenchmarkRunner.Run([typeof(Matrix4x4Benchmarks)], config);