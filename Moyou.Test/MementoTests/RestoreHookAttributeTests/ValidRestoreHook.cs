using Moyou.Aspects.Memento;

namespace Moyou.CompileTimeTest.MementoTests.RestoreHookAttributeTests;
[Memento]
internal class ValidRestoreHook
{
    [MementoRestoreHook]
    private void RestoreMementoHook(Memento memento)
    {
        memento.Foo = "bar";
    }

    private record Memento
    {
        public string Foo { get; set; }
    }
}
