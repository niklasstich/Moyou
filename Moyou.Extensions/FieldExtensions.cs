using System.Text.RegularExpressions;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;

#pragma warning disable SYSLIB1045 //can't use runtime regex in compile time code

namespace Moyou.Extensions;
[CompileTime]
public static class FieldExtensions
{
    private const string BackingFieldRegex = ".*<(.*)>k__BackingField";

    /// <summary>
    /// Determines whether a field is an automatic backing field for a property.
    /// </summary>
    /// <param name="field">The field to test.</param>
    /// <returns>True if <paramref name="field"/> is an automatic backing field, false otherwise.</returns>
    public static bool IsAutoBackingField(this IField field)
    {
        return field.IsImplicitlyDeclared && Regex.IsMatch(field.Name, BackingFieldRegex);
    }

    /// <summary>
    /// Determines the property that is backed by a given field if it is an automatic backing field.
    /// </summary>
    /// <param name="field">The field to find the property of.</param>
    /// <returns><c>null</c> if <paramref name="field"/> is not an automatic backing field, otherwise the <see cref="IProperty"/>
    /// that <paramref name="field"/> is the automatic backing field for.</returns>
    public static IProperty? GetPropertyForBackingField(this IField field)
    {
        if (!field.IsAutoBackingField()) return null;
        var candidateProperties = field.DeclaringType.AllProperties;
        var match = Regex.Match(field.Name, BackingFieldRegex);
        var propertyName = match.Groups[1].Value;
        return candidateProperties.FirstOrDefault(p => p.Name.Equals(propertyName));
    }
}
