using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using FluentChoco;
using FluentValidation;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation.Benchmarks
{
	[MemoryDiagnoser]
	public class ScopedValidationBenchmarks
	{
		private IRequestExecutor withoutValidation = default!;
		private IRequestExecutor withValidation = default!;
		private IRequestExecutor fluentChocoValidation = default!;
		private IRequestExecutor fairyBreadValidation = default!;

		[GlobalSetup]
		public async Task GlobalSetup()
		{
			withoutValidation = await BenchmarkSetup.CreateRequestExecutor(
				builder => builder.AddMutationType(new TestMutationType(field =>
					field.Argument("input", arg => arg.Type<TestInputType>()))));

			withValidation = await BenchmarkSetup.CreateRequestExecutor(
				builder => builder.AddFluentValidation()
					.AddMutationType(new TestMutationType(field => field.Argument("input", arg => arg
						.Type<TestInputType>().UseFluentValidation(opt => opt.UseValidator<TestInputValidator>()))))
					.Services.AddScoped<TestInputValidator>());

			fluentChocoValidation = await BenchmarkSetup.CreateRequestExecutor(
				builder => builder.UseFluentValidation()
					.AddMutationType(new TestMutationType(field => field.Argument("input", arg => arg.Type<TestInputType>())))
					.Services.AddScoped<IValidator<TestInput>, TestInputValidator>());

			fairyBreadValidation = await BenchmarkSetup.CreateRequestExecutor(
				builder => builder.AddFairyBread()
					.AddMutationType(new TestMutationType(field => field.Argument("input", arg => arg.Type<TestInputType>())))
					.Services.AddScoped<TestInputValidator>().AddScoped<IValidator<TestInput>, TestInputValidator>());
		}

		[Benchmark]
		public Task RunWithoutValidation()
		{
			return withoutValidation.ExecuteAsync(BenchmarkSetup.Mutations.WithEmptyName);
		}

		[Benchmark(Baseline = true)]
		public Task RunWithValidation()
		{
			return withValidation.ExecuteAsync(BenchmarkSetup.Mutations.WithEmptyName);
		}

		[Benchmark]
		public Task RunWithFluentChocoValidation()
		{
			return fluentChocoValidation.ExecuteAsync(BenchmarkSetup.Mutations.WithEmptyName);
		}

		[Benchmark]
		public Task RunWithFairyBreadValidation()
		{
			return fairyBreadValidation.ExecuteAsync(BenchmarkSetup.Mutations.WithEmptyName);
		}
	}
}
