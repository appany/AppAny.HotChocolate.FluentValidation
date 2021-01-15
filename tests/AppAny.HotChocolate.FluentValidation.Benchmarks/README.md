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

|                                   Method |      Mean |     Error |    StdDev |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|----------------------------------------- |----------:|----------:|----------:|-------:|-------:|------:|----------:|
|                     RunWithoutValidation |  7.726 μs | 0.0407 μs | 0.0381 μs | 1.2512 | 0.0153 |     - |   7.66 KB |
|                         ManualValidation |  2.974 μs | 0.0151 μs | 0.0142 μs | 0.6790 | 0.0038 |     - |   4.17 KB |
|                        RunWithValidation | 26.836 μs | 0.2680 μs | 0.2507 μs | 1.7700 | 0.0305 |     - |  10.89 KB |
|                RunWithExplicitValidation | 23.290 μs | 0.2547 μs | 0.2383 μs | 1.7395 | 0.0305 |     - |  10.69 KB |
|             RunWithFluentChocoValidation | 29.477 μs | 0.2464 μs | 0.2305 μs | 1.8005 | 0.0305 |     - |  11.11 KB |
|              RunWithFairyBreadValidation | 73.930 μs | 0.7428 μs | 0.6948 μs | 2.3193 |      - |     - |  14.04 KB |
|         RunWithoutValidation_EmptyInputs |  7.872 μs | 0.0684 μs | 0.0640 μs | 1.2512 | 0.0153 |     - |   7.64 KB |
|            RunWithValidation_EmptyInputs |  8.205 μs | 0.0391 μs | 0.0366 μs | 1.2512 | 0.0153 |     - |   7.64 KB |
|    RunWithExplicitValidation_EmptyInputs |  8.037 μs | 0.0432 μs | 0.0404 μs | 1.2512 | 0.0153 |     - |   7.64 KB |
| RunWithFluentChocoValidation_EmptyInputs |  8.104 μs | 0.0539 μs | 0.0477 μs | 1.2512 | 0.0153 |     - |   7.64 KB |
|  RunWithFairyBreadValidation_EmptyInputs | 72.490 μs | 0.5538 μs | 0.5180 μs | 2.1973 |      - |     - |  13.96 KB |
|          RunWithoutValidation_NullInputs |  7.777 μs | 0.0495 μs | 0.0463 μs | 1.2512 | 0.0153 |     - |   7.65 KB |
|             RunWithValidation_NullInputs |  8.819 μs | 0.0612 μs | 0.0572 μs | 1.2665 | 0.0153 |     - |   7.81 KB |
|     RunWithExplicitValidation_NullInputs |  8.939 μs | 0.0750 μs | 0.0702 μs | 1.2665 | 0.0153 |     - |   7.81 KB |
|  RunWithFluentChocoValidation_NullInputs |  8.865 μs | 0.0588 μs | 0.0550 μs | 1.2665 | 0.0153 |     - |   7.77 KB |
|   RunWithFairyBreadValidation_NullInputs | 72.368 μs | 0.3200 μs | 0.2993 μs | 2.3193 |      - |     - |  13.97 KB |
