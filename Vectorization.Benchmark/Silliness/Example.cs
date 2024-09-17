
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

namespace Vectorization.Benchmark.Silliness;

[WarmupCount(4)]
[MaxIterationCount(16)]
[EventPipeProfiler(EventPipeProfile.CpuSampling)]
[MemoryDiagnoser]
public class Example
{
  private const Int32 size = 53;
  private static readonly Vector left = Vector.Create(i => i / 5f, size);
  private static readonly Vector right = Vector.Create(i => (i - 7f) / 11f, size);

  [Benchmark]
  public Single Execute() => left.DotProduct(right);
}

/* Summary

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22621.4037/22H2/2022Update/SunValley2)
13th Gen Intel Core i7-13850HX, 1 CPU, 28 logical and 20 physical cores
.NET SDK 8.0.302
  [Host]     : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2
  Job-PXLTVF : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2

MaxIterationCount=16  WarmupCount=4  

| Method  | Mean     | Error    | StdDev   | Allocated |
|-------- |---------:|---------:|---------:|----------:|
| Execute | 48.93 ns | 0.395 ns | 0.370 ns |         - |
Summary */
