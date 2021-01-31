using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using FluentValidation;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation.Benchmarks
{
	[MemoryDiagnoser]
	public class MultipleArgumentsBenchmarks
	{
		private IRequestExecutor withSingleArgument = default!;
		private IRequestExecutor withTwoArguments = default!;
		private IRequestExecutor withFiveArguments = default!;

		[GlobalSetup]
		public async Task GlobalSetup()
		{
			withSingleArgument = await BenchmarkSetup.CreateRequestExecutor(
				builder => builder.AddFluentValidation()
					.AddMutationType(new TestMutationType(field => field
						.Argument("input", arg => arg.Type<TestInputType>().UseFluentValidation())))
					.Services.AddSingleton<IValidator<TestInput>, TestInputValidator>());

			withTwoArguments = await BenchmarkSetup.CreateRequestExecutor(
				builder => builder.AddFluentValidation()
					.AddMutationType(new TestMutationType(field => field
						.Argument("input", arg => arg.Type<TestInputType>().UseFluentValidation())
						.Argument("input2", arg => arg.Type<TestInputType>().UseFluentValidation())))
					.Services.AddSingleton<IValidator<TestInput>, TestInputValidator>());

			withFiveArguments = await BenchmarkSetup.CreateRequestExecutor(
				builder => builder.AddFluentValidation()
					.AddMutationType(new TestMutationType(field => field
						.Argument("input", arg => arg.Type<TestInputType>().UseFluentValidation())
						.Argument("input2", arg => arg.Type<TestInputType>().UseFluentValidation())
						.Argument("input3", arg => arg.Type<TestInputType>().UseFluentValidation())
						.Argument("input4", arg => arg.Type<TestInputType>().UseFluentValidation())
						.Argument("input5", arg => arg.Type<TestInputType>().UseFluentValidation())))
					.Services.AddSingleton<IValidator<TestInput>, TestInputValidator>());
		}

		[Benchmark]
		public Task RunWithSingleArgument()
		{
			return withSingleArgument.ExecuteAsync(BenchmarkSetup.Mutations.WithEmptyName);
		}

		[Benchmark]
		public Task RunWithTwoArguments()
		{
			return withTwoArguments.ExecuteAsync(BenchmarkSetup.Mutations.WithEmptyName);
		}

		[Benchmark]
		public Task RunWithFiveArguments()
		{
			return withFiveArguments.ExecuteAsync(BenchmarkSetup.Mutations.WithEmptyName);
		}
	}
}
