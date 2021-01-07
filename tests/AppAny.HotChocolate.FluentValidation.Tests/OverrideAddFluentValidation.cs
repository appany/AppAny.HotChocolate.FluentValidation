using System;
using System.Threading.Tasks;
using FluentValidation;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class OverrideAddFluentValidation
	{
		[Fact]
		public async Task Should_HaveNullResult_ValidationError_WithoutExtensionCodes()
		{
			var executor = await new ServiceCollection()
				.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>()
				.AddTestGraphQL()
				.AddFluentValidation(configurator => configurator
					.UseErrorMappers(ValidationDefaults.ErrorMappers.Default))
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
				});
		}

		[Fact]
		public async Task Should_HaveNullResult_WithValidatorOverride()
		{
			var executor = await new ServiceCollection()
				.AddTransient<NotEmptyNameValidator>()
				.AddTestGraphQL()
				.AddFluentValidation(configurator => configurator
					.UseErrorMappers(ValidationDefaults.ErrorMappers.Default))
				.AddMutationType(new TestMutation(arg => arg.UseFluentValidation(configurator =>
				{
					configurator.UseValidator<NotEmptyNameValidator>();
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
				});
		}

		[Fact]
		public async Task Should_Throw_NoMessageSet()
		{
			var executor = await new ServiceCollection()
				.AddTransient<NotEmptyNameValidator>()
				.AddTestGraphQL()
				.AddFluentValidation(configurator => configurator
					.UseErrorMappers(ValidationDefaults.ErrorMappers.Details))
				.AddMutationType(new TestMutation(arg => arg.UseFluentValidation(configurator =>
				{
					configurator.UseValidator<NotEmptyNameValidator>();
				})))
				.BuildRequestExecutorAsync();

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestMutations.EmptyName));

			result.AssertNullResult();

			var error = Assert.Single(result.Errors);

			Assert.IsType<InvalidOperationException>(error.Exception);
		}

		[Fact]
		public async Task Should_NullReference_WithoutValidation()
		{
			var executor = await new ServiceCollection()
				.AddTestGraphQL()
				.AddFluentValidation(configurator => configurator.UseInputValidatorFactories(_ => default!))
				.AddMutationType(new TestMutation(arg => arg.UseFluentValidation()))
				.BuildRequestExecutorAsync();

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestMutations.EmptyName));

			result.AssertNullResult();

			var error = Assert.Single(result.Errors);

			Assert.IsType<NullReferenceException>(error.Exception);
		}

		[Fact]
		public async Task Should_Execute_WithoutValidation()
		{
			var executor = await new ServiceCollection()
				.AddTestGraphQL()
				.AddFluentValidation(configurator => configurator.UseInputValidatorFactories(_ => Array.Empty<InputValidator>()))
				.AddMutationType(new TestMutation(arg => arg.UseFluentValidation()))
				.BuildRequestExecutorAsync();

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestMutations.EmptyName));

			var (key, value) = Assert.Single(result.Data);

			Assert.Equal("test", key);
			Assert.Equal("test", value);

			Assert.Null(result.Errors);
		}
	}
}
