using Moyou.Aspects.Memento;

namespace Moyou.CompileTimeTest.MementoTests.CreateHookAttributeTests;
[Memento]
internal class NonVoidReturnType
{
    [MementoCreateHook]
    public int CreateMementoHook()
    {
        return 0;
    }

    private record Memento
    {
    }
}
