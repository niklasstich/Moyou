using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Code.Collections;

namespace Moyou.Extensions;

[CompileTime]
public static class FieldOrPropertyExtensions
{
    public static bool HasAttribute(this IFieldOrProperty fieldOrProperty, Type attributeType) =>
        fieldOrProperty.Attributes.Any(attribute => attribute.Type.FullName == attributeType.FullName);
}
