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

    /// <summary>
    /// Checks if the first type argument of an IEnumerable field or property has a given attribute.
    /// </summary>
    /// <param name="fieldOrProperty">The field or property to check.</param>
    /// <param name="attributeType">The type of the attribute to check for.</param>
    /// <exception cref="ArgumentException"><paramref name="fieldOrProperty"/> is not of type <see cref="IEnumerable{T}"/></exception>
    /// <returns>True if the first type argument has the attribute, false otherwise.</returns>
    public static bool TypeArgumentOfEnumerableHasAttribute(this IFieldOrProperty fieldOrProperty, Type attributeType)
    {
        var memberType = (INamedType)fieldOrProperty.Type;
        if (!memberType.Is(typeof(IEnumerable<>), ConversionKind.TypeDefinition))
            throw new ArgumentException("Field or property must be of type IEnumerable<T>.", nameof(fieldOrProperty));
        var firstTypeArgument = (INamedType)memberType.TypeArguments[0];
        return firstTypeArgument.Attributes.Any(attribute => attribute.Type.FullName == attributeType.FullName);
    }
}