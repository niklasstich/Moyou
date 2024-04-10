using Moyou.Aspects.Singleton;

namespace Moyou.CompileTimeTest.Singleton.SingletonAttributeTests;

[Singleton(Lazy = false)]
public class ValidNonLazySingletonTest
{
    private ValidNonLazySingletonTest()
    {
    }
}