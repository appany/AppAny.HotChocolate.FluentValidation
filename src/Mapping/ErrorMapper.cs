using HotChocolate;

namespace AppAny.HotChocolate.FluentValidation
{
	public delegate void ErrorMapper(ErrorBuilder errorBuilder, ErrorMappingContext mappingContext);
}
