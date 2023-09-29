using BenchmarkDotNet.Attributes;

namespace Vectorization.Benchmark;

// Do not seal benchmark classes. Benchmark.Net subclasses them...
public class VectorizedDotProductVariantsBenchmark
{
    private MyVector left, right;

    [Params(3, 12, 128, 1521)]
    public Int32 Size { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        this.left = new MyVector(i => i, Size);
        this.right = new MyVector(i => 1f / i, Size);
    }

    [Benchmark(Baseline = true)]
    public Double Scalar() => DotProduct.Scalar(this.left, this.right);

    [Benchmark]
    public Double Vectorized() => DotProduct.Vectorized(this.left, this.right);

    [Benchmark]
    public Double Vectorized128() => DotProduct.Vectorized(this.left, this.right);

    [Benchmark]
    public Double Vectorized256() => DotProduct.Vectorized(this.left, this.right);

    [Benchmark]
    public Double VectorizedRecursive() => DotProduct.RecursiveVectorized128(this.left, this.right);
}

/*
// * Summary *

BenchmarkDotNet v0.13.8, Windows 10 (10.0.19045.3448/22H2/2022Update)
12th Gen Intel Core i7-1260P, 1 CPU, 16 logical and 12 physical cores
.NET SDK 7.0.401
[Host]     : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2
DefaultJob : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2


| Method         | Size | Mean       | Error     | StdDev    | Ratio | RatioSD |
|--------------- |----- |-----------:|----------:|----------:|------:|--------:|
| Scalar         | 3    |   3.277 ns | 0.0248 ns | 0.0220 ns |  1.00 |    0.00 |
| UnrolledScalar | 3    |   3.234 ns | 0.0345 ns | 0.0288 ns |  0.99 |    0.01 |
| Vectorized     | 3    |   3.869 ns | 0.1034 ns | 0.1106 ns |  1.17 |    0.03 |
|                |      |            |           |           |       |         |
| Scalar         | 12   |   7.543 ns | 0.0626 ns | 0.0586 ns |  1.00 |    0.00 |
| UnrolledScalar | 12   |   5.972 ns | 0.0464 ns | 0.0411 ns |  0.79 |    0.01 |
| Vectorized     | 12   |   5.337 ns | 0.0190 ns | 0.0177 ns |  0.71 |    0.00 |
|                |      |            |           |           |       |         |
| Scalar         | 128  |  67.126 ns | 0.4829 ns | 0.4517 ns |  1.00 |    0.00 |
| UnrolledScalar | 128  |  58.977 ns | 0.1889 ns | 0.1578 ns |  0.88 |    0.01 |
| Vectorized     | 128  |  23.592 ns | 0.1146 ns | 0.1016 ns |  0.35 |    0.00 |
|                |      |            |           |           |       |         |
| Scalar         | 1521 | 712.213 ns | 3.2871 ns | 2.9139 ns |  1.00 |    0.00 |
| UnrolledScalar | 1521 | 693.594 ns | 2.9926 ns | 2.6529 ns |  0.97 |    0.01 |
| Vectorized     | 1521 | 267.090 ns | 5.2579 ns | 7.0192 ns |  0.38 |    0.01 |
*/