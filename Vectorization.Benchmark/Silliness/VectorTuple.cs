using System.Runtime.Intrinsics;

namespace Vectorization.Benchmark.Silliness;

public readonly struct VectorTuple<TLeft, TRight> : IVector
    where TLeft : IVector
    where TRight : IVector
{
    private readonly TLeft left;
    private readonly TRight right;

    public VectorTuple(in TLeft left, in TRight right) => (this.left, this.right) = (left, right);

    public void Deconstruct(out Vector256<Single> vector)
    {
        this.left.Deconstruct(out var leftVec);
        this.right.Deconstruct(out var rightVec);
        vector = leftVec * rightVec;
    }
}
