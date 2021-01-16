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
			builder.ConfigureSchemaServices(services =>
			{
				var configurator = new DefaultValidationBuilder(services);

				configurator.UseDefaultErrorMapper();
				configurator.UseDefaultInputValidatorProvider();
				configurator.SkipValidation(ValidationDefaults.SkipValidation.Default);

				configure.Invoke(configurator);
			});

			builder.ConfigureSchema(schemaBuilder =>
			{
				schemaBuilder.Use(ValidationDefaults.Middleware);
			});

			return builder;
		}
	}
}
