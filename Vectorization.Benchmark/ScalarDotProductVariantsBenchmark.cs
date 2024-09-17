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

/* The same with varying SIMD capabilities (indicated by the 'Job' column)

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22621.4037/22H2/2022Update/SunValley2)
13th Gen Intel Core i7-13850HX, 1 CPU, 28 logical and 20 physical cores
.NET SDK 8.0.302
  [Host]    : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2
  Scalar    : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT
  Vector128 : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX
  Vector256 : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2

IterationTime=250ms  MaxIterationCount=20  WarmupCount=3  

| Method           | Job       | Size | Mean       | StdDev     | Ratio | Code Size |
|----------------- |---------- |----- |-----------:|-----------:|------:|----------:|
| Naive            | Scalar    | 3    |   1.735 ns |  0.0832 ns |  1.00 |     209 B |
| Generic          | Scalar    | 3    |   1.679 ns |  0.0669 ns |  0.97 |     209 B |
| Recursive        | Scalar    | 3    |   9.548 ns |  0.1087 ns |  5.51 |     579 B |
| Unrolled         | Scalar    | 3    |   3.468 ns |  0.0952 ns |  2.00 |     432 B |
| FusedMultiplyAdd | Scalar    | 3    |   8.942 ns |  0.1687 ns |  5.16 |     251 B |
| Naive            | Vector128 | 3    |   2.869 ns |  0.0401 ns |  1.66 |     212 B |
| Generic          | Vector128 | 3    |   2.907 ns |  0.0467 ns |  1.68 |     212 B |
| Recursive        | Vector128 | 3    |   9.378 ns |  0.2728 ns |  5.42 |     591 B |
| Unrolled         | Vector128 | 3    |   4.853 ns |  1.3701 ns |  2.80 |     424 B |
| FusedMultiplyAdd | Vector128 | 3    |   2.159 ns |  0.2159 ns |  1.25 |     217 B |
| Naive            | Vector256 | 3    |   2.884 ns |  0.0254 ns |  1.67 |     212 B |
| Generic          | Vector256 | 3    |   3.399 ns |  0.7822 ns |  1.96 |     212 B |
| Recursive        | Vector256 | 3    |   9.622 ns |  0.2642 ns |  5.56 |     591 B |
| Unrolled         | Vector256 | 3    |   4.768 ns |  1.5139 ns |  2.75 |     424 B |
| FusedMultiplyAdd | Vector256 | 3    |   2.658 ns |  1.0403 ns |  1.53 |     217 B |
|                  |           |      |            |            |       |           |
| Naive            | Scalar    | 12   |   5.104 ns |  0.1546 ns |  1.00 |     209 B |
| Generic          | Scalar    | 12   |   4.976 ns |  0.1965 ns |  0.98 |     209 B |
| Recursive        | Scalar    | 12   |  49.323 ns |  1.1079 ns |  9.67 |     563 B |
| Unrolled         | Scalar    | 12   |   5.079 ns |  0.1257 ns |  1.00 |     421 B |
| FusedMultiplyAdd | Scalar    | 12   |  46.028 ns |  0.9601 ns |  9.03 |     251 B |
| Naive            | Vector128 | 12   |   6.770 ns |  0.1866 ns |  1.33 |     212 B |
| Generic          | Vector128 | 12   |   6.731 ns |  0.1709 ns |  1.32 |     212 B |
| Recursive        | Vector128 | 12   |  49.763 ns |  1.2320 ns |  9.76 |     575 B |
| Unrolled         | Vector128 | 12   |   5.515 ns |  0.1595 ns |  1.08 |     409 B |
| FusedMultiplyAdd | Vector128 | 12   |   5.270 ns |  0.1194 ns |  1.03 |     217 B |
| Naive            | Vector256 | 12   |   6.632 ns |  0.1382 ns |  1.30 |     212 B |
| Generic          | Vector256 | 12   |   6.856 ns |  0.1556 ns |  1.34 |     212 B |
| Recursive        | Vector256 | 12   |  48.792 ns |  1.1325 ns |  9.57 |     575 B |
| Unrolled         | Vector256 | 12   |   5.520 ns |  0.2203 ns |  1.08 |     409 B |
| FusedMultiplyAdd | Vector256 | 12   |   5.202 ns |  0.1366 ns |  1.02 |     217 B |
|                  |           |      |            |            |       |           |
| Naive            | Scalar    | 128  |  49.771 ns |  0.4455 ns |  1.00 |     209 B |
| Generic          | Scalar    | 128  |  49.723 ns |  0.5508 ns |  1.00 |     209 B |
| Recursive        | Scalar    | 128  | 412.626 ns |  9.1671 ns |  8.29 |     572 B |
| Unrolled         | Scalar    | 128  |  42.838 ns |  0.3795 ns |  0.86 |     421 B |
| FusedMultiplyAdd | Scalar    | 128  | 399.225 ns |  4.7633 ns |  8.02 |     251 B |
| Naive            | Vector128 | 128  |  57.450 ns |  0.4304 ns |  1.15 |     212 B |
| Generic          | Vector128 | 128  |  58.687 ns |  0.9553 ns |  1.18 |     212 B |
| Recursive        | Vector128 | 128  | 416.948 ns | 11.0371 ns |  8.38 |     584 B |
| Unrolled         | Vector128 | 128  |  52.548 ns |  1.3614 ns |  1.06 |     409 B |
| FusedMultiplyAdd | Vector128 | 128  |  77.494 ns |  1.1821 ns |  1.56 |     217 B |
| Naive            | Vector256 | 128  |  58.970 ns |  0.7746 ns |  1.18 |     212 B |
| Generic          | Vector256 | 128  |  58.764 ns |  0.9418 ns |  1.18 |     212 B |
| Recursive        | Vector256 | 128  | 409.922 ns |  9.1371 ns |  8.24 |     578 B |
| Unrolled         | Vector256 | 128  |  52.989 ns |  1.1033 ns |  1.06 |     409 B |
| FusedMultiplyAdd | Vector256 | 128  |  78.921 ns |  0.7225 ns |  1.59 |     217 B |
*/
