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
	public class InputValidationMiddlewareBenchmarks
	{
		private IRequestExecutor withoutValidation = null!;
		private IRequestExecutor withValidation = null!;
		private IRequestExecutor withExplicitValidation = null!;
		private IRequestExecutor fluentChocoValidation = null!;
		private IRequestExecutor fairyBreadValidation = null!;

		[GlobalSetup]
		public async Task GlobalSetup()
		{
			withoutValidation = await new ServiceCollection()
				.AddGraphQL()
				.AddQueryType<TestQueryType>()
				.AddMutationType(new TestMutationType(_ =>
				{
				}))
				.BuildRequestExecutorAsync();

			withValidation = await new ServiceCollection()
				.AddSingleton<IValidator<TestInput>, TestInputValidator>()
				.AddGraphQL()
				.AddFluentValidation()
				.AddQueryType<TestQueryType>()
				.AddMutationType(new TestMutationType(arg => arg.UseFluentValidation()))
				.BuildRequestExecutorAsync();

			withExplicitValidation = await new ServiceCollection()
				.AddSingleton<IValidator<TestInput>, TestInputValidator>()
				.AddGraphQL()
				.AddFluentValidation()
				.AddQueryType<TestQueryType>()
				.AddMutationType(new TestMutationType(arg =>
					arg.UseFluentValidation(opt => opt.UseValidator<IValidator<TestInput>>())))
				.BuildRequestExecutorAsync();

			fluentChocoValidation = await new ServiceCollection()
				.AddSingleton<IValidator<TestInput>, TestInputValidator>()
				.AddGraphQL()
				.UseFluentValidation()
				.AddQueryType<TestQueryType>()
				.AddMutationType(new TestMutationType(_ =>
				{
				}))
				.BuildRequestExecutorAsync();

			fairyBreadValidation = await new ServiceCollection()
				.AddSingleton<IValidator<TestInput>, TestInputValidator>()
				.AddGraphQL()
				.AddFairyBread(opt => opt.AssembliesToScanForValidators = new []{ typeof(Program).Assembly })
				.AddErrorFilter<ValidationErrorFilter>()
				.AddQueryType<TestQueryType>()
				.AddMutationType(new TestMutationType(arg => arg.UseValidation()))
				.BuildRequestExecutorAsync();
		}

		[Benchmark]
		public Task RunWithoutValidation()
		{
			return withoutValidation.ExecuteAsync("mutation { test(input: { name: \"\" }) }");
		}

		[Benchmark]
		public Task ManualValidation()
		{
			var validationContext = ValidationContext<TestInput>.CreateWithOptions(new TestInput(), _ =>
			{
			});

			var validator = new TestInputValidator();

			return validator.ValidateAsync(validationContext);
		}

		[Benchmark]
		public Task RunWithValidation()
		{
			return withValidation.ExecuteAsync("mutation { test(input: { name: \"\" }) }");
		}

		[Benchmark]
		public Task RunWithExplicitValidation()
		{
			return withExplicitValidation.ExecuteAsync("mutation { test(input: { name: \"\" }) }");
		}

		[Benchmark]
		public Task RunWithFluentChocoValidation()
		{
			return fluentChocoValidation.ExecuteAsync("mutation { test(input: { name: \"\" }) }");
		}

		[Benchmark]
		public Task RunWithFairyBreadValidation()
		{
			return fairyBreadValidation.ExecuteAsync("mutation { test(input: { name: \"\" }) }");
		}

		[Benchmark]
		public Task RunWithoutValidation_EmptyInputs()
		{
			return withoutValidation.ExecuteAsync("mutation { test() }");
		}

		[Benchmark]
		public Task RunWithValidation_EmptyInputs()
		{
			return withValidation.ExecuteAsync("mutation { test() }");
		}

		[Benchmark]
		public Task RunWithExplicitValidation_EmptyInputs()
		{
			return withExplicitValidation.ExecuteAsync("mutation { test() }");
		}

		[Benchmark]
		public Task RunWithFluentChocoValidation_EmptyInputs()
		{
			return fluentChocoValidation.ExecuteAsync("mutation { test() }");
		}

		[Benchmark]
		public Task RunWithFairyBreadValidation_EmptyInputs()
		{
			return fairyBreadValidation.ExecuteAsync("mutation { test() }");
		}

		[Benchmark]
		public Task RunWithoutValidation_NullInputs()
		{
			return withoutValidation.ExecuteAsync("mutation { test(input: null) }");
		}

		[Benchmark]
		public Task RunWithValidation_NullInputs()
		{
			return withValidation.ExecuteAsync("mutation { test(input: null) }");
		}

		[Benchmark]
		public Task RunWithExplicitValidation_NullInputs()
		{
			return withExplicitValidation.ExecuteAsync("mutation { test(input: null) }");
		}

		[Benchmark]
		public Task RunWithFluentChocoValidation_NullInputs()
		{
			return fluentChocoValidation.ExecuteAsync("mutation { test(input: null) }");
		}

		[Benchmark]
		public Task RunWithFairyBreadValidation_NullInputs()
		{
			return fairyBreadValidation.ExecuteAsync("mutation { test(input: null) }");
		}
	}
}
