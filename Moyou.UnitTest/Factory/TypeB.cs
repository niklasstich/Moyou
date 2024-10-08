namespace Moyou.UnitTest.Factory;

public class TypeB : ITypeB, ISomeOtherInterface
{
    public TypeB()
    {
        Bar = "foobar";
    }
    
    public string Bar { get; init; }
}