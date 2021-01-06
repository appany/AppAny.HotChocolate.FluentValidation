using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;

namespace AppAny.HotChocolate.FluentValidation
{
	public interface IInputValidator
	{
		Task<ValidationResult> ValidateAsync(object argument, CancellationToken cancellationToken);

		public static IInputValidator FromValidator(IValidator validator)
		{
			return FromValidatorWithStrategy<object>(validator, ValidationDefaults.ValidationStrategies.Default);
		}

		public static IInputValidator FromValidatorWithStrategy<TInput>(
			IValidator validator,
			Action<ValidationStrategy<TInput>> validationStrategy)
		{
			return new InputValidator<TInput>(validator, validationStrategy);
		}
	}

	internal sealed class InputValidator<TInput> : IInputValidator
	{
		private readonly IValidator validator;
		private readonly Action<ValidationStrategy<TInput>> validationStrategy;

		public InputValidator(IValidator validator, Action<ValidationStrategy<TInput>> validationStrategy)
		{
			this.validator = validator;
			this.validationStrategy = validationStrategy;
		}

		public Task<ValidationResult> ValidateAsync(object argument, CancellationToken cancellationToken)
		{
			var validationContext = ValidationContext<TInput>.CreateWithOptions(
				(TInput)argument,
				validationStrategy);

			return validator.ValidateAsync(validationContext, cancellationToken);
		}
	}
}
