using Moyou.Aspects.Factory;

namespace Moyou.CompileTimeTest.Factory.FactoryAttributeTests;

[FactoryMember(TargetType = typeof(TypeB))]
public class FactoryB
{
    
}

public class TypeB 
{
    
}