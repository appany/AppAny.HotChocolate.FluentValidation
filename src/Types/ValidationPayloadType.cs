using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation.Types
{
	public class ValidationPayloadType : ObjectType<ValidationPayload>
	{
		protected override void Configure(IObjectTypeDescriptor<ValidationPayload> descriptor)
		{
			descriptor.Field(x => x.Message);
			descriptor.Field(x => x.Validator);
			descriptor.Field(x => x.Severity);
		}
	}
}
