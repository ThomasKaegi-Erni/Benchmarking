using System.Numerics;

namespace Vectorization;

public readonly struct MyVector
    : IMultiplyOperators<MyVector, MyVector, Double>
{
    private readonly Double[] data;
    public MyVector(Double[] data) => this.data = data;
    public MyVector(Func<Int32, Double> init, Int32 size)
    {
        var data = new Double[size];
        for (Int32 i = 0; i < data.Length; i++)
        {
            data[i] = init(i);
        }
        this.data = data;
    }

    public static Double operator *(MyVector left, MyVector right) => DotProduct.Execute(left.data, right.data);
}
