using Moyou.Aspects.Memento;

namespace Moyou.CompileTimeTest.MementoTests.CreateHookAttributeTests;
[Memento]
internal class ValidCreateHook
{
    [MementoCreateHook]
    private void CreateMementoHook(Memento memento)
    {
        memento.Foo = "bar";
    }

    private record Memento
    {
        public string Foo { get; set; }
    }
}
