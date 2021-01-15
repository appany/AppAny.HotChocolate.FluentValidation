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
		/// Adds default validation services with <see cref="InputValidationConfigurator"/> overrides
		/// </summary>
		public static IRequestExecutorBuilder AddFluentValidation(
			this IRequestExecutorBuilder builder,
			Action<InputValidationConfigurator> configure)
		{
			builder.ConfigureSchemaServices(services =>
			{
				var configurator = new DefaultInputValidationConfigurator(services);

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
