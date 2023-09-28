namespace Vectorization.Test;

public class DotProductTests
{
    const Int32 precision = 12;
    private const Int32 size = 50;
    private static readonly MyVector vectorA = new(i => i % 7, size);
    private static readonly MyVector vectorB = new(i => i % 11, size);
    [Fact]
    public void ScalarProductIsZeroForOrthogonalVectors()
    {
        var left = new MyVector(0, 2, 0, -1);
        var right = new MyVector(1, 0, 4, 0);

        var actual = DotProduct.Scalar(left, right);

        Assert.Equal(0, actual);
    }

    [Fact]
    public void ScalarProductIsEqualToScaledLengthForConstantVectors()
    {
        const Int32 size = 118;
        const Double scaling = 1.37;
        var ones = new MyVector(_ => 1, size);
        var scaled = new MyVector(_ => scaling, size);
        var expected = scaling * size;

        var actual = DotProduct.Scalar(scaled, ones);

        Assert.Equal(expected, actual, precision);
    }

    [Fact]
    public void UnrolledScalarComputesSameAsScalarVersion()
    {
        var expected = DotProduct.Scalar(vectorA, vectorB);

        var actual = DotProduct.UnrolledScalar(vectorA, vectorB);

        Assert.Equal(expected, actual);
    }
    [Fact]
    public void VectorizedComputesSameAsScalarVersion()
    {
        var expected = DotProduct.Scalar(vectorA, vectorB);

        var actual = DotProduct.Vectorized(vectorA, vectorB);

        Assert.Equal(expected, actual);
    }
}