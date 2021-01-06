using HotChocolate;

namespace AppAny.HotChocolate.FluentValidation
{
	/// <summary>
	/// Maps validation result to <see cref="IError"/> properties
	/// </summary>
	public delegate void ErrorMapper(ErrorBuilder errorBuilder, ErrorMappingContext mappingContext);
}
