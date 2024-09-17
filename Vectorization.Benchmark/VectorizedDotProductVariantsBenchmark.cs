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
    public Single Scalar() => DotProduct.Scalar(this.left, this.right);

    [Benchmark]
    public Single Vectorized() => DotProduct.Vectorized(this.left, this.right);

    [Benchmark]
    public Single Vectorized128() => DotProduct.Vectorized(this.left, this.right);

    [Benchmark]
    public Single Vectorized256() => DotProduct.Vectorized(this.left, this.right);

    [Benchmark]
    public Single VectorizedRecursive() => DotProduct.RecursiveVectorized128(this.left, this.right);
}

/* Summary

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22621.4037/22H2/2022Update/SunValley2)
13th Gen Intel Core i7-13850HX, 1 CPU, 28 logical and 20 physical cores
.NET SDK 8.0.302
  [Host]     : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2


| Method              | Size | Mean         | Error      | StdDev     | Ratio | RatioSD |
|-------------------- |----- |-------------:|-----------:|-----------:|------:|--------:|
| Scalar              | 3    |     2.234 ns |  0.0536 ns |  0.0501 ns |  1.00 |    0.03 |
| Vectorized          | 3    |     3.747 ns |  0.0918 ns |  0.0859 ns |  1.68 |    0.05 |
| Vectorized128       | 3    |     3.805 ns |  0.0950 ns |  0.0933 ns |  1.70 |    0.06 |
| Vectorized256       | 3    |     3.800 ns |  0.0890 ns |  0.0833 ns |  1.70 |    0.05 |
| VectorizedRecursive | 3    |     2.951 ns |  0.0802 ns |  0.0788 ns |  1.32 |    0.04 |
|                     |      |              |            |            |       |         |
| Scalar              | 12   |     5.379 ns |  0.0656 ns |  0.0614 ns |  1.00 |    0.02 |
| Vectorized          | 12   |     4.623 ns |  0.0956 ns |  0.0894 ns |  0.86 |    0.02 |
| Vectorized128       | 12   |     4.619 ns |  0.1131 ns |  0.1111 ns |  0.86 |    0.02 |
| Vectorized256       | 12   |     4.610 ns |  0.0926 ns |  0.0867 ns |  0.86 |    0.02 |
| VectorizedRecursive | 12   |    29.983 ns |  0.4318 ns |  0.4040 ns |  5.57 |    0.10 |
|                     |      |              |            |            |       |         |
| Scalar              | 128  |    59.408 ns |  0.8086 ns |  0.7564 ns |  1.00 |    0.02 |
| Vectorized          | 128  |    19.253 ns |  0.2920 ns |  0.2732 ns |  0.32 |    0.01 |
| Vectorized128       | 128  |    19.236 ns |  0.3117 ns |  0.2915 ns |  0.32 |    0.01 |
| Vectorized256       | 128  |    19.091 ns |  0.2430 ns |  0.2154 ns |  0.32 |    0.01 |
| VectorizedRecursive | 128  |   248.558 ns |  4.3209 ns |  4.0418 ns |  4.18 |    0.08 |
|                     |      |              |            |            |       |         |
| Scalar              | 1521 |   620.050 ns | 11.4508 ns | 10.7111 ns |  1.00 |    0.02 |
| Vectorized          | 1521 |   219.801 ns |  2.3570 ns |  2.2047 ns |  0.35 |    0.01 |
| Vectorized128       | 1521 |   220.734 ns |  2.0259 ns |  1.8950 ns |  0.36 |    0.01 |
| Vectorized256       | 1521 |   219.252 ns |  2.8049 ns |  2.6237 ns |  0.35 |    0.01 |
| VectorizedRecursive | 1521 | 3,929.271 ns | 76.7761 ns | 71.8164 ns |  6.34 |    0.15 |
Summary */
