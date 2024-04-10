using Moyou.Aspects.Singleton;

namespace Moyou.UnitTest.Singleton;

[Singleton(Lazy = false)]
public partial class SingletonNonLazyDummy
{
    public static bool ConstructorCalled { get; private set; }

    private SingletonNonLazyDummy()
    {
        ConstructorCalled = true;
    }
}