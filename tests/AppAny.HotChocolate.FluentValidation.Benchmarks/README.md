# Benchmarks

Feel free to optimize or add more benchmark test suites

**External libraries used:**

- [FairyBread (3.0.0)](https://github.com/benmccallum/fairybread)
- [DarkHills.HotChocolate.FluentValidation (1.1.0)](https://github.com/DarkHills/HotChocolate.FluentValidation)

```ini
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.746 (2004/?/20H1)
Intel Core i7-9700K CPU 3.60GHz (Coffee Lake), 1 CPU, 8 logical and 8 physical cores
.NET Core SDK=5.0.101
  [Host]     : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT
  DefaultJob : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT
```

|                                  Method |      Mean |     Error |    StdDev |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------------------- |----------:|----------:|----------:|-------:|-------:|------:|----------:|
|                    RunWithoutValidation |  7.872 μs | 0.0326 μs | 0.0305 μs | 1.2512 | 0.0153 |     - |   7.66 KB |
|                        ManualValidation |  2.835 μs | 0.0301 μs | 0.0282 μs | 0.6599 | 0.0038 |     - |   4.05 KB |
|                       RunWithValidation | 25.923 μs | 0.2514 μs | 0.2351 μs | 1.7700 | 0.0305 |     - |  10.89 KB |
|               RunWithExplicitValidation | 22.589 μs | 0.2478 μs | 0.2318 μs | 1.7395 | 0.0305 |     - |  10.69 KB |
|              RunWithDarkHillsValidation | 29.711 μs | 0.2193 μs | 0.1944 μs | 1.8005 | 0.0305 |     - |  11.12 KB |
|             RunWithFairyBreadValidation | 58.827 μs | 0.3499 μs | 0.3273 μs | 2.0142 | 0.0610 |     - |  12.27 KB |
|        RunWithoutValidation_EmptyInputs |  8.020 μs | 0.0463 μs | 0.0411 μs | 1.2512 | 0.0153 |     - |   7.64 KB |
|           RunWithValidation_EmptyInputs |  8.096 μs | 0.0482 μs | 0.0427 μs | 1.2512 | 0.0153 |     - |   7.64 KB |
|   RunWithExplicitValidation_EmptyInputs |  8.127 μs | 0.0432 μs | 0.0405 μs | 1.2512 | 0.0153 |     - |   7.64 KB |
|  RunWithDarkHillsValidation_EmptyInputs |  8.149 μs | 0.0463 μs | 0.0410 μs | 1.2512 | 0.0153 |     - |   7.64 KB |
| RunWithFairyBreadValidation_EmptyInputs | 57.344 μs | 0.3152 μs | 0.2794 μs | 2.0142 |      - |     - |  12.25 KB |
|         RunWithoutValidation_NullInputs |  7.921 μs | 0.0365 μs | 0.0305 μs | 1.2512 | 0.0153 |     - |   7.65 KB |
|            RunWithValidation_NullInputs |  9.183 μs | 0.0951 μs | 0.0843 μs | 1.2512 |      - |     - |   7.81 KB |
|    RunWithExplicitValidation_NullInputs |  8.977 μs | 0.0670 μs | 0.0626 μs | 1.2665 | 0.0153 |     - |   7.81 KB |
|   RunWithDarkHillsValidation_NullInputs |  9.106 μs | 0.0495 μs | 0.0413 μs | 1.2665 | 0.0153 |     - |   7.77 KB |
|  RunWithFairyBreadValidation_NullInputs | 58.645 μs | 0.6036 μs | 0.4712 μs | 1.9531 |      - |     - |  12.26 KB |
