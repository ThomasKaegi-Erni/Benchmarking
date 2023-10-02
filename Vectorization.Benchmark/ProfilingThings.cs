using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

namespace Vectorization.Benchmark;

// Config with attributes
[WarmupCount(4)]
[MaxIterationCount(16)]
[EventPipeProfiler(EventPipeProfile.CpuSampling)]
[MemoryDiagnoser]
public class ProfilingThings
{
    private const Int32 size = 514;
    private static readonly MyVector left = new(i => i / 5f, size);
    private static readonly MyVector right = new(i => (i - 7f) / 11f, size);

    [Benchmark]
    public Double Execute() => DotProduct.RecursiveVectorized128(left, right);
}
