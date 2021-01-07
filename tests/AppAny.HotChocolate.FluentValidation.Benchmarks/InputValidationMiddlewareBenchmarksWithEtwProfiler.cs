using BenchmarkDotNet.Diagnostics.Windows.Configs;

namespace AppAny.HotChocolate.FluentValidation.Benchmarks
{
	[EtwProfiler]
	public class InputValidationMiddlewareBenchmarksWithEtwProfiler : InputValidationMiddlewareBenchmarks
	{
	}
}
