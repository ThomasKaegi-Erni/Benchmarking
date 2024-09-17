using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

namespace Vectorization.Benchmark;

// Config with attributes
[WarmupCount(4)]
[MaxIterationCount(20)]
[MemoryDiagnoser]
[EventPipeProfiler(EventPipeProfile.CpuSampling)]
public class InstantiationBenchmark
{
  private const Int32 size = 133;

  [Benchmark(Baseline = true)]
  public Int32 Array() => new Single[size].Length;

  [Benchmark]
  public Int32 MyVectorWithStaticLambda() => new MyVector(i => i, size).Size;

  [Benchmark]
  public Int32 MyVectorWithStaticMethod() => new MyVector(SomeStaticMethod, size).Size;

  private static Single SomeStaticMethod(Int32 value) => value;
}

/* Summary

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22621.4037/22H2/2022Update/SunValley2)
13th Gen Intel Core i7-13850HX, 1 CPU, 28 logical and 20 physical cores
.NET SDK 8.0.302
  [Host]     : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2
  Job-FVYEDE : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2

MaxIterationCount=20  WarmupCount=4  

| Method                   | Mean        | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |------------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| Array                    |   0.0148 ns | 0.0126 ns | 0.0118 ns |     ? |       ? |      - |         - |           ? |
| MyVectorWithStaticLambda |  65.3351 ns | 1.0060 ns | 0.9410 ns |     ? |       ? | 0.0356 |     560 B |           ? |
| MyVectorWithStaticMethod | 253.5813 ns | 2.2708 ns | 2.1241 ns |     ? |       ? | 0.0353 |     560 B |           ? |
Summary */
