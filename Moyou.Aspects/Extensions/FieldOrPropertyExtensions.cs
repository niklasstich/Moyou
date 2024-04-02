using Metalama.Framework.Aspects;
using Metalama.Framework.Code;

namespace Moyou.Aspects.Extensions;

[CompileTime]
internal static class FieldOrPropertyExtensions
{
    public static bool HasAttribute(this IFieldOrProperty fieldOrProperty, Type attributeType) =>
        fieldOrProperty.Attributes.Any(attribute => attribute.Type.FullName == attributeType.FullName);
}
