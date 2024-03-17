using Moyou.Aspects.Memento;

namespace Moyou.CompileTimeTest.MementoTests.RestoreHookAttributeTests;

internal class NoMementoAttribute
{
    [MementoRestoreHook]
    public void RestoreMementoHook()
    {
    }
}
