using System.Threading.Tasks;
using FluentValidation;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class ErrorMappingContextProperties
	{
		[Fact]
		public async Task AddFluentValidation()
		{
			var executor = await TestSetup.CreateRequestExecutor(
				builder =>
				{
					builder.AddFluentValidation(opt =>
						{
							opt.UseDefaultErrorMapper(
								(_, context) =>
								{
									Assert.Equal("input", context.Argument.Name);
									Assert.Single(context.ValidationResult.Errors);
									Assert.Equal(nameof(TestPersonInput.Name), context.ValidationFailure.PropertyName);
								});
						})
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
		public async Task UseFluentValidation()
		{
			var executor = await TestSetup.CreateRequestExecutor(
				builder =>
				{
					builder.AddFluentValidation()
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
							{
								opt.UseDefaultErrorMapper(
									(_, context) =>
									{
										Assert.Equal("input", context.Argument.Name);
										Assert.Single(context.ValidationResult.Errors);
										Assert.Equal(nameof(TestPersonInput.Name), context.ValidationFailure.PropertyName);
									});
							}));
						}));
				},
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
				});

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
	}
}
