using Moyou.Aspects.Memento;

namespace Moyou.CompileTimeTest.MementoTests.RestoreHookAttributeTests;
[Memento]
internal class HasTooManyParameters
{
    [MementoRestoreHook]
    public void RestoreMementoHook(int parameter1, int parameter2, int parameter3)
    {
    }

    private record Memento
    {

    }
}
