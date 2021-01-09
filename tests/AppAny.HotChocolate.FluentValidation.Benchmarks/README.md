```ini
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.685 (2004/?/20H1)
Intel Core i7-9700K CPU 3.60GHz (Coffee Lake), 1 CPU, 8 logical and 8 physical cores
.NET Core SDK=5.0.101
  [Host]     : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT
  DefaultJob : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT
```

|                      Method |      Mean |     Error |    StdDev |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |----------:|----------:|----------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation |  8.206 μs | 0.0329 μs | 0.0308 μs | 1.2512 | 0.0153 |     - |   7.66 KB |
|           RunWithValidation | 29.377 μs | 0.2483 μs | 0.2323 μs | 1.8616 | 0.0305 |     - |  11.41 KB |
|  RunWithDarkHillsValidation | 33.878 μs | 0.2141 μs | 0.1898 μs | 1.7700 |      - |     - |  11.12 KB |
| RunWithFairyBreadValidation | 62.043 μs | 0.5160 μs | 0.4827 μs | 1.9531 |      - |     - |  12.27 KB |
|                  Validation |  3.007 μs | 0.0321 μs | 0.0300 μs | 0.6599 | 0.0038 |     - |   4.05 KB |
