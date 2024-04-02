using Moyou.Aspects.Memento;

namespace Moyou.CompileTimeTest.MementoTests.CreateHookAttributeTests;
[Memento]
internal class HasTooManyParameters
{
    [MementoCreateHook]
    public void CreateMementoHook(int parameter1, int parameter2)
    {
    }

    private record Memento
    {

    }
}
