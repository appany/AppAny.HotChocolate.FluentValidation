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
|         RunWithoutValidation |  8.200 μs | 0.0372 μs | 0.0348 μs |  0.28 |    0.01 | 1.2512 | 0.0153 |     - |   7.66 KB |
|            RunWithValidation | 28.846 μs | 0.5273 μs | 0.4932 μs |  1.00 |    0.00 | 1.7700 | 0.0305 |     - |  10.89 KB |
| RunWithFluentChocoValidation | 32.304 μs | 0.3515 μs | 0.3288 μs |  1.12 |    0.02 | 1.7700 |      - |     - |  11.11 KB |
|  RunWithFairyBreadValidation | 76.914 μs | 0.4755 μs | 0.4215 μs |  2.67 |    0.06 | 2.8076 |      - |     - |   17.1 KB |

## ExplicitValidationBenchmarks

|                       Method |      Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|----------------------------- |----------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|         RunWithoutValidation |  8.080 μs | 0.0279 μs | 0.0261 μs |  0.31 |    0.00 | 1.2512 | 0.0153 |     - |   7.66 KB |
|            RunWithValidation | 25.998 μs | 0.1885 μs | 0.1763 μs |  1.00 |    0.00 | 1.7395 | 0.0305 |     - |   10.69 KB |
| RunWithFluentChocoValidation | 32.471 μs | 0.5910 μs | 0.5529 μs |  1.25 |    0.02 | 1.7700 |      - |     - |  11.11 KB |
|  RunWithFairyBreadValidation | 76.605 μs | 0.5781 μs | 0.5125 μs |  2.95 |    0.03 | 2.8076 |      - |     - |   17.1 KB |

## InputValidatorBenchmarks

|                   Method |     Mean |   Error |  StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|------------------------- |---------:|--------:|--------:|------:|-------:|-------:|------:|----------:|
|               Validation | 919.4 ns | 2.58 ns | 2.16 ns |  1.00 | 0.3157 | 0.0010 |     - |   1.94 KB |
| InputValidatorValidation | 972.6 ns | 2.93 ns | 2.74 ns |  1.06 | 0.3529 |      - |     - |   2.17 KB |

## EmptyInputsValidationBenchmarks

|                       Method |     Mean |     Error |    StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|----------------------------- |---------:|----------:|----------:|------:|-------:|-------:|------:|----------:|
|         RunWithoutValidation | 8.228 μs | 0.0506 μs | 0.0448 μs |  1.00 | 1.2512 | 0.0153 |     - |   7.64 KB |
|            RunWithValidation | 8.237 μs | 0.0498 μs | 0.0465 μs |  1.00 | 1.2512 | 0.0153 |     - |   7.64 KB |
| RunWithFluentChocoValidation | 8.622 μs | 0.0445 μs | 0.0416 μs |  1.05 | 1.2512 | 0.0153 |     - |   7.64 KB |
|  RunWithFairyBreadValidation | 9.700 μs | 0.0421 μs | 0.0373 μs |  1.18 | 1.2817 | 0.0153 |     - |   7.87 KB |

## EmptyInputsExplicitValidationBenchmarks

|                       Method |     Mean |     Error |    StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|----------------------------- |---------:|----------:|----------:|------:|-------:|-------:|------:|----------:|
|         RunWithoutValidation | 8.202 μs | 0.0534 μs | 0.0499 μs |  1.01 | 1.2512 | 0.0153 |     - |   7.64 KB |
|            RunWithValidation | 8.158 μs | 0.0680 μs | 0.0603 μs |  1.00 | 1.2512 | 0.0153 |     - |   7.64 KB |
| RunWithFluentChocoValidation | 8.372 μs | 0.0472 μs | 0.0442 μs |  1.03 | 1.2512 | 0.0153 |     - |   7.64 KB |
|  RunWithFairyBreadValidation | 9.317 μs | 0.0587 μs | 0.0549 μs |  1.14 | 1.2817 | 0.0153 |     - |   7.87 KB |

## NullInputsValidationBenchmarks

|                       Method |     Mean |     Error |    StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|----------------------------- |---------:|----------:|----------:|------:|-------:|-------:|------:|----------:|
|         RunWithoutValidation | 8.251 μs | 0.0456 μs | 0.0404 μs |  0.88 | 1.2512 | 0.0153 |     - |   7.65 KB |
|            RunWithValidation | 9.409 μs | 0.0626 μs | 0.0585 μs |  1.00 | 1.2665 | 0.0153 |     - |   7.81 KB |
| RunWithFluentChocoValidation | 9.362 μs | 0.0504 μs | 0.0471 μs |  1.00 | 1.2665 | 0.0153 |     - |   7.77 KB |
|  RunWithFairyBreadValidation | 9.436 μs | 0.0468 μs | 0.0415 μs |  1.00 | 1.2817 | 0.0153 |     - |   7.88 KB |

## NullInputsExplicitValidationBenchmarks

|                       Method |     Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|----------------------------- |---------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|         RunWithoutValidation | 7.736 μs | 0.0331 μs | 0.0293 μs |  0.86 |    0.01 | 1.2512 | 0.0153 |     - |   7.65 KB |
|            RunWithValidation | 8.989 μs | 0.0885 μs | 0.0828 μs |  1.00 |    0.00 | 1.2665 | 0.0153 |     - |   7.81 KB |
| RunWithFluentChocoValidation | 9.267 μs | 0.0638 μs | 0.0533 μs |  1.03 |    0.01 | 1.2665 | 0.0153 |     - |   7.77 KB |
|  RunWithFairyBreadValidation | 9.430 μs | 0.0821 μs | 0.0768 μs |  1.05 |    0.02 | 1.2817 | 0.0153 |     - |   7.88 KB |

## ErrorMappersBenchmarks

|                                       Method |     Mean |    Error |   StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|--------------------------------------------- |---------:|---------:|---------:|------:|-------:|-------:|------:|----------:|
|                    RunWithDefaultErrorMapper | 30.31 μs | 0.227 μs | 0.212 μs |  1.00 | 1.7700 | 0.0305 |     - |   10.89 KB |
|         RunWithDefaultErrorMapperWithDetails | 31.66 μs | 0.113 μs | 0.105 μs |  1.04 | 1.8311 |      - |     - |  11.33 KB |
| RunWithDefaultErrorMapperWithExtendedDetails | 32.58 μs | 0.249 μs | 0.221 μs |  1.08 | 1.9531 |      - |     - |  11.85 KB |
