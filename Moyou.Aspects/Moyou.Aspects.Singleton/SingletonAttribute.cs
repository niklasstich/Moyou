using Metalama.Framework.Advising;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Diagnostics;
using Metalama.Framework.Eligibility;
using Moyou.Diagnostics;
using Moyou.Extensions;

namespace Moyou.Aspects.Singleton;

[AttributeUsage(AttributeTargets.Class)]
public class SingletonAttribute : TypeAspect
{
    /// <summary>
    /// Whether the singleton should be lazy initialized.
    /// </summary>
    public bool Lazy { get; set; } = true;

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
                .Select(stringList => string.Join(",", stringList))
                .Select(str => string.IsNullOrWhiteSpace(str) ? "void" : str)
                .Select(str => $"({str})");
            var constructorSignaturesString = string.Join(", ", constructorSignatures);
            builder.Diagnostics.Report(
                WarningHasAccessibleConstructors.WithArguments((builder.Target, constructorSignaturesString)));
        }

        if (Lazy) GenerateLazyImplementation(builder);
        else GenerateNonLazyImplementation(builder);
    }

    private void GenerateLazyImplementation(IAspectBuilder<INamedType> builder)
    {
        var lazyGeneric = typeof(Lazy<>).MakeGenericType([builder.Target.ToType()]);

        // add private lazy field
        builder.Advice.IntroduceField(builder.Target, "_instance", lazyGeneric, IntroductionScope.Static,
            OverrideStrategy.Override);
        builder.Advice.AddInitializer(builder.Target, nameof(CreateLazyInstance), InitializerKind.BeforeTypeConstructor,
            args: new { T = builder.Target });

        // add public property
        builder.Advice.IntroduceProperty(builder.Target, "Instance", nameof(GetLazyInstance), null,
            IntroductionScope.Static,
            OverrideStrategy.Override,
            pbuilder => pbuilder.Accessibility = Accessibility.Public,
            args: new { T = builder.Target });
    }

    private void GenerateNonLazyImplementation(IAspectBuilder<INamedType> builder)
    {
        //introduce private static instance field
        builder.Advice.IntroduceField(builder.Target, "_instance", builder.Target, IntroductionScope.Static,
            OverrideStrategy.Override,
            fbuilder => fbuilder.Accessibility = Accessibility.Private);
        //add initializer in static constructor (BeforeTypeConstructor)
        builder.Advice.AddInitializer(builder.Target, nameof(CreateInstance), InitializerKind.BeforeTypeConstructor,
            args: new { T = builder.Target });

        //add public static property
        builder.Advice.IntroduceProperty(builder.Target, "Instance", nameof(GetInstance), null,
            IntroductionScope.Static,
            OverrideStrategy.Override,
            pbuilder => pbuilder.Accessibility = Accessibility.Public,
            args: new { T = builder.Target });
    }

    [Template]
    private static T GetLazyInstance<[CompileTime] T>()
    {
        return meta.ThisType._instance.Value;
    }

    [Template]
    private static void CreateLazyInstance<[CompileTime] T>() where T : new()
    {
        meta.ThisType._instance = new Lazy<T>(() => new T());
    }

    [Template]
    private static T GetInstance<[CompileTime] T>() where T : new()
    {
        return meta.ThisType._instance;
    }

    [Template]
    private static void CreateInstance<[CompileTime] T>() where T : new()
    {
        meta.ThisType._instance = new T();
    }
}