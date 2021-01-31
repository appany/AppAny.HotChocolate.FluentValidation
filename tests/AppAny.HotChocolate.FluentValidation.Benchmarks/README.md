# Benchmarks

Feel free to optimize or add more benchmark test suites

**External libraries used:**

- [![Nuget](https://img.shields.io/nuget/v/FairyBread.svg)](https://www.nuget.org/packages/FairyBread) [FairyBread (4.0.0)](https://github.com/benmccallum/fairybread)
- [![Nuget](https://img.shields.io/nuget/v/FluentChoco.svg)](https://www.nuget.org/packages/FluentChoco) [FluentChoco (2.0.0)](https://github.com/dalrankov/FluentChoco)

```ini
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.746 (2004/?/20H1)
Intel Core i7-9700K CPU 3.60GHz (Coffee Lake), 1 CPU, 8 logical and 8 physical cores
.NET Core SDK=5.0.102
  [Host]     : .NET Core 5.0.2 (CoreCLR 5.0.220.61120, CoreFX 5.0.220.61120), X64 RyuJIT
  DefaultJob : .NET Core 5.0.2 (CoreCLR 5.0.220.61120, CoreFX 5.0.220.61120), X64 RyuJIT
```

## ValidationBenchmarks

|                      Method |      Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |----------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation |  8.069 μs | 0.0582 μs | 0.0516 μs |  0.21 |    0.00 | 1.2512 | 0.0153 |     - |   7.66 KB |
|           RunWithValidation | 37.705 μs | 0.2828 μs | 0.2645 μs |  1.00 |    0.00 | 2.1973 | 0.0610 |     - |  13.44 KB |
|         Broken since 11.0.8 | 98.943 μs | 1.1200 μs | 1.0477 μs |  2.62 |    0.04 | 2.0752 |      - |     - |  13.06 KB |
| RunWithFairyBreadValidation | 38.881 μs | 0.3862 μs | 0.3613 μs |  1.03 |    0.01 | 2.6245 | 0.0610 |     - |  16.23 KB |

## ExplicitValidationBenchmarks

|                      Method |      Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |----------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation |  8.027 μs | 0.0564 μs | 0.0527 μs |  0.22 |    0.00 | 1.2512 | 0.0153 |     - |   7.66 KB |
|           RunWithValidation | 37.233 μs | 0.4494 μs | 0.4204 μs |  1.00 |    0.00 | 2.0752 | 0.0610 |     - |  12.92 KB |
|         Broken since 11.0.8 | 98.576 μs | 0.5300 μs | 0.4426 μs |  2.64 |    0.03 | 2.0752 |      - |     - |  13.06 KB |
| RunWithFairyBreadValidation | 39.021 μs | 0.2512 μs | 0.2227 μs |  1.05 |    0.02 | 2.6245 | 0.0610 |     - |  16.23 KB |

## InputValidatorBenchmarks

|                   Method |     Mean |   Error |  StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|------------------------- |---------:|--------:|--------:|------:|-------:|-------:|------:|----------:|
|               Validation | 840.8 ns | 1.16 ns | 1.08 ns |  1.00 | 0.3157 | 0.0010 |     - |   1.94 KB |
| InputValidatorValidation | 943.0 ns | 2.55 ns | 2.39 ns |  1.12 | 0.3538 | 0.0010 |     - |   2.17 KB |

## EmptyInputsValidationBenchmarks

|                      Method |      Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |----------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation |  7.696 μs | 0.0668 μs | 0.0625 μs |  0.94 |    0.01 | 1.2512 | 0.0153 |     - |   7.64 KB |
|           RunWithValidation |  8.171 μs | 0.0767 μs | 0.0717 μs |  1.00 |    0.00 | 1.2512 | 0.0153 |     - |   7.64 KB |
|         Broken since 11.0.8 |  7.826 μs | 0.0621 μs | 0.0551 μs |  0.96 |    0.01 | 1.2512 | 0.0153 |     - |   7.64 KB |
| RunWithFairyBreadValidation | 22.966 μs | 0.1634 μs | 0.1448 μs |  2.81 |    0.03 | 1.6785 | 0.0305 |     - |  10.29 KB |

## EmptyInputsExplicitValidationBenchmarks

|                      Method |      Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |----------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation |  7.658 μs | 0.0677 μs | 0.1255 μs |  0.99 |    0.01 | 1.2512 | 0.0153 |     - |   7.64 KB |
|           RunWithValidation |  7.876 μs | 0.1034 μs | 0.0967 μs |  1.00 |    0.00 | 1.2512 | 0.0153 |     - |   7.64 KB |
|         Broken since 11.0.8 |  7.758 μs | 0.0394 μs | 0.0368 μs |  0.99 |    0.01 | 1.2512 | 0.0153 |     - |   7.64 KB |
| RunWithFairyBreadValidation | 24.368 μs | 0.3052 μs | 0.2855 μs |  3.09 |    0.04 | 1.6785 | 0.0305 |     - |   10.3 KB |

## NullInputsValidationBenchmarks

|                      Method |      Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |----------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation |  7.643 μs | 0.0769 μs | 0.0719 μs |  0.84 |    0.01 | 1.2512 | 0.0153 |     - |   7.65 KB |
|           RunWithValidation |  9.111 μs | 0.0470 μs | 0.0440 μs |  1.00 |    0.00 | 1.2665 | 0.0153 |     - |   7.81 KB |
|         Broken since 11.0.8 | 95.787 μs | 0.9526 μs | 0.8445 μs | 10.52 |    0.12 | 2.0752 |      - |     - |  12.99 KB |
| RunWithFairyBreadValidation | 23.560 μs | 0.1873 μs | 0.1752 μs |  2.59 |    0.02 | 1.6785 | 0.0305 |     - |   10.3 KB |

## NullInputsExplicitValidationBenchmarks

|                      Method |      Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |----------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation |  7.878 μs | 0.0571 μs | 0.0477 μs |  0.83 |    0.01 | 1.2512 | 0.0153 |     - |   7.65 KB |
|           RunWithValidation |  9.444 μs | 0.0600 μs | 0.0562 μs |  1.00 |    0.00 | 1.2665 | 0.0153 |     - |   7.81 KB |
|         Broken since 11.0.8 | 98.533 μs | 0.3496 μs | 0.3270 μs | 10.43 |    0.09 | 2.0752 |      - |     - |  12.99 KB |
| RunWithFairyBreadValidation | 23.837 μs | 0.1461 μs | 0.1295 μs |  2.52 |    0.02 | 1.6785 | 0.0305 |     - |   10.3 KB |

## ErrorMappersBenchmarks

|                                       Method |     Mean |    Error |   StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|--------------------------------------------- |---------:|---------:|---------:|------:|-------:|-------:|------:|----------:|
|                    RunWithDefaultErrorMapper | 38.23 μs | 0.329 μs | 0.308 μs |  1.00 | 2.1973 | 0.0610 |     - |  13.44 KB |
|         RunWithDefaultErrorMapperWithDetails | 39.22 μs | 0.480 μs | 0.449 μs |  1.03 | 2.2583 | 0.0610 |     - |  13.88 KB |
| RunWithDefaultErrorMapperWithExtendedDetails | 39.34 μs | 0.454 μs | 0.424 μs |  1.03 | 2.3804 | 0.0610 |     - |  14.39 KB |
