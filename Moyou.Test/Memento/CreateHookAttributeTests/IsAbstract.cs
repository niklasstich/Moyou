using Moyou.Aspects.Memento;

namespace Moyou.CompileTimeTest.MementoTests.CreateHookAttributeTests;
[Memento]
internal abstract class IsAbstract
{
    [MementoCreateHook]
    public abstract void CreateMementoHook();

    private record Memento
    {

    }
}
