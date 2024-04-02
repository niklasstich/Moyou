using JetBrains.Annotations;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Eligibility;
using Moyou.Extensions;

namespace Moyou.Aspects.Memento;

/// <summary>
/// Declares a method as a hook which is called when <see cref="IOriginator.CreateMemento"/> is called.
/// </summary>
[UsedImplicitly]
public class MementoCreateHookAttribute : MethodAspect
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
        var createMementoMethod = builder.Target.DeclaringType.Methods.FirstOrDefault(method => method.Name == "CreateMemento");
        if (createMementoMethod == null) //aspect is on type but there is no CreateMemento method - ergo building the method must've failed
            return;
        builder.Advice.Override(createMementoMethod, nameof(CreateMementoTemplate),
            args: new { target = builder.Target, });
    }

    [Template]
    public dynamic CreateMementoTemplate(IMethod target)
    {
        var memento = meta.Proceed();
        target.Invoke(memento);
        return memento!;
    }
}