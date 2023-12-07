using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Vectorization.Benchmark;

// Config with attributes
[WarmupCount(4)]
[MaxIterationCount(20)]
[HideColumns("StdDev", "RatioSD")]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class CreateBenchmark
{
  [Benchmark]
  [Arguments(3)]
  [Arguments(6)]
  [Arguments(12)]
  [Arguments(24)]
  public Int32 MyVector(Int32 size) => new MyVector(i => i, size).Size;
}

/*
BenchmarkDotNet v0.13.10, Windows 10 (10.0.19045.3693/22H2/2022Update)
12th Gen Intel Core i7-1260P, 1 CPU, 16 logical and 12 physical cores
.NET SDK 7.0.404
  [Host]     : .NET 7.0.14 (7.0.1423.51910), X64 RyuJIT AVX2
  Job-OUAERB : .NET 7.0.14 (7.0.1423.51910), X64 RyuJIT AVX2

MaxIterationCount=20  WarmupCount=4  

| Method   | size | Mean      | Error     | 
|--------- |----- |----------:|----------:|
| MyVector | 3    |  9.801 ns | 0.0937 ns | 
| MyVector | 6    | 14.872 ns | 0.1060 ns | 
| MyVector | 12   | 24.760 ns | 0.2711 ns | 
| MyVector | 24   | 46.298 ns | 0.9222 ns | 
*/
