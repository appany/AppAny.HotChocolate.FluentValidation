using System;
using HotChocolate;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation
{
	public static class RequestExecutorBuilderExtensions
	{
		public static IRequestExecutorBuilder AddFluentValidation(this IRequestExecutorBuilder builder)
		{
			return builder.AddFluentValidation(_ =>
			{
			});
		}

		public static IRequestExecutorBuilder AddFluentValidation(
			this IRequestExecutorBuilder builder,
			Action<IValidationConfigurator> configure)
		{
			builder.ConfigureSchemaServices(services =>
			{
				var configurator = new ValidationConfigurator(services);

				configurator.UseErrorMappers(
					ValidationDefaults.ErrorMappers.Default,
					ValidationDefaults.ErrorMappers.Details);

				configurator.UseValidatorFactories(ValidationDefaults.ValidatorFactories.Default);

				configure.Invoke(configurator);
			});

			builder.ConfigureSchema(schemaBuilder =>
			{
				schemaBuilder.Use<ValidationMiddleware>();
			});

			return builder;
		}
	}
}
