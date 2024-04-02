using Moyou.Aspects.Memento;

namespace Moyou.CompileTimeTest.MementoTests.MementoAttributeTests;

[Memento]
internal class UnsupportedTypeInStrictMode
{
    public object A { get; set; }
    private record Memento
    {

    }
}
