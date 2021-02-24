using System;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class OverrideAddFluentValidation
	{
		[Fact]
		public async Task NullResult_WithCodeExtension()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				{
					builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation());
						}));
				},
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertNullResult();

			result.AssertDefaultErrorMapper(
				nameof(NotEmptyValidator),
				NotEmptyNameValidator.Message);
		}

		[Fact]
		public async Task NullResult_ValidatorOverride()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				{
					builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input",
								arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
								{
									opt.UseValidator<NotEmptyNameValidator>();
								}));
						}));
				},
				services =>
				{
					services.AddTransient<NotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertNullResult();

			result.AssertDefaultErrorMapper(
				nameof(NotEmptyValidator),
				NotEmptyNameValidator.Message);
		}

		[Fact]
		public async Task ThrowsNoMessageSet()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				{
					builder.AddFluentValidation(opt => opt.UseErrorMapper(ValidationDefaults.ErrorMappers.Details))
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input",
								arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
								{
									opt.UseValidator<NotEmptyNameValidator>();
								}));
						}));
				},
				services =>
				{
					services.AddTransient<NotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				error => Assert.IsType<InvalidOperationException>(error.Exception));
		}

		[Fact]
		public async Task ThrowsNullInputValidator()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
			{
				builder.AddFluentValidation(opt => opt.UseInputValidators(default!))
					.AddMutationType(new TestMutation(field =>
					{
						field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation());
					}));
			});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				error => Assert.IsType<NullReferenceException>(error.Exception));
		}

		[Fact]
		public async Task CustomInputValidatorHasNoValidators()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
			{
				builder.AddFluentValidation(opt =>
					{
						opt.UseInputValidators(_ => Task.FromResult((ValidationResult?)null));
					})
					.AddMutationType(new TestMutation(field =>
					{
						field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation());
					}));
			});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			result.AssertSuceessResult();
		}
	}
}
