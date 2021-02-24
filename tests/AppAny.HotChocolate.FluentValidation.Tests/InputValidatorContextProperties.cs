using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class InputValidatorContextProperties
	{
		[Fact]
		public async Task AddFluentValidation()
		{
			var executor = await TestSetup.CreateRequestExecutor(
				builder =>
				{
					builder.AddFluentValidation(opt =>
						{
							opt.UseDefaultErrorMapper()
								.UseInputValidators(context =>
								{
									Assert.Equal(typeof(TestPersonInput), context.Argument.RuntimeType);

									return ValidationDefaults.InputValidators.Default(context);
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

			Assert.Equal(nameof(NotEmptyValidator), error.Code);
			Assert.Equal(NotEmptyNameValidator.Message, error.Message);

			Assert.Collection(error.Extensions,
				code =>
				{
					Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
					Assert.Equal(nameof(NotEmptyValidator), code.Value);
				});
		}

		[Fact]
		public async Task UseFluentValidation()
		{
			var executor = await TestSetup.CreateRequestExecutor(
				builder =>
				{
					builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input",
								arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
								{
									opt.UseInputValidators(context =>
									{
										Assert.Equal(typeof(TestPersonInput), context.Argument.RuntimeType);

										return ValidationDefaults.InputValidators.Default(context);
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

			Assert.Equal(nameof(NotEmptyValidator), error.Code);
			Assert.Equal(NotEmptyNameValidator.Message, error.Message);

			Assert.Collection(error.Extensions,
				code =>
				{
					Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
					Assert.Equal(nameof(NotEmptyValidator), code.Value);
				});
		}
	}
}
