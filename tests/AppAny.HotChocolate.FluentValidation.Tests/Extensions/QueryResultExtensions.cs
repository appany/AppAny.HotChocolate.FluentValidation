using System;
using System.Collections.Generic;
using System.Linq;
using HotChocolate.Execution;
using Xunit;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
  public static class QueryResultExtensions
  {
    public static void AssertNullResult(this QueryResult result)
    {
      var (key, value) = Assert.Single(result.Data);

      Assert.Equal("test", key);
      Assert.Null(value);
    }

    public static void AssertSuceessResult(this QueryResult result)
    {
      var (key, value) = Assert.Single(result.Data);

      Assert.Equal("test", key);
      Assert.Equal("test", value);

      Assert.Null(result.Errors);
    }

    public static void AssertDefaultErrorMapper(
      this QueryResult result,
      string code,
      string message,
      params Action<KeyValuePair<string, object?>>[] elementInspectors)
    {
      Assert.Collection(result.Errors,
        error =>
        {
          Assert.Equal(code, error.Code);
          Assert.Equal(message, error.Message);

          var extensions = new Action<KeyValuePair<string, object?>>[]
          {
            codeExtension =>
            {
              Assert.Equal(ValidationDefaults.ExtensionKeys.CodeKey, codeExtension.Key);
              Assert.Equal(code, codeExtension.Value);
            }
          }.Concat(elementInspectors).ToArray();

          Assert.Collection(error.Extensions, extensions);
        });
    }
  }
}
