using System.Collections.Concurrent;
using System.Collections.Generic;
using HotChocolate.Types;

namespace AppAny.HotChocolate.FluentValidation
{
	internal sealed class ObjectFieldValidationOptions
	{
		public IDictionary<string, IInputField> Arguments { get; } = new ConcurrentDictionary<string, IInputField>();
	}
}
