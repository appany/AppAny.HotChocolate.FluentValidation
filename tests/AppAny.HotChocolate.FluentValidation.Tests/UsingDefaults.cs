using System.Threading.Tasks;
using FluentValidation;
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
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation()
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
		public async Task Should_UseMultipleValidators_ByConvention()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
					.AddMutationType(new TestMutation(field => field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation())))
					.Services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>()
					.AddTransient<IValidator<TestPersonInput>, NotEmptyAddressValidator>());

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyNameAndAddress));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				name =>
				{
					Assert.Equal(ValidationDefaults.Code, name.Code);
					Assert.Equal(NotEmptyNameValidator.Message, name.Message);

					Assert.Collection(name.Extensions,
						code =>
						{
							Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
							Assert.Equal(ValidationDefaults.Code, code.Value);
						});
				},
				address =>
				{
					Assert.Equal(ValidationDefaults.Code, address.Code);
					Assert.Equal(NotEmptyAddressValidator.Message, address.Message);

					Assert.Collection(address.Extensions,
						code =>
						{
							Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
							Assert.Equal(ValidationDefaults.Code, code.Value);
						});
				});
		}

		[Fact]
		public async Task Should_UseValidatorByConvention_DoubleProperty()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
					.AddMutationType(new TestMutation(field => field.Argument("input",  arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation())))
					.Services.AddTransient<IValidator<TestPersonInput>, DoubleNotEmptyNameValidator>());

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				name =>
				{
					Assert.Equal(ValidationDefaults.Code, name.Code);
					Assert.Equal(DoubleNotEmptyNameValidator.Message1, name.Message);

					Assert.Collection(name.Extensions,
						code =>
						{
							Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
							Assert.Equal(ValidationDefaults.Code, code.Value);
						});
				},
				name =>
				{
					Assert.Equal(ValidationDefaults.Code, name.Code);
					Assert.Equal(DoubleNotEmptyNameValidator.Message2, name.Message);

					Assert.Collection(name.Extensions,
						code =>
						{
							Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
							Assert.Equal(ValidationDefaults.Code, code.Value);
						});
				});
		}
	}
}
