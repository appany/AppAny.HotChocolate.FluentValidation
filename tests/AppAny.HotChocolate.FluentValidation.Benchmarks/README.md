# Benchmarks

Feel free to optimize or add more benchmark test suites

**External libraries used:**

- [![Nuget](https://img.shields.io/nuget/v/FairyBread.svg)](https://www.nuget.org/packages/FairyBread) [FairyBread (3.0.0)](https://github.com/benmccallum/fairybread)
- [![Nuget](https://img.shields.io/nuget/v/FluentChoco.svg)](https://www.nuget.org/packages/FluentChoco) [FluentChoco (2.0.0)](https://github.com/dalrankov/FluentChoco)

``` ini
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.746 (2004/?/20H1)
Intel Core i7-9700K CPU 3.60GHz (Coffee Lake), 1 CPU, 8 logical and 8 physical cores
.NET Core SDK=5.0.101
  [Host]     : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT
  DefaultJob : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT
```

|                                   Method |      Mean |     Error |    StdDev |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|----------------------------------------- |----------:|----------:|----------:|-------:|-------:|------:|----------:|
|                     RunWithoutValidation |  8.074 μs | 0.0280 μs | 0.0262 μs | 1.2512 | 0.0153 |     - |   7.66 KB |
|                         ManualValidation |  2.987 μs | 0.0278 μs | 0.0260 μs | 0.6790 | 0.0038 |     - |   4.17 KB |
|                        RunWithValidation | 25.708 μs | 0.3755 μs | 0.3329 μs | 1.7700 | 0.0305 |     - |  10.89 KB |
|                RunWithExplicitValidation | 22.946 μs | 0.2379 μs | 0.2226 μs | 1.7395 | 0.0305 |     - |  10.69 KB |
|             RunWithFluentChocoValidation | 29.378 μs | 0.0983 μs | 0.0871 μs | 1.8005 | 0.0305 |     - |  11.11 KB |
|              RunWithFairyBreadValidation | 73.820 μs | 0.7364 μs | 0.6888 μs | 2.3193 |      - |     - |  14.04 KB |
|         RunWithoutValidation_EmptyInputs |  8.039 μs | 0.0695 μs | 0.0650 μs | 1.2512 | 0.0153 |     - |   7.64 KB |
|            RunWithValidation_EmptyInputs |  8.225 μs | 0.0456 μs | 0.0426 μs | 1.2512 | 0.0153 |     - |   7.64 KB |
|    RunWithExplicitValidation_EmptyInputs |  7.958 μs | 0.0519 μs | 0.0486 μs | 1.2512 | 0.0153 |     - |   7.64 KB |
| RunWithFluentChocoValidation_EmptyInputs |  8.477 μs | 0.0297 μs | 0.0278 μs | 1.2512 | 0.0153 |     - |   7.64 KB |
|  RunWithFairyBreadValidation_EmptyInputs | 71.802 μs | 0.4484 μs | 0.4194 μs | 2.1973 |      - |     - |  13.96 KB |
|          RunWithoutValidation_NullInputs |  8.068 μs | 0.0251 μs | 0.0210 μs | 1.2512 | 0.0153 |     - |   7.65 KB |
|             RunWithValidation_NullInputs |  8.829 μs | 0.0328 μs | 0.0307 μs | 1.2665 | 0.0153 |     - |   7.81 KB |
|     RunWithExplicitValidation_NullInputs |  9.111 μs | 0.1000 μs | 0.0886 μs | 1.2512 |      - |     - |   7.81 KB |
|    RunWithDarkHillsValidation_NullInputs |  8.496 μs | 0.0453 μs | 0.0401 μs | 1.2665 | 0.0153 |     - |   7.77 KB |
|   RunWithFairyBreadValidation_NullInputs | 71.700 μs | 0.3627 μs | 0.3029 μs | 2.3193 |      - |     - |  13.97 KB |
