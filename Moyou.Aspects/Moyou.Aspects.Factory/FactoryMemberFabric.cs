using Metalama.Framework.Code;
using Metalama.Framework.Fabrics;
using Moyou.Extensions;

namespace Moyou.Aspects.Factory;

public class FactoryMemberFabric : TransitiveProjectFabric
{
    public override void AmendProject(IProjectAmender amender)
    {
        var types = amender.SelectTypes().Where(type => type.HasAttribute<FactoryMemberAttribute>());
        types.AddAspect(type => BuildAspect(type, amender));
    }

    private static FactoryMemberAspect BuildAspect(INamedType type, IProjectAmender amender)
    {
        var memberAttributes =
            type.Attributes.Where(attr => attr.Type.FullName == typeof(FactoryMemberAttribute).FullName);
        var targetTuples =
            memberAttributes.Select(GetTypeAndInterfaceTuple).ToList();
        var aspect = new FactoryMemberAspect(targetTuples);
        return aspect;

        (INamedType, INamedType?) GetTypeAndInterfaceTuple(IAttribute attr)
        {
            if (!attr.NamedArguments.TryGetValue(nameof(FactoryMemberAttribute.TargetType), out var targetTypeConstant))
                throw new Exception("No target type found"); //TODO: proper diagnostic reporting
            var targetType = (INamedType)targetTypeConstant.Value!;
            if (attr.NamedArguments.TryGetValue(nameof(FactoryMemberAttribute.PrimaryInterface),
                    out var primaryInterface))
                return (targetType, primaryInterface.Value as INamedType);
            var interfaces = targetType.ImplementedInterfaces;
            if (interfaces.Count != 1)
                throw new Exception(
                    "Type doesn't have exactly one interface, you must define the primary interface"); //TODO: proper diagnostic reporting
            return (targetType, interfaces.First());
        }
    }
}