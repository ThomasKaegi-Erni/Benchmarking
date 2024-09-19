using BenchmarkDotNet.Attributes;

namespace Vectorization.Benchmark;

public class DotProductBenchmark
{
    private MyVector left, right;

    [Params(3, 12, 128, 516)]
    public Int32 Size { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        this.left = new MyVector(i => i, Size);
        this.right = new MyVector(i => 1f / i, Size);
    }

    [Benchmark]
    public Single DotProduct() => this.left * this.right;
}

/* Summary

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22621.4037/22H2/2022Update/SunValley2)
13th Gen Intel Core i7-13850HX, 1 CPU, 28 logical and 20 physical cores
.NET SDK 8.0.302
  [Host]     : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2
  Job-HUYOQX : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2

EnvironmentVariables=Empty  IterationTime=250ms  MaxIterationCount=13  
MinIterationCount=5  WarmupCount=3  

| Method     | Size | Mean      | StdDev    |
|----------- |----- |----------:|----------:|
| DotProduct | 3    |  2.797 ns | 0.0155 ns |
| DotProduct | 12   |  4.492 ns | 0.1208 ns |
| DotProduct | 128  | 18.908 ns | 0.4098 ns |
| DotProduct | 516  | 81.014 ns | 1.2332 ns |
Summary */

