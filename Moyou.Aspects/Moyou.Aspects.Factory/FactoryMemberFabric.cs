using System.Diagnostics;
using JetBrains.Annotations;
using Metalama.Framework.Code;
using Metalama.Framework.Diagnostics;
using Metalama.Framework.Fabrics;
using Moyou.Diagnostics;
using Moyou.Extensions;

namespace Moyou.Aspects.Factory;

[UsedImplicitly]
public class FactoryMemberFabric : TransitiveProjectFabric
{
    //MOYOU2201
    private static readonly DiagnosticDefinition<INamedType> ErrorNoTargetTypeInMemberAttribute =
        new(Errors.Factory.NoTargetTypeInMemberAttributeId, Severity.Error,
            Errors.Factory.NoTargetTypeInMemberAttributeMessageFormat,
            Errors.Factory.NoTargetTypeInMemberAttributeTitle,
            Errors.Factory.NoTargetTypeInMemberAttributeCategory);
    
    //MOYOU2202
    private static readonly DiagnosticDefinition<(INamedType, INamedType)> ErrorTypeDoesntImplementAnyInterfaces =
        new(Errors.Factory.TypeDoesntImplementAnyInterfacesId, Severity.Error,
            Errors.Factory.TypeDoesntImplementAnyInterfacesMessageFormat,
            Errors.Factory.TypeDoesntImplementAnyInterfacesTitle,
            Errors.Factory.TypeDoesntImplementAnyInterfacesCategory);
    
    //MOYOU2203
    private static readonly DiagnosticDefinition<(INamedType, INamedType)> ErrorAmbiguousInterfacesOnTargetType =
        new(Errors.Factory.AmbiguousInterfacesOnTargetTypeId, Severity.Error,
            Errors.Factory.AmbiguousInterfacesOnTargetTypeMessageFormat,
            Errors.Factory.AmbiguousInterfacesOnTargetTypeTitle,
            Errors.Factory.AmbiguousInterfacesOnTargetTypeCategory);
    
    //MOYOU2204
    private static readonly DiagnosticDefinition<(INamedType, INamedType, INamedType)> ErrorTargetTypeDoesntImplementPrimaryInterface =
        new(Errors.Factory.TargetTypeDoesntImplementPrimaryInterfaceId, Severity.Error,
            Errors.Factory.TargetTypeDoesntImplementPrimaryInterfaceMessageFormat,
            Errors.Factory.TargetTypeDoesntImplementPrimaryInterfaceTitle,
            Errors.Factory.TargetTypeDoesntImplementPrimaryInterfaceCategory);
    
    public override void AmendProject(IProjectAmender amender)
    {
        var types = amender.SelectTypes().Where(type => type.HasAttribute<FactoryMemberAttribute>());
        types.AddAspect(type => BuildAspect(type, amender));
    }

    private static FactoryMemberAspect BuildAspect(INamedType type, IProjectAmender amender)
    {
        var memberAttributes =
            type.Attributes.Where(attr => attr.Type.FullName == typeof(FactoryMemberAttribute).FullName);
        var targetTuples = GetTypeTuplesFromAttributes(type, memberAttributes, amender);
        var aspect = new FactoryMemberAspect(targetTuples);
        return aspect;
    }

    private static List<(INamedType, INamedType?)> GetTypeTuplesFromAttributes(INamedType factoryType,
        IEnumerable<IAttribute> memberAttributes,
        IProjectAmender amender)
    {
        return memberAttributes.Select(GetTypeAndInterfaceTuple).Where(tuple => tuple.HasValue)
            .Select(tuple => tuple.Value).ToList();

        (INamedType, INamedType?)? GetTypeAndInterfaceTuple(IAttribute attr)
        {
            Debugger.Break();
            if (!attr.NamedArguments.TryGetValue(nameof(FactoryMemberAttribute.TargetType), out var targetTypeConstant))
            {
                //TODO: figure out why this doesn't show up in compile time test
                amender.ReportDiagnostic(_ => ErrorNoTargetTypeInMemberAttribute.WithArguments(factoryType));
                return null;
            }
            var targetType = (INamedType)targetTypeConstant.Value!;
            var implementedInterfaces = targetType.ImplementedInterfaces;
            if (attr.NamedArguments.TryGetValue(nameof(FactoryMemberAttribute.PrimaryInterface),
                    out var primaryInterface))
            {
                var primaryInterfaceType = (INamedType)primaryInterface.Value!;
                if (implementedInterfaces.Contains(primaryInterfaceType))
                    return (targetType, primaryInterface.Value as INamedType);
                amender.ReportDiagnostic(_ => ErrorTargetTypeDoesntImplementPrimaryInterface.WithArguments((targetType, factoryType, primaryInterfaceType)));
                return null;
            }
            switch (implementedInterfaces.Count)
            {
                case 0:
                    amender.ReportDiagnostic(_ =>
                        ErrorTypeDoesntImplementAnyInterfaces.WithArguments((targetType, factoryType)));
                    return null;
                case > 1: //at this point we know there is no PrimaryInterface property on the attribute, so if there
                          //are multiple interfaces implemented, we don't know which one to pick
                    amender.ReportDiagnostic(_ => ErrorAmbiguousInterfacesOnTargetType.WithArguments((targetType, factoryType)));
                    return null;
                default:
                    return (targetType, implementedInterfaces.First());
            }
        }
        
    }
}