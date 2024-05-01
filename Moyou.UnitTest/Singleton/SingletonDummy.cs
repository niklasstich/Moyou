using Moyou.Aspects.Singleton;

namespace Moyou.UnitTest.Singleton;

[Singleton]
public partial class SingletonDummy
{
    public static bool ConstructorCalled { get; private set; }
    private SingletonDummy()
    {
        ConstructorCalled = true;
    }
}