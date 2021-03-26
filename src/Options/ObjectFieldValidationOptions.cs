using HotChocolate.Types;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace AppAny.HotChocolate.FluentValidation
{
  internal sealed class ObjectFieldValidationOptions
  {
    public IDictionary<string, IInputField> Arguments { get; } = new ConcurrentDictionary<string, IInputField>();
  }
}
