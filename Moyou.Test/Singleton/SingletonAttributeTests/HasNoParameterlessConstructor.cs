using Moyou.Aspects.Singleton;

namespace Moyou.CompileTimeTest.Singleton.SingletonAttributeTests;

[Singleton]
public class HasNoParameterlessConstructor
{
    private HasNoParameterlessConstructor(int a)
    {
        A = a;
    }

    public int A { get; set; }
}