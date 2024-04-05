using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Code.SyntaxBuilders;
using Metalama.Framework.Diagnostics;
using Metalama.Framework.Eligibility;
using Moyou.Diagnostics;
using Moyou.Extensions;

namespace Moyou.Aspects.Singleton;

[AttributeUsage(AttributeTargets.Class)]
public class SingletonAttribute : TypeAspect
{
    /// <summary>
    /// MOYOU1101
    /// </summary>
    /// <remarks>
    /// INamedType should be the relevant type, string should be a comma separated list of violating constructor
    /// signatures.
    /// </remarks>
    private static readonly DiagnosticDefinition<(INamedType, string)> WarningHasAccessibleConstructors =
        new(Warnings.Singleton.HasAccessibleConstructorsId, Severity.Warning,
            Warnings.Singleton.HasAccessibleConstructorsMessageFormat,
            Warnings.Singleton.HasAccessibleConstructorsTitle,
            Warnings.Singleton.HasAccessibleConstructorsCategory);

    public override void BuildEligibility(IEligibilityBuilder<INamedType> builder)
    {
        base.BuildEligibility(builder);
        builder.MustNotBeInterface();
        builder.MustNotBeAbstract();
        builder.MustNotBeStatic();
        builder.MustHaveParameterlessConstructor();
    }

    public override void BuildAspect(IAspectBuilder<INamedType> builder)
    {
        base.BuildAspect(builder);

        // warning if there are any non-private constructors
        if (builder.Target.Constructors.Any(constructor => constructor.Accessibility != Accessibility.Private))
        {
            var constructorSignatures = builder.Target.Constructors
                .Where(constructor => constructor.Accessibility != Accessibility.Private)
                .Select(constructor => constructor.Parameters)
                .Select(parameters => parameters.Select(parameter => parameter.Type.ToDisplayString()))
                .Select(strings => string.Join(",", strings))
                .Select(str => string.IsNullOrWhiteSpace(str) ? "void" : str)
                .Select(str => $"({str})");
            var constructorSignaturesString = string.Join(", ", constructorSignatures);
            builder.Diagnostics.Report(
                WarningHasAccessibleConstructors.WithArguments((builder.Target, constructorSignaturesString)));
        }

        var lazyGeneric = typeof(Lazy<>).MakeGenericType([builder.Target.ToType()]);

        // add private lazy field
        var fieldInitializerBuilder = new ExpressionBuilder();
        fieldInitializerBuilder.AppendVerbatim("new ");
        // fieldInitializerBuilder.AppendTypeName(lazyGeneric);
        fieldInitializerBuilder.AppendVerbatim("(() => new ");
        fieldInitializerBuilder.AppendTypeName(builder.Target);
        fieldInitializerBuilder.AppendVerbatim("())");

        builder.Advice.IntroduceField(builder.Target, "_instance", lazyGeneric, IntroductionScope.Static,
            OverrideStrategy.Override,
            fbuilder => fbuilder.InitializerExpression = fieldInitializerBuilder.ToExpression());

        // add public property
        builder.Advice.IntroduceProperty(builder.Target, "Instance", nameof(GetInstance), null, IntroductionScope.Static,
            OverrideStrategy.Override,
            pbuilder => pbuilder.Accessibility = Accessibility.Public,
            args: new {T = builder.Target} );
    }

    [Template]
    private static T GetInstance<[CompileTime]T>()
    {
        return meta.ThisType._instance.Value;
    }
}