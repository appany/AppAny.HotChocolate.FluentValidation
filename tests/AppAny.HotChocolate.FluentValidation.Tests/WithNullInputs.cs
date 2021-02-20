using System.Threading.Tasks;
using FluentValidation;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class WithNullInputs
	{
		[Fact]
		public async Task Default()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation()
					.AddMutationType(new TestMutation(field =>
					{
						field.Argument("input", arg => arg.Type<TestPersonInputType>().UseFluentValidation());
					})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithNullInput));

			var (key, value) = Assert.Single(result.Data);

			Assert.Equal("test", key);
			Assert.Equal("test", value);

			Assert.Null(result.Errors);
		}

		[Fact]
		public async Task MultipleValidators()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
					.AddMutationType(new TestMutation(field =>
					{
						field.Argument("input", arg => arg.Type<TestPersonInputType>().UseFluentValidation());
					})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>()
						.AddTransient<IValidator<TestPersonInput>, NotEmptyAddressValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithNullInput));

			var (key, value) = Assert.Single(result.Data);

			Assert.Equal("test", key);
			Assert.Equal("test", value);

			Assert.Null(result.Errors);
		}

		[Fact]
		public async Task UseValidator()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
					.AddMutationType(new TestMutation(field =>
					{
						field.Argument("input", arg => arg.Type<TestPersonInputType>().UseFluentValidation());
					})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, DoubleNotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithNullInput));

			var (key, value) = Assert.Single(result.Data);

			Assert.Equal("test", key);
			Assert.Equal("test", value);

			Assert.Null(result.Errors);
		}

		[Fact]
		public async Task UseValidatorOverride()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
					builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input", arg => arg.Type<TestPersonInputType>().UseFluentValidation(opt =>
							{
								opt.UseValidator<IValidator<TestPersonInput>>();
							}));
						})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, DoubleNotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithNullInput));

			var (key, value) = Assert.Single(result.Data);

			Assert.Equal("test", key);
			Assert.Equal("test", value);

			Assert.Null(result.Errors);
		}

		[Fact]
		public async Task UseValidatorsOverride()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
					builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input", arg => arg.Type<TestPersonInputType>().UseFluentValidation(opt =>
							{
								opt.UseValidators<IValidator<TestPersonInput>>();
							}));
						})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, DoubleNotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithNullInput));

			var (key, value) = Assert.Single(result.Data);

			Assert.Equal("test", key);
			Assert.Equal("test", value);

			Assert.Null(result.Errors);
		}

		[Fact]
		public async Task UseValidatorTypedOverride()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
					builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input", arg => arg.Type<TestPersonInputType>().UseFluentValidation(opt =>
							{
								opt.UseValidator(typeof(IValidator<TestPersonInput>));
							}));
						})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, DoubleNotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithNullInput));

			var (key, value) = Assert.Single(result.Data);

			Assert.Equal("test", key);
			Assert.Equal("test", value);

			Assert.Null(result.Errors);
		}

		[Fact]
		public async Task UseValidatorsTypedOverride()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
					builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input", arg => arg.Type<TestPersonInputType>().UseFluentValidation(opt =>
							{
								opt.UseValidators(typeof(IValidator<TestPersonInput>));
							}));
						})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, DoubleNotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithNullInput));

			var (key, value) = Assert.Single(result.Data);

			Assert.Equal("test", key);
			Assert.Equal("test", value);

			Assert.Null(result.Errors);
		}

		[Fact]
		public async Task UseValidatorOverrideWithValidationStrategy()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
					builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input", arg => arg.Type<TestPersonInputType>().UseFluentValidation(opt =>
							{
								opt.UseValidator<IValidator<TestPersonInput>>(strategy => strategy.IncludeProperties("Name"));
							}));
						})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, DoubleNotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithNullInput));

			var (key, value) = Assert.Single(result.Data);

			Assert.Equal("test", key);
			Assert.Equal("test", value);

			Assert.Null(result.Errors);
		}

		[Fact]
		public async Task UseValidatorsOverrideWithValidationStrategy()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
					builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input", arg => arg.Type<TestPersonInputType>().UseFluentValidation(opt =>
							{
								opt.UseValidators<IValidator<TestPersonInput>>(strategy => strategy.IncludeProperties("Name"));
							}));
						})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, DoubleNotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithNullInput));

			var (key, value) = Assert.Single(result.Data);

			Assert.Equal("test", key);
			Assert.Equal("test", value);

			Assert.Null(result.Errors);
		}

		[Fact]
		public async Task UseValidatorTypedOverrideWithValidationStrategy()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
					builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input", arg => arg.Type<TestPersonInputType>().UseFluentValidation(opt =>
							{
								opt.UseValidator(typeof(IValidator<TestPersonInput>), strategy => strategy.IncludeProperties("Name"));
							}));
						})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, DoubleNotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithNullInput));

			var (key, value) = Assert.Single(result.Data);

			Assert.Equal("test", key);
			Assert.Equal("test", value);

			Assert.Null(result.Errors);
		}

		[Fact]
		public async Task UseValidatorsTypedOverrideWithValidationStrategy()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
					builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input", arg => arg.Type<TestPersonInputType>().UseFluentValidation(opt =>
							{
								opt.UseValidators(typeof(IValidator<TestPersonInput>), strategy => strategy.IncludeProperties("Name"));
							}));
						})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, DoubleNotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithNullInput));

			var (key, value) = Assert.Single(result.Data);

			Assert.Equal("test", key);
			Assert.Equal("test", value);

			Assert.Null(result.Errors);
		}

		[Fact]
		public async Task UseValidatorOverrideFullWithValidationStrategy()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
					builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input", arg => arg.Type<TestPersonInputType>().UseFluentValidation(opt =>
							{
								opt.UseValidator<TestPersonInput, IValidator<TestPersonInput>>
									(strategy => strategy.IncludeProperties("Name"));
							}));
						})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, DoubleNotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithNullInput));

			var (key, value) = Assert.Single(result.Data);

			Assert.Equal("test", key);
			Assert.Equal("test", value);

			Assert.Null(result.Errors);
		}

		[Fact]
		public async Task UseValidatorsOverrideFullWithValidationStrategy()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
					builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input", arg => arg.Type<TestPersonInputType>().UseFluentValidation(opt =>
							{
								opt.UseValidators<TestPersonInput, IValidator<TestPersonInput>>
									(strategy => strategy.IncludeProperties("Name"));
							}));
						})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, DoubleNotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithNullInput));

			var (key, value) = Assert.Single(result.Data);

			Assert.Equal("test", key);
			Assert.Equal("test", value);

			Assert.Null(result.Errors);
		}
	}
}
