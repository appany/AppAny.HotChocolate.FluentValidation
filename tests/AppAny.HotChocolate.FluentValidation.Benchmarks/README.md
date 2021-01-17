# Benchmarks

Feel free to optimize or add more benchmark test suites

**External libraries used:**

- [![Nuget](https://img.shields.io/nuget/v/FairyBread.svg)](https://www.nuget.org/packages/FairyBread) [FairyBread (3.0.0)](https://github.com/benmccallum/fairybread)
- [![Nuget](https://img.shields.io/nuget/v/FluentChoco.svg)](https://www.nuget.org/packages/FluentChoco) [FluentChoco (2.0.0)](https://github.com/dalrankov/FluentChoco)

```ini
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.746 (2004/?/20H1)
Intel Core i7-9700K CPU 3.60GHz (Coffee Lake), 1 CPU, 8 logical and 8 physical cores
.NET Core SDK=5.0.101
  [Host]     : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT
  DefaultJob : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT
```

## ValidationBenchmarks

|                       Method |      Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|----------------------------- |----------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|         RunWithoutValidation |  8.178 μs | 0.0584 μs | 0.0546 μs |  0.32 |    0.00 | 1.2512 | 0.0153 |     - |   7.66 KB |
|            RunWithValidation | 25.933 μs | 0.2294 μs | 0.2146 μs |  1.00 |    0.00 | 1.7700 | 0.0305 |     - |  10.89 KB |
| RunWithFluentChocoValidation | 29.365 μs | 0.3242 μs | 0.3032 μs |  1.13 |    0.02 | 1.8005 | 0.0305 |     - |  11.12 KB |
|  RunWithFairyBreadValidation | 70.872 μs | 0.3726 μs | 0.3486 μs |  2.73 |    0.02 | 2.8076 |      - |     - |   17.1 KB |

## ExplicitValidationBenchmarks

|                       Method |      Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|----------------------------- |----------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|         RunWithoutValidation |  7.990 μs | 0.0734 μs | 0.0686 μs |  0.36 |    0.00 | 1.2512 | 0.0153 |     - |   7.66 KB |
|            RunWithValidation | 22.330 μs | 0.1025 μs | 0.0909 μs |  1.00 |    0.00 | 1.7395 | 0.0305 |     - |  10.69 KB |
| RunWithFluentChocoValidation | 29.843 μs | 0.3016 μs | 0.2821 μs |  1.34 |    0.01 | 1.7700 |      - |     - |  11.11 KB |
|  RunWithFairyBreadValidation | 70.974 μs | 0.6976 μs | 0.6525 μs |  3.18 |    0.03 | 2.8076 |      - |     - |   17.1 KB |

## InputValidatorBenchmarks

|                   Method |     Mean |   Error |  StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|------------------------- |---------:|--------:|--------:|------:|-------:|-------:|------:|----------:|
|               Validation | 849.1 ns | 4.06 ns | 3.80 ns |  1.00 | 0.3157 | 0.0010 |     - |   1.94 KB |
| InputValidatorValidation | 986.0 ns | 1.60 ns | 1.49 ns |  1.16 | 0.3529 |      - |     - |   2.17 KB |

## EmptyInputsValidationBenchmarks

|                                             Method |      Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|--------------------------------------------------- |----------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|                               RunWithoutValidation |  7.887 μs | 0.0463 μs | 0.0433 μs |  1.00 |    0.01 | 1.2512 | 0.0153 |     - |   7.64 KB |
|                                  RunWithValidation |  7.894 μs | 0.0547 μs | 0.0512 μs |  1.00 |    0.00 | 1.2512 | 0.0153 |     - |   7.64 KB |
|                       RunWithFluentChocoValidation |  8.096 μs | 0.0581 μs | 0.0543 μs |  1.03 |    0.01 | 1.2512 | 0.0153 |     - |   7.64 KB |
|   No checks for empty input. Validator is throwing | 67.208 μs | 0.3890 μs | 0.3448 μs |  8.51 |    0.06 | 2.4414 |      - |     - |  15.09 KB |

## EmptyInputsExplicitValidationBenchmarks

|                                             Method |      Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|--------------------------------------------------- |----------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|                               RunWithoutValidation |  8.205 μs | 0.0679 μs | 0.0636 μs |  1.01 |    0.01 | 1.2512 | 0.0153 |     - |   7.64 KB |
|                                  RunWithValidation |  8.107 μs | 0.0528 μs | 0.0494 μs |  1.00 |    0.00 | 1.2512 | 0.0153 |     - |   7.64 KB |
|                       RunWithFluentChocoValidation |  8.219 μs | 0.0478 μs | 0.0447 μs |  1.01 |    0.01 | 1.2512 | 0.0153 |     - |   7.64 KB |
|   No checks for empty input. Validator is throwing | 68.551 μs | 0.5422 μs | 0.5071 μs |  8.46 |    0.09 | 2.4414 |      - |     - |  15.09 KB |

## NullInputsValidationBenchmarks

|                                            Method |      Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|-------------------------------------------------- |----------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|                              RunWithoutValidation |  8.117 μs | 0.0644 μs | 0.0603 μs |  0.88 |    0.01 | 1.2512 | 0.0153 |     - |   7.65 KB |
|                                 RunWithValidation |  9.212 μs | 0.0583 μs | 0.0545 μs |  1.00 |    0.00 | 1.2665 | 0.0153 |     - |   7.81 KB |
|                      RunWithFluentChocoValidation |  9.323 μs | 0.0545 μs | 0.0510 μs |  1.01 |    0.01 | 1.2665 | 0.0153 |     - |   7.77 KB |
|   No checks for null input. Validator is throwing | 69.169 μs | 0.2876 μs | 0.2550 μs |  7.51 |    0.05 | 2.4414 |      - |     - |  15.09 KB |

## NullInputsExplicitValidationBenchmarks

|                                            Method |      Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|-------------------------------------------------- |----------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|                              RunWithoutValidation |  8.018 μs | 0.0376 μs | 0.0351 μs |  0.91 |    0.00 | 1.2512 | 0.0153 |     - |   7.65 KB |
|                                 RunWithValidation |  8.770 μs | 0.0554 μs | 0.0518 μs |  1.00 |    0.00 | 1.2665 | 0.0153 |     - |   7.81 KB |
|                      RunWithFluentChocoValidation |  9.358 μs | 0.0820 μs | 0.0767 μs |  1.07 |    0.01 | 1.2665 | 0.0153 |     - |   7.77 KB |
|   No checks for null input. Validator is throwing | 68.021 μs | 0.4024 μs | 0.3764 μs |  7.76 |    0.03 | 2.4414 |      - |     - |  15.09 KB |

## ErrorMappersBenchmarks

|                                       Method |     Mean |    Error |   StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|--------------------------------------------- |---------:|---------:|---------:|------:|--------:|-------:|-------:|------:|----------:|
|                    RunWithDefaultErrorMapper | 28.10 μs | 0.395 μs | 0.350 μs |  1.00 |    0.00 | 1.7700 | 0.0305 |     - |  10.89 KB |
|         RunWithDefaultErrorMapperWithDetails | 29.56 μs | 0.211 μs | 0.197 μs |  1.05 |    0.01 | 1.8616 | 0.0305 |     - |  11.33 KB |
| RunWithDefaultErrorMapperWithExtendedDetails | 30.91 μs | 0.235 μs | 0.219 μs |  1.10 |    0.02 | 1.8921 |      - |     - |  11.85 KB |
