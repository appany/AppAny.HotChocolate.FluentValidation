# Benchmarks

Feel free to optimize or add more benchmark test suites

**External libraries used:**

- [![Nuget](https://img.shields.io/nuget/v/FairyBread.svg)](https://www.nuget.org/packages/FairyBread) [FairyBread (4.0.0)](https://github.com/benmccallum/fairybread)
- [![Nuget](https://img.shields.io/nuget/v/FluentChoco.svg)](https://www.nuget.org/packages/FluentChoco) [FluentChoco (Broken since 11.0.8)](https://github.com/dalrankov/FluentChoco)

```ini
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.746 (2004/?/20H1)
Intel Core i7-9700K CPU 3.60GHz (Coffee Lake), 1 CPU, 8 logical and 8 physical cores
.NET Core SDK=5.0.102
  [Host]     : .NET Core 5.0.2 (CoreCLR 5.0.220.61120, CoreFX 5.0.220.61120), X64 RyuJIT
  DefaultJob : .NET Core 5.0.2 (CoreCLR 5.0.220.61120, CoreFX 5.0.220.61120), X64 RyuJIT
```

## SingletonValidationBenchmarks

|                      Method |      Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |----------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation |  7.961 μs | 0.0383 μs | 0.0358 μs |  0.33 |    0.00 | 1.2512 | 0.0153 |     - |   7.66 KB |
|           RunWithValidation | 24.140 μs | 0.2255 μs | 0.2109 μs |  1.00 |    0.00 | 1.7090 | 0.0305 |     - |  10.44 KB |
| RunWithFairyBreadValidation | 25.801 μs | 0.3395 μs | 0.3010 μs |  1.07 |    0.02 | 1.8616 | 0.0305 |     - |   11.5 KB |

## ScopedValidationBenchmarks

|                      Method |      Mean |     Error |    StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |----------:|----------:|----------:|------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation |  8.112 μs | 0.0641 μs | 0.0569 μs |  0.23 | 1.2512 | 0.0153 |     - |   7.66 KB |
|           RunWithValidation | 34.869 μs | 0.1993 μs | 0.1766 μs |  1.00 | 2.0752 | 0.0610 |     - |  12.67 KB |
| RunWithFairyBreadValidation | 39.196 μs | 0.3058 μs | 0.2711 μs |  1.12 | 2.6245 | 0.0610 |     - |  16.23 KB |

## EmptyInputsValidationBenchmarks

|                      Method |     Mean |     Error |    StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |---------:|----------:|----------:|------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation | 7.983 μs | 0.0794 μs | 0.0743 μs |  0.99 | 1.2512 | 0.0153 |     - |   7.64 KB |
|           RunWithValidation | 8.069 μs | 0.0555 μs | 0.0519 μs |  1.00 | 1.2512 | 0.0153 |     - |   7.64 KB |
| RunWithFairyBreadValidation | 9.020 μs | 0.0870 μs | 0.0814 μs |  1.12 | 1.2970 | 0.0153 |     - |   7.93 KB |

## NullInputsValidationBenchmarks

|                      Method |     Mean |     Error |    StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |---------:|----------:|----------:|------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation | 7.943 μs | 0.0745 μs | 0.0696 μs |  0.93 | 1.2512 | 0.0153 |     - |   7.65 KB |
|           RunWithValidation | 8.520 μs | 0.0366 μs | 0.0325 μs |  1.00 | 1.2512 | 0.0153 |     - |   7.65 KB |
| RunWithFairyBreadValidation | 9.157 μs | 0.0487 μs | 0.0431 μs |  1.07 | 1.2970 | 0.0153 |     - |   7.94 KB |
