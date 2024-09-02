using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Metalama.Framework.Advising;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Code.DeclarationBuilders;
using Moyou.Extensions;

namespace Moyou.Aspects.Factory;

[AttributeUsage(AttributeTargets.Class)]
public class FactoryAttribute : TypeAspect
{
    public Type? AbstractFactoryType { get; set; }
    public Type[] FactoryMembers { get; set; }
    private INamedType? _abstractFactoryType;

    [SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly")] //property is argument
    public override void BuildAspect(IAspectBuilder<INamedType> builder)
    {
        base.BuildAspect(builder);

        //read the annotations from FactoryMemberAspect and process all tuples 
        var annotations = builder.Target.Enhancements().GetAnnotations<FactoryMemberAnnotation>();
        var tuples = annotations.Select(annotation => annotation.AsTuple()).ToList();
        foreach (var tuple in tuples)
        {
            AddMemberToFactory(builder, tuple);
        }
    }

    private void AddMemberToFactory(IAspectBuilder<INamedType> builder, (INamedType, INamedType) tuple)
    {
        Debugger.Break();
        var memberType = tuple.Item1;
        var primaryInterface = tuple.Item2;
        var trimmedInterfaceName = primaryInterface.Name.StartsWith("I")
            ? primaryInterface.Name[1..]
            : primaryInterface.Name;
        if (memberType.HasDefaultConstructor)
        {
            builder.IntroduceMethod(nameof(CreateTemplateDefaultConstructor), IntroductionScope.Instance,
                buildMethod: builder =>
                {
                    //drop the leading 'I' from the interface in the method name
                    builder.Name = $"Create{trimmedInterfaceName}";
                    builder.Accessibility = Accessibility.Public;
                }, args: new { TInterface = primaryInterface, memberType });
        }
        else
        {
            IConstructor constructor;
            if (memberType.Constructors.Count == 1)
            {
                constructor = memberType.Constructors.Single();
            }
            else
            {
                //dont know what constructor to use, panic
                //TODO: check for [FactoryMemberConstructor] and proper error diagnostic
                throw new NotImplementedException();
            }

            builder.IntroduceMethod(nameof(CreateTemplate), IntroductionScope.Instance, buildMethod: builder =>
            {
                // Debugger.Break();
                builder.Name = $"Create{trimmedInterfaceName}";
                builder.Accessibility = Accessibility.Public;
                //add all constructor parameters to factory method
                foreach (var constructorParameter in constructor.Parameters)
                {
                    builder.AddParameter(constructorParameter.Name, constructorParameter.Type);
                }
            }, args: new { TInterface = primaryInterface, memberType, constructor });
        }
    }

    [Template]
    public TInterface CreateTemplateDefaultConstructor<[CompileTime] TInterface>([CompileTime] INamedType memberType)
    {
        var constructor = meta.CompileTime(memberType.Constructors.GetDefaultConstructor());
        return constructor.Invoke();
    }

    [Template]
    public TInterface CreateTemplate<[CompileTime] TInterface>([CompileTime] INamedType memberType,
        [CompileTime] IConstructor constructor)
    {
        // meta.DebugBreak();
        return constructor.Invoke(constructor.Parameters.Select(param => (IExpression)param.Value));
    }
}