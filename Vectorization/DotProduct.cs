using System.Numerics;
using System.Runtime.Intrinsics;

namespace Vectorization;

public static class DotProduct
{
    /* Optimizations to explore
        - shortcut to small vectors when input size is small
    */

    public static Single Execute(in ReadOnlySpan<Single> left, in ReadOnlySpan<Single> right)
    {
        return 0;
    }
    public static Single Scalar(in ReadOnlySpan<Single> left, in ReadOnlySpan<Single> right)
    {
        Single sum = left[0] * right[0];
        for (Int32 i = 1; i < left.Length; ++i)
        {
            sum += left[i] * right[i];
        }
        return sum;
    }
    public static Single FusedScalar(in ReadOnlySpan<Single> left, in ReadOnlySpan<Single> right)
    {
        Single sum = left[0] * right[0];
        for (Int32 i = 1; i < left.Length; ++i)
        {
            sum = Single.FusedMultiplyAdd(left[i], right[i], sum);
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

    public static Single UnrolledScalar(in ReadOnlySpan<Single> left, in ReadOnlySpan<Single> right)
    {
        const Int32 stride = 4;
        Single sum = 0;
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
    public static Single Vectorized(in ReadOnlySpan<Single> left, in ReadOnlySpan<Single> right)
    {
        Vector<Single> vectorSum = Vector<Single>.Zero;
        Int32 top = left.Length - (left.Length % Vector<Single>.Count);
        for (Int32 i = 0; i < top; i += Vector<Single>.Count)
        {
            var range = new Range(i, i + Vector<Single>.Count);
            var l = new Vector<Single>(left[range]);
            var r = new Vector<Single>(right[range]);
            vectorSum += l * r;
        }
        Single sum = Vector.Sum(vectorSum);
        for (Int32 i = top; i < left.Length; ++i)
        {
            sum += left[i] * right[i];
        }
        return sum;
    }

    public static Single Vectorized128(in ReadOnlySpan<Single> left, in ReadOnlySpan<Single> right)
    {
        const Int32 width = 4; // 4 * 32 = 128;
        Vector128<Single> vectorSum = Vector128<Single>.Zero;
        Int32 top = left.Length - (left.Length % width);
        for (Int32 i = 0; i < top; i += width)
        {
            var l = Vector128.Create(left[i], left[i + 1], left[i + 2], left[i + 3]);
            var r = Vector128.Create(right[i], right[i + 1], right[i + 2], right[i + 3]);
            vectorSum += l * r;
        }
        Single sum = Vector128.Sum(vectorSum);
        for (Int32 i = top; i < left.Length; ++i)
        {
            sum += left[i] * right[i];
        }
        return sum;
    }
    public static Single Vectorized256(in ReadOnlySpan<Single> left, in ReadOnlySpan<Single> right)
    {
        const Int32 width = 8; // 8 * 32 = 256;
        Vector256<Single> vectorSum = Vector256<Single>.Zero;
        Int32 top = left.Length - (left.Length % width);
        for (Int32 i = 0; i < top; i += width)
        {
            var l = Vector256.Create(left[i], left[i + 1], left[i + 2], left[i + 3], left[i + 4], left[i + 5], left[i + 6], left[i + 7]);
            var r = Vector256.Create(right[i], right[i + 1], right[i + 2], right[i + 3], right[i + 4], right[i + 5], right[i + 6], right[i + 7]);
            vectorSum += l * r;
        }
        Single sum = Vector256.Sum(vectorSum);
        for (Int32 i = top; i < left.Length; ++i)
        {
            sum += left[i] * right[i];
        }
        return sum;
    }
}
