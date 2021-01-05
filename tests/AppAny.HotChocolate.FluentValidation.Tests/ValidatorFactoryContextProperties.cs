using System.Threading.Tasks;
using FluentValidation;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class ValidatorFactoryContextProperties
	{
		[Fact]
		public async Task Should_Pass_Values_AddFluentValidation()
		{
			var executor = await new ServiceCollection()
				.AddTransient<IValidator<TestInput>, NotEmptyNameValidator>()
				.AddTestGraphQL()
				.AddFluentValidation(configurator => configurator
					.UseErrorMappers(ValidationDefaults.ErrorMappers.Default)
					.UseValidatorFactories(context =>
					{
						Assert.Equal(typeof(TestInput), context.InputFieldType);

						return ValidationDefaults.ValidationFactories.Default(context);
					}))
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
				});
		}

		[Fact]
		public async Task Should_Pass_Values_UseFluentValidation()
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
						.Argument("input", arg => arg.Type<NonNullType<TestInputType>>().UseFluentValidation(configurator =>
						{
							configurator.UseValidatorFactories(context =>
							{
								Assert.Equal(typeof(TestInput), context.InputFieldType);

								return ValidationDefaults.ValidationFactories.Default(context);
							});
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
	}
}