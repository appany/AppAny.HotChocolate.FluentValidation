using System.Threading.Tasks;
using FluentValidation;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class UseValidationStrategy
	{
		[Fact]
		public async Task DynamicValidationStrategy_Generic_WithContext()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				{
					builder.AddFluentValidation()
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
							{
								opt.UseValidationStrategy<TestPersonInput>((context, strategy) =>
								{
									var input = context.MiddlewareContext.ArgumentValue<TestPersonInput>(context.Argument.Name);

									if (input.Name == "WithName")
									{
										strategy.IncludeProperties(x => x.Name);
									}

									if (input.Address == "WithAddress")
									{
										strategy.IncludeProperties(x => x.Address);
									}
								});
							}));
						}));
				},
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
				});

			var result1 = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result1.AssertNullResult();

			result1.AssertDefaultErrorMapper(
				"NotEmptyValidator",
				NotEmptyNameValidator.Message);

			var result2 = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithName("WithName")));

			result2.AssertSuceessResult();

			var result3 = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithAddress("WithAddress")));

			result3.AssertSuceessResult();
		}

		[Fact]
		public async Task DynamicValidationStrategy_Generic_WithoutContext()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				{
					builder.AddFluentValidation()
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
							{
								opt.UseValidationStrategy<TestPersonInput>(strategy =>
								{
									strategy.IncludeProperties(x => x.Address);
								});
							}));
						}));
				},
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertSuceessResult();
		}

		[Fact]
		public async Task DynamicValidationStrategy_WithContext()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				{
					builder.AddFluentValidation()
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
							{
								opt.UseValidationStrategy((context, strategy) =>
								{
									var input = context.MiddlewareContext.ArgumentValue<TestPersonInput>(context.Argument.Name);

									if (input.Name == "WithName")
									{
										strategy.IncludeProperties("Name");
									}

									if (input.Address == "WithAddress")
									{
										strategy.IncludeProperties("Address");
									}
								});
							}));
						}));
				},
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
				});

			var result1 = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result1.AssertNullResult();

			result1.AssertDefaultErrorMapper(
				"NotEmptyValidator",
				NotEmptyNameValidator.Message);

			var result2 = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithName("WithName")));

			result2.AssertSuceessResult();

			var result3 = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithAddress("WithAddress")));

			result3.AssertSuceessResult();
		}

		[Fact]
		public async Task DynamicValidationStrategy_WithoutContext()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				{
					builder.AddFluentValidation()
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
							{
								opt.UseValidationStrategy(strategy =>
								{
									strategy.IncludeProperties("Address");
								});
							}));
						}));
				},
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertSuceessResult();
		}

		[Fact]
		public async Task DynamicValidationStrategy_WithNullInput()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				{
					builder.AddFluentValidation()
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input", arg => arg.Type<TestPersonInputType>().UseFluentValidation(opt =>
							{
								opt.UseValidationStrategy(strategy =>
								{
									strategy.IncludeProperties("Address");
								});
							}));
						}));
				},
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithNullInput));

			result.AssertSuceessResult();
		}
	}
}
