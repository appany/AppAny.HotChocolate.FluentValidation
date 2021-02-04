using BenchmarkDotNet.Running;

namespace AppAny.HotChocolate.FluentValidation.Benchmarks
{
	internal class Program
	{
		private static void Main()
		{
			BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly)
				.Run();
				// .RunAllJoined();
		}
	}
}
