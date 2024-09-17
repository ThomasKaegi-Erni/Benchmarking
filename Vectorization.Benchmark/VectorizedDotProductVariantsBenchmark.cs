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

/* The same with varying SIMD capabilities (indicated by the 'Job' column)

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22621.4037/22H2/2022Update/SunValley2)
13th Gen Intel Core i7-13850HX, 1 CPU, 28 logical and 20 physical cores
.NET SDK 8.0.302
  [Host]    : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2
  Scalar    : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT
  Vector128 : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX
  Vector256 : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2

IterationTime=250ms  MaxIterationCount=20  WarmupCount=3  

| Method              | Job       | Size | Mean          | StdDev      | Median        | Ratio | Code Size |
|-------------------- |---------- |----- |--------------:|------------:|--------------:|------:|----------:|
| Scalar              | Scalar    | 3    |      3.105 ns |   1.7265 ns |      1.781 ns |  1.31 |     209 B |
| Vectorized          | Scalar    | 3    |      3.451 ns |   0.1198 ns |      3.449 ns |  1.46 |     617 B |
| Vectorized128       | Scalar    | 3    |      4.990 ns |   2.1607 ns |      3.662 ns |  2.11 |     617 B |
| Vectorized256       | Scalar    | 3    |      4.896 ns |   2.1401 ns |      3.517 ns |  2.07 |     617 B |
| VectorizedRecursive | Scalar    | 3    |     30.712 ns |   0.1103 ns |     30.682 ns | 13.00 |   1,937 B |
| Scalar              | Vector128 | 3    |      2.904 ns |   0.0661 ns |      2.925 ns |  1.23 |     212 B |
| Vectorized          | Vector128 | 3    |      5.144 ns |   3.0703 ns |      3.256 ns |  2.18 |     423 B |
| Vectorized128       | Vector128 | 3    |      3.121 ns |   0.1108 ns |      3.129 ns |  1.32 |     423 B |
| Vectorized256       | Vector128 | 3    |      3.216 ns |   0.1131 ns |      3.213 ns |  1.36 |     423 B |
| VectorizedRecursive | Vector128 | 3    |      4.829 ns |   2.2010 ns |      3.439 ns |  2.04 |     958 B |
| Scalar              | Vector256 | 3    |      2.911 ns |   0.0698 ns |      2.914 ns |  1.23 |     212 B |
| Vectorized          | Vector256 | 3    |      3.808 ns |   0.1337 ns |      3.775 ns |  1.61 |     436 B |
| Vectorized128       | Vector256 | 3    |      3.760 ns |   0.1728 ns |      3.730 ns |  1.59 |     436 B |
| Vectorized256       | Vector256 | 3    |      3.745 ns |   0.1546 ns |      3.694 ns |  1.58 |     436 B |
| VectorizedRecursive | Vector256 | 3    |      3.238 ns |   0.1300 ns |      3.230 ns |  1.37 |     994 B |
|                     |           |      |               |             |               |       |           |
| Scalar              | Scalar    | 12   |      5.089 ns |   0.1487 ns |      5.114 ns |  1.00 |     209 B |
| Vectorized          | Scalar    | 12   |     33.388 ns |   0.2741 ns |     33.321 ns |  6.57 |     652 B |
| Vectorized128       | Scalar    | 12   |     33.662 ns |   0.1830 ns |     33.649 ns |  6.62 |     654 B |
| Vectorized256       | Scalar    | 12   |     33.677 ns |   0.2091 ns |     33.596 ns |  6.62 |     654 B |
| VectorizedRecursive | Scalar    | 12   |    173.463 ns |   1.9238 ns |    172.454 ns | 34.11 |   1,938 B |
| Scalar              | Vector128 | 12   |      6.744 ns |   0.1461 ns |      6.750 ns |  1.33 |     212 B |
| Vectorized          | Vector128 | 12   |      5.223 ns |   0.1097 ns |      5.248 ns |  1.03 |     417 B |
| Vectorized128       | Vector128 | 12   |      5.247 ns |   0.1301 ns |      5.279 ns |  1.03 |     417 B |
| Vectorized256       | Vector128 | 12   |      6.269 ns |   0.1337 ns |      6.295 ns |  1.23 |     417 B |
| VectorizedRecursive | Vector128 | 12   |     28.416 ns |   0.3068 ns |     28.417 ns |  5.59 |     944 B |
| Scalar              | Vector256 | 12   |      6.656 ns |   0.1284 ns |      6.659 ns |  1.31 |     212 B |
| Vectorized          | Vector256 | 12   |      5.515 ns |   0.1781 ns |      5.563 ns |  1.08 |     432 B |
| Vectorized128       | Vector256 | 12   |      5.372 ns |   0.1742 ns |      5.373 ns |  1.06 |     432 B |
| Vectorized256       | Vector256 | 12   |      5.249 ns |   0.0718 ns |      5.249 ns |  1.03 |     434 B |
| VectorizedRecursive | Vector256 | 12   |     29.080 ns |   0.6421 ns |     28.983 ns |  5.72 |     944 B |
|                     |           |      |               |             |               |       |           |
| Scalar              | Scalar    | 128  |     50.809 ns |   0.8930 ns |     50.567 ns |  1.00 |     209 B |
| Vectorized          | Scalar    | 128  |    315.662 ns |   2.0052 ns |    315.505 ns |  6.21 |     652 B |
| Vectorized128       | Scalar    | 128  |    314.036 ns |   1.3791 ns |    314.132 ns |  6.18 |     652 B |
| Vectorized256       | Scalar    | 128  |    314.293 ns |   1.6915 ns |    313.936 ns |  6.19 |     652 B |
| VectorizedRecursive | Scalar    | 128  |  1,490.347 ns |  11.5270 ns |  1,491.105 ns | 29.34 |   1,910 B |
| Scalar              | Vector128 | 128  |     58.529 ns |   0.7070 ns |     58.330 ns |  1.15 |     212 B |
| Vectorized          | Vector128 | 128  |     42.525 ns |   0.7428 ns |     42.404 ns |  0.84 |     419 B |
| Vectorized128       | Vector128 | 128  |     42.600 ns |   0.7551 ns |     42.766 ns |  0.84 |     417 B |
| Vectorized256       | Vector128 | 128  |     42.677 ns |   0.7867 ns |     42.941 ns |  0.84 |     417 B |
| VectorizedRecursive | Vector128 | 128  |    251.558 ns |   5.5649 ns |    251.761 ns |  4.95 |     887 B |
| Scalar              | Vector256 | 128  |     59.484 ns |   0.7958 ns |     59.499 ns |  1.17 |     212 B |
| Vectorized          | Vector256 | 128  |     19.308 ns |   0.5198 ns |     19.341 ns |  0.38 |     434 B |
| Vectorized128       | Vector256 | 128  |     19.055 ns |   0.5127 ns |     19.116 ns |  0.38 |     434 B |
| Vectorized256       | Vector256 | 128  |     19.277 ns |   0.5805 ns |     19.387 ns |  0.38 |     434 B |
| VectorizedRecursive | Vector256 | 128  |    250.940 ns |   4.6690 ns |    251.435 ns |  4.94 |     887 B |
|                     |           |      |               |             |               |       |           |
| Scalar              | Scalar    | 1521 |    607.197 ns |   8.9327 ns |    606.484 ns |  1.00 |     209 B |
| Vectorized          | Scalar    | 1521 |  3,702.192 ns |  17.7244 ns |  3,693.266 ns |  6.10 |     659 B |
| Vectorized128       | Scalar    | 1521 |  3,766.386 ns |  40.4839 ns |  3,760.896 ns |  6.20 |     659 B |
| Vectorized256       | Scalar    | 1521 |  3,728.499 ns |  20.0454 ns |  3,722.482 ns |  6.14 |     659 B |
| VectorizedRecursive | Scalar    | 1521 | 23,678.888 ns | 171.6468 ns | 23,644.865 ns | 39.00 |   1,908 B |
| Scalar              | Vector128 | 1521 |    623.658 ns |   8.3922 ns |    620.175 ns |  1.03 |     212 B |
| Vectorized          | Vector128 | 1521 |    430.486 ns |   6.5931 ns |    433.143 ns |  0.71 |     418 B |
| Vectorized128       | Vector128 | 1521 |    437.890 ns |   3.5251 ns |    438.056 ns |  0.72 |     418 B |
| Vectorized256       | Vector128 | 1521 |    435.634 ns |   7.0668 ns |    436.374 ns |  0.72 |     420 B |
| VectorizedRecursive | Vector128 | 1521 |  4,117.747 ns |  69.5078 ns |  4,115.740 ns |  6.78 |     888 B |
| Scalar              | Vector256 | 1521 |    634.120 ns |   9.4239 ns |    636.473 ns |  1.04 |     212 B |
| Vectorized          | Vector256 | 1521 |    223.819 ns |   3.8289 ns |    224.696 ns |  0.37 |     435 B |
| Vectorized128       | Vector256 | 1521 |    222.860 ns |   3.0391 ns |    223.252 ns |  0.37 |     435 B |
| Vectorized256       | Vector256 | 1521 |    221.076 ns |   2.0913 ns |    221.415 ns |  0.36 |     435 B |
| VectorizedRecursive | Vector256 | 1521 |  3,890.614 ns |  72.6268 ns |  3,868.519 ns |  6.41 |     888 B |
*/
