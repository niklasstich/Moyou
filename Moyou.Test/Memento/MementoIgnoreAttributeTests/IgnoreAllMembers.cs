using Moyou.Aspects.Memento;

namespace Moyou.CompileTimeTest.MementoTests.MementoIgnoreAttributeTests;

[Memento(StrictnessMode = MementoStrictnessMode.Loose)]
public class IgnoreAllMembers
{
    [MementoIgnore]
    public string IgnoreMe { get; set; }
    [MementoIgnore]
    public string IgnoreMeToo { get; set; }
    
    private record Memento
    {
    }
}