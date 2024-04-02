using Moyou.Aspects.Memento;

namespace Moyou.CompileTimeTest.MementoTests.RestoreHookAttributeTests;

[Memento]
internal abstract class IsAbstract
{
    [MementoRestoreHook]
    public abstract void RestoreMementoHook();

    private record Memento
    {

    }
}
