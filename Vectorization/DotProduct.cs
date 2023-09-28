using System.Numerics;

namespace Vectorization;

public static class DotProduct
{
    /* Optimizations to explore
        - shortcut to small vectors when input size is small
    */

    public static Double Execute(in ReadOnlySpan<Double> left, in ReadOnlySpan<Double> right)
    {
        return 0;
    }
    public static Double Scalar(in ReadOnlySpan<Double> left, in ReadOnlySpan<Double> right)
    {
        Double sum = left[0] * right[0];
        for (Int32 i = 1; i < left.Length; ++i)
        {
            sum += left[i] * right[i];
        }
        return sum;
    }
    public static Double FusedScalar(in ReadOnlySpan<Double> left, in ReadOnlySpan<Double> right)
    {
        Double sum = left[0] * right[0];
        for (Int32 i = 1; i < left.Length; ++i)
        {
            sum = Double.FusedMultiplyAdd(left[i], right[i], sum);
        }
        return sum;
    }
    public static T GenericScalar<T>(in ReadOnlySpan<T> left, in ReadOnlySpan<T> right)
        where T : IMultiplyOperators<T, T, T>, IAdditionOperators<T, T, T>
    {
        T sum = left[0] * right[0];
        for (Int32 i = 1; i < left.Length; ++i)
        {
            sum += left[i] * right[i];
        }
        return sum;
    }

    public static Double UnrolledScalar(in ReadOnlySpan<Double> left, in ReadOnlySpan<Double> right)
    {
        const Int32 stride = 4;
        Double sum = 0;
        Int32 top = left.Length - (left.Length % stride);
        for (Int32 i = 0; i < top; i += stride)
        {
            sum += left[i] * right[i];
            sum += left[i + 1] * right[i + 1];
            sum += left[i + 2] * right[i + 2];
            sum += left[i + 3] * right[i + 3];
        }
        for (Int32 i = top; i < left.Length; ++i)
        {
            sum += left[i] * right[i];
        }
        return sum;
    }

    // Note that this is numerically a more stable algorithm than the scalar version (but only by a constant factor)
    public static Double Vectorized(in ReadOnlySpan<Double> left, in ReadOnlySpan<Double> right)
    {
        Vector<Double> vectorSum = Vector<Double>.Zero;
        Int32 top = left.Length - (left.Length % Vector<Double>.Count);
        for (Int32 i = 0; i < top; i += Vector<Double>.Count)
        {
            var range = new Range(i, i + Vector<Double>.Count);
            var l = new Vector<Double>(left[range]);
            var r = new Vector<Double>(right[range]);
            vectorSum += l * r;
        }
        Double sum = Vector.Sum(vectorSum);
        for (Int32 i = top; i < left.Length; ++i)
        {
            sum += left[i] * right[i];
        }
        return sum;
    }
}
