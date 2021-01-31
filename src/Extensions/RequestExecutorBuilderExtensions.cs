using System;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation
{
	public static class RequestExecutorBuilderExtensions
	{
		/// <summary>
		/// Adds default validation services
		/// </summary>
		public static IRequestExecutorBuilder AddFluentValidation(this IRequestExecutorBuilder builder)
		{
			return builder.AddFluentValidation(_ =>
			{
			});
		}

		/// <summary>
		/// Adds default validation services with <see cref="ValidationBuilder"/> overrides
		/// </summary>
		public static IRequestExecutorBuilder AddFluentValidation(
			this IRequestExecutorBuilder builder,
			Action<ValidationBuilder> configure)
		{
			var validationOptions = new ValidationOptions();
			configure.Invoke(new DefaultValidationBuilder(validationOptions));
			builder.SetContextData(ValidationDefaults.ValidationOptionsKey, validationOptions);

			builder.TryAddTypeInterceptor<ValidationTypeInterceptor>();
			builder.TryAddSchemaInterceptor<ValidationSchemaInterceptor>();

			return builder;
		}
	}
}
