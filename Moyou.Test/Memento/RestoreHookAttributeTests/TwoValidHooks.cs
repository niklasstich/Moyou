using Moyou.Aspects.Memento;

namespace Moyou.CompileTimeTest.MementoTests.RestoreHookAttributeTests;

[Memento]
internal class TwoValidHooks
{
    [MementoRestoreHook]
    private void RestoreMementoHook(Memento memento)
    {
        memento.Foo = "hook1";
    }

    [MementoRestoreHook]
    private void RestoreMementoHook2(Memento memento)
    {
        memento.Foo = "hook2";
    }

    private record Memento
    {
        public string Foo { get; set; }
    }
}
