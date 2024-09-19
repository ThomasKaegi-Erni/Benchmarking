using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

namespace Vectorization.Benchmark;

[WarmupCount(3)]
[MinIterationCount(3)]
[MaxIterationCount(7)]
[DisassemblyDiagnoser]
public class DisassemblingThings
{
    private const Int32 size = 514;
    private static readonly MyVector left = new(i => i / 5f, size);
    private static readonly MyVector right = new(i => (i - 7f) / 11f, size);

    [Benchmark]
    public Single Execute() => DotProduct.Scalar(left, right);
}

/* Summary

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22621.4037/22H2/2022Update/SunValley2)
13th Gen Intel Core i7-13850HX, 1 CPU, 28 logical and 20 physical cores
.NET SDK 8.0.302
  [Host]     : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2
  Job-SGKTJT : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2

EnvironmentVariables=Empty  IterationTime=250ms  MaxIterationCount=7  
MinIterationCount=3  WarmupCount=3  Error=2.26 ns  

| Method  | Mean     | StdDev  | Code Size |
|-------- |---------:|--------:|----------:|
| Execute | 211.0 ns | 0.59 ns |     235 B |
Summary */
