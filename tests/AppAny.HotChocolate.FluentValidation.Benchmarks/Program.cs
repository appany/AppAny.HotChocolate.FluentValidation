using System.Threading.Tasks;
using BenchmarkDotNet.Running;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation.Benchmarks
{
	internal class Program
	{
		private static async Task Main()
		{
			var executor = await BenchmarkSetup.CreateRequestExecutor(
				builder => builder.AddFluentValidation()
					.AddMutationType(new TestMutationType(field => field.Argument("input", arg => arg
						.Type<TestInputType>().UseFluentValidation(opt => opt.UseValidator<TestInputValidator>()))))
					.Services.AddSingleton<TestInputValidator>());

			var result = await executor.ExecuteAsync("mutation { test(input: { name: \"\" }) { ... on MyClass { name } ... on MyClassValidation { input { name { message } } } } }");

			BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly)
				.Run();
				// .RunAllJoined();
		}
	}
}
