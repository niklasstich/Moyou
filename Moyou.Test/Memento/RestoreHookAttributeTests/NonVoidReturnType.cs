using Moyou.Aspects.Memento;

namespace Moyou.CompileTimeTest.MementoTests.RestoreHookAttributeTests;

[Memento]
internal class NonVoidReturnType
{
    [MementoRestoreHook]
    public int RestoreMementoHook()
    {
        return 0;
    }

    private record Memento
    {
    }
}
