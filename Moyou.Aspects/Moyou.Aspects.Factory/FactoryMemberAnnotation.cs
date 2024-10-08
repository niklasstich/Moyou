using Metalama.Framework.Aspects;
using Metalama.Framework.Code;

namespace Moyou.Aspects.Factory;

[CompileTime]
public record FactoryMemberAnnotation : IAnnotation<INamedType>
{
    public IRef<IDeclaration> FactoryMemberType { get; }
    public IRef<IDeclaration> PrimaryInterface { get; }

    public FactoryMemberAnnotation(IRef<IDeclaration> factoryMemberType, IRef<IDeclaration> primaryInterface)
    {
        FactoryMemberType = factoryMemberType;
        PrimaryInterface = primaryInterface;
    }

    public (INamedType, INamedType) AsTuple() => (
        (INamedType)FactoryMemberType.GetTarget(ReferenceResolutionOptions.Default),
        (INamedType)PrimaryInterface.GetTarget(ReferenceResolutionOptions.Default));
}