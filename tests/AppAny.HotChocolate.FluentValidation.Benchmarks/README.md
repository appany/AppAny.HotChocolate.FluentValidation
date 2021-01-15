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
|                     RunWithoutValidation |  7.973 μs | 0.0435 μs | 0.0407 μs | 1.2512 | 0.0153 |     - |   7.66 KB |
|                         ManualValidation |  2.997 μs | 0.0255 μs | 0.0239 μs | 0.6790 | 0.0038 |     - |   4.17 KB |
|                        RunWithValidation | 25.291 μs | 0.4799 μs | 0.4714 μs | 1.7700 | 0.0305 |     - |  10.89 KB |
|                RunWithExplicitValidation | 23.041 μs | 0.2227 μs | 0.2083 μs | 1.7395 | 0.0305 |     - |  10.69 KB |
|             RunWithFluentChocoValidation | 29.448 μs | 0.2192 μs | 0.2050 μs | 1.8005 | 0.0305 |     - |  11.11 KB |
|              RunWithFairyBreadValidation | 73.477 μs | 0.7394 μs | 0.6916 μs | 2.3193 |      - |     - |  14.04 KB |
|         RunWithoutValidation_EmptyInputs |  7.997 μs | 0.0195 μs | 0.0173 μs | 1.2512 | 0.0153 |     - |   7.64 KB |
|            RunWithValidation_EmptyInputs |  8.029 μs | 0.0488 μs | 0.0456 μs | 1.2512 | 0.0153 |     - |   7.64 KB |
|    RunWithExplicitValidation_EmptyInputs |  8.245 μs | 0.0679 μs | 0.0602 μs | 1.2512 | 0.0153 |     - |   7.64 KB |
| RunWithFluentChocoValidation_EmptyInputs |  8.207 μs | 0.1049 μs | 0.0981 μs | 1.2512 | 0.0153 |     - |   7.64 KB |
|  RunWithFairyBreadValidation_EmptyInputs | 70.782 μs | 0.2303 μs | 0.2042 μs | 2.1973 |      - |     - |  13.96 KB |
|          RunWithoutValidation_NullInputs |  8.070 μs | 0.0503 μs | 0.0471 μs | 1.2512 | 0.0153 |     - |   7.65 KB |
|             RunWithValidation_NullInputs |  8.885 μs | 0.0502 μs | 0.0445 μs | 1.2665 | 0.0153 |     - |   7.81 KB |
|     RunWithExplicitValidation_NullInputs |  9.180 μs | 0.1120 μs | 0.1048 μs | 1.2665 | 0.0153 |     - |   7.81 KB |
|    RunWithDarkHillsValidation_NullInputs |  8.971 μs | 0.0265 μs | 0.0248 μs | 1.2665 | 0.0153 |     - |   7.77 KB |
|   RunWithFairyBreadValidation_NullInputs | 71.161 μs | 0.7118 μs | 0.6658 μs | 2.3193 |      - |     - |  13.97 KB |

