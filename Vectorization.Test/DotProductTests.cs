namespace Vectorization.Test;

public class DotProductTests
{
    const Int32 precision = 4;
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
        const Single scaling = 1.367f;
        var ones = new MyVector(_ => 1, size);
        var scaled = new MyVector(_ => scaling, size);
        var expected = scaling * size;

        var actual = DotProduct.Scalar(scaled, ones);

        Assert.Equal(expected, actual, precision);
    }

    [Fact]
    public void RecursiveIsMorePreciseThanOtherDotProducts()
    {
        const Int32 size = 118;
        const Single scaling = 1.367f;
        var ones = new MyVector(_ => 1, size);
        var scaled = new MyVector(_ => scaling, size);
        var expected = scaling * size;

        var actual = DotProduct.RecursiveScalar(scaled, ones);

        Assert.Equal(expected, actual); // i.e. no tolerance required :-)
    }

    [Fact]
    public void VectorizedRecursiveIsMorePreciseThanOtherDotProducts()
    {
        const Int32 size = 118;
        const Single scaling = 1.367f;
        var ones = new MyVector(_ => 1, size);
        var scaled = new MyVector(_ => scaling, size);
        var expected = scaling * size;

        var actual = DotProduct.RecursiveVectorized128(scaled, ones);

        Assert.Equal(expected, actual); // i.e. no tolerance required :-)
    }

    [Fact]
    public void UnrolledScalarComputesSameAsScalarVersion()
    {
        ComparisonToScalar((l, r) => DotProduct.UnrolledScalar(l, r));
    }

    [Fact]
    public void GenericScalarComputesSameAsScalarVersion()
    {
        ComparisonToScalar((l, r) => DotProduct.GenericScalar<Single>(l, r));
    }

    [Fact]
    public void ScalarFusedMultiplyComputesSameAsScalarVersion()
    {
        ComparisonToScalar((l, r) => DotProduct.FusedScalar(l, r));
    }

    [Fact]
    public void VectorizedComputesSameAsScalarVersion()
    {
        ComparisonToScalar((l, r) => DotProduct.Vectorized(l, r));
    }

    [Fact]
    public void Vectorized128ComputesSameAsScalarVersion()
    {
        ComparisonToScalar((l, r) => DotProduct.Vectorized128(l, r));
    }

    [Fact]
    public void Vectorized256ComputesSameAsScalarVersion()
    {
        ComparisonToScalar((l, r) => DotProduct.Vectorized256(l, r));
    }

    [Fact]
    public void RecursiveVectorized128ComputesSameAsScalarVersion()
    {
        ComparisonToScalar((l, r) => DotProduct.RecursiveVectorized128(l, r));
    }

    public static void ComparisonToScalar(Func<MyVector, MyVector, Single> dotProduct)
    {
        var expected = DotProduct.Scalar(vectorA, vectorB);

        var actual = dotProduct(vectorA, vectorB);

        Assert.Equal(expected, actual);
    }
}