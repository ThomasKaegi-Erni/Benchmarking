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

/*
// * Summary *

BenchmarkDotNet v0.13.8, Windows 10 (10.0.19045.3448/22H2/2022Update)
12th Gen Intel Core i7-1260P, 1 CPU, 16 logical and 12 physical cores
.NET SDK 7.0.401
  [Host]     : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2
  Job-EBLLUQ : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2

MaxIterationCount=20  WarmupCount=4  

| Method                   | Mean      | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |----------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| Array                    |  16.74 ns | 0.352 ns | 0.377 ns |  1.00 |    0.00 | 0.0595 |     560 B |        1.00 |
| MyVectorWithStaticLambda | 218.79 ns | 1.404 ns | 1.313 ns | 13.10 |    0.29 | 0.0594 |     560 B |        1.00 |
| MyVectorWithStaticMethod | 274.04 ns | 1.863 ns | 1.454 ns | 16.51 |    0.35 | 0.0591 |     560 B |        1.00 |*/
