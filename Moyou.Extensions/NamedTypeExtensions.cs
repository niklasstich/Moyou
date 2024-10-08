using Metalama.Framework.Aspects;
using Metalama.Framework.Code;

namespace Moyou.Extensions;

[CompileTime]
public static class NamedTypeExtensions
{
    [CompileTime]
    public static bool HasAttribute<TAttribute>(this INamedType type) where TAttribute : Attribute =>
        type.Attributes.Any(attr => attr.Type.FullName == typeof(TAttribute).FullName);

    public static bool HasPublicDefaultConstructor(this INamedType type) =>
        type.HasDefaultConstructor && type.Constructors.GetDefaultConstructor().Accessibility == Accessibility.Public;
}