using System.Numerics;
using System.Runtime.Intrinsics;

namespace Vectorization;

public static class DotProduct
{
    /* Optimizations to explore
        - remove premature optimisation of sum with first multiplication (var sum = left[0]*right[0])
        - improve numerical accuracy of unrolled scalar.
        - improve readability by always using the scalar variant for the "tail". (is performance impacted?)
        - increase base case size of all recursive implementations...
    */

    public static Single Execute(in ReadOnlySpan<Single> left, in ReadOnlySpan<Single> right)
    {
        if (!Vector<Single>.IsSupported || left.Length < Vector<Single>.Count) {
            return Scalar(in left, in right);
        }
        if (Vector<Single>.IsSupported) {
            return Vectorized(in left, in right);
        }
        if (Vector256<Single>.IsSupported && left.Length >= Vector256<Single>.Count) {
            return Vectorized256(in left, in right);
        }
        if (Vector128<Single>.IsSupported && left.Length >= Vector128<Single>.Count) {
            return Vectorized128(in left, in right);
        }
        return UnrolledScalar(in left, in right);
    }
    public static Single Scalar(in ReadOnlySpan<Single> left, in ReadOnlySpan<Single> right)
    {
        Single sum = left[0] * right[0];
        for (Int32 i = 1; i < left.Length; ++i) {
            sum += left[i] * right[i];
        }
        return sum;
    }
    public static Single FusedScalar(in ReadOnlySpan<Single> left, in ReadOnlySpan<Single> right)
    {
        Single sum = left[0] * right[0];
        for (Int32 i = 1; i < left.Length; ++i) {
            sum = Single.FusedMultiplyAdd(left[i], right[i], sum);
        }
        return sum;
    }
    public static Single RecursiveScalar(in ReadOnlySpan<Single> l, in ReadOnlySpan<Single> r) => l.Length switch {
        0 => 0f,
        1 => l[0] * r[0],
        2 => l[0] * r[0] + l[1] * r[1],
        var n => RecursiveScalar(l[..(n / 2)], r[..(n / 2)]) + RecursiveScalar(l[(n / 2)..], r[(n / 2)..])
    };
    public static T GenericScalar<T>(in ReadOnlySpan<T> left, in ReadOnlySpan<T> right)
        where T : IMultiplyOperators<T, T, T>, IAdditionOperators<T, T, T>
    {
        T sum = left[0] * right[0];
        for (Int32 i = 1; i < left.Length; ++i) {
            sum += left[i] * right[i];
        }
        return sum;
    }

    public static Single UnrolledScalar(in ReadOnlySpan<Single> left, in ReadOnlySpan<Single> right)
    {
        const Int32 stride = 4;
        Single sum = 0;
        Int32 top = left.Length - (left.Length % stride);
        for (Int32 i = 0; i < top; i += stride) {
            sum += left[i] * right[i];
            sum += left[i + 1] * right[i + 1];
            sum += left[i + 2] * right[i + 2];
            sum += left[i + 3] * right[i + 3];
            // numerically more stable, possibly also faster:
            // sum += a + b + c + d;
        }
        for (Int32 i = top; i < left.Length; ++i) {
            sum += left[i] * right[i];
        }
        return sum;
    }

    // Note that this is numerically a more stable algorithm than the scalar version (but only by a constant factor)
    public static Single Vectorized(in ReadOnlySpan<Single> left, in ReadOnlySpan<Single> right)
    {
        Vector<Single> vectorSum = Vector<Single>.Zero;
        Int32 top = left.Length - (left.Length % Vector<Single>.Count);
        for (Int32 i = 0; i < top; i += Vector<Single>.Count) {
            var range = new Range(i, i + Vector<Single>.Count);
            var l = new Vector<Single>(left[range]);
            var r = new Vector<Single>(right[range]);
            vectorSum += l * r;
        }
        Single sum = Vector.Sum(vectorSum);
        for (Int32 i = top; i < left.Length; ++i) {
            sum += left[i] * right[i];
        }
        return sum;
    }

    public static Single Vectorized128(in ReadOnlySpan<Single> left, in ReadOnlySpan<Single> right)
    {
        const Int32 width = 4; // 4 * 32 = 128;
        Vector128<Single> vectorSum = Vector128<Single>.Zero;
        Int32 top = left.Length - (left.Length % width);
        for (Int32 i = 0; i < top; i += width) {
            var l = Vector128.Create(left[i], left[i + 1], left[i + 2], left[i + 3]);
            var r = Vector128.Create(right[i], right[i + 1], right[i + 2], right[i + 3]);
            vectorSum += l * r;
        }
        Single sum = Vector128.Sum(vectorSum);
        for (Int32 i = top; i < left.Length; ++i) {
            sum += left[i] * right[i];
        }
        return sum;
    }
    public static Single RecursiveVectorized128(in ReadOnlySpan<Single> left, in ReadOnlySpan<Single> right)
    {
        // 4 * 32 = 128;
        return Vector128.Sum(Recursive(in left, in right));

        static Vector128<Single> Recursive(in ReadOnlySpan<Single> l, in ReadOnlySpan<Single> r) => l.Length switch {
            0 => Vector128<Single>.Zero,
            1 => Vector128.Create(l[0] * r[0], 0f, 0f, 0f),
            2 => Vector128.Create(l[0] * r[0], l[1] * r[1], 0f, 0f),
            3 => Vector128.Create(l[0], l[1], l[2], 0f) * Vector128.Create(r[0], r[1], r[2], 0f),
            4 => Vector128.Create(l[0], l[1], l[2], l[3]) * Vector128.Create(r[0], r[1], r[2], r[3]),
            var n => Recursive(l[..(n / 2)], r[..(n / 2)]) + Recursive(l[(n / 2)..], r[(n / 2)..])
        };
    }
    public static Single Vectorized256(in ReadOnlySpan<Single> left, in ReadOnlySpan<Single> right)
    {
        const Int32 width = 8; // 8 * 32 = 256;
        Vector256<Single> vectorSum = Vector256<Single>.Zero;
        Int32 top = left.Length - (left.Length % width);
        for (Int32 i = 0; i < top; i += width) {
            var l = Vector256.Create(left[i], left[i + 1], left[i + 2], left[i + 3], left[i + 4], left[i + 5], left[i + 6], left[i + 7]);
            var r = Vector256.Create(right[i], right[i + 1], right[i + 2], right[i + 3], right[i + 4], right[i + 5], right[i + 6], right[i + 7]);
            vectorSum += l * r;
        }
        Single sum = Vector256.Sum(vectorSum);
        for (Int32 i = top; i < left.Length; ++i) {
            sum += left[i] * right[i];
        }
        return sum;
    }
}
