namespace AppAny.HotChocolate.FluentValidation
{
	internal static class ArgumentValidationOptionsExtensions
	{
		public static void MergeValidationOptions(
			this ArgumentValidationOptions options,
			ValidationOptions validationOptions)
		{
			options.ErrorMapper ??= validationOptions.ErrorMapper;
			options.SkipValidation ??= validationOptions.SkipValidation;
			options.InputValidators ??= validationOptions.InputValidators;
		}
	}
}
