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
  Job-HUYOQX : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2

EnvironmentVariables=Empty  IterationTime=250ms  MaxIterationCount=13  
MinIterationCount=5  WarmupCount=3  

| Method           | Size | Mean       | StdDev     | Ratio | 
|----------------- |----- |-----------:|-----------:|------:|
| Naive            | 3    |   2.937 ns |  0.0743 ns |  1.00 | 
| Generic          | 3    |   2.913 ns |  0.0970 ns |  0.99 | 
| Recursive        | 3    |   9.951 ns |  0.2484 ns |  3.39 | 
| Unrolled         | 3    |   3.770 ns |  0.0413 ns |  1.28 | 
| FusedMultiplyAdd | 3    |   2.083 ns |  0.0380 ns |  0.71 | 
|                  |      |            |            |       | 
| Naive            | 12   |   6.735 ns |  0.1167 ns |  1.00 | 
| Generic          | 12   |   6.369 ns |  0.1492 ns |  0.95 | 
| Recursive        | 12   |  49.382 ns |  0.5108 ns |  7.33 | 
| Unrolled         | 12   |   5.281 ns |  0.0518 ns |  0.78 | 
| FusedMultiplyAdd | 12   |   5.088 ns |  0.0270 ns |  0.76 | 
|                  |      |            |            |       | 
| Naive            | 128  |  59.241 ns |  1.1633 ns |  1.00 | 
| Generic          | 128  |  58.728 ns |  0.8221 ns |  0.99 | 
| Recursive        | 128  | 413.703 ns | 10.6886 ns |  6.99 | 
| Unrolled         | 128  |  52.338 ns |  1.3407 ns |  0.88 | 
| FusedMultiplyAdd | 128  |  77.690 ns |  1.1850 ns |  1.31 | 
Summary */

/* The same with varying SIMD capabilities (indicated by the 'Job' column)

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22621.4037/22H2/2022Update/SunValley2)
13th Gen Intel Core i7-13850HX, 1 CPU, 28 logical and 20 physical cores
.NET SDK 8.0.302
  [Host]    : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2
  Scalar    : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT
  Vector128 : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX
  Vector256 : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2

IterationTime=250ms  MaxIterationCount=13  MinIterationCount=5  
WarmupCount=3  

| Method           | Job       | Size | Mean       | StdDev     | Ratio | Code Size |
|----------------- |---------- |----- |-----------:|-----------:|------:|----------:|
| Naive            | Scalar    | 3    |   2.497 ns |  0.0559 ns |  1.00 |     209 B |
| Generic          | Scalar    | 3    |   2.618 ns |  0.0716 ns |  1.05 |     209 B |
| Recursive        | Scalar    | 3    |   9.587 ns |  0.2823 ns |  3.84 |     579 B |
| Unrolled         | Scalar    | 3    |   3.319 ns |  0.1048 ns |  1.33 |     432 B |
| FusedMultiplyAdd | Scalar    | 3    |   8.851 ns |  0.2119 ns |  3.55 |     251 B |
| Naive            | Vector128 | 3    |   2.222 ns |  0.0518 ns |  0.89 |     212 B |
| Generic          | Vector128 | 3    |   2.370 ns |  0.0899 ns |  0.95 |     212 B |
| Recursive        | Vector128 | 3    |   9.628 ns |  0.0280 ns |  3.86 |     591 B |
| Unrolled         | Vector128 | 3    |   3.417 ns |  0.0997 ns |  1.37 |     424 B |
| FusedMultiplyAdd | Vector128 | 3    |   2.857 ns |  0.0629 ns |  1.14 |     217 B |
| Naive            | Vector256 | 3    |   2.342 ns |  0.0816 ns |  0.94 |     212 B |
| Generic          | Vector256 | 3    |   2.191 ns |  0.0229 ns |  0.88 |     212 B |
| Recursive        | Vector256 | 3    |   9.685 ns |  0.2343 ns |  3.88 |     591 B |
| Unrolled         | Vector256 | 3    |   3.404 ns |  0.1401 ns |  1.36 |     424 B |
| FusedMultiplyAdd | Vector256 | 3    |   2.904 ns |  0.0571 ns |  1.16 |     217 B |
|                  |           |      |            |            |       |           |
| Naive            | Scalar    | 12   |   5.155 ns |  0.1359 ns |  1.00 |     209 B |
| Generic          | Scalar    | 12   |   6.050 ns |  0.1980 ns |  1.17 |     209 B |
| Recursive        | Scalar    | 12   |  49.394 ns |  0.9664 ns |  9.59 |     563 B |
| Unrolled         | Scalar    | 12   |   5.238 ns |  0.1246 ns |  1.02 |     421 B |
| FusedMultiplyAdd | Scalar    | 12   |  44.832 ns |  0.8567 ns |  8.70 |     251 B |
| Naive            | Vector128 | 12   |   5.480 ns |  0.1233 ns |  1.06 |     212 B |
| Generic          | Vector128 | 12   |   5.557 ns |  0.1222 ns |  1.08 |     212 B |
| Recursive        | Vector128 | 12   |  47.061 ns |  0.4998 ns |  9.14 |     575 B |
| Unrolled         | Vector128 | 12   |   5.337 ns |  0.1301 ns |  1.04 |     409 B |
| FusedMultiplyAdd | Vector128 | 12   |   6.487 ns |  0.1122 ns |  1.26 |     217 B |
| Naive            | Vector256 | 12   |   5.483 ns |  0.1215 ns |  1.06 |     212 B |
| Generic          | Vector256 | 12   |   5.520 ns |  0.1296 ns |  1.07 |     212 B |
| Recursive        | Vector256 | 12   |  48.915 ns |  0.8715 ns |  9.49 |     575 B |
| Unrolled         | Vector256 | 12   |   5.475 ns |  0.1214 ns |  1.06 |     409 B |
| FusedMultiplyAdd | Vector256 | 12   |   6.793 ns |  0.1361 ns |  1.32 |     217 B |
|                  |           |      |            |            |       |           |
| Naive            | Scalar    | 128  |  51.131 ns |  0.9427 ns |  1.00 |     209 B |
| Generic          | Scalar    | 128  |  50.864 ns |  0.5496 ns |  1.00 |     209 B |
| Recursive        | Scalar    | 128  | 410.075 ns |  6.9714 ns |  8.02 |     572 B |
| Unrolled         | Scalar    | 128  |  43.594 ns |  0.7115 ns |  0.85 |     421 B |
| FusedMultiplyAdd | Scalar    | 128  | 407.622 ns | 10.9842 ns |  7.97 |     251 B |
| Naive            | Vector128 | 128  |  58.980 ns |  0.3953 ns |  1.15 |     212 B |
| Generic          | Vector128 | 128  |  59.170 ns |  0.2394 ns |  1.16 |     212 B |
| Recursive        | Vector128 | 128  | 427.857 ns |  3.6825 ns |  8.37 |     584 B |
| Unrolled         | Vector128 | 128  |  53.368 ns |  0.8841 ns |  1.04 |     409 B |
| FusedMultiplyAdd | Vector128 | 128  |  77.120 ns |  1.0934 ns |  1.51 |     217 B |
| Naive            | Vector256 | 128  |  61.258 ns |  0.2725 ns |  1.20 |     212 B |
| Generic          | Vector256 | 128  |  60.180 ns |  0.5114 ns |  1.18 |     212 B |
| Recursive        | Vector256 | 128  | 420.261 ns |  2.1060 ns |  8.22 |     584 B |
| Unrolled         | Vector256 | 128  |  50.907 ns |  0.4385 ns |  1.00 |     409 B |
| FusedMultiplyAdd | Vector256 | 128  |  76.776 ns |  1.2539 ns |  1.50 |     217 B |
*/
