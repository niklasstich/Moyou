using Moyou.Aspects.Memento;

namespace Moyou.CompileTimeTest.MementoTests.MementoAttributeTests;

[Memento(MemberMode = MementoMemberMode.FieldsOnly, StrictnessMode = MementoStrictnessMode.Loose)]
public class FieldsOnly
{
    public int A { get; set; }
    private string B { get; set; }
    internal string? C { get; init; }

    public int D;
    private object _e;

    public object E { get => _e; set => _e = value; }

    public int F { get; set; }
    public int G { get; }
    public int H { get => 123; }
    public int I => 123;
    protected readonly object _j;
    public readonly object K;
    public List<object> L { get; set; }
    public Dictionary<int, int> N { get; set; }


    private record Memento
    {

    }
}