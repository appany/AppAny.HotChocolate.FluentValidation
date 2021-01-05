using System;
using System.Threading.Tasks;
using FluentValidation;
using HotChocolate.Execution;
using HotChocolate.Types;
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
				.AddTransient<IValidator<TestInput>, NotEmptyNameValidator>()
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
		public async Task Should_HaveNullResult_WithValidatorOverride()
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
						.Argument("input", arg => arg.Type<NonNullType<TestInputType>>().UseFluentValidation(configurator =>
						{
							configurator.UseValidator<NotEmptyNameValidator>();
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
		public async Task Should_Throw_NoMessageSet()
		{
			var executor = await new ServiceCollection()
				.AddTransient<NotEmptyNameValidator>()
				.AddTestGraphQL()
				.AddFluentValidation(configurator => configurator
					.UseErrorMappers(ValidationDefaults.ErrorMappers.Extensions))
				.AddMutationType(descriptor =>
				{
					descriptor.Name("Mutation");

					descriptor.Field("test")
						.Type<StringType>()
						.Argument("input", arg => arg.Type<NonNullType<TestInputType>>().UseFluentValidation(configurator =>
						{
							configurator.UseValidator<NotEmptyNameValidator>();
						}))
						.Resolve("test");
				})
				.BuildRequestExecutorAsync();

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync("mutation { test(input: { name: \"\" }) }"));

			result.AssertNullResult();

			var error = Assert.Single(result.Errors);

			Assert.IsType<InvalidOperationException>(error.Exception);
		}

		[Fact]
		public async Task Should_NullReference_WithoutValidation()
		{
			var executor = await new ServiceCollection()
				.AddTestGraphQL()
				.AddFluentValidation(configurator => configurator.UseValidatorFactories(_ => default!))
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

			Assert.IsType<NullReferenceException>(error.Exception);
		}

		[Fact]
		public async Task Should_Execute_WithoutValidation()
		{
			var executor = await new ServiceCollection()
				.AddTestGraphQL()
				.AddFluentValidation(configurator => configurator.UseValidatorFactories(_ => Array.Empty<IValidator>()))
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

			var (key, value) = Assert.Single(result.Data);

			Assert.Equal("test", key);
			Assert.Equal("test", value);

			Assert.Null(result.Errors);
		}
	}
}
