using System;
using HotChocolate;
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
		/// Adds default validation services with <see cref="IInputValidationConfigurator"/> overrides
		/// </summary>
		public static IRequestExecutorBuilder AddFluentValidation(
			this IRequestExecutorBuilder builder,
			Action<IInputValidationConfigurator> configure)
		{
			builder.ConfigureSchemaServices(services =>
			{
				var configurator = new InputValidationConfigurator(services);

				configurator.AddDefaultErrorMapperWithDetails();
				configurator.AddDefaultValidatorFactory();
				configurator.SkipValidation(ValidationDefaults.SkipValidation.Default);

				configure.Invoke(configurator);
			});

			builder.ConfigureSchema(schemaBuilder =>
			{
				schemaBuilder.Use<InputValidationMiddleware>();
			});

			return builder;
		}
	}
}
