using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using Vectorization.Benchmark.Silliness;

namespace Vectorization.Benchmark;

// Config with attributes
// View the results with: https://www.speedscope.app/
[WarmupCount(3)]
[MinIterationCount(3)]
[MaxIterationCount(7)]
[EventPipeProfiler(EventPipeProfile.CpuSampling)]
[MemoryDiagnoser]
public class ProfilingThings
{
    private const Int32 size = 514;
    private static readonly MyVector left = new(i => i / 5f, size);
    private static readonly MyVector right = new(i => (i - 7f) / 11f, size);

    /* Clutters the profiling view
    [Benchmark]
    public Double Execute() => DotProduct.RecursiveVectorized128(left, right);
    */

    [Benchmark]
    public Double ExecuteSlow() => SlowDotProduct.RecursiveVectorized128(left, right);
}

/* Summary

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22621.4037/22H2/2022Update/SunValley2)
13th Gen Intel Core i7-13850HX, 1 CPU, 28 logical and 20 physical cores
.NET SDK 8.0.302
  [Host]     : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2
  Job-ACEQNM : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2

MaxIterationCount=7  MinIterationCount=3  WarmupCount=3  

| Method      | Mean         | Error       | StdDev      | Allocated |
|------------ |-------------:|------------:|------------:|----------:|
| Execute     |     1.066 μs |   0.0381 μs |   0.0169 μs |         - |
| ExecuteSlow | 2,553.570 μs | 439.7944 μs | 195.2714 μs |    6892 B |
Summary */
