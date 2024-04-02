using Moyou.Aspects.Memento;

namespace Moyou.CompileTimeTest.MementoTests.MementoAttributeTests;

[Memento]
public class StringIsAnException
{
    public string DontCloneMe { get; set; }
    private record Memento
    {
    }
}