using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class OverrideUseFluentValidation
	{
		[Fact]
		public async Task Should_UseOnlyDefaultErrorMapper()
		{
			var executor = await new ServiceCollection()
				.AddTransient<IValidator<TestInput>, NotEmptyNameValidator>()
				.AddTestGraphQL()
				.AddFluentValidation(configurator => configurator
					.UseErrorMappers(ValidationDefaults.ErrorMappers.Extensions))
				.AddMutationType(descriptor =>
				{
					descriptor.Name("Mutation");

					descriptor.Field("test")
						.Type<StringType>()
						.Argument("input", arg => arg.Type<NonNullType<TestInputType>>().UseFluentValidation(fv =>
						{
							fv.UseErrorMappers(ValidationDefaults.ErrorMappers.Default);
						}))
						.Resolve("test");
				})
				.BuildRequestExecutorAsync();

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync("mutation { test(input: { name: \"\" }) }"));

			result.AssertNullResult();

			var error = Assert.Single(result.Errors);

			Assert.Equal(ValidationDefaults.ErrorCode, error.Code);
			Assert.Equal(NotEmptyNameValidator.Message, error.Message);

			Assert.Collection(error.Extensions,
				code =>
				{
					Assert.Equal(ValidationDefaults.Keys.ErrorCodeKey, code.Key);
					Assert.Equal(ValidationDefaults.ErrorCode, code.Value);
				});
		}

		[Fact]
		public async Task Should_UseDefaultAndExtensionsErrorMapper()
		{
			var executor = await new ServiceCollection()
				.AddTransient<IValidator<TestInput>, NotEmptyNameValidator>()
				.AddTestGraphQL()
				.AddFluentValidation(configurator => configurator
					.UseErrorMappers(ValidationDefaults.ErrorMappers.Default))
				.AddMutationType(descriptor =>
				{
					descriptor.Name("Mutation");

					descriptor.Field("test")
						.Type<StringType>()
						.Argument("input", arg => arg.Type<NonNullType<TestInputType>>().UseFluentValidation(fv =>
						{
							fv.UseErrorMappers(ValidationDefaults.ErrorMappers.Default, ValidationDefaults.ErrorMappers.Extensions);
						}))
						.Resolve("test");
				})
				.BuildRequestExecutorAsync();

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync("mutation { test(input: { name: \"\" }) }"));

			result.AssertNullResult();

			var error = Assert.Single(result.Errors);

			Assert.Equal(ValidationDefaults.ErrorCode, error.Code);
			Assert.Equal(NotEmptyNameValidator.Message, error.Message);

			Assert.Collection(error.Extensions,
				code =>
				{
					Assert.Equal(ValidationDefaults.Keys.ErrorCodeKey, code.Key);
					Assert.Equal(ValidationDefaults.ErrorCode, code.Value);
				},
				validator =>
				{
					Assert.Equal(ValidationDefaults.Keys.ValidatorKey, validator.Key);
					Assert.Equal(nameof(NotEmptyValidator), validator.Value);
				},
				inputField =>
				{
					Assert.Equal(ValidationDefaults.Keys.InputFieldKey, inputField.Key);
					Assert.Equal(new NameString("input"), inputField.Value);
				},
				property =>
				{
					Assert.Equal(ValidationDefaults.Keys.PropertyKey, property.Key);
					Assert.Equal("Name", property.Value);
				},
				severity =>
				{
					Assert.Equal(ValidationDefaults.Keys.SeverityKey, severity.Key);
					Assert.Equal(Severity.Error, severity.Value);
				},
				attemptedValue =>
				{
					Assert.Equal(ValidationDefaults.Keys.AttemptedValueKey, attemptedValue.Key);
					Assert.Equal("", attemptedValue.Value);
				});
		}

		[Fact]
		public async Task Should_UseCustomValidator()
		{
			var executor = await new ServiceCollection()
				.AddTransient<NotEmptyNameValidator>()
				.AddTestGraphQL()
				.AddFluentValidation(configurator => configurator
					.UseErrorMappers(ValidationDefaults.ErrorMappers.Default))
				.AddMutationType(descriptor =>
				{
					descriptor.Name("Mutation");

					descriptor.Field("test")
						.Type<StringType>()
						.Argument("input", arg => arg.Type<NonNullType<TestInputType>>().UseFluentValidation(fv =>
						{
							fv.UseValidator<NotEmptyNameValidator>();
						}))
						.Resolve("test");
				})
				.BuildRequestExecutorAsync();

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync("mutation { test(input: { name: \"\" }) }"));

			result.AssertNullResult();

			var error = Assert.Single(result.Errors);

			Assert.Equal(ValidationDefaults.ErrorCode, error.Code);
			Assert.Equal(NotEmptyNameValidator.Message, error.Message);

			Assert.Collection(error.Extensions,
				code =>
				{
					Assert.Equal(ValidationDefaults.Keys.ErrorCodeKey, code.Key);
					Assert.Equal(ValidationDefaults.ErrorCode, code.Value);
				});
		}

		[Fact]
		public async Task Should_UseCustomValidatorFactory()
		{
			var executor = await new ServiceCollection()
				.AddTransient<NotEmptyNameValidator>()
				.AddTestGraphQL()
				.AddFluentValidation(configurator => configurator
					.UseErrorMappers(ValidationDefaults.ErrorMappers.Default))
				.AddMutationType(descriptor =>
				{
					descriptor.Name("Mutation");

					descriptor.Field("test")
						.Type<StringType>()
						.Argument("input", arg => arg.Type<NonNullType<TestInputType>>().UseFluentValidation(fv =>
						{
							fv.UseValidatorFactories(context => context.ServiceProvider.GetServices<NotEmptyNameValidator>());
						}))
						.Resolve("test");
				})
				.BuildRequestExecutorAsync();

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync("mutation { test(input: { name: \"\" }) }"));

			result.AssertNullResult();

			var error = Assert.Single(result.Errors);

			Assert.Equal(ValidationDefaults.ErrorCode, error.Code);
			Assert.Equal(NotEmptyNameValidator.Message, error.Message);

			Assert.Collection(error.Extensions,
				code =>
				{
					Assert.Equal(ValidationDefaults.Keys.ErrorCodeKey, code.Key);
					Assert.Equal(ValidationDefaults.ErrorCode, code.Value);
				});
		}

		[Fact]
		public async Task Should_UseMultipleCustomValidators()
		{
			var executor = await new ServiceCollection()
				.AddTransient<NotEmptyNameValidator>()
				.AddTransient<NotEmptyAddressValidator>()
				.AddTestGraphQL()
				.AddFluentValidation(configurator => configurator
					.UseErrorMappers(ValidationDefaults.ErrorMappers.Default))
				.AddMutationType(descriptor =>
				{
					descriptor.Name("Mutation");

					descriptor.Field("test")
						.Type<StringType>()
						.Argument("input", arg => arg.Type<NonNullType<TestInputType>>().UseFluentValidation(fv =>
						{
							fv.UseValidator<NotEmptyNameValidator>()
								.UseValidator<NotEmptyAddressValidator>();
						}))
						.Resolve("test");
				})
				.BuildRequestExecutorAsync();

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync("mutation { test(input: { name: \"\", address: \"\" }) }"));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				name =>
				{
					Assert.Equal(ValidationDefaults.ErrorCode, name.Code);
					Assert.Equal(NotEmptyNameValidator.Message, name.Message);

					Assert.Collection(name.Extensions,
						code =>
						{
							Assert.Equal(ValidationDefaults.Keys.ErrorCodeKey, code.Key);
							Assert.Equal(ValidationDefaults.ErrorCode, code.Value);
						});
				},
				address =>
				{
					Assert.Equal(ValidationDefaults.ErrorCode, address.Code);
					Assert.Equal(NotEmptyAddressValidator.Message, address.Message);

					Assert.Collection(address.Extensions,
						code =>
						{
							Assert.Equal(ValidationDefaults.Keys.ErrorCodeKey, code.Key);
							Assert.Equal(ValidationDefaults.ErrorCode, code.Value);
						});
				});
		}

		[Fact]
		public async Task Should_UseMultipleCustomValidators_SameProperty()
		{
			var executor = await new ServiceCollection()
				.AddTransient<NotEmptyNameValidator>()
				.AddTransient<NotEmptyAddressValidator>()
				.AddTestGraphQL()
				.AddFluentValidation(configurator => configurator
					.UseErrorMappers(ValidationDefaults.ErrorMappers.Default))
				.AddMutationType(descriptor =>
				{
					descriptor.Name("Mutation");

					descriptor.Field("test")
						.Type<StringType>()
						.Argument("input", arg => arg.Type<NonNullType<TestInputType>>().UseFluentValidation(fv =>
						{
							fv.UseValidator<NotEmptyNameValidator>()
								.UseValidator<NotEmptyNameValidator>();
						}))
						.Resolve("test");
				})
				.BuildRequestExecutorAsync();

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync("mutation { test(input: { name: \"\" }) }"));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				name =>
				{
					Assert.Equal(ValidationDefaults.ErrorCode, name.Code);
					Assert.Equal(NotEmptyNameValidator.Message, name.Message);

					Assert.Collection(name.Extensions,
						code =>
						{
							Assert.Equal(ValidationDefaults.Keys.ErrorCodeKey, code.Key);
							Assert.Equal(ValidationDefaults.ErrorCode, code.Value);
						});
				},
				name =>
				{
					Assert.Equal(ValidationDefaults.ErrorCode, name.Code);
					Assert.Equal(NotEmptyNameValidator.Message, name.Message);

					Assert.Collection(name.Extensions,
						code =>
						{
							Assert.Equal(ValidationDefaults.Keys.ErrorCodeKey, code.Key);
							Assert.Equal(ValidationDefaults.ErrorCode, code.Value);
						});
				});
		}

		[Fact]
		public async Task Should_UseSingleCustomValidator_DoubleProperty()
		{
			var executor = await new ServiceCollection()
				.AddTransient<DoubleNotEmptyNameValidator>()
				.AddTestGraphQL()
				.AddFluentValidation(configurator => configurator
					.UseErrorMappers(ValidationDefaults.ErrorMappers.Default))
				.AddMutationType(descriptor =>
				{
					descriptor.Name("Mutation");

					descriptor.Field("test")
						.Type<StringType>()
						.Argument("input", arg => arg.Type<NonNullType<TestInputType>>().UseFluentValidation(fv =>
						{
							fv.UseValidator<DoubleNotEmptyNameValidator>();
						}))
						.Resolve("test");
				})
				.BuildRequestExecutorAsync();

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync("mutation { test(input: { name: \"\" }) }"));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				name =>
				{
					Assert.Equal(ValidationDefaults.ErrorCode, name.Code);
					Assert.Equal(DoubleNotEmptyNameValidator.Message1, name.Message);

					Assert.Collection(name.Extensions,
						code =>
						{
							Assert.Equal(ValidationDefaults.Keys.ErrorCodeKey, code.Key);
							Assert.Equal(ValidationDefaults.ErrorCode, code.Value);
						});
				},
				name =>
				{
					Assert.Equal(ValidationDefaults.ErrorCode, name.Code);
					Assert.Equal(DoubleNotEmptyNameValidator.Message2, name.Message);

					Assert.Collection(name.Extensions,
						code =>
						{
							Assert.Equal(ValidationDefaults.Keys.ErrorCodeKey, code.Key);
							Assert.Equal(ValidationDefaults.ErrorCode, code.Value);
						});
				});
		}

		[Fact]
		public async Task Should_Execute_SkipValidation()
		{
			var executor = await new ServiceCollection()
				.AddTransient<IValidator<TestInput>, NotEmptyNameValidator>()
				.AddTestGraphQL()
				.AddFluentValidation()
				.AddMutationType(descriptor =>
				{
					descriptor.Name("Mutation");

					descriptor.Field("test")
						.Type<StringType>()
						.Argument("input", arg => arg.Type<NonNullType<TestInputType>>().UseFluentValidation(configurator =>
						{
							configurator.SkipValidation();
						}))
						.Resolve("test");
				})
				.BuildRequestExecutorAsync();

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync("mutation { test(input: { name: \"\" }) }"));

			var (key, value) = Assert.Single(result.Data);

			Assert.Equal("test", key);
			Assert.Equal("test", value);

			Assert.Null(result.Errors);
		}

		[Fact]
		public async Task Should_Execute_SkipValidation_WithCustomValidator()
		{
			var executor = await new ServiceCollection()
				.AddTransient<NotEmptyNameValidator>()
				.AddTestGraphQL()
				.AddFluentValidation()
				.AddMutationType(descriptor =>
				{
					descriptor.Name("Mutation");

					descriptor.Field("test")
						.Type<StringType>()
						.Argument("input", arg => arg.Type<NonNullType<TestInputType>>().UseFluentValidation(configurator =>
						{
							configurator.SkipValidation().UseValidator<NotEmptyNameValidator>();
						}))
						.Resolve("test");
				})
				.BuildRequestExecutorAsync();

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync("mutation { test(input: { name: \"\" }) }"));

			var (key, value) = Assert.Single(result.Data);

			Assert.Equal("test", key);
			Assert.Equal("test", value);

			Assert.Null(result.Errors);
		}

		[Fact]
		public async Task Should_Execute_SkipValidation_WithCustomValidatorFactory()
		{
			var executor = await new ServiceCollection()
				.AddTransient<NotEmptyNameValidator>()
				.AddTestGraphQL()
				.AddFluentValidation()
				.AddMutationType(descriptor =>
				{
					descriptor.Name("Mutation");

					descriptor.Field("test")
						.Type<StringType>()
						.Argument("input", arg => arg.Type<NonNullType<TestInputType>>().UseFluentValidation(configurator =>
						{
							configurator.SkipValidation().UseValidatorFactories(_ => new[]
							{
								new NotEmptyNameValidator()
							});
						}))
						.Resolve("test");
				})
				.BuildRequestExecutorAsync();

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync("mutation { test(input: { name: \"\" }) }"));

			var (key, value) = Assert.Single(result.Data);

			Assert.Equal("test", key);
			Assert.Equal("test", value);

			Assert.Null(result.Errors);
		}
	}
}
