using Moyou.Aspects.Memento;

namespace Moyou.CompileTimeTest.MementoTests.MementoIgnoreAttributeTests;

[Memento(StrictnessMode = StrictnessMode.Loose, MemberMode = MemberMode.FieldsOnly)]
public class IgnoreOnFieldsOnly
{
    [MementoIgnore]
    public string _ignoreMe;
    public string _dontIgnoreMe;
    
    public string IgnoreMeToo { get; set; }
    
    private record Memento
    {
        
    }
}