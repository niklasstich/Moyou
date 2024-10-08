using Moyou.Aspects.Factory;

namespace Moyou.UnitTest.Factory;

public class TypeA : ITypeA
{
    public TypeA()
    {
        Foo = 123;
    }
    
    [FactoryConstructor]
    public TypeA(int foo)
    {
        Foo = foo;
    }
    
    public int Foo { get; set; }
}