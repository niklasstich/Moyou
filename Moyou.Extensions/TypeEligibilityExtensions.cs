using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Eligibility;

namespace Moyou.Extensions;

[CompileTime]
public static class TypeEligibilityExtensions
{
    /// <summary>
    /// Target type must have a parameterless constructor.
    /// </summary>
    public static void MustHaveParameterlessConstructor(this IEligibilityBuilder<INamedType> builder)
    {
        builder.MustSatisfy(type => type.Constructors.Any(constructor => !constructor.Parameters.Any()),
            type => $"{type.Description} must have a parameterless constructor");
    }
}