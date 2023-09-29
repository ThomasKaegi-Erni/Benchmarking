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

    private Single SomeStaticMethod(Int32 value) => value;
}

/*
// * Summary *

BenchmarkDotNet v0.13.8, Windows 10 (10.0.19045.3448/22H2/2022Update)
12th Gen Intel Core i7-1260P, 1 CPU, 16 logical and 12 physical cores
.NET SDK 7.0.401
  [Host]     : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2
  Job-EEZESO : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2

MaxIterationCount=20  WarmupCount=4  

| Method                   | Mean      | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |----------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| Array                    |  16.40 ns | 0.346 ns | 0.399 ns |  1.00 |    0.00 | 0.0595 |     560 B |        1.00 |
| MyVectorWithStaticLambda | 218.76 ns | 1.365 ns | 1.210 ns | 13.34 |    0.30 | 0.0594 |     560 B |        1.00 |
| MyVectorWithStaticMethod | 218.06 ns | 1.295 ns | 1.081 ns | 13.31 |    0.34 | 0.0663 |     624 B |        1.11 |
*/
