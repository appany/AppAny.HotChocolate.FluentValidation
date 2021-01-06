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
				.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>()
				.AddTestGraphQL()
				.AddFluentValidation(configurator => configurator
					.UseErrorMappers(ValidationDefaults.ErrorMappers.Default)
					.UseValidatorFactories(context =>
					{
						Assert.Equal(typeof(TestPersonInput), context.InputFieldType);

						return ValidationDefaults.ValidatorFactories.Default(context);
					}))
				.AddMutationType(descriptor =>
				{
					descriptor.Name("Mutation");

					descriptor.Field("test")
						.Type<StringType>()
						.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation())
						.Resolve("test");
				})
				.BuildRequestExecutorAsync();

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestMutations.EmptyName));

			result.AssertNullResult();

			var error = Assert.Single(result.Errors);

			Assert.Equal(ValidationDefaults.Code, error.Code);
			Assert.Equal(NotEmptyNameValidator.Message, error.Message);

			Assert.Collection(error.Extensions,
				code =>
				{
					Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
					Assert.Equal(ValidationDefaults.Code, code.Value);
				});
		}

		[Fact]
		public async Task Should_Pass_Values_UseFluentValidation()
		{
			var executor = await new ServiceCollection()
				.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>()
				.AddTestGraphQL()
				.AddFluentValidation(configurator => configurator
					.UseErrorMappers(ValidationDefaults.ErrorMappers.Default))
				.AddMutationType(descriptor =>
				{
					descriptor.Name("Mutation");

					descriptor.Field("test")
						.Type<StringType>()
						.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(configurator =>
						{
							configurator.UseValidatorFactories(context =>
							{
								Assert.Equal(typeof(TestPersonInput), context.InputFieldType);

								return ValidationDefaults.ValidatorFactories.Default(context);
							});
						}))
						.Resolve("test");
				})
				.BuildRequestExecutorAsync();

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestMutations.EmptyName));

			result.AssertNullResult();

			var error = Assert.Single(result.Errors);

			Assert.Equal(ValidationDefaults.Code, error.Code);
			Assert.Equal(NotEmptyNameValidator.Message, error.Message);

			Assert.Collection(error.Extensions,
				code =>
				{
					Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
					Assert.Equal(ValidationDefaults.Code, code.Value);
				});
		}
	}
}
