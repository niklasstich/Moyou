using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Diagnostics;
using Metalama.Framework.Eligibility;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Metalama.Framework.Advising;
using Metalama.Framework.Code.DeclarationBuilders;
using Moyou.Diagnostics;
using Moyou.Extensions;

namespace Moyou.Aspects.Memento;

public class MementoAttribute : TypeAspect
{
    /// <summary>
    /// Defines the strictness of the memento aspect.
    /// </summary>
    /// <remarks>
    /// See <seealso cref="Memento.StrictnessMode"/> for details.
    /// </remarks>
    public StrictnessMode StrictnessMode { get; set; } = StrictnessMode.Strict;

    /// <summary>
    /// Defines which members of the target type should be included in the memento.
    /// </summary>
    /// <remarks>
    /// See <seealso cref="Memento.MemberMode"/> for details.
    /// </remarks>
    public MemberMode MemberMode { get; set; } = MemberMode.All;

    /// <summary>
    /// MOYOU1001
    /// </summary>
    /// <remarks>
    /// IFieldOrProperty should be the relevant member, first INamedType should be the type of the member,
    /// second INamedType should be the target type.
    /// </remarks>
    private static readonly DiagnosticDefinition<(IFieldOrProperty, INamedType, INamedType)>
        WarningNonSupportedMemberInStrictMode =
            new(Warnings.Memento.NonSupportedMemberInStrictModeId, Severity.Warning,
                Warnings.Memento.NonSupportedMemberInStrictModeMessageFormat,
                Warnings.Memento.NonSupportedMemberInStrictModeTitle,
                Warnings.Memento.NonSupportedMemberInStrictModeCategory);
    
    /// <summary>
    /// MOYOU1002
    /// </summary>
    /// <remarks>
    /// INamedType should be target originator type
    /// </remarks>
    private static readonly DiagnosticDefinition<INamedType>
        WarningNoMementoNestedClass =
            new(Warnings.Memento.NoMementoNestedClassId, Severity.Warning,
                Warnings.Memento.NoMementoNestedClassMessageFormat,
                Warnings.Memento.NoMementoNestedClassTitle,
                Warnings.Memento.NoMementoNestedClassCategory);

    public override void BuildAspect(IAspectBuilder<INamedType> builder)
    {
        base.BuildAspect(builder);

        var relevantMembers = GetRelevantMembers()
            .ToList();

        if (StrictnessMode is StrictnessMode.Strict)
        {
            var membersWithUnsupportedTypes =
                relevantMembers.Where(member => !IsTypeOfMemberSupported(member)).ToList();
            foreach (var unsupportedType in membersWithUnsupportedTypes)
            {
                builder.Diagnostics.Report(
                    WarningNonSupportedMemberInStrictMode.WithArguments((unsupportedType, (INamedType)unsupportedType.Type,
                        builder.Target)), unsupportedType);
            }

            relevantMembers = relevantMembers.Except(membersWithUnsupportedTypes).ToList();
        }

        var res = builder.Advice.IntroduceClass(builder.Target, "Memento", OverrideStrategy.Ignore);
        if (res.Outcome == AdviceOutcome.Default)
        {
            builder.Diagnostics.Report(WarningNoMementoNestedClass.WithArguments(builder.Target), builder.Target);
        }
        var nestedMementoType = res.Outcome == AdviceOutcome.Default
            ? res.Declaration
            : builder.Target.NestedTypes.First(NestedTypeIsEligible);

        //introduce relevant fields and properties to the memento type
        var introducedFieldsOnMemento = IntroduceMementoTypeFields().ToList();



        builder.Advice.ImplementInterface(nestedMementoType, typeof(IMemento), OverrideStrategy.Ignore);

        builder.Advice.ImplementInterface(builder.Target, typeof(IOriginator), OverrideStrategy.Override);


        builder.Advice.IntroduceMethod(builder.Target, nameof(RestoreMementoImpl),
            args: new { nestedMementoType, relevantMembers, introducedFieldsOnMemento },
            scope: IntroductionScope.Instance, whenExists: OverrideStrategy.Override, buildMethod: builder =>
            {
                builder.Accessibility = Accessibility.Private;
            });

        builder.Advice.IntroduceMethod(builder.Target, nameof(CreateMementoImpl),
            args: new { TMementoType = nestedMementoType, relevantMembers, introducedFieldsOnMemento },
            scope: IntroductionScope.Instance, whenExists: OverrideStrategy.Override, buildMethod: builder =>
            {
                builder.Accessibility = Accessibility.Private;
            });

        return;

        IEnumerable<IField> IntroduceMementoTypeFields() => relevantMembers
            .Select(fieldOrProperty => 
                builder.Advice.IntroduceField(nestedMementoType, fieldOrProperty.Name,
                    fieldOrProperty.Type, IntroductionScope.Instance,
                    buildField: fBuilder => fBuilder.Accessibility = Accessibility.Public)
                )
            .Select(r => r.Declaration);

        IEnumerable<IFieldOrProperty> GetRelevantMembers()
        {
            return (MemberMode switch
            {
                MemberMode.All => GetRelevantFields().Union(GetRelevantProperties()),
                MemberMode.FieldsOnly => GetRelevantFields(),
                MemberMode.PropertiesOnly => GetRelevantProperties(),
                _ => throw new InvalidOperationException($"Invalid value for {nameof(MemberMode)}")
            })
                //Only collect members we can write to
                .Where(member => member.Writeability == Writeability.All)
                //Only collect members that don't have the MementoIgnoreAttribute 
                .Where(member => !member.HasAttribute(typeof(MementoIgnoreAttribute)));
        }

        IEnumerable<IFieldOrProperty> GetRelevantFields()
        {
            //TODO: figure out a way to determine that a field is a backing field for a non-auto property (e.g. heuristically by analyzing the property setter/getter)?
            return builder.Target.AllFields
                .Where(field => !field.IsAutoBackingField());
        }

        IEnumerable<IFieldOrProperty> GetRelevantProperties()
        {
            return builder.Target.AllProperties;
        }

        bool IsTypeOfMemberSupported(IFieldOrProperty member)
        {
            var knownCollectionTypes = new[]
            {
                typeof(Dictionary<,>), typeof(List<>), typeof(HashSet<>), typeof(ReadOnlyCollection<>),
                typeof(ReadOnlyDictionary<,>), typeof(FrozenDictionary<,>), typeof(FrozenSet<>),
                typeof(ImmutableList<>), typeof(ImmutableHashSet<>), typeof(ImmutableArray<>),
                typeof(ImmutableDictionary<,>), typeof(ImmutableSortedDictionary<,>), typeof(ImmutableSortedSet<>),
                typeof(Lookup<,>)
            };
            var type = member.Type;
            return !(type.IsReferenceType ?? false) ||
                   type.GetType().IsArray ||
                   type.Is(typeof(ICloneable)) ||
                   knownCollectionTypes.Select(ct => type.Is(ct, ConversionKind.TypeDefinition)).Any(b => b);
        }
    }

    public override void BuildEligibility(IEligibilityBuilder<INamedType> builder)
    {
        base.BuildEligibility(builder);
        builder.MustNotBeAbstract();
        builder.MustNotBeInterface();
    }

    private static bool NestedTypeIsEligible(INamedType nestedType)
    {
        return nestedType is
        {
            Name: "Memento", Accessibility: Accessibility.Private,
            TypeKind: TypeKind.Class or TypeKind.RecordClass or TypeKind.RecordStruct or TypeKind.Struct
        };
    }

    [InterfaceMember]
    public void RestoreMemento(IMemento memento)
    {
        meta.This.RestoreMementoImpl(memento);
    }

    [InterfaceMember]
    public IMemento CreateMemento()
    {
        return meta.This.CreateMementoImpl();
    }

    [Template]
    public void RestoreMementoImpl(IMemento memento,
        [CompileTime] INamedType nestedMementoType,
        [CompileTime] IEnumerable<IFieldOrProperty> relevantMembers,
        [CompileTime] IEnumerable<IFieldOrProperty> introducedFieldsOnMemento
    )
    {
        try
        {
            var cast = meta.Cast(nestedMementoType, memento);
            //if (cast is null) return;
            //prevent multiple enumerations
            var mementoTypeMembers = introducedFieldsOnMemento.ToList();
            foreach (var fieldOrProp in relevantMembers)
            {
                var nestedTypeMember =
                    mementoTypeMembers.First(m => m.Name == fieldOrProp.Name).With((IExpression)cast!);
                fieldOrProp.Value = nestedTypeMember.Value;
            }
        }
        catch (InvalidCastException icex)
        {
            throw new ArgumentException("Incorrect memento type", nameof(memento), icex);
        }
    }

    [Template]
    public IMemento CreateMementoImpl<[CompileTime] TMementoType>(
        [CompileTime] IEnumerable<IFieldOrProperty> relevantMembers,
        [CompileTime] IEnumerable<IFieldOrProperty> introducedFieldsOnMemento) where TMementoType : IMemento, new()
    {
        var memento = new TMementoType();
        //prevent multiple enumerations
        var relevantMembersList = relevantMembers.ToList();
        var introducedFieldsOnMementoList = introducedFieldsOnMemento.ToList();
        foreach (var sourceFieldOrProp in relevantMembersList)
        {
            //assign some fields
            var targetFieldOrProp = introducedFieldsOnMementoList
                .Single(memFieldOrProp => memFieldOrProp.Name == sourceFieldOrProp.Name).With(memento);
            if (!(sourceFieldOrProp.Type.IsReferenceType ?? false))
                targetFieldOrProp.Value = sourceFieldOrProp.Value;
            else if (sourceFieldOrProp.Type.Is(SpecialType.String, ConversionKind.TypeDefinition)) //strings are immutable
                targetFieldOrProp.Value = sourceFieldOrProp.Value;
            else if (sourceFieldOrProp.Type.Is(typeof(ICloneable)))
            {
                targetFieldOrProp.Value = meta.Cast(sourceFieldOrProp.Type,
                    sourceFieldOrProp.Value is not null ? sourceFieldOrProp.Value?.Clone() : null);
            }
            else if (sourceFieldOrProp.Type.Is(SpecialType.IEnumerable_T, ConversionKind.TypeDefinition))
            {
                HandleIEnumerable(sourceFieldOrProp, targetFieldOrProp);
            }
            else
            {
                targetFieldOrProp.Value = sourceFieldOrProp.Value;
            }
        }

        return memento;

    }

    [Template]
    private void HandleIEnumerable(IFieldOrProperty sourceFieldOrProp, IExpression targetFieldOrProp)
    {
        var namedType = (INamedType)sourceFieldOrProp.Type;
        //copy-via-To[Collection]() types
        if (namedType.Is(typeof(Dictionary<,>), ConversionKind.TypeDefinition))
            targetFieldOrProp.Value = sourceFieldOrProp.Value?.ToDictionary();
        else if (namedType.Is(typeof(List<>), ConversionKind.TypeDefinition))
            targetFieldOrProp.Value = sourceFieldOrProp.Value?.ToList();
        else if (namedType.Is(typeof(HashSet<>), ConversionKind.TypeDefinition))
            targetFieldOrProp.Value = sourceFieldOrProp.Value?.ToHashSet();
        else if (namedType.GetType().IsArray)
            targetFieldOrProp.Value = sourceFieldOrProp.Value?.ToArray();

        //assign immutable collections directly
        else if (namedType.Is(typeof(ReadOnlyCollection<>), ConversionKind.TypeDefinition) ||
                 namedType.Is(typeof(ReadOnlyDictionary<,>), ConversionKind.TypeDefinition) ||
                 namedType.Is(typeof(FrozenDictionary<,>), ConversionKind.TypeDefinition) ||
                 namedType.Is(typeof(FrozenSet<>), ConversionKind.TypeDefinition) ||
                 namedType.Is(typeof(ImmutableList<>), ConversionKind.TypeDefinition) ||
                 namedType.Is(typeof(ImmutableHashSet<>), ConversionKind.TypeDefinition) ||
                 namedType.Is(typeof(ImmutableArray<>), ConversionKind.TypeDefinition) ||
                 namedType.Is(typeof(ImmutableDictionary<,>), ConversionKind.TypeDefinition) ||
                 namedType.Is(typeof(ImmutableSortedDictionary<,>), ConversionKind.TypeDefinition) ||
                 namedType.Is(typeof(ImmutableSortedSet<>), ConversionKind.TypeDefinition) ||
                 namedType.Is(typeof(Lookup<,>), ConversionKind.TypeDefinition))
            targetFieldOrProp.Value = sourceFieldOrProp.Value;

        //fallback, should never reach this point
        else
            targetFieldOrProp.Value = sourceFieldOrProp.Value;
    }
}