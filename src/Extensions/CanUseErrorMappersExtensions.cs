using System.Linq;

namespace AppAny.HotChocolate.FluentValidation
{
	public static class CanUseErrorMappersExtensions
	{
		/// <summary>
		/// Uses default <see cref="ErrorMapper"/>. See <see cref="ValidationDefaults.ErrorMappers.Default"/>
		/// </summary>
		public static TBuilder UseDefaultErrorMapper<TBuilder>(
			this CanUseErrorMappers<TBuilder> builder,
			params ErrorMapper[] errorMappers)
		{
			return builder.UseErrorMappers(
				new ErrorMapper[] { ValidationDefaults.ErrorMappers.Default }
					.Concat(errorMappers)
					.ToArray());
		}

		/// <summary>
		/// Adds default <see cref="ErrorMapper"/> with details. See <see cref="ValidationDefaults.ErrorMappers.Default"/> and <see cref="ValidationDefaults.ErrorMappers.Details"/>
		/// </summary>
		public static TBuilder UseDefaultErrorMapperWithDetails<TBuilder>(
			this CanUseErrorMappers<TBuilder> builder,
			params ErrorMapper[] errorMappers)
		{
			return builder.UseErrorMappers(
				new ErrorMapper[]
					{
						ValidationDefaults.ErrorMappers.Default,
						ValidationDefaults.ErrorMappers.Details
					}
					.Concat(errorMappers)
					.ToArray());
		}

		/// <summary>
		/// Adds default <see cref="ErrorMapper"/> with details. See <see cref="ValidationDefaults.ErrorMappers.Default"/> and <see cref="ValidationDefaults.ErrorMappers.Details"/>
		/// </summary>
		public static TBuilder UseDefaultErrorMapperWithExtendedDetails<TBuilder>(
			this CanUseErrorMappers<TBuilder> builder,
			params ErrorMapper[] errorMappers)
		{
			return builder.UseErrorMappers(
				new ErrorMapper[]
					{
						ValidationDefaults.ErrorMappers.Default,
						ValidationDefaults.ErrorMappers.Details,
						ValidationDefaults.ErrorMappers.Extended
					}
					.Concat(errorMappers)
					.ToArray());
		}
	}
}
