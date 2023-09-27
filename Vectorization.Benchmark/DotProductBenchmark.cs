using BenchmarkDotNet.Attributes;

namespace Vectorization.Benchmark
{
    // Do not seal benchmark classes. Benchmark.Net subclasses them...
    public class DotProductBenchmark
    {
        private MyVector left, right;

        [Params(3, 12, 128, 156)]
        public Int32 Size { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            this.left = new MyVector(i => i, Size);
            this.right = new MyVector(i => 1d / i, Size);
        }

        [Benchmark]
        public Double DotProduct() => this.left * this.right;
    }
}
