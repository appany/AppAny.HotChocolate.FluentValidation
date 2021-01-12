using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using FluentValidation;
using HotChocolate.Execution;
using HotChocolate.FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation.Benchmarks
{
	[MemoryDiagnoser]
	public class InputValidationMiddlewareBenchmarksWithoutInputs
	{
		private IRequestExecutor withoutValidation = null!;
		private IRequestExecutor withValidation = null!;
		private IRequestExecutor darkHillsValidation = null!;
		private IRequestExecutor fairyBreadValidation = null!;

		[GlobalSetup]
		public async Task GlobalSetup()
		{
			withoutValidation = await new ServiceCollection()
				.AddGraphQL()
				.AddQueryType<TestQueryType>()
				.AddMutationType<TestMutationWithoutValidationType>()
				.BuildRequestExecutorAsync();

			withValidation = await new ServiceCollection()
				.AddSingleton<IValidator<TestInput>, TestInputValidator>()
				.AddGraphQL()
				.AddFluentValidation()
				.AddQueryType<TestQueryType>()
				.AddMutationType<TestMutationWithValidationType>()
				.BuildRequestExecutorAsync();

			darkHillsValidation = await new ServiceCollection()
				.AddSingleton<IValidator<TestInput>, TestInputValidator>()
				.AddGraphQL()
				.UseFluentValidation()
				.AddQueryType<TestQueryType>()
				.AddMutationType<TestMutationWithDarkHillsValidationType>()
				.BuildRequestExecutorAsync();

			fairyBreadValidation = await new ServiceCollection()
				.AddSingleton<IValidator<TestInput>, TestInputValidator>()
				.AddGraphQL()
				.AddFairyBread()
				.AddQueryType<TestQueryType>()
				.AddMutationType<TestMutationWithFairyBreadValidationType>()
				.BuildRequestExecutorAsync();
		}

		[Benchmark]
		public Task RunWithoutValidation()
		{
			return withoutValidation.ExecuteAsync("mutation { test() }");
		}

		[Benchmark]
		public Task RunWithValidation()
		{
			return withValidation.ExecuteAsync("mutation { test() }");
		}

		[Benchmark]
		public Task RunWithDarkHillsValidation()
		{
			return darkHillsValidation.ExecuteAsync("mutation { test() }");
		}

		[Benchmark]
		public Task RunWithFairyBreadValidation()
		{
			return fairyBreadValidation.ExecuteAsync("mutation { test() }");
		}

		[Benchmark]
		public Task Validation()
		{
			var validationContext = ValidationContext<TestInput>.CreateWithOptions(new TestInput(), _ =>
			{
			});

			var validator = new TestInputValidator();

			return validator.ValidateAsync(validationContext);
		}
	}
}
