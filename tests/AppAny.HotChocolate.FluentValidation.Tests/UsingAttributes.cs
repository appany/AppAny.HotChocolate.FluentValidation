using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
	public class UsingAttributes
	{
		[Fact]
		public async Task Should_Use_AttributeConfiguration()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				{
					builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
						.AddMutationType(new TestUseFluentValidationMutation());
				},
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

		[Fact]
		public async Task Should_Use_DefaultInputValidator()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				{
					builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
						.AddMutationType(new TestUseDefaultInputValidatorMutation());
				},
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

		[Fact]
		public async Task Should_Use_DefaultErrorMapper()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				{
					builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
						.AddMutationType(new TestUseDefaultErrorMapperMutation());
				},
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

		[Fact]
		public async Task Should_Use_DefaultErrorMapperWithDetails()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				{
					builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
						.AddMutationType(new TestUseDefaultErrorMapperWithDetailsMutation());
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
				},
				validator =>
				{
					Assert.Equal(ValidationDefaults.ExtensionKeys.ValidatorKey, validator.Key);
					Assert.Equal("NotEmptyValidator", validator.Value);
				},
				argument =>
				{
					Assert.Equal(ValidationDefaults.ExtensionKeys.ArgumentKey, argument.Key);
					Assert.Equal(new NameString("input"), argument.Value);
				},
				property =>
				{
					Assert.Equal(ValidationDefaults.ExtensionKeys.PropertyKey, property.Key);
					Assert.Equal("Name", property.Value);
				},
				severity =>
				{
					Assert.Equal(ValidationDefaults.ExtensionKeys.SeverityKey, severity.Key);
					Assert.Equal(Severity.Error, severity.Value);
				});
		}

		[Fact]
		public async Task Should_Use_DefaultErrorMapperWithExtendedDetails()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				{
					builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
						.AddMutationType(new TestUseDefaultErrorMapperWithExtendedDetailsMutation());
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
				},
				validator =>
				{
					Assert.Equal(ValidationDefaults.ExtensionKeys.ValidatorKey, validator.Key);
					Assert.Equal("NotEmptyValidator", validator.Value);
				},
				argument =>
				{
					Assert.Equal(ValidationDefaults.ExtensionKeys.ArgumentKey, argument.Key);
					Assert.Equal(new NameString("input"), argument.Value);
				},
				property =>
				{
					Assert.Equal(ValidationDefaults.ExtensionKeys.PropertyKey, property.Key);
					Assert.Equal("Name", property.Value);
				},
				severity =>
				{
					Assert.Equal(ValidationDefaults.ExtensionKeys.SeverityKey, severity.Key);
					Assert.Equal(Severity.Error, severity.Value);
				},
				attemptedValue =>
				{
					Assert.Equal(ValidationDefaults.ExtensionKeys.AttemptedValueKey, attemptedValue.Key);
					Assert.Equal("", attemptedValue.Value);
				},
				customState =>
				{
					Assert.Equal(ValidationDefaults.ExtensionKeys.CustomStateKey, customState.Key);
					Assert.Null(customState.Value);
				},
				formattedMessagePlaceholerValues =>
				{
					Assert.Equal(
						ValidationDefaults.ExtensionKeys.FormattedMessagePlaceholderValuesKey,
						formattedMessagePlaceholerValues.Key);

					var values = Assert.IsType<Dictionary<string, object>>(formattedMessagePlaceholerValues.Value);

					Assert.Collection(values,
						propertyName =>
						{
							Assert.Equal("PropertyName", Assert.IsType<string>(propertyName.Key));
							Assert.Equal("Name", Assert.IsType<string>(propertyName.Value));
						},
						propertyValue =>
						{
							Assert.Equal("PropertyValue", Assert.IsType<string>(propertyValue.Key));
							Assert.Equal("", Assert.IsType<string>(propertyValue.Value));
						});
				});
		}

		[Fact]
		public async Task Should_Use_AttributeConfiguration_CustomValidator()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				{
					builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
						.AddMutationType(new TestUseValidatorMutation());
				},
				services =>
				{
					services.AddTransient<NotEmptyNameValidator>();
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

		[Fact]
		public async Task Should_SkipValidation_AttributeConfiguration()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				{
					builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
						.AddMutationType(new TestSkipValidationMutation());
				},
				services =>
				{
					services.AddTransient<NotEmptyNameValidator>();
				});

			var result = Assert.IsType<QueryResult>(
				await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

			var (key, value) = Assert.Single(result.Data);

			Assert.Equal("test", key);
			Assert.Equal("test", value);

			Assert.Null(result.Errors);
		}

		[Fact]
		public async Task Should_Use_AttributeConfiguration_CustomValidators()
		{
			var executor = await TestSetup.CreateRequestExecutor(builder =>
				{
					builder.AddFluentValidation(opt => opt.UseDefaultErrorMapper())
						.AddMutationType(new TestUseValidatorsMutation());
				},
				services =>
				{
					services.AddTransient<NotEmptyNameValidator>().AddTransient<NotEmptyNameValidator>();
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
				},
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
