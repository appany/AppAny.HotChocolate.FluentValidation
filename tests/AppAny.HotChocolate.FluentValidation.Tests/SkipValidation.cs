using System.Threading.Tasks;
using FluentValidation;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class SkipValidation
	{
		[Fact]
		public async Task Should_Execute_FieldSkipValidation()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation()
					.AddMutationType(new TestMutation(arg => arg.UseFluentValidation(configurator =>
					{
						configurator.SkipValidation();
					})))
					.Services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>());

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			var (key, value) = Assert.Single(result.Data);

			Assert.Equal("test", key);
			Assert.Equal("test", value);

			Assert.Null(result.Errors);
		}

		[Fact]
		public async Task Should_Execute_FieldSkipValidation_Predecate()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation()
					.AddMutationType(new TestMutation(arg => arg.UseFluentValidation(configurator =>
					{
						configurator.SkipValidation(context => new ValueTask<bool>(context.Argument.Name == "input"));
					})))
					.Services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>());

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			var (key, value) = Assert.Single(result.Data);

			Assert.Equal("test", key);
			Assert.Equal("test", value);

			Assert.Null(result.Errors);
		}

		[Fact]
		public async Task Should_Execute_GlobalSkipValidation()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation(opt => opt.SkipValidation())
					.AddMutationType(new TestMutation(arg => arg.UseFluentValidation()))
					.Services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>());

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			var (key, value) = Assert.Single(result.Data);

			Assert.Equal("test", key);
			Assert.Equal("test", value);

			Assert.Null(result.Errors);
		}

		[Fact]
		public async Task Should_Execute_GlobalSkipValidation_Predecate()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation(options =>
						options.SkipValidation(context => new ValueTask<bool>(context.Argument.Name == "input")))
					.AddMutationType(new TestMutation(arg => arg.UseFluentValidation()))
					.Services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>());

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			var (key, value) = Assert.Single(result.Data);

			Assert.Equal("test", key);
			Assert.Equal("test", value);

			Assert.Null(result.Errors);
		}

		[Fact]
		public async Task Should_Execute_GlobalSkipValidationDefault_OverrideSkip()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation(opt => opt.SkipValidation(ValidationDefaults.SkipValidation.Default))
					.AddMutationType(new TestMutation(arg => arg.UseFluentValidation(options =>
					{
						options.SkipValidation(ValidationDefaults.SkipValidation.Skip);
					})))
					.Services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>());

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			var (key, value) = Assert.Single(result.Data);

			Assert.Equal("test", key);
			Assert.Equal("test", value);

			Assert.Null(result.Errors);
		}

		[Fact]
		public async Task Should_Fail_GlobalSkipValidation_OverrideToDefault()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				builder.AddFluentValidation(opt => opt.SkipValidation().UseDefaultErrorMapper())
					.AddMutationType(new TestMutation(arg => arg.UseFluentValidation(options =>
					{
						options.SkipValidation(ValidationDefaults.SkipValidation.Default);
					})))
					.Services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>());

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

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
				});
		}
	}
}
