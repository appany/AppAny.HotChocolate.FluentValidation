using System.Threading.Tasks;
using FluentValidation;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class UseValidators
	{
		[Fact]
		public async Task UseValidatorsGeneric()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation()
					.AddMutationType(new TestMutation(field =>
					{
						field.Argument("input",
							arg => arg.Type<NonNullType<TestPersonInputType>>()
								.UseFluentValidation(opt =>
								{
									opt.UseValidators<IValidator<TestPersonInput>>();
								}));
					})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>()
						.AddTransient<IValidator<TestPersonInput>, NotEmptyAddressValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				nameIsEmpty => Assert.Equal(NotEmptyNameValidator.Message, nameIsEmpty.Message),
				addressIsEmpty => Assert.Equal(NotEmptyAddressValidator.Message, addressIsEmpty.Message));
		}

		[Fact]
		public async Task UseValidatorsWithType()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation()
					.AddMutationType(new TestMutation(field =>
					{
						field.Argument("input",
							arg => arg.Type<NonNullType<TestPersonInputType>>()
								.UseFluentValidation(opt =>
								{
									opt.UseValidators(typeof(IValidator<TestPersonInput>));
								}));
					})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>()
						.AddTransient<IValidator<TestPersonInput>, NotEmptyAddressValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				nameIsEmpty => Assert.Equal(NotEmptyNameValidator.Message, nameIsEmpty.Message),
				addressIsEmpty => Assert.Equal(NotEmptyAddressValidator.Message, addressIsEmpty.Message));
		}

		[Fact]
		public async Task UseValidatorsGenericWithInputParameter()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation()
					.AddMutationType(new TestMutation(field =>
					{
						field.Argument("input",
							arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
							{
								opt.UseValidators<TestPersonInput, IValidator<TestPersonInput>>();
							}));
					})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>()
						.AddTransient<IValidator<TestPersonInput>, NotEmptyAddressValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				nameIsEmpty => Assert.Equal(NotEmptyNameValidator.Message, nameIsEmpty.Message),
				addressIsEmpty => Assert.Equal(NotEmptyAddressValidator.Message, addressIsEmpty.Message));
		}

		[Fact]
		public async Task UseValidatorsGenericWithInputParameterAndValidationStrategy()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation()
					.AddMutationType(new TestMutation(field =>
					{
						field.Argument("input",
							arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
							{
								opt.UseValidators<TestPersonInput, IValidator<TestPersonInput>>(strategy =>
								{
									strategy.IncludeProperties(input => input.Name, input => input.Address);
								});
							}));
					})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>()
						.AddTransient<IValidator<TestPersonInput>, NotEmptyAddressValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				nameIsEmpty => Assert.Equal(NotEmptyNameValidator.Message, nameIsEmpty.Message),
				addressIsEmpty => Assert.Equal(NotEmptyAddressValidator.Message, addressIsEmpty.Message));
		}

		[Fact]
		public async Task UseValidatorsGenericWithInputParameterAndPartialValidationStrategy()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation()
					.AddMutationType(new TestMutation(field =>
					{
						field.Argument("input",
							arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
							{
								opt.UseValidators<TestPersonInput, IValidator<TestPersonInput>>(strategy =>
								{
									strategy.IncludeProperties(input => input.Name);
								});
							}));
					})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>()
						.AddTransient<IValidator<TestPersonInput>, NotEmptyAddressValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				nameIsEmpty => Assert.Equal(NotEmptyNameValidator.Message, nameIsEmpty.Message));
		}

		[Fact]
		public async Task UseValidatorGenericWithValidationStrategyName()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
					builder.AddFluentValidation()
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input",
								arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
								{
									opt.UseValidator<IValidator<TestPersonInput>>(strategy =>
									{
										strategy.IncludeProperties("Name");
									});
								}));
						})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				nameIsEmpty => Assert.Equal(NotEmptyNameValidator.Message, nameIsEmpty.Message));
		}

		[Fact]
		public async Task UseValidatorGenericWithValidationStrategyAddress()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
					builder.AddFluentValidation()
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input",
								arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
								{
									opt.UseValidator<IValidator<TestPersonInput>>(strategy =>
									{
										strategy.IncludeProperties("Address");
									});
								}));
						})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyAddressValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				addressIsEmpty => Assert.Equal(NotEmptyAddressValidator.Message, addressIsEmpty.Message));
		}

		[Fact]
		public async Task UseValidatorGenericWithValidationStrategy()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
					builder.AddFluentValidation()
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input",
								arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
								{
									opt.UseValidator<IValidator<TestPersonInput>>(strategy =>
									{
										strategy.IncludeProperties("NotExists");
									});
								}));
						})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>()
						.AddTransient<IValidator<TestPersonInput>, NotEmptyAddressValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			var (key, value) = Assert.Single(result.Data);

			Assert.Equal("test", key);
			Assert.Equal("test", value);

			Assert.Null(result.Errors);
		}

		[Fact]
		public async Task UseValidatorsGenericWithValidationStrategyNameAndAddress()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation()
					.AddMutationType(new TestMutation(field =>
					{
						field.Argument("input",
							arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
							{
								opt.UseValidators<IValidator<TestPersonInput>>(strategy =>
								{
									strategy.IncludeProperties("Name", "Address");
								});
							}));
					})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>()
						.AddTransient<IValidator<TestPersonInput>, NotEmptyAddressValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				nameIsEmpty => Assert.Equal(NotEmptyNameValidator.Message, nameIsEmpty.Message),
				addressIsEmpty => Assert.Equal(NotEmptyAddressValidator.Message, addressIsEmpty.Message));
		}

		[Fact]
		public async Task UseValidatorsGenericWithValidationStrategyName()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation()
					.AddMutationType(new TestMutation(field =>
					{
						field.Argument("input",
							arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
							{
								opt.UseValidators<TestPersonInput, IValidator<TestPersonInput>>(strategy =>
								{
									strategy.IncludeProperties("Name");
								});
							}));
					})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>()
						.AddTransient<IValidator<TestPersonInput>, NotEmptyAddressValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				nameIsEmpty => Assert.Equal(NotEmptyNameValidator.Message, nameIsEmpty.Message));
		}
	}
}
