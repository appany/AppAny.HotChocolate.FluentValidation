# Benchmarks

```ini
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.685 (2004/?/20H1)
Intel Core i7-9700K CPU 3.60GHz (Coffee Lake), 1 CPU, 8 logical and 8 physical cores
.NET Core SDK=5.0.101
  [Host]     : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT
  DefaultJob : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT
```

|                      Method |      Mean |     Error |    StdDev |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |----------:|----------:|----------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation |  7.582 μs | 0.0384 μs | 0.0359 μs | 1.2512 | 0.0153 |     - |   7.66 KB |
|           RunWithValidation | 25.451 μs | 0.3457 μs | 0.3234 μs | 1.8005 | 0.0305 |     - |  10.96 KB |
|  RunWithDarkHillsValidation | 27.645 μs | 0.3295 μs | 0.3082 μs | 1.8005 | 0.0305 |     - |  11.11 KB |
| RunWithFairyBreadValidation | 56.255 μs | 0.1440 μs | 0.1347 μs | 2.0142 |      - |     - |  12.27 KB |
|                  Validation |  2.709 μs | 0.0098 μs | 0.0091 μs | 0.6599 | 0.0038 |     - |   4.05 KB |
