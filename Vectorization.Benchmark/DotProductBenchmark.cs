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
    public Double DotProduct() => this.left * this.right;
}

/*
// * Summary *

BenchmarkDotNet v0.13.8, Windows 10 (10.0.19045.3448/22H2/2022Update)
12th Gen Intel Core i7-1260P, 1 CPU, 16 logical and 12 physical cores
.NET SDK 7.0.401
  [Host]    : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2
  Scalar    : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT
  Vector128 : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX
  Vector256 : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2

IterationTime=250.0000 ms  MaxIterationCount=20  WarmupCount=3  

| Method     | Job       | Size | Mean         | StdDev     | Ratio | Code Size |
|----------- |---------- |----- |-------------:|-----------:|------:|----------:|
| DotProduct | Scalar    | 3    |     3.339 ns |  0.0263 ns |  1.00 |     131 B |
| DotProduct | Vector128 | 3    |     3.567 ns |  0.0306 ns |  1.07 |     139 B |
| DotProduct | Vector256 | 3    |     3.582 ns |  0.0303 ns |  1.07 |     139 B |
|            |           |      |              |            |       |           |
| DotProduct | Scalar    | 12   |    44.585 ns |  0.3368 ns |  1.00 |     131 B |
| DotProduct | Vector128 | 12   |     6.423 ns |  0.0777 ns |  0.14 |     139 B |
| DotProduct | Vector256 | 12   |     5.663 ns |  0.0389 ns |  0.13 |     139 B |
|            |           |      |              |            |       |           |
| DotProduct | Scalar    | 128  |   380.213 ns |  2.5079 ns |  1.00 |     131 B |
| DotProduct | Vector128 | 128  |    45.516 ns |  0.5536 ns |  0.12 |     139 B |
| DotProduct | Vector256 | 128  |    23.466 ns |  0.1500 ns |  0.06 |     139 B |
|            |           |      |              |            |       |           |
| DotProduct | Scalar    | 516  | 1,494.336 ns |  7.2264 ns |  1.00 |     131 B |
| DotProduct | Vector128 | 516  |   186.140 ns | 11.8729 ns |  0.12 |     139 B |
| DotProduct | Vector256 | 516  |    91.477 ns |  0.6176 ns |  0.06 |     139 B |
*/
