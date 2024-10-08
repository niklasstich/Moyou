using Moyou.Aspects.Factory;

namespace Moyou.UnitTest.Factory;

[Factory]
[FactoryMember(TargetType = typeof(TypeA))]
[FactoryMember(TargetType = typeof(TypeB), PrimaryInterface = typeof(ITypeB))]
public partial class Factory
{
    
}