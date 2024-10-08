using Moyou.Aspects.Factory;

namespace Moyou.CompileTimeTest.Factory.FactoryAttributeTests;

[Factory]
[FactoryMember(TargetType = typeof(ClassE))]
public class FactoryE
{
    
}

public interface InterfaceE
{
    
}

public class ClassE : InterfaceE
{
    public ClassE()
    {
        
    }
    
    [FactoryConstructor]
    public ClassE(int foobar, string barbaz, object bobo)
    {
        
    }
}