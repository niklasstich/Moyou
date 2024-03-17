using JetBrains.Annotations;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Eligibility;

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
        var createMementoMethod = builder.Target.DeclaringType.Methods.First(method => method.Name == "CreateMemento");
        builder.Advice.Override(createMementoMethod, nameof(CreateMementoTemplate),
            args: new { target = builder.Target, });
    }

    [Template]
    public dynamic CreateMementoTemplate(IMethod target)
    {
        var memento = meta.Proceed();
        target.Invoke(memento);
        return memento;
    }
}