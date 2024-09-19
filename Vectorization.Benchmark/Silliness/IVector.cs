using System.Runtime.Intrinsics;

namespace Vectorization.Benchmark.Silliness;

public interface IVector
{
    public void Deconstruct(out Vector256<Single> vector);
}
