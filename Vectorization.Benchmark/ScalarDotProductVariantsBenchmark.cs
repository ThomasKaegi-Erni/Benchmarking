using BenchmarkDotNet.Attributes;

namespace Vectorization.Benchmark
{
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
        public Double Naive() => DotProduct.Scalar(this.left, this.right);

        [Benchmark]
        public Double Generic() => DotProduct.GenericScalar<Single>(this.left, this.right);

        [Benchmark]
        public Double Recursive() => DotProduct.RecursiveScalar(this.left, this.right);

        [Benchmark]
        public Double Unrolled() => DotProduct.UnrolledScalar(this.left, this.right);

        [Benchmark]
        public Double FusedMultiplyAdd() => DotProduct.FusedScalar(this.left, this.right);
    }
}

/*
// * Summary *

BenchmarkDotNet v0.13.8, Windows 10 (10.0.19045.3448/22H2/2022Update)
12th Gen Intel Core i7-1260P, 1 CPU, 16 logical and 12 physical cores
.NET SDK 7.0.401
  [Host]     : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2


| Method           | Size | Mean       | Error      | StdDev     | Ratio | RatioSD |
|----------------- |----- |-----------:|-----------:|-----------:|------:|--------:|
| Naive            | 3    |   3.301 ns |  0.0316 ns |  0.0280 ns |  1.00 |    0.00 |
| Generic          | 3    |   2.503 ns |  0.0190 ns |  0.0178 ns |  0.76 |    0.01 |
| Recursive        | 3    |   9.545 ns |  0.0523 ns |  0.0489 ns |  2.89 |    0.03 |
| Unrolled         | 3    |   3.278 ns |  0.0210 ns |  0.0196 ns |  0.99 |    0.01 |
| FusedMultiplyAdd | 3    |   3.271 ns |  0.0214 ns |  0.0179 ns |  0.99 |    0.01 |
|                  |      |            |            |            |       |         |
| Naive            | 12   |   7.574 ns |  0.0316 ns |  0.0264 ns |  1.00 |    0.00 |
| Generic          | 12   |   6.125 ns |  0.0241 ns |  0.0225 ns |  0.81 |    0.00 |
| Recursive        | 12   |  44.256 ns |  0.2228 ns |  0.1975 ns |  5.84 |    0.04 |
| Unrolled         | 12   |   5.960 ns |  0.0768 ns |  0.0599 ns |  0.79 |    0.01 |
| FusedMultiplyAdd | 12   |   7.585 ns |  0.0703 ns |  0.0587 ns |  1.00 |    0.01 |
|                  |      |            |            |            |       |         |
| Naive            | 128  |  67.290 ns |  0.5280 ns |  0.4409 ns |  1.00 |    0.00 |
| Generic          | 128  |  68.721 ns |  0.2047 ns |  0.1709 ns |  1.02 |    0.01 |
| Recursive        | 128  | 435.913 ns | 10.3856 ns | 30.2952 ns |  6.53 |    0.58 |
| Unrolled         | 128  |  59.532 ns |  0.7221 ns |  0.6401 ns |  0.88 |    0.01 |
| FusedMultiplyAdd | 128  |  91.445 ns |  1.8484 ns |  1.8154 ns |  1.36 |    0.03 |
*/