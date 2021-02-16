using System.Runtime.CompilerServices;
using FluentValidation.Results;

namespace AppAny.HotChocolate.FluentValidation
{
	internal static class ValidationResultExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MergeFailures(this ValidationResult validationResult, ValidationResult validatorResult)
		{
			for (var index = 0; index < validatorResult.Errors.Count; index++)
			{
				validationResult.Errors.Add(validatorResult.Errors[index]);
			}
		}
	}
}
