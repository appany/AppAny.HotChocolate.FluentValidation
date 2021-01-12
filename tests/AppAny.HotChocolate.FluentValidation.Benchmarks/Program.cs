using BenchmarkDotNet.Running;

namespace AppAny.HotChocolate.FluentValidation.Benchmarks
{
	class Program
	{
		static void Main(string[] args)
		{
			BenchmarkSwitcher
				.FromAssembly(typeof(Program).Assembly)
				.Run(args
					//, DefaultConfig.Instance.AddDiagnoser(new EtwProfiler())
				);
		}
	}
}
