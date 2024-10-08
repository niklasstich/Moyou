using Moyou.Aspects.Factory;

namespace Moyou.CompileTimeTest.Factory.FactoryAttributeTests;

[FactoryMember(TargetType = typeof(TypeD), PrimaryInterface = typeof(InterfaceC))]
public class FactoryD 
{
    
}

public class TypeD
{
    
}

public interface InterfaceC
{
    
}