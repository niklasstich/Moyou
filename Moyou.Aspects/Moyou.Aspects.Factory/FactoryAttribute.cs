using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Metalama.Framework.Advising;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
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

        Debugger.Break();
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
        if (memberType.HasDefaultConstructor)
        {
            builder.IntroduceMethod(nameof(CreateTemplateDefaultConstructor), IntroductionScope.Instance, buildMethod: builder =>
            {
                //drop the leading 'I' from the interface in the method name
                var trimmedInterfaceName = primaryInterface.Name.StartsWith('I')
                    ? primaryInterface.Name[1..]
                    : primaryInterface.Name;
                builder.Name = $"Create{trimmedInterfaceName}";
                builder.Accessibility = Accessibility.Public;
            }, args: new { TInterface = primaryInterface, memberType });
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    [Template]
    public TInterface CreateTemplateDefaultConstructor<[CompileTime] TInterface>([CompileTime] INamedType memberType)
    {
        var constructor = meta.CompileTime(memberType.Constructors.GetDefaultConstructor());
        return constructor.Invoke();
    }
}