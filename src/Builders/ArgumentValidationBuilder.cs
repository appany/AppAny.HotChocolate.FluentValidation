using System.Linq;

namespace AppAny.HotChocolate.FluentValidation
{
	/// <summary>
	/// Configures input field validation options
	/// </summary>
	public interface ArgumentValidationBuilder
		: CanSkipValidation<ArgumentValidationBuilder>,
			CanUseInputValidatorProviders<ArgumentValidationBuilder>,
			CanUseErrorMapper<ArgumentValidationBuilder>
	{
	}

	internal sealed class DefaultArgumentValidationBuilder : ArgumentValidationBuilder
	{
		private readonly ArgumentValidationOptions options;

		public DefaultArgumentValidationBuilder(ArgumentValidationOptions options)
		{
			this.options = options;
		}

		public ArgumentValidationBuilder SkipValidation(SkipValidation skipValidation)
		{
			options.SkipValidation = skipValidation;

			return this;
		}

		public ArgumentValidationBuilder UseInputValidatorProviders(params InputValidatorProvider[] inputValidatorProviders)
		{
			if (options.InputValidatorProviders is null)
			{
				options.InputValidatorProviders = inputValidatorProviders.ToList();
			}
			else
			{
				foreach (var inputValidatorProvider in inputValidatorProviders)
				{
					options.InputValidatorProviders.Add(inputValidatorProvider);
				}
			}

			return this;
		}

		public ArgumentValidationBuilder UseErrorMapper(ErrorMapper errorMapper)
		{
			if (options.ErrorMapper is null)
			{
				options.ErrorMapper = errorMapper;
			}
			else
			{
				var previousErrorMapper = options.ErrorMapper;

				options.ErrorMapper = (builder, context) =>
				{
					previousErrorMapper(builder, context);
					errorMapper(builder, context);
				};
			}

			return this;
		}
	}
}
