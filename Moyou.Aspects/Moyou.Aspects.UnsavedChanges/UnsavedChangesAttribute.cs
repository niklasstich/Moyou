using System.Diagnostics;
using Metalama.Framework.Advising;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Code.SyntaxBuilders;
using Moyou.Extensions;

namespace Moyou.Aspects.UnsavedChanges;

//TODO: refactor
public class UnsavedChangesAttribute : TypeAspect
{
    public override void BuildAspect(IAspectBuilder<INamedType> builder)
    {
        base.BuildAspect(builder);

        builder.ImplementInterface(typeof(IUnsavedChanges), OverrideStrategy.Ignore);

        builder.IntroduceField(nameof(_internalUnsavedChanges), IntroductionScope.Instance, buildField:
            fbuilder => { fbuilder.Accessibility = Accessibility.Private; });

        //find all members whose type implements UnsavedChangesAttribute themselves (via interface)
        var relevantMembers = builder.Target.AllFieldsAndProperties
            .Where(member => member.TypeHasAttribute(typeof(UnsavedChangesAttribute)))
            //exclude auto backing fields
            .Where(member => member is not IField field || !field.IsAutoBackingField())
            .ToList();

        //find all members whose type is ienumerable of a type that implements UnsavedChangesAttribute (via interface)
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

    [Template] private bool _internalUnsavedChanges = false;
    [Template] public bool UnsavedChanges => meta.This.GetUnsavedChanges();

    [Template]
    private bool GetUnsavedChanges(
        [CompileTime] IEnumerable<IFieldOrProperty> relevantMembers,
        [CompileTime] IEnumerable<IFieldOrProperty> relevantIEnumerableMembers
    )
    {
        meta.DebugBreak();
        var exprBuilder = new ExpressionBuilder();
        exprBuilder.AppendExpression(meta.This._internalUnsavedChanges);
        foreach (var member in relevantMembers)
        {
            exprBuilder.AppendVerbatim("||");
            if (member.Type.IsNullable!.Value)
                // ReSharper disable once ArrangeRedundantParentheses because we need the parantheses in the expression
                exprBuilder.AppendExpression((member.Value?.UnsavedChanges ?? false));
            else
                exprBuilder.AppendExpression(member.Value?.UnsavedChanges);
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
    /// 
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
                ? $"{member.Name} is null ? false : {member.Name}.Any(v => v?.UnsavedChanges ?? false)"
                : $"{member.Name} is null ? false : {member.Name}.Any(v => v.UnsavedChanges)");
        }
        else
        {
            exprBuilder.AppendExpression(genericTypeNullable
                ? ((IEnumerable<IUnsavedChanges?>)member.Value!).Any(v => v?.UnsavedChanges ?? false)
                : ((IEnumerable<IUnsavedChanges>)member.Value!).Any(v => v.UnsavedChanges));
        }
    }

    [Template]
    public void ResetUnsavedChanges(
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
            // if(member.Value is null) continue;
            foreach (var value in (IEnumerable<IUnsavedChanges>)member.Value)
            {
                value.ResetUnsavedChanges();
            }
        }
    }
}

[RunTimeOrCompileTime]
public interface IUnsavedChanges
{
    bool UnsavedChanges { get; }
    void ResetUnsavedChanges();
}