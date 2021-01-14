# Benchmarks

``` ini
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.685 (2004/?/20H1)
Intel Core i7-9700K CPU 3.60GHz (Coffee Lake), 1 CPU, 8 logical and 8 physical cores
.NET Core SDK=5.0.101
  [Host]     : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT
  DefaultJob : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT
```

|                                  Method |      Mean |     Error |    StdDev |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------------------- |----------:|----------:|----------:|-------:|-------:|------:|----------:|
|                    RunWithoutValidation |  8.385 μs | 0.0480 μs | 0.0449 μs | 1.2512 | 0.0153 |     - |   7.66 KB |
|                        ManualValidation |  2.992 μs | 0.0294 μs | 0.0275 μs | 0.6599 | 0.0038 |     - |   4.05 KB |
|                       RunWithValidation | 26.971 μs | 0.2462 μs | 0.2303 μs | 1.7700 | 0.0305 |     - |  10.89 KB |
|               RunWithExplicitValidation | 22.889 μs | 0.2396 μs | 0.2241 μs | 1.7395 | 0.0305 |     - |  10.69 KB |
|              RunWithDarkHillsValidation | 30.601 μs | 0.0865 μs | 0.0809 μs | 1.7700 |      - |     - |  11.12 KB |
|             RunWithFairyBreadValidation | 58.588 μs | 0.3706 μs | 0.3466 μs | 2.0142 |      - |     - |  12.27 KB |
|        RunWithoutValidation_EmptyInputs |  8.282 μs | 0.0394 μs | 0.0349 μs | 1.2512 | 0.0153 |     - |   7.64 KB |
|           RunWithValidation_EmptyInputs |  8.878 μs | 0.0482 μs | 0.0427 μs | 1.2665 | 0.0153 |     - |    7.8 KB |
|   RunWithExplicitValidation_EmptyInputs |  8.862 μs | 0.0494 μs | 0.0412 μs | 1.2665 | 0.0153 |     - |    7.8 KB |
|  RunWithDarkHillsValidation_EmptyInputs |  8.291 μs | 0.0423 μs | 0.0396 μs | 1.2512 | 0.0153 |     - |   7.64 KB |
| RunWithFairyBreadValidation_EmptyInputs | 58.850 μs | 0.5020 μs | 0.4696 μs | 2.0142 |      - |     - |  12.25 KB |
