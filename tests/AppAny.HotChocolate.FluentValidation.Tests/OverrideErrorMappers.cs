using System.Threading.Tasks;
using FluentValidation;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class OverrideErrorMappers
	{
		[Fact]
		public async Task Should_SetExtension_AddFluentValidation()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation(opt =>
						opt.UseErrorMappers(
							ValidationDefaults.ErrorMappers.Default,
							(errorBuilder, _) => errorBuilder.SetExtension("test", "test")))
					.AddMutationType(new TestMutation(arg => arg.UseFluentValidation()))
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
				},
				test =>
				{
					Assert.Equal("test", test.Key);
					Assert.Equal("test", test.Value);
				});
		}

		[Fact]
		public async Task Should_SetExtension_UseFluentValidation()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation(opt => opt
						.UseErrorMappers(ValidationDefaults.ErrorMappers.Default))
					.AddMutationType(new TestMutation(arg => arg.UseFluentValidation(
						opt =>
						{
							opt.UseErrorMappers(
								ValidationDefaults.ErrorMappers.Default,
								(errorBuilder, _) => errorBuilder.SetExtension("test", "test"));
						})))
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
				},
				test =>
				{
					Assert.Equal("test", test.Key);
					Assert.Equal("test", test.Value);
				});
		}
	}
}
