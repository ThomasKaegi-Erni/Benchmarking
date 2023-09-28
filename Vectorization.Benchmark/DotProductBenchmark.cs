using BenchmarkDotNet.Attributes;

namespace Vectorization.Benchmark
{
    // Do not seal benchmark classes. Benchmark.Net subclasses them...
    public class DotProductBenchmark
    {
        private MyVector left, right;

        [Params(3, 12, 128, 1521)]
        public Int32 Size { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            this.left = new MyVector(i => i, Size);
            this.right = new MyVector(i => 1d / i, Size);
        }

        [Benchmark(Baseline = true)]
        public Double Scalar() => DotProduct.Scalar(this.left, this.right);

        [Benchmark]
        public Double UnrolledScalar() => DotProduct.UnrolledScalar(this.left, this.right);

        [Benchmark]
        public Double Vectorized() => DotProduct.Vectorized(this.left, this.right);
    }
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
| Scalar         | 3    |   2.560 ns | 0.0383 ns | 0.0359 ns |  1.00 |    0.00 |
| UnrolledScalar | 3    |   4.231 ns | 0.0307 ns | 0.0256 ns |  1.65 |    0.02 |
| Vectorized     | 3    |   3.707 ns | 0.0455 ns | 0.0426 ns |  1.45 |    0.02 |
|                |      |            |           |           |       |         |
| Scalar         | 12   |   6.154 ns | 0.0626 ns | 0.0585 ns |  1.00 |    0.00 |
| UnrolledScalar | 12   |   5.967 ns | 0.0258 ns | 0.0241 ns |  0.97 |    0.01 |
| Vectorized     | 12   |   6.249 ns | 0.0236 ns | 0.0210 ns |  1.02 |    0.01 |
|                |      |            |           |           |       |         |
| Scalar         | 128  |  69.638 ns | 0.6972 ns | 0.6180 ns |  1.00 |    0.00 |
| UnrolledScalar | 128  |  59.837 ns | 0.3311 ns | 0.2585 ns |  0.86 |    0.01 |
| Vectorized     | 128  |  45.456 ns | 0.3964 ns | 0.3514 ns |  0.65 |    0.01 |
|                |      |            |           |           |       |         |
| Scalar         | 1521 | 721.803 ns | 2.2077 ns | 1.7236 ns |  1.00 |    0.00 |
| UnrolledScalar | 1521 | 702.205 ns | 5.0069 ns | 4.6835 ns |  0.97 |    0.01 |
| Vectorized     | 1521 | 532.212 ns | 2.7249 ns | 2.4156 ns |  0.74 |    0.00 |
*/