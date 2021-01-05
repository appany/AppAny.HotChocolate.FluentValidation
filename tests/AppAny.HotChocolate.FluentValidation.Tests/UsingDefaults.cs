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
	public class UsingDefaults
	{
		[Fact]
		public async Task Should_HaveNullResult_ValidationError_ExtensionCodes()
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
						.Argument("input", arg => arg.Type<NonNullType<TestInputType>>().UseFluentValidation())
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
		public async Task Should_UseMultipleValidators_ByConvention()
		{
			var executor = await new ServiceCollection()
				.AddTransient<IValidator<TestInput>, NotEmptyNameValidator>()
				.AddTransient<IValidator<TestInput>, NotEmptyAddressValidator>()
				.AddTestGraphQL()
				.AddFluentValidation(configurator => configurator
					.UseErrorMappers(ValidationDefaults.ErrorMappers.Default))
				.AddMutationType(descriptor =>
				{
					descriptor.Name("Mutation");

					descriptor.Field("test")
						.Type<StringType>()
						.Argument("input", arg => arg.Type<NonNullType<TestInputType>>().UseFluentValidation())
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
		public async Task Should_UseValidatorByConvention_DoubleProperty()
		{
			var executor = await new ServiceCollection()
				.AddTransient<IValidator<TestInput>, DoubleNotEmptyNameValidator>()
				.AddTestGraphQL()
				.AddFluentValidation(configurator => configurator
					.UseErrorMappers(ValidationDefaults.ErrorMappers.Default))
				.AddMutationType(descriptor =>
				{
					descriptor.Name("Mutation");

					descriptor.Field("test")
						.Type<StringType>()
						.Argument("input", arg => arg.Type<NonNullType<TestInputType>>().UseFluentValidation())
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
	}
}