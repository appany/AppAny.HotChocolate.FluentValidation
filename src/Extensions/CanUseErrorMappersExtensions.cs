using System.Linq;

namespace AppAny.HotChocolate.FluentValidation
{
	public static class CanUseErrorMappersExtensions
	{
		/// <summary>
		/// Uses default <see cref="ErrorMapper"/>. See <see cref="ValidationDefaults.ErrorMappers.Default"/>
		/// </summary>
		public static TConfigurator UseDefaultErrorMapper<TConfigurator>(
			this CanUseErrorMappers<TConfigurator> configurator,
			params ErrorMapper[] errorMappers)
		{
			return configurator.UseErrorMappers(
				new ErrorMapper[] { ValidationDefaults.ErrorMappers.Default }
					.Concat(errorMappers)
					.ToArray());
		}

		/// <summary>
		/// Adds default <see cref="ErrorMapper"/> with details. See <see cref="ValidationDefaults.ErrorMappers.Default"/> and <see cref="ValidationDefaults.ErrorMappers.Details"/>
		/// </summary>
		public static TConfigurator UseDefaultErrorMapperWithDetails<TConfigurator>(
			this CanUseErrorMappers<TConfigurator> configurator,
			params ErrorMapper[] errorMappers)
		{
			return configurator.UseErrorMappers(
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
		public static TConfigurator UseDefaultErrorMapperWithExtendedDetails<TConfigurator>(
			this CanUseErrorMappers<TConfigurator> configurator,
			params ErrorMapper[] errorMappers)
		{
			return configurator.UseErrorMappers(
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
