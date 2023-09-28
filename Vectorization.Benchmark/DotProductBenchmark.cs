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
| Scalar         | 3    |   2.546 ns | 0.0283 ns | 0.0237 ns |  1.00 |    0.00 |
| UnrolledScalar | 3    |   4.266 ns | 0.0503 ns | 0.0393 ns |  1.67 |    0.02 |
|                |      |            |           |           |       |         |
| Scalar         | 12   |   6.186 ns | 0.0431 ns | 0.0382 ns |  1.00 |    0.00 |
| UnrolledScalar | 12   |   5.992 ns | 0.0425 ns | 0.0397 ns |  0.97 |    0.01 |
|                |      |            |           |           |       |         |
| Scalar         | 128  |  69.391 ns | 0.6418 ns | 0.6003 ns |  1.00 |    0.00 |
| UnrolledScalar | 128  |  59.404 ns | 0.2396 ns | 0.2242 ns |  0.86 |    0.01 |
|                |      |            |           |           |       |         |
| Scalar         | 1521 | 718.090 ns | 5.9852 ns | 5.3057 ns |  1.00 |    0.00 |
| UnrolledScalar | 1521 | 701.315 ns | 4.2346 ns | 3.9610 ns |  0.98 |    0.01 |
*/