# Performance Benchmarks with Benchmark.Net

## Resources

### Vectorization

- Microsoft [Vectorization Guidelines](https://github.com/dotnet/runtime/blob/main/docs/coding-guidelines/vectorization-guidelines.md)
- [Hardware Intrinsics in .Net Core](https://devblogs.microsoft.com/dotnet/hardware-intrinsics-in-net-core/)

### Benchmarking

- 

## Setup

Install the benchmarking template.

```shell
dotnet new install BenchmarkDotNet.Templates
```

Create the benchmarking class library.

```shell
dotnet new benchmark -n "Your Name"
```

When not using the template create an executable and reference Benchmark.Net

```shell
dotnet add package BenchmarkDotNet
```

Run your benchmarks

```shell
dotnet run -c Release --project path/to/your/benchmark.csproj
```
