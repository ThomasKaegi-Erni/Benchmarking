using System.Runtime.Intrinsics;

namespace Vectorization.Benchmark.Silliness;

public struct Vector8 : IVector
{
    private Single v0, v1, v2, v3, v4, v5, v6, v7;
    public Vector8(Func<Int32, Single> init, Int32 offset = 0)
    {
        this.v0 = init(offset); this.v1 = init(offset + 1); this.v2 = init(offset + 2); this.v3 = init(offset + 3);
        this.v4 = init(offset + 4); this.v5 = init(offset + 5); this.v6 = init(offset + 6); this.v7 = init(offset + 7);
    }

    public void Deconstruct(out Vector256<Single> vector)
    {
        vector = Vector256.Create(this.v0, this.v1, this.v2, this.v3, this.v4, this.v5, this.v6, this.v7);
    }
}