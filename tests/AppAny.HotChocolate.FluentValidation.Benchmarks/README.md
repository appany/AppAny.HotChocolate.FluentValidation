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

|                      Method |      Mean |     Error |    StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |----------:|----------:|----------:|------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation |  8.267 μs | 0.1551 μs | 0.1375 μs |  0.33 | 1.2512 | 0.0153 |     - |   7.66 KB |
|           RunWithValidation | 24.994 μs | 0.2869 μs | 0.2543 μs |  1.00 | 1.7090 | 0.0305 |     - |  10.44 KB |
| RunWithFairyBreadValidation | 26.924 μs | 0.2665 μs | 0.2225 μs |  1.08 | 1.8616 | 0.0305 |     - |   11.5 KB |

## ScopedValidationBenchmarks

|                      Method |      Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |----------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation |  8.375 μs | 0.1190 μs | 0.1055 μs |  0.22 |    0.00 | 1.2512 | 0.0153 |     - |   7.66 KB |
|           RunWithValidation | 37.700 μs | 0.3985 μs | 0.3727 μs |  1.00 |    0.00 | 2.0752 | 0.0610 |     - |  12.67 KB |
| RunWithFairyBreadValidation | 42.125 μs | 0.3202 μs | 0.2674 μs |  1.12 |    0.02 | 2.6245 | 0.0610 |     - |  16.23 KB |

## EmptyInputsValidationBenchmarks

|                      Method |     Mean |     Error |    StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |---------:|----------:|----------:|------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation | 8.148 μs | 0.0919 μs | 0.0860 μs |  0.96 | 1.2512 | 0.0153 |     - |   7.64 KB |
|           RunWithValidation | 8.509 μs | 0.0709 μs | 0.0663 μs |  1.00 | 1.2512 | 0.0153 |     - |   7.64 KB |
| RunWithFairyBreadValidation | 9.530 μs | 0.0441 μs | 0.0344 μs |  1.12 | 1.2970 | 0.0153 |     - |   7.93 KB |

## NullInputsValidationBenchmarks

|                      Method |     Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |---------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation | 8.395 μs | 0.0370 μs | 0.0346 μs |  0.94 |    0.01 | 1.2512 | 0.0153 |     - |   7.65 KB |
|           RunWithValidation | 8.924 μs | 0.0637 μs | 0.0565 μs |  1.00 |    0.00 | 1.2512 | 0.0153 |     - |   7.65 KB |
| RunWithFairyBreadValidation | 9.828 μs | 0.1939 μs | 0.2961 μs |  1.09 |    0.04 | 1.2970 | 0.0153 |     - |   7.94 KB |
