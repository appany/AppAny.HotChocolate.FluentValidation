using System;
using System.Collections.Generic;
using System.Linq;
using AppAny.HotChocolate.FluentValidation.Types;
using HotChocolate.Configuration;
using HotChocolate.Execution.Configuration;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Descriptors.Definitions;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation
{
	public static class RequestExecutorBuilderExtensions
	{
		/// <summary>
		/// Adds default validation services with <see cref="ValidationBuilder"/> overrides
		/// </summary>
		public static IRequestExecutorBuilder AddFluentValidation(
			this IRequestExecutorBuilder builder,
			Action<ValidationBuilder>? configure = null)
		{
			var validationOptions = new ValidationOptions();

			configure?.Invoke(new DefaultValidationBuilder(validationOptions));

			builder.SetContextData(ValidationDefaults.ValidationOptionsKey, validationOptions);

			builder.OnBeforeCompleteType(ValidationDefaults.Interceptors.OnBeforeCompleteType);

			builder.TryAddSchemaInterceptor(
				new DelegateSchemaInterceptor(
					onAfterCreate: ValidationDefaults.Interceptors.OnAfterCreate));

			builder.OnBeforeRegisterDependencies((context, definition, _) =>
			{
				if (definition is not ObjectTypeDefinition objectTypeDefinition)
				{
					return;
				}

				foreach (var objectFieldDefinition in objectTypeDefinition.Fields)
				{
					var arguments = objectFieldDefinition.Arguments
						.Where(argument => argument.ContextData.ShouldValidateArgument())
						.ToList();

					if (arguments is { Count: > 0 })
					{
						var innerType = objectFieldDefinition.Type switch
						{
							ExtendedTypeReference extendedTypeReference => extendedTypeReference.Type.Type,
							SchemaTypeReference schemaTypeReference => schemaTypeReference.Type.GetType(),
							_ => throw new InvalidOperationException()
						};

						var type = typeof(ValidationResultType<>).MakeGenericType(innerType);

						var fieldName = objectFieldDefinition.Name.Value;
						var field = char.ToUpper(fieldName[0]) + fieldName[1..];

						var __arguments = new Dictionary<string, string[]>();

						foreach (var argument in arguments)
						{
							// TODO: filter
							var names = context.TypeInspector
								.GetMembers(context.TypeInspector.GetArgumentType(argument.Parameter!).Type)
								.Select(x => char.ToLower(x.Name[0]) + x.Name[1..])
								.ToArray();

							__arguments.Add(argument.Name, names);
						}

						var typeInstance = type.GetConstructors()
							.Single()
							.Invoke(new object[] { field, __arguments });

						objectFieldDefinition.Type = new SchemaTypeReference((IOutputType)typeInstance);
					}
				}
			});

			return builder;
		}
	}
}
