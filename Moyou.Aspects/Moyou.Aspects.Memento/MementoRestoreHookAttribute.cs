using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Eligibility;
using Moyou.Extensions;

namespace Moyou.Aspects.Memento;

/// <summary>
/// Declares a method as a hook which is called when <see cref="IOriginator.RestoreMemento"/> is called.
/// </summary>
public class MementoRestoreHookAttribute : MethodAspect
{
    public override void BuildEligibility(IEligibilityBuilder<IMethod> builder)
    {
        base.BuildEligibility(builder);
        builder.DeclaringType().MustHaveAspectOfType(typeof(MementoAttribute));
        builder.ReturnType().MustBe(typeof(void), ConversionKind.TypeDefinition);
        builder.HasExactlyOneParameterOfTypeNestedMemento();
        builder.MustNotBeAbstract();
    }

    public override void BuildAspect(IAspectBuilder<IMethod> builder)
    {
        base.BuildAspect(builder);
        var restoreMementoMethod = builder.Target.DeclaringType.Methods.FirstOrDefault(method => method.Name == "RestoreMemento");
        if (restoreMementoMethod == null) //aspect is on type but there is no RestoreMemento method - ergo building the method must've failed
            return;
        builder.Advice.Override(restoreMementoMethod, nameof(RestoreMementoTemplate),
                       args: new { target = builder.Target, });
    }

    [Template]
    public dynamic RestoreMementoTemplate(IMethod target)
    {
        meta.Proceed();
        var memento = target.Parameters[0].Value;
        target.Invoke(memento);
        return memento!;
    }
}