using System.Threading.Tasks;
using FluentValidation;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class InputValidatorProviderContextProperties
	{
		[Fact]
		public async Task Should_Pass_Values_AddFluentValidation()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation(opt => opt.UseErrorMappers(ValidationDefaults.ErrorMappers.Default)
					.UseInputValidatorProviders(context =>
					{
						Assert.Equal(typeof(TestPersonInput), context.Argument.RuntimeType);

						return ValidationDefaults.InputValidatorProviders.Default(context);
					}))
					.AddMutationType(new TestMutation(field => field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation())))
					.Services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>());

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

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
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation(opt => opt.UseErrorMappers(ValidationDefaults.ErrorMappers.Default))
					.AddMutationType(new TestMutation(field => field.Argument("input",
						arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
						{
							opt.UseInputValidatorProviders(context =>
							{
								Assert.Equal(typeof(TestPersonInput), context.Argument.RuntimeType);

								return ValidationDefaults.InputValidatorProviders.Default(context);
							});
						}))))
					.Services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>());

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

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
