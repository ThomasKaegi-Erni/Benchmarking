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
  Job-HUYOQX : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2

EnvironmentVariables=Empty  IterationTime=250ms  MaxIterationCount=13  
MinIterationCount=5  WarmupCount=3  

| Method              | Size | Mean         | StdDev     | Ratio | 
|-------------------- |----- |-------------:|-----------:|------:|
| Scalar              | 3    |     2.905 ns |  0.0264 ns |  1.00 | 
| Vectorized          | 3    |     3.614 ns |  0.0575 ns |  1.24 | 
| Vectorized128       | 3    |     3.926 ns |  0.1445 ns |  1.35 | 
| Vectorized256       | 3    |     3.745 ns |  0.1151 ns |  1.29 | 
| VectorizedRecursive | 3    |     3.129 ns |  0.0645 ns |  1.08 | 
|                     |      |              |            |       | 
| Scalar              | 12   |     6.718 ns |  0.0858 ns |  1.00 | 
| Vectorized          | 12   |     5.158 ns |  0.0558 ns |  0.77 | 
| Vectorized128       | 12   |     5.393 ns |  0.1124 ns |  0.80 | 
| Vectorized256       | 12   |     5.198 ns |  0.0950 ns |  0.77 | 
| VectorizedRecursive | 12   |    28.416 ns |  0.3893 ns |  4.23 | 
|                     |      |              |            |       | 
| Scalar              | 128  |    57.725 ns |  0.4853 ns |  1.00 | 
| Vectorized          | 128  |    19.147 ns |  0.2809 ns |  0.33 | 
| Vectorized128       | 128  |    18.923 ns |  0.4550 ns |  0.33 | 
| Vectorized256       | 128  |    18.912 ns |  0.5080 ns |  0.33 | 
| VectorizedRecursive | 128  |   246.679 ns |  2.1626 ns |  4.27 | 
|                     |      |              |            |       | 
| Scalar              | 1521 |   610.911 ns |  1.3862 ns |  1.00 | 
| Vectorized          | 1521 |   216.122 ns |  1.7497 ns |  0.35 | 
| Vectorized128       | 1521 |   215.655 ns |  1.4545 ns |  0.35 | 
| Vectorized256       | 1521 |   222.265 ns |  1.9570 ns |  0.36 | 
| VectorizedRecursive | 1521 | 3,941.152 ns | 84.2492 ns |  6.45 | 
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

| Method              | Job       | Size | Mean          | StdDev      | Ratio | Code Size |
|-------------------- |---------- |----- |--------------:|------------:|------:|----------:|
| Scalar              | Scalar    | 3    |      2.538 ns |   0.0656 ns |  1.00 |     209 B |
| Vectorized          | Scalar    | 3    |      3.524 ns |   0.1148 ns |  1.39 |     617 B |
| Vectorized128       | Scalar    | 3    |      3.505 ns |   0.0391 ns |  1.38 |     617 B |
| Vectorized256       | Scalar    | 3    |      3.475 ns |   0.0540 ns |  1.37 |     617 B |
| VectorizedRecursive | Scalar    | 3    |     30.881 ns |   0.0856 ns | 12.17 |   1,937 B |
| Scalar              | Vector128 | 3    |      2.253 ns |   0.0625 ns |  0.89 |     212 B |
| Vectorized          | Vector128 | 3    |      3.481 ns |   0.0857 ns |  1.37 |     423 B |
| Vectorized128       | Vector128 | 3    |      3.445 ns |   0.1298 ns |  1.36 |     423 B |
| Vectorized256       | Vector128 | 3    |      3.404 ns |   0.1016 ns |  1.34 |     423 B |
| VectorizedRecursive | Vector128 | 3    |      2.921 ns |   0.0860 ns |  1.15 |     994 B |
| Scalar              | Vector256 | 3    |      2.238 ns |   0.0576 ns |  0.88 |     212 B |
| Vectorized          | Vector256 | 3    |      3.236 ns |   0.0360 ns |  1.28 |     436 B |
| Vectorized128       | Vector256 | 3    |      3.840 ns |   0.1149 ns |  1.51 |     436 B |
| Vectorized256       | Vector256 | 3    |      3.744 ns |   0.1214 ns |  1.48 |     436 B |
| VectorizedRecursive | Vector256 | 3    |      3.008 ns |   0.0965 ns |  1.19 |     994 B |
|                     |           |      |               |             |       |           |
| Scalar              | Scalar    | 12   |      5.355 ns |   0.1161 ns |  1.00 |     209 B |
| Vectorized          | Scalar    | 12   |     33.328 ns |   0.4873 ns |  6.23 |     652 B |
| Vectorized128       | Scalar    | 12   |     33.570 ns |   0.2821 ns |  6.27 |     652 B |
| Vectorized256       | Scalar    | 12   |     33.218 ns |   0.5802 ns |  6.21 |     652 B |
| VectorizedRecursive | Scalar    | 12   |    172.941 ns |   0.9676 ns | 32.31 |   1,938 B |
| Scalar              | Vector128 | 12   |      5.694 ns |   0.1358 ns |  1.06 |     212 B |
| Vectorized          | Vector128 | 12   |      5.184 ns |   0.1072 ns |  0.97 |     417 B |
| Vectorized128       | Vector128 | 12   |      5.054 ns |   0.0662 ns |  0.94 |     417 B |
| Vectorized256       | Vector128 | 12   |      5.199 ns |   0.0611 ns |  0.97 |     419 B |
| VectorizedRecursive | Vector128 | 12   |     29.152 ns |   0.5404 ns |  5.45 |     944 B |
| Scalar              | Vector256 | 12   |      5.559 ns |   0.1440 ns |  1.04 |     212 B |
| Vectorized          | Vector256 | 12   |      4.718 ns |   0.1036 ns |  0.88 |     432 B |
| Vectorized128       | Vector256 | 12   |      4.655 ns |   0.1310 ns |  0.87 |     432 B |
| Vectorized256       | Vector256 | 12   |      4.628 ns |   0.1411 ns |  0.86 |     432 B |
| VectorizedRecursive | Vector256 | 12   |     29.055 ns |   0.5515 ns |  5.43 |     948 B |
|                     |           |      |               |             |       |           |
| Scalar              | Scalar    | 128  |     50.964 ns |   0.8056 ns |  1.00 |     209 B |
| Vectorized          | Scalar    | 128  |    311.705 ns |   0.2143 ns |  6.12 |     652 B |
| Vectorized128       | Scalar    | 128  |    311.824 ns |   1.2088 ns |  6.12 |     652 B |
| Vectorized256       | Scalar    | 128  |    310.924 ns |   0.1512 ns |  6.10 |     652 B |
| VectorizedRecursive | Scalar    | 128  |  1,464.098 ns |   3.2491 ns | 28.73 |   1,910 B |
| Scalar              | Vector128 | 128  |     58.836 ns |   0.4225 ns |  1.15 |     212 B |
| Vectorized          | Vector128 | 128  |     43.083 ns |   0.7917 ns |  0.85 |     417 B |
| Vectorized128       | Vector128 | 128  |     41.884 ns |   0.1761 ns |  0.82 |     417 B |
| Vectorized256       | Vector128 | 128  |     42.247 ns |   0.7104 ns |  0.83 |     417 B |
| VectorizedRecursive | Vector128 | 128  |    244.269 ns |   0.7623 ns |  4.79 |     887 B |
| Scalar              | Vector256 | 128  |     58.963 ns |   0.2537 ns |  1.16 |     212 B |
| Vectorized          | Vector256 | 128  |     18.918 ns |   0.4172 ns |  0.37 |     434 B |
| Vectorized128       | Vector256 | 128  |     18.679 ns |   0.0571 ns |  0.37 |     434 B |
| Vectorized256       | Vector256 | 128  |     18.963 ns |   0.3329 ns |  0.37 |     434 B |
| VectorizedRecursive | Vector256 | 128  |    243.834 ns |   2.1696 ns |  4.79 |     887 B |
|                     |           |      |               |             |       |           |
| Scalar              | Scalar    | 1521 |    604.570 ns |  10.3355 ns |  1.00 |     209 B |
| Vectorized          | Scalar    | 1521 |  3,758.483 ns |  33.9720 ns |  6.22 |     659 B |
| Vectorized128       | Scalar    | 1521 |  3,736.919 ns |  10.9833 ns |  6.18 |     659 B |
| Vectorized256       | Scalar    | 1521 |  3,754.341 ns |  43.3790 ns |  6.21 |     659 B |
| VectorizedRecursive | Scalar    | 1521 | 23,574.953 ns | 153.9269 ns | 39.01 |   1,908 B |
| Scalar              | Vector128 | 1521 |    629.227 ns |  11.9200 ns |  1.04 |     212 B |
| Vectorized          | Vector128 | 1521 |    431.338 ns |   4.8713 ns |  0.71 |     418 B |
| Vectorized128       | Vector128 | 1521 |    435.583 ns |   4.1496 ns |  0.72 |     418 B |
| Vectorized256       | Vector128 | 1521 |    430.770 ns |   5.5453 ns |  0.71 |     418 B |
| VectorizedRecursive | Vector128 | 1521 |  3,880.901 ns |  11.2419 ns |  6.42 |     888 B |
| Scalar              | Vector256 | 1521 |    622.387 ns |   4.5000 ns |  1.03 |     212 B |
| Vectorized          | Vector256 | 1521 |    217.904 ns |   2.5045 ns |  0.36 |     435 B |
| Vectorized128       | Vector256 | 1521 |    216.087 ns |   1.1044 ns |  0.36 |     435 B |
| Vectorized256       | Vector256 | 1521 |    223.689 ns |   4.0361 ns |  0.37 |     435 B |
| VectorizedRecursive | Vector256 | 1521 |  4,004.653 ns | 101.1369 ns |  6.63 |     888 B |
*/
