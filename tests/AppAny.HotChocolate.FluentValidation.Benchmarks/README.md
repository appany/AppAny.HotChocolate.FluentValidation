# Benchmarks

Feel free to optimize or add more benchmark test suites

**External libraries used:**

- [![Nuget](https://img.shields.io/nuget/v/FairyBread.svg)](https://www.nuget.org/packages/FairyBread) [FairyBread (3.0.0)](https://github.com/benmccallum/fairybread)
- [![Nuget](https://img.shields.io/nuget/v/DarkHills.HotChocolate.FluentValidation.svg)](https://www.nuget.org/packages/DarkHills.HotChocolate.FluentValidation) [DarkHills.HotChocolate.FluentValidation (1.1.0)](https://github.com/DarkHills/HotChocolate.FluentValidation)

``` ini
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.746 (2004/?/20H1)
Intel Core i7-9700K CPU 3.60GHz (Coffee Lake), 1 CPU, 8 logical and 8 physical cores
.NET Core SDK=5.0.101
  [Host]     : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT
  DefaultJob : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT
```

|                                  Method |      Mean |     Error |    StdDev |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------------------- |----------:|----------:|----------:|-------:|-------:|------:|----------:|
|                    RunWithoutValidation |  8.275 μs | 0.0369 μs | 0.0346 μs | 1.2512 | 0.0153 |     - |   7.66 KB |
|                        ManualValidation |  3.049 μs | 0.0284 μs | 0.0265 μs | 0.6790 | 0.0038 |     - |   4.17 KB |
|                       RunWithValidation | 25.654 μs | 0.4262 μs | 0.3986 μs | 1.7700 | 0.0305 |     - |  10.89 KB |
|               RunWithExplicitValidation | 23.021 μs | 0.1802 μs | 0.1686 μs | 1.7395 | 0.0305 |     - |  10.69 KB |
|              RunWithDarkHillsValidation | 30.575 μs | 0.2807 μs | 0.2626 μs | 1.8005 | 0.0305 |     - |  11.12 KB |
|             RunWithFairyBreadValidation | 74.492 μs | 0.3756 μs | 0.3513 μs | 2.3193 |      - |     - |  14.04 KB |
|        RunWithoutValidation_EmptyInputs |  8.246 μs | 0.0413 μs | 0.0386 μs | 1.2512 | 0.0153 |     - |   7.64 KB |
|           RunWithValidation_EmptyInputs |  8.046 μs | 0.0686 μs | 0.0642 μs | 1.2512 | 0.0153 |     - |   7.64 KB |
|   RunWithExplicitValidation_EmptyInputs |  8.178 μs | 0.0799 μs | 0.0748 μs | 1.2512 | 0.0153 |     - |   7.64 KB |
|  RunWithDarkHillsValidation_EmptyInputs |  8.288 μs | 0.0583 μs | 0.0545 μs | 1.2512 | 0.0153 |     - |   7.64 KB |
| RunWithFairyBreadValidation_EmptyInputs | 70.276 μs | 0.3779 μs | 0.3535 μs | 2.1973 |      - |     - |  13.96 KB |
|         RunWithoutValidation_NullInputs |  7.851 μs | 0.0353 μs | 0.0313 μs | 1.2512 | 0.0153 |     - |   7.65 KB |
|            RunWithValidation_NullInputs |  8.378 μs | 0.0414 μs | 0.0388 μs | 1.2665 | 0.0153 |     - |   7.81 KB |
|    RunWithExplicitValidation_NullInputs |  9.138 μs | 0.0395 μs | 0.0351 μs | 1.2665 | 0.0153 |     - |   7.81 KB |
|   RunWithDarkHillsValidation_NullInputs |  9.109 μs | 0.0576 μs | 0.0511 μs | 1.2665 | 0.0153 |     - |   7.77 KB |
|  RunWithFairyBreadValidation_NullInputs | 72.235 μs | 0.4564 μs | 0.4269 μs | 2.3193 |      - |     - |  13.97 KB |
