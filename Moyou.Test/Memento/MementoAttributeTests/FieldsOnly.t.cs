using Moyou.Aspects.Memento;
namespace Moyou.CompileTimeTest.MementoTests.MementoAttributeTests;
[Memento(MemberMode = MementoMemberMode.FieldsOnly, StrictnessMode = MementoStrictnessMode.Loose)]
public class FieldsOnly : global::Moyou.Aspects.Memento.IOriginator
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
    public global::System.Int32 D;
    public global::System.Object _e;
  }
  public global::Moyou.Aspects.Memento.IMemento CreateMemento()
  {
    return (global::Moyou.Aspects.Memento.IMemento)this.CreateMementoImpl();
  }
  private global::Moyou.Aspects.Memento.IMemento CreateMementoImpl()
  {
    var memento = new global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.FieldsOnly.Memento();
    memento.D = this.D;
    memento._e = this._e;
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
      var cast = ((global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.FieldsOnly.Memento)memento);
      this.D = ((global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.FieldsOnly.Memento)cast!).D;
      this._e = ((global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.FieldsOnly.Memento)cast!)._e;
    }
    catch (global::System.InvalidCastException icex)
    {
      throw new global::System.ArgumentException("Incorrect memento type", nameof(memento), icex);
    }
  }
}
