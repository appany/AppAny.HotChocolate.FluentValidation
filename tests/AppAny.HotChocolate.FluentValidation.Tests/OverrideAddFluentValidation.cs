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
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation(opt => opt.UseErrorMappers(ValidationDefaults.ErrorMappers.Default))
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
				});
		}

		[Fact]
		public async Task Should_HaveNullResult_WithValidatorOverride()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation(opt => opt.UseErrorMappers(ValidationDefaults.ErrorMappers.Default))
					.AddMutationType(new TestMutation(arg => arg.UseFluentValidation(opt =>
					{
						opt.UseValidator<NotEmptyNameValidator>();
					})))
					.Services.AddTransient<NotEmptyNameValidator>());

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
		public async Task Should_Throw_NoMessageSet()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation(opt => opt.UseErrorMappers(ValidationDefaults.ErrorMappers.Details))
					.AddMutationType(new TestMutation(arg => arg.UseFluentValidation(opt =>
					{
						opt.UseValidator<NotEmptyNameValidator>();
					})))
					.Services.AddTransient<NotEmptyNameValidator>());

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertNullResult();

			var error = Assert.Single(result.Errors);

			Assert.IsType<InvalidOperationException>(error.Exception);
		}

		[Fact]
		public async Task Should_NullReference_WithoutValidation()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation(opt => opt.UseInputValidatorProviders(_ => default!))
					.AddMutationType(new TestMutation(arg => arg.UseFluentValidation())));

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertNullResult();

			var error = Assert.Single(result.Errors);

			Assert.IsType<NullReferenceException>(error.Exception);
		}

		[Fact]
		public async Task Should_Execute_WithoutValidation()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation(opt => opt.UseInputValidatorProviders(_ =>
						ValidationDefaults.InputValidators.FromValidators(Array.Empty<IValidator>())))
					.AddMutationType(new TestMutation(arg => arg.UseFluentValidation())));

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			var (key, value) = Assert.Single(result.Data);

			Assert.Equal("test", key);
			Assert.Equal("test", value);

			Assert.Null(result.Errors);
		}
	}
}
