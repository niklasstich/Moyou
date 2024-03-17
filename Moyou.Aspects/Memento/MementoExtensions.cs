using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Eligibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moyou.Aspects.Memento;

[CompileTime]
public static class MementoExtensions
{
    [CompileTime]
    public static void HasExactlyOneParameterOfTypeNestedMemento(this IEligibilityBuilder<IMethod> builder)
    {
        builder.MustSatisfyAll(innerBuilder =>
        {
            innerBuilder.MustSatisfy(method => method.Parameters.Count == 1,
                method => $"{method.Description} must have exactly one parameter");
            innerBuilder.MustSatisfy(method =>
            {
                var mementoType = method.DeclaringType.NestedTypes.FirstOrDefault(type => type.Name == "Memento");
                return mementoType != null && method.Parameters[0].Type.Is(mementoType);
            }, method =>
            {
                var mementoType =
                    method.Object.DeclaringType.NestedTypes.FirstOrDefault(type => type.Name == "Memento");
                return $"{method.Description} must have exactly one parameter of type {mementoType?.FullName}";
            });
        });
    }
}