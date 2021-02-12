using FluentValidation.Results;

namespace AppAny.HotChocolate.FluentValidation
{
	internal static class ValidationResultExtensions
	{
		public static void MergeFailures(this global::FluentValidation.Results.ValidationResult validationResult, global::FluentValidation.Results.ValidationResult validatorResult)
		{
			for (var index = 0; index < validatorResult.Errors.Count; index++)
			{
				validationResult.Errors.Add(validatorResult.Errors[index]);
			}
		}
	}
}
