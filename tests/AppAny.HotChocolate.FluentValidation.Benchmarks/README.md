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
|        RunWithoutValidation |  7.867 μs | 0.0654 μs | 0.0611 μs |  0.34 |    0.00 | 1.2512 | 0.0153 |     - |   7.66 KB |
|           RunWithValidation | 23.161 μs | 0.0769 μs | 0.0682 μs |  1.00 |    0.00 | 1.7090 | 0.0305 |     - |  10.44 KB |
| RunWithFairyBreadValidation | 27.293 μs | 0.5176 μs | 0.5084 μs |  1.18 |    0.02 | 1.8616 | 0.0305 |     - |   11.5 KB |

## ScopedValidationBenchmarks

|                      Method |      Mean |     Error |    StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |----------:|----------:|----------:|------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation |  8.375 μs | 0.0701 μs | 0.0547 μs |  0.25 | 1.2512 | 0.0153 |     - |   7.66 KB |
|           RunWithValidation | 34.070 μs | 0.3079 μs | 0.2880 μs |  1.00 | 2.0752 | 0.0610 |     - |  12.67 KB |
| RunWithFairyBreadValidation | 37.546 μs | 0.3047 μs | 0.2701 μs |  1.10 | 2.6245 | 0.0610 |     - |  16.23 KB |

## EmptyInputsValidationBenchmarks

|                      Method |     Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |---------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation | 7.887 μs | 0.1505 μs | 0.1257 μs |  0.99 |    0.02 | 1.2512 | 0.0153 |     - |   7.64 KB |
|           RunWithValidation | 7.968 μs | 0.0712 μs | 0.0631 μs |  1.00 |    0.00 | 1.2512 | 0.0153 |     - |   7.64 KB |
| RunWithFairyBreadValidation | 8.926 μs | 0.0845 μs | 0.0749 μs |  1.12 |    0.01 | 1.2970 | 0.0153 |     - |   7.93 KB |

## NullInputsValidationBenchmarks

|                      Method |     Mean |     Error |    StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |---------:|----------:|----------:|------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation | 7.643 μs | 0.0495 μs | 0.0463 μs |  0.90 | 1.2512 | 0.0153 |     - |   7.65 KB |
|           RunWithValidation | 8.508 μs | 0.0608 μs | 0.0539 μs |  1.00 | 1.2512 | 0.0153 |     - |   7.65 KB |
| RunWithFairyBreadValidation | 8.700 μs | 0.0609 μs | 0.0570 μs |  1.02 | 1.2970 | 0.0153 |     - |   7.94 KB |
