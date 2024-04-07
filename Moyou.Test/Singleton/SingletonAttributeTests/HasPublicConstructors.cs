using Moyou.Aspects.Singleton;

namespace Moyou.CompileTimeTest.Singleton.SingletonAttributeTests;

[Singleton]
public class HasPublicConstructors
{
    private HasPublicConstructors()
    {
        A = 1;
    }
    
    public HasPublicConstructors(int a)
    {
        A = a;
    }

    public int A { get; set; }
}