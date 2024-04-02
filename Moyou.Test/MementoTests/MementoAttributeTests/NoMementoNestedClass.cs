using Moyou.Aspects.Memento;

namespace Moyou.CompileTimeTest.MementoTests.MementoAttributeTests;

[Memento]
internal class NoMementoNestedClass
{
    string A { get; set; }
}
