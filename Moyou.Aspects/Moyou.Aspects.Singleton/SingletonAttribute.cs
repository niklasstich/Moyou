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
    // ReSharper disable once GrammarMistakeInComment
    /// <summary>
    /// Whether the singleton should be lazy initialized.
    /// </summary>
    /// <remarks>
    /// If true, the singleton will be wrapped inside a <see cref="Lazy{T}"/> instance and be initialized when you first
    /// access the instance.
    /// If false, the singleton will be initialized when the types static constructor is first called (see
    /// https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/static-constructors for
    /// more information).
    /// </remarks>
    public bool Lazy { get; set; } = true;

    /// <summary>
    /// MOYOU1101
    /// </summary>
    /// <remarks>
    /// INamedType should be the relevant type, string should be a comma separated list of violating constructor
    /// signatures.
    /// </remarks>
    private static readonly DiagnosticDefinition<(INamedType, string)> WarningHasAccessibleConstructor =
        new(Warnings.Singleton.HasAccessibleConstructorId, Severity.Warning,
            Warnings.Singleton.HasAccessibleConstructorMessageFormat,
            Warnings.Singleton.HasAccessibleConstructorTitle,
            Warnings.Singleton.HasAccessibleConstructorCategory);
    
    /// <summary>
    /// MOYOU1102
    /// </summary>
    /// <remarks>
    /// INamedType should be the relevant type
    /// </remarks>
    private static readonly DiagnosticDefinition<INamedType> WarningHasImplicitPublicConstructor =
        new(Warnings.Singleton.HasImplicitPublicConstructorId, Severity.Warning,
            Warnings.Singleton.HasImplicitPublicConstructorMessageFormat,
            Warnings.Singleton.HasImplicitPublicConstructorTitle,
            Warnings.Singleton.HasImplicitPublicConstructorCategory);

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
        var constructors = builder.Target.Constructors.ToList();
        var first = constructors.First();
        if (constructors.Count == 1 && first is { IsImplicitlyDeclared: true, Parameters.Count: 0, Accessibility: Accessibility.Public })
        {
            ReportImplicitConstructor(builder);
        }
        else
        {
            foreach (var constructor in constructors.Where(constructor => constructor.Accessibility != Accessibility.Private))
            {
                ReportPublicConstructor(builder, constructor);
            }
        }

        if (Lazy) GenerateLazyImplementation(builder);
        else GenerateNonLazyImplementation(builder);
    }

    private static void ReportImplicitConstructor(IAspectBuilder<INamedType> builder)
    {
        //special warning for implicit constructor
        builder.Diagnostics.Report(WarningHasImplicitPublicConstructor.WithArguments(builder.Target), builder.Target);
    }

    private static void ReportPublicConstructor(IAspectBuilder<INamedType> builder, IConstructor constructor)
    {
        var typeSignature = string.Join(", ", constructor.Parameters.Select(param => $"{param.Type.ToDisplayString()} {param.Name}"));
        var signatureString = $"({typeSignature})";
        builder.Diagnostics.Report(WarningHasAccessibleConstructor.WithArguments((builder.Target, signatureString)),
            constructor);
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