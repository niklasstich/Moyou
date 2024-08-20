using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
        

        if (AbstractFactoryType is not null)
        {
            _abstractFactoryType = (INamedType)TypeFactory.GetType(AbstractFactoryType!);
            if (_abstractFactoryType.TypeKind != TypeKind.Interface)
                throw new ArgumentException("Type must be an interface", nameof(AbstractFactoryType));

            if (!_abstractFactoryType.HasAttribute<AbstractFactoryAttribute>())
            {
                throw new ArgumentException($"Type must be marked with {nameof(AbstractFactoryAttribute)}",
                    nameof(AbstractFactoryType));
            }
        }
        
        // //try to find all types with FactoryMemberAttribute
        // var targetTypes = builder.Target.Compilation.AllTypes.Where(type => type.HasAttribute<FactoryMemberAttribute>());
        Debugger.Break();
        //TODO: read the annotation from FactoryMemberAspect and process all tuples 
    }

    private static void IntroduceAttributeToAbstractFactory(IAspectBuilder<INamedType> builder, INamedType abstractFactory)
    {
    }
}