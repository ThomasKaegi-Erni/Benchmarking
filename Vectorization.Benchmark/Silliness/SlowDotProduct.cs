using System.Runtime.Intrinsics;

namespace Vectorization.Benchmark.Silliness;

public static class SlowDotProduct
{
    static readonly String filename = Path.Combine(Path.GetTempPath(), "dump.txt");

    public static Single RecursiveVectorized128(in ReadOnlySpan<Single> left, in ReadOnlySpan<Single> right)
    {
        if (File.Exists(filename)) {
            File.Delete(filename);
        }
        return Vector128.Sum(Dump(Recursive(in left, in right)));
    }

    private static Vector128<Single> Recursive(in ReadOnlySpan<Single> l, in ReadOnlySpan<Single> r) => l.Length switch {
        0 => Vector128<Single>.Zero,
        1 => Vector128.Create(l[0] * r[0], 0f, 0f, 0f),
        2 => Vector128.Create(l[0] * r[0], l[1] * r[1], 0f, 0f),
        3 => Vector128.Create(l[0], l[1], l[2], 0f) * Vector128.Create(r[0], r[1], r[2], 0f),
        4 => Vector128.Create(l[0], l[1], l[2], l[3]) * Vector128.Create(r[0], r[1], r[2], r[3]),
        var n => Recursive(l[..(n / 2)], r[..(n / 2)]) + Recursive(l[(n / 2)..], r[(n / 2)..])
    };

    private static Vector128<Single> Dump(Vector128<Single> vector)
    {
        using var text = File.AppendText(filename);
        text.WriteLine(vector);
        return vector;
    }
}
