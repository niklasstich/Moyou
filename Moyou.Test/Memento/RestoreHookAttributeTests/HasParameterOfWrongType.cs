using Moyou.Aspects.Memento;

namespace Moyou.CompileTimeTest.MementoTests.RestoreHookAttributeTests;
[Memento]
internal class HasParameterOfWrongType
{
    [MementoRestoreHook]
    public void RestoreMementoHook(int parameter)
    {
    }

    private record Memento
    {

    }
}
