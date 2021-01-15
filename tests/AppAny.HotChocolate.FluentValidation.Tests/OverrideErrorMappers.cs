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
			var executor = await new ServiceCollection()
				.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>()
				.AddTestGraphQL()
				.AddFluentValidation(opt =>
					opt.UseErrorMappers(
						ValidationDefaults.ErrorMappers.Default,
						(builder, _) => builder.SetExtension("test", "test")))
				.AddMutationType(new TestMutation(arg => arg.UseFluentValidation()))
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
			var executor = await new ServiceCollection()
				.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>()
				.AddTestGraphQL()
				.AddFluentValidation(opt => opt
					.UseErrorMappers(ValidationDefaults.ErrorMappers.Default))
				.AddMutationType(new TestMutation(arg => arg.UseFluentValidation(
					configurator =>
					{
						configurator.UseErrorMappers(
							ValidationDefaults.ErrorMappers.Default,
							(builder, _) => builder.SetExtension("test", "test"));
					})))
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
				},
				test =>
				{
					Assert.Equal("test", test.Key);
					Assert.Equal("test", test.Value);
				});
		}
	}
}
