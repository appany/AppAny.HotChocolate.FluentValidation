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

|                      Method |       Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |-----------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation |   8.179 μs | 0.0574 μs | 0.0509 μs |  0.20 |    0.00 | 1.2512 | 0.0153 |     - |   7.66 KB |
|           RunWithValidation |  40.715 μs | 0.1550 μs | 0.1450 μs |  1.00 |    0.00 | 2.1973 | 0.0610 |     - |  13.44 KB |
|         Broken since 11.0.8 | 106.753 μs | 1.1093 μs | 1.0377 μs |  2.62 |    0.03 | 2.0752 |      - |     - |  13.06 KB |
| RunWithFairyBreadValidation |  41.623 μs | 0.3184 μs | 0.2979 μs |  1.02 |    0.01 | 2.6245 | 0.0610 |     - |  16.23 KB |

## ExplicitValidationBenchmarks

|                      Method |       Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |-----------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation |   8.246 μs | 0.0284 μs | 0.0266 μs |  0.22 |    0.00 | 1.2512 | 0.0153 |     - |   7.66 KB |
|           RunWithValidation |  37.837 μs | 0.2270 μs | 0.2123 μs |  1.00 |    0.00 | 2.0752 | 0.0610 |     - |  12.92 KB |
|         Broken since 11.0.8 | 108.000 μs | 0.3741 μs | 0.3316 μs |  2.86 |    0.02 | 2.0752 |      - |     - |  13.06 KB |
| RunWithFairyBreadValidation |  41.671 μs | 0.2261 μs | 0.2115 μs |  1.10 |    0.01 | 2.6245 | 0.0610 |     - |  16.23 KB |

## InputValidatorBenchmarks

|                   Method |       Mean |   Error |  StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|------------------------- |-----------:|--------:|--------:|------:|-------:|-------:|------:|----------:|
|               Validation |   918.5 ns | 7.24 ns | 6.77 ns |  1.00 | 0.3157 | 0.0010 |     - |   1.94 KB |
| InputValidatorValidation | 1,032.9 ns | 3.19 ns | 2.82 ns |  1.12 | 0.3529 |      - |     - |   2.17 KB |

## EmptyInputsValidationBenchmarks

|                      Method |      Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |----------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation |  8.077 μs | 0.0339 μs | 0.0317 μs |  0.99 |    0.01 | 1.2512 | 0.0153 |     - |   7.64 KB |
|           RunWithValidation |  8.156 μs | 0.0532 μs | 0.0498 μs |  1.00 |    0.00 | 1.2512 | 0.0153 |     - |   7.64 KB |
|         Broken since 11.0.8 |  8.305 μs | 0.0437 μs | 0.0409 μs |  1.02 |    0.01 | 1.2512 | 0.0153 |     - |   7.64 KB |
| RunWithFairyBreadValidation | 24.234 μs | 0.3514 μs | 0.3287 μs |  2.97 |    0.05 | 1.6785 | 0.0305 |     - |  10.29 KB |

## EmptyInputsExplicitValidationBenchmarks

|                      Method |      Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |----------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation |  8.189 μs | 0.0434 μs | 0.0406 μs |  1.00 |    0.01 | 1.2512 | 0.0153 |     - |   7.64 KB |
|           RunWithValidation |  8.201 μs | 0.0488 μs | 0.0432 μs |  1.00 |    0.00 | 1.2512 | 0.0153 |     - |   7.64 KB |
|         Broken since 11.0.8 |  8.205 μs | 0.0422 μs | 0.0395 μs |  1.00 |    0.00 | 1.2512 | 0.0153 |     - |   7.64 KB |
| RunWithFairyBreadValidation | 24.677 μs | 0.2123 μs | 0.1773 μs |  3.01 |    0.03 | 1.6785 | 0.0305 |     - |  10.29 KB |

## NullInputsValidationBenchmarks

|                      Method |       Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |-----------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation |   8.434 μs | 0.0399 μs | 0.0373 μs |  0.90 |    0.01 | 1.2512 | 0.0153 |     - |   7.65 KB |
|           RunWithValidation |   9.320 μs | 0.0486 μs | 0.0430 μs |  1.00 |    0.00 | 1.2665 | 0.0153 |     - |   7.81 KB |
|         Broken since 11.0.8 | 104.801 μs | 0.6465 μs | 0.6047 μs | 11.24 |    0.09 | 2.0752 |      - |     - |  12.99 KB |
| RunWithFairyBreadValidation |  24.447 μs | 0.3673 μs | 0.3436 μs |  2.63 |    0.04 | 1.6785 | 0.0305 |     - |   10.3 KB |

## NullInputsExplicitValidationBenchmarks

|                      Method |       Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |-----------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation |   8.247 μs | 0.0629 μs | 0.0588 μs |  0.87 |    0.01 | 1.2512 | 0.0153 |     - |   7.65 KB |
|           RunWithValidation |   9.448 μs | 0.0638 μs | 0.0597 μs |  1.00 |    0.00 | 1.2665 | 0.0153 |     - |   7.81 KB |
|         Broken since 11.0.8 | 102.742 μs | 0.3604 μs | 0.3009 μs | 10.88 |    0.05 | 2.0752 |      - |     - |  12.99 KB |
| RunWithFairyBreadValidation |  24.593 μs | 0.3060 μs | 0.2862 μs |  2.60 |    0.04 | 1.6785 | 0.0305 |     - |   10.3 KB |

## ErrorMappersBenchmarks

|                                       Method |     Mean |    Error |   StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|--------------------------------------------- |---------:|---------:|---------:|------:|-------:|-------:|------:|----------:|
|                    RunWithDefaultErrorMapper | 40.82 μs | 0.223 μs | 0.209 μs |  1.00 | 2.1973 | 0.0610 |     - |  13.44 KB |
|         RunWithDefaultErrorMapperWithDetails | 41.63 μs | 0.198 μs | 0.185 μs |  1.02 | 2.2583 | 0.0610 |     - |  13.88 KB |
| RunWithDefaultErrorMapperWithExtendedDetails | 41.93 μs | 0.296 μs | 0.277 μs |  1.03 | 2.3804 | 0.0610 |     - |  14.39 KB |
