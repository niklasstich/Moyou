using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Code.Collections;

namespace Moyou.Extensions;

[CompileTime]
public static class FieldOrPropertyExtensions
{
    public static bool HasAttribute(this IFieldOrProperty fieldOrProperty, Type attributeType) =>
        fieldOrProperty.Attributes.Any(attribute => attribute.Type.FullName == attributeType.FullName);

    public static bool TypeHasAttribute(this IFieldOrProperty fieldOrProperty, Type attributeType) =>
        ((INamedType)fieldOrProperty.Type).Attributes.Any(attribute =>
            attribute.Type.FullName == attributeType.FullName);

    public static bool
        TypeArgumentOfEnumerableHasAttribute(this IFieldOrProperty fieldOrProperty, Type attributeType)
    {
        var memberType = (INamedType)fieldOrProperty.Type;
        var firstTypeArgument = (INamedType)memberType.TypeArguments[0];
        return firstTypeArgument.Attributes.Any(attribute => attribute.Type.FullName == attributeType.FullName);
    }
}