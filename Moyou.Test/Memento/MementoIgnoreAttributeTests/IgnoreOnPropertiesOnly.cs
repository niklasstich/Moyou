using Moyou.Aspects.Memento;

namespace Moyou.CompileTimeTest.MementoTests.MementoIgnoreAttributeTests;

[Memento(StrictnessMode = MementoStrictnessMode.Loose, MemberMode = MementoMemberMode.PropertiesOnly)]
public class IgnoreOnPropertiesOnly
{
    [MementoIgnore]
    public string IgnoreMe { get; set; }
    public string DontIgnoreMe { get; set; }
    
    public string _ignoreMeToo;
    
    private record Memento
    {
    }
}