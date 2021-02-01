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

|                            Type |                                       Method |        Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|-------------------------------- |--------------------------------------------- |------------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|   SingletonValidationBenchmarks |                         RunWithoutValidation |  8,479.6 ns |  61.50 ns |  54.52 ns |  0.30 |    0.01 | 1.2512 | 0.0153 |     - |   7.66 KB |
|   SingletonValidationBenchmarks |                            RunWithValidation | 25,260.1 ns | 341.10 ns | 319.07 ns |  0.88 |    0.02 | 1.7090 | 0.0305 |     - |  10.53 KB |
|   SingletonValidationBenchmarks |                  RunWithFairyBreadValidation | 27,657.7 ns | 450.65 ns | 421.54 ns |  0.97 |    0.03 | 1.8616 | 0.0305 |     - |   11.5 KB |
|      ScopedValidationBenchmarks |                         RunWithoutValidation |  8,317.4 ns |  52.49 ns |  46.53 ns |  0.29 |    0.00 | 1.2512 | 0.0153 |     - |   7.66 KB |
|      ScopedValidationBenchmarks |                            RunWithValidation | 38,690.3 ns | 666.77 ns | 623.69 ns |  1.35 |    0.03 | 2.0752 |      - |     - |  12.76 KB |
|      ScopedValidationBenchmarks |                  RunWithFairyBreadValidation | 43,161.2 ns | 285.90 ns | 267.44 ns |  1.51 |    0.03 | 2.6245 | 0.0610 |     - |  16.23 KB |
| EmptyInputsValidationBenchmarks |                         RunWithoutValidation |  7,970.4 ns |  48.10 ns |  44.99 ns |  0.28 |    0.01 | 1.2512 | 0.0153 |     - |   7.64 KB |
| EmptyInputsValidationBenchmarks |                            RunWithValidation |  8,395.0 ns |  63.72 ns |  53.21 ns |  0.29 |    0.01 | 1.2512 | 0.0153 |     - |   7.64 KB |
| EmptyInputsValidationBenchmarks |                  RunWithFairyBreadValidation |  9,480.9 ns |  89.16 ns |  79.03 ns |  0.33 |    0.01 | 1.2970 | 0.0153 |     - |   7.93 KB |
|  NullInputsValidationBenchmarks |                         RunWithoutValidation |  8,288.5 ns |  99.49 ns |  93.06 ns |  0.29 |    0.00 | 1.2512 | 0.0153 |     - |   7.65 KB |
|  NullInputsValidationBenchmarks |                            RunWithValidation |  8,841.5 ns |  57.33 ns |  53.63 ns |  0.31 |    0.01 | 1.2512 | 0.0153 |     - |   7.65 KB |
|  NullInputsValidationBenchmarks |                  RunWithFairyBreadValidation |  9,611.5 ns |  56.74 ns |  44.30 ns |  0.34 |    0.00 | 1.2970 | 0.0153 |     - |   7.94 KB |
|        InputValidatorBenchmarks |                                   Validation |    894.1 ns |   5.06 ns |   4.74 ns |  0.03 |    0.00 | 0.3157 | 0.0010 |     - |   1.94 KB |
|        InputValidatorBenchmarks |                     InputValidatorValidation |  1,025.9 ns |  11.01 ns |  10.30 ns |  0.04 |    0.00 | 0.3529 |      - |     - |   2.17 KB |
|          ErrorMappersBenchmarks |                    RunWithDefaultErrorMapper | 28,636.1 ns | 542.63 ns | 507.58 ns |  1.00 |    0.00 | 1.7395 | 0.0305 |     - |  10.72 KB |
|          ErrorMappersBenchmarks |         RunWithDefaultErrorMapperWithDetails | 29,424.7 ns | 393.17 ns | 367.78 ns |  1.03 |    0.02 | 1.8311 | 0.0305 |     - |  11.16 KB |
|          ErrorMappersBenchmarks | RunWithDefaultErrorMapperWithExtendedDetails | 31,723.2 ns | 386.73 ns | 361.75 ns |  1.11 |    0.02 | 1.8921 |      - |     - |  11.69 KB |
|     MultipleArgumentsBenchmarks |                     RunWithoutSingleArgument |  8,399.1 ns |  59.77 ns |  52.99 ns |  0.29 |    0.01 | 1.2512 | 0.0153 |     - |   7.66 KB |
|     MultipleArgumentsBenchmarks |                        RunWithSingleArgument | 28,573.9 ns | 253.39 ns | 211.59 ns |  1.00 |    0.02 | 1.7395 | 0.0305 |     - |  10.73 KB |
|     MultipleArgumentsBenchmarks |                      RunWithoutFiveArguments |  8,305.3 ns |  76.11 ns |  71.20 ns |  0.29 |    0.01 | 1.3275 | 0.0153 |     - |   8.15 KB |
|     MultipleArgumentsBenchmarks |                         RunWithFiveArguments | 29,423.5 ns | 251.90 ns | 210.35 ns |  1.03 |    0.02 | 1.8311 | 0.0305 |     - |  11.22 KB |
