using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using FairyBread;
using FluentChoco;
using FluentValidation;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation.Benchmarks
{
	[MemoryDiagnoser]
	public class ExplicitValidationBenchmarks
	{
		private IRequestExecutor withoutValidation = default!;
		private IRequestExecutor withExplicitValidation = default!;
		private IRequestExecutor fluentChocoValidation = default!;
		private IRequestExecutor fairyBreadValidation = default!;

		[GlobalSetup]
		public async Task GlobalSetup()
		{
			withoutValidation = await BenchmarkSetup.CreateRequestExecutor(
				builder => builder.AddMutationType(new TestMutationType()));

			withExplicitValidation = await BenchmarkSetup.CreateRequestExecutor(
				builder => builder.AddFluentValidation()
					.AddMutationType(new TestMutationType(arg =>
						arg.UseFluentValidation(opt => opt.UseValidator<IValidator<TestInput>>())))
					.Services.AddScoped<IValidator<TestInput>, TestInputValidator>());

			fluentChocoValidation = await BenchmarkSetup.CreateRequestExecutor(
				builder => builder.UseFluentValidation()
					.AddMutationType(new TestMutationType())
					.Services.AddScoped<IValidator<TestInput>, TestInputValidator>());

			fairyBreadValidation = await BenchmarkSetup.CreateRequestExecutor(
				builder => builder.AddFairyBread(opt => opt.AssembliesToScanForValidators = new[] { typeof(Program).Assembly })
					.AddMutationType(new TestMutationType(arg => arg.UseValidation()))
					.Services.AddScoped<TestInputValidator>());
		}

		[Benchmark]
		public Task RunWithoutValidation()
		{
			return withoutValidation.ExecuteAsync(BenchmarkSetup.Mutations.WithEmptyName);
		}

		[Benchmark(Baseline = true)]
		public Task RunWithValidation()
		{
			return withExplicitValidation.ExecuteAsync(BenchmarkSetup.Mutations.WithEmptyName);
		}

		[Benchmark(Description = "Broken since 11.0.8")]
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
