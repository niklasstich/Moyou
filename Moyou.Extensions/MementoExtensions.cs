using System.Diagnostics;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Eligibility;

namespace Moyou.Extensions;

[CompileTime]
public static class MementoExtensions
{
    [CompileTime]
    public static void HasExactlyOneParameterOfTypeNestedMemento(this IEligibilityBuilder<IMethod> builder)
    {
        builder.MustSatisfyAll(innerBuilder =>
        {
            innerBuilder.MustSatisfy(method => method.Parameters.Count == 1,
                method => $"{method.Description} must have exactly one parameter of the Memento type");
            innerBuilder.MustSatisfy(method =>
            {
                var parameterType = (INamedType)method.Parameters[0].Type;
                return parameterType.Name.Split('.').Last() == "Memento" && parameterType.DeclaringType == method.DeclaringType;
            }, method =>
            {
                var mementoType =
                    method.Object.DeclaringType.Types.FirstOrDefault(type => type.Name == "Memento");
                return $"{method.Description} must have exactly one parameter of type {mementoType?.FullName}";
            });
        });
    }
}