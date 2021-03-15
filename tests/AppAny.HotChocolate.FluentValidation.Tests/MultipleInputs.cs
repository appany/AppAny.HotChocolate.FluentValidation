using System.Threading.Tasks;
using FluentValidation;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class MultipleInputs
	{
		[Fact]
		public async Task SingleInput()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				{
					builder.AddFluentValidation()
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation())
								.Argument("input2", arg => arg.Type<TestPersonInputType>());
						}));
				},
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithMultipleInputsEmptyName));

			result.AssertNullResult();

			result.AssertDefaultErrorMapper(
				"NotEmptyValidator",
				NotEmptyNameValidator.Message);
		}

		[Fact]
		public async Task DoubleInput()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				{
					builder.AddFluentValidation()
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation())
								.Argument("input2", arg => arg.Type<TestPersonInputType>().UseFluentValidation());
						}));
				},
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithMultipleInputsEmptyName));

			result.AssertNullResult();

			Assert.Collection(result.Errors,
				input =>
				{
					Assert.Equal("NotEmptyValidator", input.Code);
					Assert.Equal(NotEmptyNameValidator.Message, input.Message);

					Assert.Collection(input.Extensions,
						code =>
						{
							Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
							Assert.Equal("NotEmptyValidator", code.Value);
						});
				},
				input2 =>
				{
					Assert.Equal("NotEmptyValidator", input2.Code);
					Assert.Equal(NotEmptyNameValidator.Message, input2.Message);

					Assert.Collection(input2.Extensions,
						code =>
						{
							Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, code.Key);
							Assert.Equal("NotEmptyValidator", code.Value);
						});
				});
		}

		[Fact]
		public async Task SecondInput()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				{
					builder.AddFluentValidation()
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>())
								.Argument("input2", arg => arg.Type<TestPersonInputType>().UseFluentValidation());
						}));
				},
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithMultipleInputsEmptyName));

			result.AssertNullResult();

			result.AssertDefaultErrorMapper(
				"NotEmptyValidator",
				NotEmptyNameValidator.Message);
		}

		[Fact]
		public async Task NoInputs()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				{
					builder.AddFluentValidation()
						.AddMutationType(new TestMutation(field =>
						{
							field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>())
								.Argument("input2", arg => arg.Type<TestPersonInputType>());
						}));
				},
				services =>
				{
					services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithMultipleInputsEmptyName));

			result.AssertSuceessResult();
		}
	}
}
