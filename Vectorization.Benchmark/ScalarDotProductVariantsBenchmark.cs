using BenchmarkDotNet.Attributes;

namespace Vectorization.Benchmark;

// Do not seal benchmark classes. Benchmark.Net subclasses them...
public class ScalarDotProductVariantsBenchmark
{
    private MyVector left, right;

    [Params(3, 12, 128)]
    public Int32 Size { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        this.left = new MyVector(i => i, Size);
        this.right = new MyVector(i => 1f / i, Size);
    }

    [Benchmark(Baseline = true)]
    public Single Naive() => DotProduct.Scalar(this.left, this.right);

    [Benchmark]
    public Single Generic() => DotProduct.GenericScalar<Single>(this.left, this.right);

    [Benchmark]
    public Single Recursive() => DotProduct.RecursiveScalar(this.left, this.right);

    [Benchmark]
    public Single Unrolled() => DotProduct.UnrolledScalar(this.left, this.right);

    [Benchmark]
    public Single FusedMultiplyAdd() => DotProduct.FusedScalar(this.left, this.right);
}

/* Summary

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22621.4037/22H2/2022Update/SunValley2)
13th Gen Intel Core i7-13850HX, 1 CPU, 28 logical and 20 physical cores
.NET SDK 8.0.302
  [Host]     : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2


| Method           | Size | Mean       | Error     | StdDev    | Ratio | RatioSD |
|----------------- |----- |-----------:|----------:|----------:|------:|--------:|
| Naive            | 3    |   2.327 ns | 0.0395 ns | 0.0369 ns |  1.00 |    0.02 |
| Generic          | 3    |   2.232 ns | 0.0572 ns | 0.0535 ns |  0.96 |    0.03 |
| Recursive        | 3    |  10.587 ns | 0.2000 ns | 0.1871 ns |  4.55 |    0.11 |
| Unrolled         | 3    |   3.383 ns | 0.0892 ns | 0.1221 ns |  1.45 |    0.06 |
| FusedMultiplyAdd | 3    |   2.856 ns | 0.0385 ns | 0.0360 ns |  1.23 |    0.02 |
|                  |      |            |           |           |       |         |
| Naive            | 12   |   5.459 ns | 0.0907 ns | 0.0848 ns |  1.00 |    0.02 |
| Generic          | 12   |  10.947 ns | 0.2214 ns | 0.1962 ns |  2.01 |    0.05 |
| Recursive        | 12   |  49.985 ns | 0.8150 ns | 1.7190 ns |  9.16 |    0.34 |
| Unrolled         | 12   |   5.494 ns | 0.1199 ns | 0.1122 ns |  1.01 |    0.02 |
| FusedMultiplyAdd | 12   |   6.764 ns | 0.0865 ns | 0.0723 ns |  1.24 |    0.02 |
|                  |      |            |           |           |       |         |
| Naive            | 128  |  59.673 ns | 0.6776 ns | 0.6338 ns |  1.00 |    0.01 |
| Generic          | 128  |  60.307 ns | 0.9171 ns | 0.8579 ns |  1.01 |    0.02 |
| Recursive        | 128  | 413.720 ns | 7.1712 ns | 6.7079 ns |  6.93 |    0.13 |
| Unrolled         | 128  |  52.081 ns | 0.7258 ns | 0.6789 ns |  0.87 |    0.01 |
| FusedMultiplyAdd | 128  |  76.689 ns | 0.7915 ns | 0.7404 ns |  1.29 |    0.02 |
Summary */
