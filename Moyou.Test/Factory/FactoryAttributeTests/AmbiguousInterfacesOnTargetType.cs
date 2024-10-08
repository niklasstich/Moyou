using Moyou.Aspects.Factory;

namespace Moyou.CompileTimeTest.Factory.FactoryAttributeTests;

[FactoryMember(TargetType = typeof(TypeC))]
public class FactoryC 
{
    
}

public class TypeC : InterfaceA, InterfaceB
{
    
}

public interface InterfaceA
{
    
}

public interface InterfaceB
{
    
}