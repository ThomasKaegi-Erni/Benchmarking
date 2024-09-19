using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Vectorization.Benchmark;

// Config with attributes
[WarmupCount(4)]
[MaxIterationCount(20)]
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
  Job-NUGTUP : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2

EnvironmentVariables=Empty  IterationTime=250ms  MaxIterationCount=20  
MinIterationCount=5  WarmupCount=4  

| Method   | size | Mean      | StdDev    |
|--------- |----- |----------:|----------:|
| MyVector | 3    |  3.339 ns | 0.0450 ns |
| MyVector | 6    |  4.251 ns | 0.0116 ns |
| MyVector | 12   |  7.118 ns | 0.0726 ns |
| MyVector | 24   | 13.009 ns | 0.2720 ns |
Summary */
