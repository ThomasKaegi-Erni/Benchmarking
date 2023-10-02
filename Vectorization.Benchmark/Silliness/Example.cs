
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
    public Double Execute() => left.DotProduct(right);
}

/*
// * Summary *

BenchmarkDotNet v0.13.8, Windows 10 (10.0.19045.3448/22H2/2022Update)
12th Gen Intel Core i7-1260P, 1 CPU, 16 logical and 12 physical cores
.NET SDK 7.0.401
  [Host]     : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2
  Job-VEUCQY : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2

MaxIterationCount=16  WarmupCount=4  

| Method  | Mean     | Error    | StdDev   | Allocated |
|-------- |---------:|---------:|---------:|----------:|
| Execute | 45.69 ns | 1.098 ns | 1.078 ns |         - |
*/