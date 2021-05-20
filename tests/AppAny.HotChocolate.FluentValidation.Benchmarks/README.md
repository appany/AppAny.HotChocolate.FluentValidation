# Benchmarks

Feel free to optimize or add more benchmark test suites

**External libraries used:**

- [![Nuget](https://img.shields.io/nuget/v/FairyBread.svg)](https://www.nuget.org/packages/FairyBread) [FairyBread (6.0.1)](https://github.com/benmccallum/fairybread)

```ini
BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19042.985 (20H2/October2020Update)
Intel Core i7-9700K CPU 3.60GHz (Coffee Lake), 1 CPU, 8 logical and 8 physical cores
.NET SDK=6.0.100-preview.3.21202.5
  [Host]     : .NET 6.0.0 (6.0.21.20104), X64 RyuJIT
  DefaultJob : .NET 6.0.0 (6.0.21.20104), X64 RyuJIT
```

## SingletonValidationBenchmarks

|                      Method |      Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |----------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation |  7.136 μs | 0.0342 μs | 0.0320 μs |  0.48 |    0.01 | 0.9842 | 0.0153 |     - |      6 KB |
|           RunWithValidation | 14.788 μs | 0.2831 μs | 0.2648 μs |  1.00 |    0.00 | 1.4496 | 0.0305 |     - |      9 KB |
| RunWithFairyBreadValidation | 16.073 μs | 0.0409 μs | 0.0362 μs |  1.09 |    0.02 | 1.5869 | 0.0305 |     - |     10 KB |

## ScopedValidationBenchmarks

|                      Method |      Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |----------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation |  7.030 μs | 0.1262 μs | 0.1181 μs |  0.35 |    0.01 | 0.9842 | 0.0153 |     - |      6 KB |
|           RunWithValidation | 19.986 μs | 0.0330 μs | 0.0275 μs |  1.00 |    0.00 | 1.8005 | 0.0305 |     - |     11 KB |
| RunWithFairyBreadValidation | 21.831 μs | 0.4029 μs | 0.3769 μs |  1.09 |    0.02 | 1.9836 | 0.0305 |     - |     12 KB |

## EmptyInputsValidationBenchmarks

|                      Method |     Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |---------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation | 6.881 μs | 0.0910 μs | 0.0851 μs |  0.99 |    0.02 | 0.9842 | 0.0153 |     - |      6 KB |
|           RunWithValidation | 6.935 μs | 0.0981 μs | 0.0918 μs |  1.00 |    0.00 | 0.9842 | 0.0153 |     - |      6 KB |
| RunWithFairyBreadValidation | 8.557 μs | 0.0959 μs | 0.0897 μs |  1.23 |    0.02 | 1.0529 | 0.0153 |     - |      6 KB |

## NullInputsValidationBenchmarks

|                      Method |     Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|---------------------------- |---------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|        RunWithoutValidation | 6.750 μs | 0.0731 μs | 0.0684 μs |  0.89 |    0.02 | 0.9842 | 0.0153 |     - |      6 KB |
|           RunWithValidation | 7.558 μs | 0.0911 μs | 0.0852 μs |  1.00 |    0.00 | 0.9766 | 0.0153 |     - |      6 KB |
| RunWithFairyBreadValidation | 8.739 μs | 0.1303 μs | 0.1218 μs |  1.16 |    0.02 | 1.0529 | 0.0153 |     - |      6 KB |
