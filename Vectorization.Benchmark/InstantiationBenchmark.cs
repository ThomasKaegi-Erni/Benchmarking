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
  Job-FSMAAT : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2

EnvironmentVariables=Empty  IterationTime=250ms  MaxIterationCount=20  
MinIterationCount=5  WarmupCount=4  RatioSD=?  

| Method                   | Mean        | StdDev    | Median      | Ratio | Gen0   | Allocated | Alloc Ratio |
|------------------------- |------------:|----------:|------------:|------:|-------:|----------:|------------:|
| Array                    |   0.0001 ns | 0.0003 ns |   0.0000 ns |     ? |      - |         - |           ? |
| MyVectorWithStaticLambda |  66.8823 ns | 0.4683 ns |  66.9622 ns |     ? | 0.0355 |     560 B |           ? |
| MyVectorWithStaticMethod | 254.4576 ns | 2.2450 ns | 254.9144 ns |     ? | 0.0351 |     560 B |           ? |
Summary */
