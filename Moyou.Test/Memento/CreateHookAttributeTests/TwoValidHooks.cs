using Moyou.Aspects.Memento;

namespace Moyou.CompileTimeTest.MementoTests.CreateHookAttributeTests;
[Memento]
internal class TwoValidHooks
{
    [MementoCreateHook]
    private void CreateMementoHook(Memento memento)
    {
        memento.Foo = "hook1";
    }

    [MementoCreateHook]
    private void CreateMementoHook2(Memento memento)
    {
        memento.Foo = "hook2";
    }

    private record Memento
    {
        public string Foo { get; set; }
    }
}
