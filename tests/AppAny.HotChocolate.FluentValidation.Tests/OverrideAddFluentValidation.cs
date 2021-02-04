using System;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
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
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
					.AddMutationType(new TestMutation(field =>
						field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation())))
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
				builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
					.AddMutationType(new TestMutation(field => field.Argument("input",
						arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
						{
							opt.UseValidator<NotEmptyNameValidator>();
						}))))
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
		public async Task Should_Throw_NoMessageSet_ErrorMapper_WithoutDefault()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation(opt => opt.UseErrorMapper(ValidationDefaults.ErrorMappers.Details))
					.AddMutationType(new TestMutation(field => field.Argument("input",
						arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
						{
							opt.UseValidator<NotEmptyNameValidator>();
						}))))
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
				builder.AddFluentValidation(opt => opt.UseInputValidators(default!))
					.AddMutationType(new TestMutation(field =>
						field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation()))));

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
				builder.AddFluentValidation(opt =>
						opt.UseInputValidators(_ => Task.FromResult((ValidationResult?)null)))
					.AddMutationType(new TestMutation(field =>
						field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation()))));

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			var (key, value) = Assert.Single(result.Data);

			Assert.Equal("test", key);
			Assert.Equal("test", value);

			Assert.Null(result.Errors);
		}
	}
}
