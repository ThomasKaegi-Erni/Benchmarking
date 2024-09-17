using System.Runtime.Intrinsics;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Perfolizer.Horology;
using Atmoos.Sphere.BenchmarkDotNet;
using System.Reflection;

namespace Vectorization.Benchmark;

public class Program
{
    public static async Task Main(String[] args)
    {
        var assembly = typeof(Program).Assembly;
        var summary = Run(args, assembly, simple: true);
        await assembly.Export(summary);
    }

    private static IEnumerable<Summary> Run(String[] args, Assembly assembly, Boolean simple = true)
    {
        Job enough = Job.Default
                    .WithWarmupCount(3)
                    .WithIterationTime(TimeInterval.FromSeconds(0.25))
                    .WithMinIterationCount(5)
                    .WithMaxIterationCount(13);
        IConfig config = DefaultConfig.Instance.HideColumns(Column.EnvironmentVariables, Column.RatioSD, Column.Error);

        if (simple)
        {
            return BenchmarkSwitcher.FromAssembly(assembly).Run(args, config.AddJob(enough));
        }

        // set-up from: https://github.com/dotnet/runtime/blob/main/docs/coding-guidelines/vectorization-guidelines.md#benchmarking
        config = config.AddDiagnoser(new DisassemblyDiagnoser(new DisassemblyDiagnoserConfig
                (exportGithubMarkdown: true, printInstructionAddresses: false)))
            .AddJob(enough.WithEnvironmentVariable("DOTNET_EnableHWIntrinsic", "0").WithId("Scalar").AsBaseline());

        if (Vector256.IsHardwareAccelerated)
        {
            config = config
                .AddJob(enough.WithId("Vector256"))
                .AddJob(enough.WithEnvironmentVariable("DOTNET_EnableAVX2", "0").WithId("Vector128"));

        }
        else if (Vector128.IsHardwareAccelerated)
        {
            config = config.AddJob(enough.WithId("Vector128"));
        }

        return BenchmarkSwitcher.FromAssembly(assembly).Run(args, config);
    }
}
