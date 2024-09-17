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

/* Summary

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22621.4037/22H2/2022Update/SunValley2)
13th Gen Intel Core i7-13850HX, 1 CPU, 28 logical and 20 physical cores
.NET SDK 8.0.302
  [Host]     : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2
  Job-GIYHGJ : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2

MaxIterationCount=20  WarmupCount=4  

| Method   | size | Mean      | Error     | 
|--------- |----- |----------:|----------:|
| MyVector | 3    |  3.337 ns | 0.1039 ns | 
| MyVector | 6    |  4.254 ns | 0.0691 ns | 
| MyVector | 12   |  7.265 ns | 0.1677 ns | 
| MyVector | 24   | 13.217 ns | 0.1331 ns | 
Summary */
