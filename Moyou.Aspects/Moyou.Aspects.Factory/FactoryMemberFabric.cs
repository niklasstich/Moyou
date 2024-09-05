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
    private static readonly DiagnosticDefinition<INamedType> ErrorTypeDoesntImplementAnyInterfaces =
        new(Errors.Factory.TypeDoesntImplementAnyInterfacesId, Severity.Error,
            Errors.Factory.TypeDoesntImplementAnyInterfacesMessageFormat,
            Errors.Factory.TypeDoesntImplementAnyInterfacesTitle,
            Errors.Factory.TypeDoesntImplementAnyInterfacesCategory);

    //MOYOU2203
    private static readonly DiagnosticDefinition<INamedType> ErrorAmbiguousInterfacesOnTargetType =
        new(Errors.Factory.AmbiguousInterfacesOnTargetTypeId, Severity.Error,
            Errors.Factory.AmbiguousInterfacesOnTargetTypeMessageFormat,
            Errors.Factory.AmbiguousInterfacesOnTargetTypeTitle,
            Errors.Factory.AmbiguousInterfacesOnTargetTypeCategory);

    //MOYOU2204
    private static readonly DiagnosticDefinition<INamedType>
        ErrorTargetTypeDoesntImplementPrimaryInterface =
            new(Errors.Factory.TargetTypeDoesntImplementPrimaryInterfaceId, Severity.Error,
                Errors.Factory.TargetTypeDoesntImplementPrimaryInterfaceMessageFormat,
                Errors.Factory.TargetTypeDoesntImplementPrimaryInterfaceTitle,
                Errors.Factory.TargetTypeDoesntImplementPrimaryInterfaceCategory);

    public override void AmendProject(IProjectAmender amender)
    {
        var types = amender
            .SelectTypes()
            .Where(type => type.HasAttribute<FactoryMemberAttribute>());

        //MOYOU2201 no target type
        types
            .Where(type => type
                .Attributes
                .Where(IsFactoryMemberAttribute)
                .Any(NoTargetTypeInAttribute)
            )
            .ReportDiagnostic(type => ErrorNoTargetTypeInMemberAttribute.WithArguments(type));

        //MOYOU2202 no implemented interfaces
        types
            .Where(type => type
                .Attributes
                .Where(IsFactoryMemberAttribute)
                .Any(TargetTypeImplementsNoInterfaces)
            )
            .ReportDiagnostic(type => ErrorTypeDoesntImplementAnyInterfaces.WithArguments(type));
        
        //MOYOU2203 ambiguous interfaces
        types
            .Where(type => type
                .Attributes
                .Where(IsFactoryMemberAttribute)
                .Where(TargetTypeInAttribute)
                .Where(TargetTypeImplementsMultipleInterfaces)
                .Any(NoPrimaryInterfaceInAttribute)
            )
            .ReportDiagnostic(type => ErrorAmbiguousInterfacesOnTargetType.WithArguments(type));
            
        //MOYOU2204 target type doesn't implement primary interface
        types
            .Where(type => type
                .Attributes
                .Where(IsFactoryMemberAttribute)
                .Where(TargetTypeInAttribute)
                .Any(TargetTypeDoesNotImplementPrimaryInterface)
            )
            .ReportDiagnostic(type => ErrorTargetTypeDoesntImplementPrimaryInterface.WithArguments(type));
        
        types.AddAspect(type => BuildAspect(type, amender));
    }

    private static bool TargetTypeImplementsMultipleInterfaces(IAttribute attribute)
    {
        var targetType = (INamedType)attribute.NamedArguments[nameof(FactoryMemberAttribute.TargetType)].Value!;
        return targetType.ImplementedInterfaces.Count > 1;
    }

    private static bool IsFactoryMemberAttribute(IAttribute attribute)
    {
        return attribute.Type.FullName == typeof(FactoryMemberAttribute).FullName;
    }

    private static bool NoTargetTypeInAttribute(IAttribute attribute)
    {
        return !attribute.TryGetNamedArgument(nameof(FactoryMemberAttribute.TargetType), out _);
    }

    private static bool NoPrimaryInterfaceInAttribute(IAttribute attribute)
    {
        return !attribute.TryGetNamedArgument(nameof(FactoryMemberAttribute.PrimaryInterface), out _);
    }
    
    private static bool TargetTypeInAttribute(IAttribute attribute)
    {
        return attribute.TryGetNamedArgument(nameof(FactoryMemberAttribute.TargetType), out _);
    }

    private static bool TargetTypeImplementsNoInterfaces(IAttribute attribute)
    {
        return attribute.TryGetNamedArgument(nameof(FactoryMemberAttribute.TargetType), out var targetType) &&
               ((INamedType)targetType.Value!).ImplementedInterfaces.Count == 0;
    }

    private static bool TargetTypeDoesNotImplementPrimaryInterface(IAttribute attribute)
    {
        var targetType = (INamedType)attribute.NamedArguments[nameof(FactoryMemberAttribute.TargetType)].Value!;
        return attribute.TryGetNamedArgument(nameof(FactoryMemberAttribute.PrimaryInterface), out var primaryInterface) && !targetType.ImplementedInterfaces.Contains((INamedType)primaryInterface.Value!);
    }

    private static FactoryMemberAspect BuildAspect(INamedType type, IProjectAmender amender)
    {
        var memberAttributes = type
            .Attributes
            .Where(attr => attr.Type.FullName == typeof(FactoryMemberAttribute).FullName);
        var targetTuples = GetTypeTuplesFromAttributes(type, memberAttributes);
        var aspect = new FactoryMemberAspect(targetTuples);
        return aspect;
    }

    private static List<(INamedType, INamedType?)> GetTypeTuplesFromAttributes(INamedType factoryType,
        IEnumerable<IAttribute> memberAttributes)
    {
        return memberAttributes
            .Select(GetTypeAndInterfaceTuple)
            .Where(tuple => tuple.HasValue)
            .Select(tuple => tuple.Value)
            .ToList();

        (INamedType, INamedType?)? GetTypeAndInterfaceTuple(IAttribute attr)
        {
            if (!attr.NamedArguments.TryGetValue(nameof(FactoryMemberAttribute.TargetType), out var targetTypeConstant))
                return null; //MOYOU2201
            var targetType = (INamedType)targetTypeConstant.Value!;
            var implementedInterfaces = targetType.ImplementedInterfaces;
            if (!attr.NamedArguments.TryGetValue(nameof(FactoryMemberAttribute.PrimaryInterface),
                    out var primaryInterface))
                return implementedInterfaces.Count == 1 ? (targetType, implementedInterfaces.First()) : null; //MOYOU2202 //MOYOU2203
            var primaryInterfaceType = (INamedType)primaryInterface.Value!;
            if (implementedInterfaces.Contains(primaryInterfaceType))
                return (targetType, primaryInterface.Value as INamedType);
            return null; //MOYOU2204

        }
    }
}