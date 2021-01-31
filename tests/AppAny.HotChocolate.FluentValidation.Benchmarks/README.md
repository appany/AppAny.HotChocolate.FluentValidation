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

## ValidationBenchmarks

|                      Method |      Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |----------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation |  8.193 μs | 0.0568 μs | 0.0531 μs |  0.30 |    0.01 | 1.2512 | 0.0153 |     - |   7.66 KB |
|           RunWithValidation | 27.554 μs | 0.5321 μs | 0.5464 μs |  1.00 |    0.00 | 1.7395 | 0.0305 |     - |  10.72 KB |
| RunWithFairyBreadValidation | 24.991 μs | 0.2246 μs | 0.2101 μs |  0.91 |    0.02 | 1.8311 | 0.0305 |     - |  11.23 KB |

## ExplicitValidationBenchmarks

|                      Method |      Mean |     Error |    StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |----------:|----------:|----------:|------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation |  7.879 μs | 0.0936 μs | 0.0876 μs |  0.33 | 1.2512 | 0.0153 |     - |   7.66 KB |
|           RunWithValidation | 24.049 μs | 0.1411 μs | 0.1320 μs |  1.00 | 1.7090 | 0.0305 |     - |  10.53 KB |
| RunWithFairyBreadValidation | 24.614 μs | 0.3174 μs | 0.2813 μs |  1.02 | 1.8311 | 0.0305 |     - |  11.23 KB |

## InputValidatorBenchmarks

|                   Method |     Mean |   Error |  StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|------------------------- |---------:|--------:|--------:|------:|-------:|-------:|------:|----------:|
|               Validation | 876.5 ns | 3.24 ns | 2.88 ns |  1.00 | 0.3157 | 0.0010 |     - |   1.94 KB |
| InputValidatorValidation | 968.3 ns | 1.22 ns | 1.08 ns |  1.10 | 0.3529 |      - |     - |   2.17 KB |

## EmptyInputsValidationBenchmarks

|                      Method |     Mean |     Error |    StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |---------:|----------:|----------:|------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation | 7.592 μs | 0.0515 μs | 0.0481 μs |  0.97 | 1.2512 | 0.0229 |     - |   7.64 KB |
|           RunWithValidation | 7.853 μs | 0.0529 μs | 0.0469 μs |  1.00 | 1.2512 | 0.0153 |     - |   7.64 KB |
| RunWithFairyBreadValidation | 8.551 μs | 0.1089 μs | 0.1019 μs |  1.09 | 1.2665 | 0.0153 |     - |    7.8 KB |

## EmptyInputsExplicitValidationBenchmarks

|                      Method |     Mean |     Error |    StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |---------:|----------:|----------:|------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation | 7.865 μs | 0.0535 μs | 0.0475 μs |  1.00 | 1.2512 | 0.0153 |     - |   7.64 KB |
|           RunWithValidation | 7.863 μs | 0.0420 μs | 0.0393 μs |  1.00 | 1.2512 | 0.0153 |     - |   7.64 KB |
| RunWithFairyBreadValidation | 8.831 μs | 0.0397 μs | 0.0371 μs |  1.12 | 1.2665 | 0.0153 |     - |    7.8 KB |

## NullInputsValidationBenchmarks

|                      Method |     Mean |     Error |    StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |---------:|----------:|----------:|------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation | 7.735 μs | 0.0474 μs | 0.0420 μs |  0.94 | 1.2512 | 0.0153 |     - |   7.65 KB |
|           RunWithValidation | 8.208 μs | 0.0355 μs | 0.0314 μs |  1.00 | 1.2512 | 0.0153 |     - |   7.65 KB |
| RunWithFairyBreadValidation | 8.867 μs | 0.0889 μs | 0.0831 μs |  1.08 | 1.2665 | 0.0153 |     - |    7.8 KB |

## NullInputsExplicitValidationBenchmarks

|                      Method |     Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |---------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation | 7.857 μs | 0.0565 μs | 0.0529 μs |  0.93 |    0.01 | 1.2512 | 0.0153 |     - |   7.65 KB |
|           RunWithValidation | 8.459 μs | 0.0569 μs | 0.0532 μs |  1.00 |    0.00 | 1.2512 | 0.0153 |     - |   7.65 KB |
| RunWithFairyBreadValidation | 8.640 μs | 0.1048 μs | 0.0980 μs |  1.02 |    0.02 | 1.2665 | 0.0153 |     - |    7.8 KB |

## ErrorMappersBenchmarks

|                                       Method |     Mean |    Error |   StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|--------------------------------------------- |---------:|---------:|---------:|------:|--------:|-------:|-------:|------:|----------:|
|                    RunWithDefaultErrorMapper | 27.26 μs | 0.473 μs | 0.525 μs |  1.00 |    0.00 | 1.7395 | 0.0305 |     - |  10.72 KB |
|         RunWithDefaultErrorMapperWithDetails | 29.18 μs | 0.263 μs | 0.246 μs |  1.07 |    0.02 | 1.8311 | 0.0305 |     - |  11.17 KB |
| RunWithDefaultErrorMapperWithExtendedDetails | 29.56 μs | 0.278 μs | 0.260 μs |  1.08 |    0.03 | 1.9226 | 0.0305 |     - |  11.69 KB |

## MultipleArgumentsBenchmarks

|                   Method |      Mean |     Error |    StdDev |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|------------------------- |----------:|----------:|----------:|-------:|-------:|------:|----------:|
| RunWithoutSingleArgument |  7.690 μs | 0.0269 μs | 0.0225 μs | 1.2512 | 0.0153 |     - |   7.66 KB |
|    RunWithSingleArgument | 26.370 μs | 0.2195 μs | 0.1946 μs | 1.7395 | 0.0305 |     - |  10.73 KB |
|  RunWithoutFiveArguments |  7.859 μs | 0.0487 μs | 0.0456 μs | 1.3275 | 0.0153 |     - |   8.15 KB |
|     RunWithFiveArguments | 27.706 μs | 0.1452 μs | 0.1133 μs | 1.8311 | 0.0305 |     - |  11.22 KB |
