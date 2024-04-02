using Moyou.Aspects.Memento;

namespace Moyou.CompileTimeTest.MementoTests.MementoIgnoreAttributeTests;

[Memento(StrictnessMode = MementoStrictnessMode.Loose)]
public class IgnoreOneMember
{
    [MementoIgnore]
    public string IgnoreMe { get; set; }
    public string DontIgnoreMe { get; set; }
    
    private record Memento
    {
    }
}