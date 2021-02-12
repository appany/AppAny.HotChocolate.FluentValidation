using System.Linq;

namespace AppAny.HotChocolate.FluentValidation
{
	/// <summary>
	/// Configures input field validation options
	/// </summary>
	public interface ArgumentValidationBuilder
		: CanSkipValidation<ArgumentValidationBuilder>,
			CanUseInputValidators<ArgumentValidationBuilder>
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

		public ArgumentValidationBuilder UseInputValidators(params InputValidator[] inputValidators)
		{
			if (options.InputValidators is null)
			{
				options.InputValidators = inputValidators.ToList();
			}
			else
			{
				foreach (var inputValidator in inputValidators)
				{
					options.InputValidators.Add(inputValidator);
				}
			}

			return this;
		}
	}
}
