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
|        RunWithoutValidation |  7.841 μs | 0.0511 μs | 0.0478 μs |  0.33 |    0.00 | 1.2512 | 0.0153 |     - |   7.66 KB |
|           RunWithValidation | 23.724 μs | 0.3094 μs | 0.2894 μs |  1.00 |    0.00 | 1.7090 | 0.0305 |     - |  10.44 KB |
| RunWithFairyBreadValidation | 27.090 μs | 0.1547 μs | 0.1447 μs |  1.14 |    0.02 | 1.8616 | 0.0305 |     - |  11.51 KB |

## ScopedValidationBenchmarks

|                      Method |      Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |----------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation |  8.064 μs | 0.0755 μs | 0.0669 μs |  0.23 |    0.00 | 1.2512 | 0.0153 |     - |   7.66 KB |
|           RunWithValidation | 34.893 μs | 0.6829 μs | 0.6707 μs |  1.00 |    0.00 | 2.0752 |      - |     - |  12.67 KB |
| RunWithFairyBreadValidation | 38.008 μs | 0.2207 μs | 0.1956 μs |  1.09 |    0.02 | 2.6245 | 0.0610 |     - |  16.23 KB |

## EmptyInputsValidationBenchmarks

|                      Method |     Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |---------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation | 7.763 μs | 0.0540 μs | 0.0505 μs |  0.98 |    0.01 | 1.2512 | 0.0153 |     - |   7.64 KB |
|           RunWithValidation | 7.905 μs | 0.0861 μs | 0.0805 μs |  1.00 |    0.00 | 1.2512 | 0.0153 |     - |   7.64 KB |
| RunWithFairyBreadValidation | 9.039 μs | 0.1150 μs | 0.1076 μs |  1.14 |    0.02 | 1.2970 | 0.0153 |     - |   7.93 KB |

## NullInputsValidationBenchmarks

|                      Method |     Mean |     Error |    StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |---------:|----------:|----------:|------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation | 7.878 μs | 0.0423 μs | 0.0395 μs |  0.91 | 1.2512 | 0.0153 |     - |   7.65 KB |
|           RunWithValidation | 8.630 μs | 0.0676 μs | 0.0599 μs |  1.00 | 1.2512 | 0.0153 |     - |   7.65 KB |
| RunWithFairyBreadValidation | 8.860 μs | 0.1183 μs | 0.1107 μs |  1.03 | 1.2970 | 0.0153 |     - |   7.94 KB |
