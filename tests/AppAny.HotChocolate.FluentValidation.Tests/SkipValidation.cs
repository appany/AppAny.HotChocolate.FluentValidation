using System.Threading.Tasks;
using FluentValidation;
using HotChocolate.Execution;
using HotChocolate.Types;
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
					.AddMutationType(new TestMutation(field =>
					{
						field.Argument("input",
							arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
							{
								opt.SkipValidation();
							}));
					})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
				});

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
					.AddMutationType(new TestMutation(field =>
					{
						field.Argument("input",
							arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
							{
								opt.SkipValidation(context => new ValueTask<bool>(context.Argument.Name == "input"));
							}));
					})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
				});

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
					.AddMutationType(new TestMutation(field =>
					{
						field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation());
					})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
				});

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
				builder.AddFluentValidation(opt =>
					{
						opt.SkipValidation(context => new ValueTask<bool>(context.Argument.Name == "input"));
					})
					.AddMutationType(new TestMutation(field =>
					{
						field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation());
					})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
				});

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
					.AddMutationType(new TestMutation(field =>
					{
						field.Argument("input",
							arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
							{
								opt.SkipValidation(ValidationDefaults.SkipValidation.Skip);
							}));
					})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
				});

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
					.AddMutationType(new TestMutation(field =>
					{
						field.Argument("input",
							arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
							{
								opt.SkipValidation(ValidationDefaults.SkipValidation.Default);
							}));
					})),
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
				});

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
