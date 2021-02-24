using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class ValidationStrategies
	{
		[Fact]
		public async Task DynamicValidationStrategy()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
					builder.AddFluentValidation()
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
							{
								opt.UseValidator<TestPersonInput, IValidator<TestPersonInput>>((context, strategy) =>
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
						})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
				});

			var result1 = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result1.AssertNullResult();

			var error1 = Assert.Single(result1.Errors);

			Assert.Equal(nameof(NotEmptyValidator), error1.Code);
			Assert.Equal(NotEmptyNameValidator.Message, error1.Message);

			Assert.Collection(error1.Extensions,
				code =>
				{
					Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
					Assert.Equal(nameof(NotEmptyValidator), code.Value);
				});

			var result2 = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithName("WithName")));

			var (key1, value1) = Assert.Single(result2.Data);

			Assert.Equal("test", key1);
			Assert.Equal("test", value1);

			Assert.Null(result2.Errors);

			var result3 = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithAddress("WithAddress")));

			var (key2, value2) = Assert.Single(result3.Data);

			Assert.Equal("test", key2);
			Assert.Equal("test", value2);

			Assert.Null(result3.Errors);
		}

		[Fact]
		public async Task DynamicValidationStrategy_UseValidator_Strategy()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
					builder.AddFluentValidation()
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
							{
								opt.UseValidator<IValidator<TestPersonInput>>((context, strategy) =>
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
						})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
				});

			var result1 = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result1.AssertNullResult();

			var error1 = Assert.Single(result1.Errors);

			Assert.Equal(nameof(NotEmptyValidator), error1.Code);
			Assert.Equal(NotEmptyNameValidator.Message, error1.Message);

			Assert.Collection(error1.Extensions,
				code =>
				{
					Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
					Assert.Equal(nameof(NotEmptyValidator), code.Value);
				});

			var result2 = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithName("WithName")));

			var (key1, value1) = Assert.Single(result2.Data);

			Assert.Equal("test", key1);
			Assert.Equal("test", value1);

			Assert.Null(result2.Errors);

			var result3 = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithAddress("WithAddress")));

			var (key2, value2) = Assert.Single(result3.Data);

			Assert.Equal("test", key2);
			Assert.Equal("test", value2);

			Assert.Null(result3.Errors);
		}

		[Fact]
		public async Task DynamicValidationStrategy_UseValidatorsStrategy()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
					builder.AddFluentValidation()
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
							{
								opt.UseValidators<IValidator<TestPersonInput>>((context, strategy) =>
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
						})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
				});

			var result1 = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result1.AssertNullResult();

			var error1 = Assert.Single(result1.Errors);

			Assert.Equal(nameof(NotEmptyValidator), error1.Code);
			Assert.Equal(NotEmptyNameValidator.Message, error1.Message);

			Assert.Collection(error1.Extensions,
				code =>
				{
					Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
					Assert.Equal(nameof(NotEmptyValidator), code.Value);
				});

			var result2 = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithName("WithName")));

			var (key1, value1) = Assert.Single(result2.Data);

			Assert.Equal("test", key1);
			Assert.Equal("test", value1);

			Assert.Null(result2.Errors);

			var result3 = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithAddress("WithAddress")));

			var (key2, value2) = Assert.Single(result3.Data);

			Assert.Equal("test", key2);
			Assert.Equal("test", value2);

			Assert.Null(result3.Errors);
		}

		[Fact]
		public async Task DynamicValidationStrategy_UseValidators()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
					builder.AddFluentValidation()
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
							{
								opt.UseValidators<TestPersonInput, IValidator<TestPersonInput>>((context, strategy) =>
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
						})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
				});

			var result1 = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result1.AssertNullResult();

			var error1 = Assert.Single(result1.Errors);

			Assert.Equal(nameof(NotEmptyValidator), error1.Code);
			Assert.Equal(NotEmptyNameValidator.Message, error1.Message);

			Assert.Collection(error1.Extensions,
				code =>
				{
					Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
					Assert.Equal(nameof(NotEmptyValidator), code.Value);
				});

			var result2 = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithName("WithName")));

			var (key1, value1) = Assert.Single(result2.Data);

			Assert.Equal("test", key1);
			Assert.Equal("test", value1);

			Assert.Null(result2.Errors);

			var result3 = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithAddress("WithAddress")));

			var (key2, value2) = Assert.Single(result3.Data);

			Assert.Equal("test", key2);
			Assert.Equal("test", value2);

			Assert.Null(result3.Errors);
		}

		[Fact]
		public async Task DynamicValidationStrategy_Typed()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
					builder.AddFluentValidation()
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
							{
								opt.UseValidator(typeof(IValidator<TestPersonInput>), (context, strategy) =>
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
						})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
				});

			var result1 = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result1.AssertNullResult();

			var error1 = Assert.Single(result1.Errors);

			Assert.Equal(nameof(NotEmptyValidator), error1.Code);
			Assert.Equal(NotEmptyNameValidator.Message, error1.Message);

			Assert.Collection(error1.Extensions,
				code =>
				{
					Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
					Assert.Equal(nameof(NotEmptyValidator), code.Value);
				});

			var result2 = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithName("WithName")));

			var (key1, value1) = Assert.Single(result2.Data);

			Assert.Equal("test", key1);
			Assert.Equal("test", value1);

			Assert.Null(result2.Errors);

			var result3 = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithAddress("WithAddress")));

			var (key2, value2) = Assert.Single(result3.Data);

			Assert.Equal("test", key2);
			Assert.Equal("test", value2);

			Assert.Null(result3.Errors);
		}

		[Fact]
		public async Task DynamicValidationStrategy_UseValidators_Typed()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
					builder.AddFluentValidation()
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
							{
								opt.UseValidators(typeof(IValidator<TestPersonInput>), (context, strategy) =>
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
						})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
				});

			var result1 = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result1.AssertNullResult();

			var error1 = Assert.Single(result1.Errors);

			Assert.Equal(nameof(NotEmptyValidator), error1.Code);
			Assert.Equal(NotEmptyNameValidator.Message, error1.Message);

			Assert.Collection(error1.Extensions,
				code =>
				{
					Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
					Assert.Equal(nameof(NotEmptyValidator), code.Value);
				});

			var result2 = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithName("WithName")));

			var (key1, value1) = Assert.Single(result2.Data);

			Assert.Equal("test", key1);
			Assert.Equal("test", value1);

			Assert.Null(result2.Errors);

			var result3 = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithAddress("WithAddress")));

			var (key2, value2) = Assert.Single(result3.Data);

			Assert.Equal("test", key2);
			Assert.Equal("test", value2);

			Assert.Null(result3.Errors);
		}
	}
}
