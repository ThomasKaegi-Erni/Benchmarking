using System.Numerics;
using System.Runtime.Intrinsics;

namespace Vectorization.Benchmark.Silliness;

public readonly struct Vector : IVector, IMultiplyOperators<Vector, Vector, Double>
{
    private readonly IVector vector;
    private Vector(IVector vector) => this.vector = vector;
    public void Deconstruct(out Vector256<Single> vector) => this.vector.Deconstruct(out vector);

    public Single DotProduct(Vector other)
    {
        // This is actually very wrong...
        this.Deconstruct(out var leftVec);
        other.Deconstruct(out var rightVec);
        var product = leftVec * rightVec;
        return Vector256.Sum(product);
    }

    public static Vector Create(Func<Int32, Single> init, Int32 size)
    {
        IVector vector = size switch
        {
            < 8 => new VectorN(init, size),
            8 => new Vector8(init, size),
            _ => Create(new Vector8(init), init, size - 8, 8)
        };
        return new Vector(vector);

        static IVector Create<TVector>(TVector left, Func<Int32, Single> init, Int32 size, Int32 offset)
            where TVector : IVector => size switch
            {
                < 8 => new VectorTuple<TVector, VectorN>(in left, new VectorN(init, size, offset)),
                8 => new VectorTuple<TVector, Vector8>(in left, new Vector8(init, offset)),
                _ => Create(new VectorTuple<TVector, Vector8>(in left, new Vector8(init, offset)), init, size - 8, offset + 8),
            };
    }

    public static Double operator *(Vector left, Vector right)
    {
        left.Deconstruct(out var leftVec);
        right.Deconstruct(out var rightVec);
        var product = leftVec * rightVec;
        return Vector256.Sum(product);
    }
}