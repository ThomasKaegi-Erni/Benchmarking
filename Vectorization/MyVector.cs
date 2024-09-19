using System.Numerics;

namespace Vectorization;

public readonly struct MyVector
    : IMultiplyOperators<MyVector, MyVector, Single>
{
    private readonly Single[] data;
    public Int32 Size => this.data.Length;
    public MyVector(params Single[] data) => this.data = data;
    public MyVector(Func<Int32, Single> init, Int32 size)
    {
        var data = new Single[size];
        for (Int32 i = 0; i < data.Length; ++i) {
            data[i] = init(i);
        }
        this.data = data;
    }

    public static Single operator *(MyVector left, MyVector right) => DotProduct.Execute(left.data, right.data);
    public static implicit operator ReadOnlySpan<Single>(in MyVector vector) => vector.data;
}
