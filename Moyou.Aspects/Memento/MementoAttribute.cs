using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Code.SyntaxBuilders;
using Metalama.Framework.Eligibility;
using Moyou.Aspects.Extensions;
using System.Diagnostics;

namespace Moyou.Aspects.Memento;

public class MementoAttribute : TypeAspect
{
    public override void BuildAspect(IAspectBuilder<INamedType> builder)
    {
        base.BuildAspect(builder);
        //ignore auto backing fields of properties because restoring properties via setter is sufficient
        //(and will also trigger for example INotifyPropertyChanged)
        //for backing fields of non-auto properties, we have no choice but to restore them twice, once via the field and once via the property
        //unless we TODO: figure out a way to determine that a field is a backing field for a non-auto property (e.g. heuristically by analyzing the property setter/getter)
        var fields = builder.Target.AllFields
            .Where(field => !field.IsAutoBackingField());
        //only collect properties with a setter, as we can only restore properties with a setter
        var properties = builder.Target.AllProperties;

        //filter ignored members
        var relevantFieldsAndProperties = fields
            .Union<IFieldOrProperty>(properties)
            .Where(prop => prop.Writeability == Writeability.All)
            .Where(fieldOrProp =>
                fieldOrProp.Attributes.All(attr => attr.Type.FullName != typeof(MementoIgnoreAttribute).FullName)
            )
            .ToList();

        var nestedMementoType = builder.Target.NestedTypes.First(NestedTypeIsEligible);

        //introduce relevant fields and properties to the memento type
        var mementoTypeFields = IntroduceMementoTypeFields().ToList();

        builder.Advice.ImplementInterface(nestedMementoType, typeof(IMemento), OverrideStrategy.Ignore);

        builder.Advice.ImplementInterface(builder.Target, typeof(IOriginator), OverrideStrategy.Override);


        builder.Advice.IntroduceMethod(builder.Target, nameof(RestoreMementoImpl),
            args: new
            {
                nestedType = nestedMementoType,
                relevantMembers = relevantFieldsAndProperties,
                introducedMementoTypeMembers = mementoTypeFields
            }, scope: IntroductionScope.Instance, whenExists: OverrideStrategy.Override, buildMethod: builder =>
            {
                builder.Accessibility = Accessibility.Private;
            });

        builder.Advice.IntroduceMethod(builder.Target, nameof(CreateMementoImpl),
            args: new
            {
                //nestedType = nestedMementoType,
                relevantMembers = relevantFieldsAndProperties,
                introducedMementoTypeMembers = mementoTypeFields,
                TNestedType = nestedMementoType
            }, scope: IntroductionScope.Instance, whenExists: OverrideStrategy.Override, buildMethod: builder =>
            {
                builder.Accessibility = Accessibility.Private;
            });

        return;

        IEnumerable<IField> IntroduceMementoTypeFields() => relevantFieldsAndProperties
            .Select(fieldOrProperty => builder.Advice.IntroduceField(nestedMementoType, fieldOrProperty.Name,
                fieldOrProperty.Type, IntroductionScope.Instance,
                buildField: fBuilder => fBuilder.Accessibility = Accessibility.Public))
            .Select(r => r.Declaration);
    }

    public override void BuildEligibility(IEligibilityBuilder<INamedType> builder)
    {
        base.BuildEligibility(builder);
        builder.MustSatisfy(type => type.NestedTypes.Any(NestedTypeIsEligible),
            type =>
                $"{type.Description} must contain a nested private class, (struct) record or struct called 'Memento''.");
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
        [CompileTime] INamedType nestedType,
        [CompileTime] IEnumerable<IFieldOrProperty> relevantMembers,
        [CompileTime] IEnumerable<IFieldOrProperty> introducedMementoTypeMembers
    )
    {
        try
        {
            var cast = meta.Cast(nestedType, memento);
            if (cast is null) return;
            //prevent multiple enumerations
            var mementoTypeMembers = introducedMementoTypeMembers.ToList();
            foreach (var fieldOrProp in relevantMembers)
            {
                var nestedTypeMember =
                    mementoTypeMembers.First(m => m.Name == fieldOrProp.Name).With((IExpression)cast);
                fieldOrProp.Value = nestedTypeMember.Value;
            }
        }
        catch (InvalidCastException icex)
        {
            throw new ArgumentException("Incorrect memento type", nameof(memento), icex);
        }
    }

    [Template]
    public IMemento CreateMementoImpl<[CompileTime] TNestedType>([CompileTime] IEnumerable<IFieldOrProperty> relevantMembers,
        [CompileTime] IEnumerable<IFieldOrProperty> introducedMementoTypeMembers
    ) where TNestedType : IMemento, new()
    {
        var memento = new TNestedType();
        //prevent multiple enumerations
        var relevantMembersList = relevantMembers.ToList();
        foreach (var fieldOrProp in relevantMembersList)
        {
            //assign some fields
            var memFieldOrProp = introducedMementoTypeMembers.Single(memFieldOrProp => memFieldOrProp.Name == fieldOrProp.Name).With(memento);
            if (fieldOrProp.Type.Is(typeof(ICloneable)))
            {
                memFieldOrProp.Value = meta.Cast(fieldOrProp.Type, fieldOrProp.Value is not null ? fieldOrProp.Value.Clone() : null);
            }
            ///TODO: handle collections
            else if (fieldOrProp.Type.Is(SpecialType.IEnumerable_T, ConversionKind.TypeDefinition))
            {
                meta.DebugBreak();
            }
            else if (fieldOrProp.Type.Is(SpecialType.List_T, ConversionKind.TypeDefinition))
            {
                var typeArg = (fieldOrProp.Type as INamedType).TypeArguments.First();
                meta.DebugBreak();
            }
            else
            {
                memFieldOrProp.Value = fieldOrProp.Value;
            }
        }

        return memento;
    }
}