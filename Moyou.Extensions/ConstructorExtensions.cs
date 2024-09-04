using Metalama.Framework.Aspects;
using Metalama.Framework.Code;

namespace Moyou.Extensions;

[CompileTime]
public static class ConstructorExtensions
{
    public static bool HasAttribute<TAttribute>(this IConstructor constructor) where TAttribute : Attribute =>
        constructor.Attributes.Any(attribute => attribute.Type.FullName == typeof(TAttribute).FullName);
}