using Metalama.Framework.Advising;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Code.SyntaxBuilders;
using Metalama.Framework.Eligibility;
using Moyou.Extensions;

namespace Moyou.Aspects.UnsavedChanges;

/// <summary>
/// Implements a bubbling-up unsaved changes field and a method to reset via <see cref="IUnsavedChanges"/>.
/// All members in a type marked with this attribute that are either themselves marked with this attribute or are an
/// IEnumerable of a type marked with this attribute will be considered for <see cref="GetUnsavedChanges"/> and
/// <see cref="ResetUnsavedChanges"/>.
/// </summary>
/// <remarks>You must still determine yourself when the object itself has unsaved changes and set
/// <see cref="_internalUnsavedChanges"/> yourself accordingly (for example, in your property setters).</remarks>
/// <seealso cref="IUnsavedChanges"/>
public class UnsavedChangesAttribute : TypeAspect
{
    public override void BuildEligibility(IEligibilityBuilder<INamedType> builder)
    {
        base.BuildEligibility(builder);
        builder.MustNotBeInterface();
    }
    
    public override void BuildAspect(IAspectBuilder<INamedType> builder)
    {
        base.BuildAspect(builder);

        builder.ImplementInterface(typeof(IUnsavedChanges), OverrideStrategy.Ignore);

        builder.IntroduceField(nameof(_internalUnsavedChanges), IntroductionScope.Instance, buildField:
            fbuilder => { fbuilder.Accessibility = Accessibility.Private; });

        //find all members whose type implements UnsavedChangesAttribute themselves (via attribute)
        var relevantMembers = builder.Target.AllFieldsAndProperties
            .Where(member => member.TypeHasAttribute(typeof(UnsavedChangesAttribute)))
            //exclude auto backing fields
            .Where(member => member is not IField field || !field.IsAutoBackingField())
            .ToList();

        //find all members whose type is ienumerable of a type that implements UnsavedChangesAttribute (via attribute)
        var relevantIEnumerableMembers = builder.Target.AllFieldsAndProperties
            .Where(member => member.Type is INamedType ntype &&
                             //strings are IEnumerable<char> so we need to exclude them
                             !ntype.Is(SpecialType.String) &&
                             ntype.Is(typeof(IEnumerable<>), ConversionKind.TypeDefinition))
            .Where(member => member.TypeArgumentOfEnumerableHasAttribute(typeof(UnsavedChangesAttribute)))
            .Where(member => member is not IField field || !field.IsAutoBackingField())
            .ToList();


        //pass all of these to the method template
        builder.IntroduceMethod(nameof(GetUnsavedChanges), IntroductionScope.Instance,
            buildMethod: mBuilder => { mBuilder.Accessibility = Accessibility.Private; },
            args: new { relevantMembers, relevantIEnumerableMembers });

        builder.IntroduceMethod(nameof(ResetUnsavedChanges), IntroductionScope.Instance,
            buildMethod: mBuilder => { mBuilder.Accessibility = Accessibility.Public; },
            args: new { relevantMembers, relevantIEnumerableMembers });

        builder.IntroduceProperty(nameof(UnsavedChanges), IntroductionScope.Instance,
            buildProperty: pBuilder => { pBuilder.Accessibility = Accessibility.Public; });
    }

    /// <summary>
    /// Whether this object itself has unsaved changes (regardless of child members).
    /// </summary>
    [Template] private bool _internalUnsavedChanges = false;

    [Template] public bool UnsavedChanges => meta.This.GetUnsavedChanges();

    [Template]
    private static bool GetUnsavedChanges(
        [CompileTime] IEnumerable<IFieldOrProperty> relevantMembers,
        [CompileTime] IEnumerable<IFieldOrProperty> relevantIEnumerableMembers
    )
    {
        var exprBuilder = new ExpressionBuilder();
        exprBuilder.AppendExpression(meta.This._internalUnsavedChanges);
        foreach (var member in relevantMembers)
        {
            exprBuilder.AppendVerbatim("||");
            if (member.Type.IsNullable!.Value)
                // ReSharper disable once ArrangeRedundantParentheses because we need the parantheses in the expression
                exprBuilder.AppendExpression((member.Value?.UnsavedChanges ?? false));
            else
                exprBuilder.AppendExpression(member.Value!.UnsavedChanges);
        }

        foreach (var member in relevantIEnumerableMembers)
        {
            exprBuilder.AppendVerbatim("||");
            var enumerableNullable = meta.CompileTime(member.Type.IsNullable!.Value);
            var genericTypeNullable = meta.CompileTime((INamedType)member.Type).TypeArguments[0].IsNullable!.Value;
            GetUnsavedChangesHandleIEnumerable(enumerableNullable, genericTypeNullable, member, exprBuilder);
        }

        return exprBuilder.ToExpression().Value;
    }

    /// <summary>
    /// Handles code generation for IEnumerable members for <see cref="GetUnsavedChanges"/>.
    /// </summary>
    /// <param name="enumerableNullable">The IEnumerable itself is nullable, i.e. <c>IEnumerable&lt;Foobar&gt;?</c>.</param>
    /// <param name="genericTypeNullable">The type inside the IEnumerable is nullable, i.e. <c>IEnumerable&lt;Foobar?&gt;</c>.</param>
    /// <param name="member">The member itself.</param>
    /// <param name="exprBuilder">The current expression builder.</param>
    [Template]
    private static void GetUnsavedChangesHandleIEnumerable(
        [CompileTime] bool enumerableNullable,
        [CompileTime] bool genericTypeNullable,
        [CompileTime] IFieldOrProperty member,
        [CompileTime] ExpressionBuilder exprBuilder
    )
    {
        if (enumerableNullable)
        {
            //TODO: maybe un-verbatim-ify this?
            exprBuilder.AppendVerbatim(genericTypeNullable
                ? $"({member.Name} is null ? false : {member.Name}.Any(v => v?.UnsavedChanges ?? false))"
                : $"({member.Name} is null ? false : {member.Name}.Any(v => v.UnsavedChanges))");
        }
        else
        {
            exprBuilder.AppendExpression(genericTypeNullable
                ? ((IEnumerable<IUnsavedChanges?>)member.Value!).Any(v => v?.UnsavedChanges ?? false)
                : ((IEnumerable<IUnsavedChanges>)member.Value!).Any(v => v.UnsavedChanges));
        }
    }

    [Template]
    public static void ResetUnsavedChanges(
        [CompileTime] IEnumerable<IFieldOrProperty> relevantMembers,
        [CompileTime] IEnumerable<IFieldOrProperty> relevantIEnumerableMembers
    )
    {
        meta.This._internalUnsavedChanges = false;
        foreach (var member in relevantMembers)
        {
            member.Value?.ResetUnsavedChanges();
        }

        foreach (var member in relevantIEnumerableMembers)
        {
            var enumerableNullable = meta.CompileTime(member.Type.IsNullable!.Value);
            var genericTypeNullable = meta.CompileTime((INamedType)member.Type).TypeArguments[0].IsNullable!.Value;
            ResetUnsavedChangesHandleIEnumerable(enumerableNullable, genericTypeNullable, member);
        }
    }

    /// <summary>
    /// Handles code generation for IEnumerable members for <see cref="ResetUnsavedChanges"/>.
    /// </summary>
    /// <param name="enumerableNullable">The IEnumerable itself is nullable, i.e. <c>IEnumerable&lt;Foobar&gt;?</c>.</param>
    /// <param name="genericTypeNullable">The type inside the IEnumerable is nullable, i.e. <c>IEnumerable&lt;Foobar?&gt;</c>.</param>
    /// <param name="member">The member itself.</param>
    [Template]
    private static void ResetUnsavedChangesHandleIEnumerable(
        [CompileTime] bool enumerableNullable,
        [CompileTime] bool genericTypeNullable,
        [CompileTime] IFieldOrProperty member
    )
    {
        if (enumerableNullable)
        {
            // ReSharper disable once InvertIf
            // limitation: https://doc.postsharp.net/metalama/conceptual/aspects/templates/auxilliary-templates see note
            if (member.Value is not null)
            {
                ResetUnsavedChangesHandleIEnumerableInternal(genericTypeNullable, member);
            }
        }
        else
        {
            ResetUnsavedChangesHandleIEnumerableInternal(genericTypeNullable, member);
        }
    }

    /// <summary>
    /// Helper method for <see cref="ResetUnsavedChangesHandleIEnumerable"/>.
    /// </summary>
    /// <param name="genericTypeNullable">The type inside the IEnumerable is nullable, i.e. <c>IEnumerable&lt;Foobar?&gt;</c>.</param>
    /// <param name="member">The member itself.</param>
    [Template]
    private static void ResetUnsavedChangesHandleIEnumerableInternal([CompileTime] bool genericTypeNullable,
        [CompileTime] IFieldOrProperty member)
    {
        if (genericTypeNullable)
        {
            foreach (var val in member.Value!)
            {
                val?.ResetUnsavedChanges();
            }
        }
        else
        {
            foreach (var val in member.Value!)
            {
                val.ResetUnsavedChanges();
            }
        }
    }
}