using Moyou.Aspects.Memento;

namespace Moyou.UnitTest.Memento;

#pragma warning disable 0649
#pragma warning disable 8618
[Memento(StrictnessMode = MementoStrictnessMode.Loose)]
internal partial class MementoDummy
{
    public int A { get; set; }
    private string B { get; set; }
    internal string? C { get; init; }

    public int D;
    private object _e;

    public object E { get => _e; set => _e = value; }

    [MementoIgnore]
    public int F { get; set; }
    public int G { get; }
    public int H { get => 123; }
    public int I => 123;
    protected readonly object _j;
    public readonly object K;
    public List<object> L { get; set; }

    public CloneableDummy M { get; set; }
    public Dictionary<int,int> N { get; set; }
    public List<CloneableDummy> O { get; set; }


    [MementoIgnore]
    public string? Hook { get; set; }

    [MementoCreateHook]
    private void CreateHook(Memento memento)
    {
        memento.Hook = "hook set";
    }

    [MementoRestoreHook]
    private void RestoreHook(Memento memento)
    {
        this.Hook = memento.Hook + " and restored";
    }


    private record Memento
    {
        public string? Hook { get; set; }
    }
}

public class CloneableDummy : ICloneable
{
    public int Foo { get; set; }
    public object Clone()
    {
        return new CloneableDummy
        {
            Foo = Foo
        };
    }
}
#pragma warning restore 0649
#pragma warning restore 8618
