using Moyou.Aspects.Memento;

namespace Moyou.CompileTimeTest.MementoTests.CreateHookAttributeTests;
[Memento]
internal class HasNoParameters
{
    [MementoCreateHook]
    public void CreateMementoHook()
    {
    }

    private record Memento
    {

    }
}
