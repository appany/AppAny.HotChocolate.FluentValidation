namespace AppAny.HotChocolate.FluentValidation
{
	public static class CanUseErrorMappersExtensions
	{
		/// <summary>
		/// Uses default <see cref="ErrorMapper"/>. See <see cref="ValidationDefaults.ErrorMappers.Default"/>
		/// </summary>
		public static TBuilder UseDefaultErrorMapper<TBuilder>(
			this CanUseErrorMapper<TBuilder> builder,
			ErrorMapper? errorMapper = null)
		{
			return builder.UseErrorMapper((errorBuilder, context) =>
			{
				ValidationDefaults.ErrorMappers.Default(errorBuilder, context);
				errorMapper?.Invoke(errorBuilder, context);
			});
		}

		/// <summary>
		/// Adds default <see cref="ErrorMapper"/> with details. See <see cref="ValidationDefaults.ErrorMappers.Default"/> and <see cref="ValidationDefaults.ErrorMappers.Details"/>
		/// </summary>
		public static TBuilder UseDefaultErrorMapperWithDetails<TBuilder>(
			this CanUseErrorMapper<TBuilder> builder,
			ErrorMapper? errorMapper = null)
		{
			return builder.UseDefaultErrorMapper((errorBuilder, context) =>
			{
				ValidationDefaults.ErrorMappers.Details(errorBuilder, context);
				errorMapper?.Invoke(errorBuilder, context);
			});
		}

		/// <summary>
		/// Adds default <see cref="ErrorMapper"/> with details. See <see cref="ValidationDefaults.ErrorMappers.Default"/> and <see cref="ValidationDefaults.ErrorMappers.Details"/>
		/// </summary>
		public static TBuilder UseDefaultErrorMapperWithExtendedDetails<TBuilder>(
			this CanUseErrorMapper<TBuilder> builder,
			ErrorMapper? errorMapper = null)
		{
			return builder.UseDefaultErrorMapperWithDetails((errorBuilder, context) =>
			{
				ValidationDefaults.ErrorMappers.Extended(errorBuilder, context);
				errorMapper?.Invoke(errorBuilder, context);
			});
		}
	}
}
