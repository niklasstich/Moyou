using Moyou.Aspects.Memento;
namespace Moyou.CompileTimeTest.MementoTests.MementoAttributeTests;
[Memento(MemberMode = MementoMemberMode.PropertiesOnly, StrictnessMode = MementoStrictnessMode.Loose)]
public class PropertiesOnly : global::Moyou.Aspects.Memento.IOriginator
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
  private record Memento : global::Moyou.Aspects.Memento.IMemento
  {
    public global::System.Int32 A;
    public global::System.String B;
    public global::System.Object E;
    public global::System.Int32 F;
    public global::System.Collections.Generic.List<global::System.Object> L;
    public global::System.Collections.Generic.Dictionary<global::System.Int32, global::System.Int32> N;
  }
  public global::Moyou.Aspects.Memento.IMemento CreateMemento()
  {
    return (global::Moyou.Aspects.Memento.IMemento)this.CreateMementoImpl();
  }
  private global::Moyou.Aspects.Memento.IMemento CreateMementoImpl()
  {
    var memento = new global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.PropertiesOnly.Memento();
    memento.A = this.A;
    memento.B = this.B;
    memento.E = this.E;
    memento.F = this.F;
    memento.L = this.L.ToList();
    memento.N = this.N.ToDictionary();
    return (global::Moyou.Aspects.Memento.IMemento)memento;
  }
  public void RestoreMemento(global::Moyou.Aspects.Memento.IMemento memento)
  {
    this.RestoreMementoImpl(memento);
  }
  private void RestoreMementoImpl(global::Moyou.Aspects.Memento.IMemento memento)
  {
    try
    {
      var cast = ((global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.PropertiesOnly.Memento)memento);
      this.A = ((global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.PropertiesOnly.Memento)cast!).A;
      this.B = ((global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.PropertiesOnly.Memento)cast!).B;
      this.E = ((global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.PropertiesOnly.Memento)cast!).E;
      this.F = ((global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.PropertiesOnly.Memento)cast!).F;
      this.L = ((global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.PropertiesOnly.Memento)cast!).L;
      this.N = ((global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.PropertiesOnly.Memento)cast!).N;
    }
    catch (global::System.InvalidCastException icex)
    {
      throw new global::System.ArgumentException("Incorrect memento type", nameof(memento), icex);
    }
  }
}
