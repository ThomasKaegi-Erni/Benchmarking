using System.Runtime.Intrinsics;

namespace Vectorization.Benchmark.Silliness;

public readonly struct VectorN : IVector
{
    private readonly Single[] data;

    public VectorN(Func<Int32, Single> init, Int32 size, Int32 offset = 0)
    {
        var data = new Single[8];
        for (Int32 i = 0; i < Math.Min(size, data.Length); ++i) {
            data[i] = init(i + offset);
        }
        this.data = data;
    }

    public void Deconstruct(out Vector256<Single> vector)
    {
        vector = Vector256.Create(this.data[0], this.data[1], this.data[2], this.data[3], this.data[4], this.data[5], this.data[6], this.data[7]);
    }
}
