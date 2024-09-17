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
  DefaultJob : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2


| Method     | Size | Mean      | Error     | StdDev    |
|----------- |----- |----------:|----------:|----------:|
| DotProduct | 3    |  2.207 ns | 0.0511 ns | 0.0453 ns |
| DotProduct | 12   |  4.613 ns | 0.1116 ns | 0.1371 ns |
| DotProduct | 128  | 19.220 ns | 0.2959 ns | 0.2768 ns |
| DotProduct | 516  | 80.797 ns | 1.1628 ns | 1.0877 ns |
Summary */

