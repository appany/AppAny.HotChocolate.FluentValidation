# Benchmarks

Feel free to optimize or add more benchmark test suites

**External libraries used:**

- [![Nuget](https://img.shields.io/nuget/v/FairyBread.svg)](https://www.nuget.org/packages/FairyBread) [FairyBread (5.1.0)](https://github.com/benmccallum/fairybread)
- [![Nuget](https://img.shields.io/nuget/v/FluentChoco.svg)](https://www.nuget.org/packages/FluentChoco) [FluentChoco (2.0.1)](https://github.com/dalrankov/FluentChoco)

```ini
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.746 (2004/?/20H1)
Intel Core i7-9700K CPU 3.60GHz (Coffee Lake), 1 CPU, 8 logical and 8 physical cores
.NET Core SDK=5.0.102
  [Host]     : .NET Core 5.0.2 (CoreCLR 5.0.220.61120, CoreFX 5.0.220.61120), X64 RyuJIT
  DefaultJob : .NET Core 5.0.2 (CoreCLR 5.0.220.61120, CoreFX 5.0.220.61120), X64 RyuJIT
```

## SingletonValidationBenchmarks

0.1.*
|                       Method |      Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|----------------------------- |----------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|         RunWithoutValidation |  8.235 μs | 0.0560 μs | 0.0524 μs |  0.33 |    0.00 | 1.2512 | 0.0153 |     - |   7.66 KB |
|            RunWithValidation | 24.593 μs | 0.2724 μs | 0.2548 μs |  1.00 |    0.00 | 1.7090 | 0.0305 |     - |  10.44 KB |
| RunWithFluentChocoValidation | 32.901 μs | 0.3530 μs | 0.3302 μs |  1.34 |    0.02 | 1.7700 |      - |     - |  11.12 KB |
|  RunWithFairyBreadValidation | 27.432 μs | 0.2594 μs | 0.2166 μs |  1.12 |    0.02 | 1.8616 | 0.0305 |     - |  11.51 KB |

0.2.*
|               Method |      Mean |     Error |    StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|--------------------- |----------:|----------:|----------:|------:|-------:|-------:|------:|----------:|
| RunWithoutValidation |  6.053 μs | 0.0929 μs | 0.0869 μs |  0.47 | 0.9003 | 0.0076 |     - |   5.52 KB |
|    RunWithValidation | 12.775 μs | 0.0864 μs | 0.0808 μs |  1.00 | 1.3733 | 0.0153 |     - |   8.43 KB |

## ScopedValidationBenchmarks

0.1.*
|                       Method |      Mean |     Error |    StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|----------------------------- |----------:|----------:|----------:|------:|-------:|-------:|------:|----------:|
|         RunWithoutValidation |  8.401 μs | 0.0429 μs | 0.0381 μs |  0.23 | 1.2512 | 0.0153 |     - |   7.66 KB |
|            RunWithValidation | 36.397 μs | 0.4875 μs | 0.4560 μs |  1.00 | 2.0752 | 0.0610 |     - |  12.67 KB |
| RunWithFluentChocoValidation | 40.018 μs | 0.1387 μs | 0.1230 μs |  1.10 | 2.1362 | 0.0610 |     - |  13.31 KB |
|  RunWithFairyBreadValidation | 40.941 μs | 0.3280 μs | 0.2908 μs |  1.12 | 2.6245 | 0.0610 |     - |  16.23 KB |

0.2.*
|               Method |      Mean |     Error |    StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|--------------------- |----------:|----------:|----------:|------:|-------:|-------:|------:|----------:|
| RunWithoutValidation |  5.761 μs | 0.0942 μs | 0.0835 μs |  0.31 | 0.9003 | 0.0076 |     - |   5.52 KB |
|    RunWithValidation | 18.540 μs | 0.2494 μs | 0.2333 μs |  1.00 | 1.7395 | 0.0305 |     - |  10.66 KB |

## EmptyInputsValidationBenchmarks

0.1.*
|                       Method |     Mean |     Error |    StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|----------------------------- |---------:|----------:|----------:|------:|-------:|-------:|------:|----------:|
|         RunWithoutValidation | 8.329 μs | 0.0600 μs | 0.0532 μs |  1.03 | 1.2512 | 0.0153 |     - |   7.64 KB |
|            RunWithValidation | 8.108 μs | 0.0346 μs | 0.0306 μs |  1.00 | 1.2512 | 0.0153 |     - |   7.64 KB |
| RunWithFluentChocoValidation | 8.139 μs | 0.0759 μs | 0.0710 μs |  1.00 | 1.2512 | 0.0153 |     - |   7.64 KB |
|  RunWithFairyBreadValidation | 9.369 μs | 0.0695 μs | 0.0617 μs |  1.16 | 1.2970 | 0.0153 |     - |   7.93 KB |


0.2.*
|               Method |     Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|--------------------- |---------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
| RunWithoutValidation | 5.568 μs | 0.0716 μs | 0.0670 μs |  0.96 |    0.02 | 0.8926 | 0.0076 |     - |   5.51 KB |
|    RunWithValidation | 5.782 μs | 0.0630 μs | 0.0559 μs |  1.00 |    0.00 | 0.8926 | 0.0076 |     - |   5.51 KB |

## NullInputsValidationBenchmarks

0.1.*
|                      Method |     Mean |     Error |    StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |---------:|----------:|----------:|------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation | 7.643 μs | 0.0495 μs | 0.0463 μs |  0.90 | 1.2512 | 0.0153 |     - |   7.65 KB |
|           RunWithValidation | 8.508 μs | 0.0608 μs | 0.0539 μs |  1.00 | 1.2512 | 0.0153 |     - |   7.65 KB |
| RunWithFairyBreadValidation | 8.700 μs | 0.0609 μs | 0.0570 μs |  1.02 | 1.2970 | 0.0153 |     - |   7.94 KB |

0.2.*
|               Method |     Mean |     Error |    StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|--------------------- |---------:|----------:|----------:|------:|-------:|-------:|------:|----------:|
| RunWithoutValidation | 5.669 μs | 0.1108 μs | 0.0982 μs |  0.87 | 0.9003 | 0.0076 |     - |   5.52 KB |
|    RunWithValidation | 6.490 μs | 0.0757 μs | 0.0708 μs |  1.00 | 0.9003 | 0.0076 |     - |   5.52 KB |
