namespace Vectorization;

internal static class DotProduct
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
        return 0;
    }
    public static Double UnrolledScalar(in ReadOnlySpan<Double> left, in ReadOnlySpan<Double> right)
    {
        return 0;
    }
}
