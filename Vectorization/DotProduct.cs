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
}
