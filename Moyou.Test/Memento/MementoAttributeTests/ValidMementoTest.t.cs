using Moyou.Aspects.Memento;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS0169 // Field is never used
namespace Moyou.CompileTimeTest.MementoTests.MementoAttributeTests;
[Memento(StrictnessMode = MementoStrictnessMode.Loose)]
internal class ValidMementoTest : global::Moyou.Aspects.Memento.IOriginator
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
  public Dictionary<int, int> N { get; set; }
  private record Memento : global::Moyou.Aspects.Memento.IMemento
  {
    public global::System.Int32 A;
    public global::System.String B;
    public global::System.Int32 D;
    public global::System.Object E;
    public global::System.Collections.Generic.List<global::System.Object> L;
    public global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.CloneableDummy M;
    public global::System.Collections.Generic.Dictionary<global::System.Int32, global::System.Int32> N;
    public global::System.Object _e;
  }
  public global::Moyou.Aspects.Memento.IMemento CreateMemento()
  {
    return (global::Moyou.Aspects.Memento.IMemento)this.CreateMementoImpl();
  }
  private global::Moyou.Aspects.Memento.IMemento CreateMementoImpl()
  {
    var memento = new global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.ValidMementoTest.Memento();
    memento.D = this.D;
    memento._e = this._e;
    memento.A = this.A;
    memento.B = this.B;
    memento.E = this.E;
    memento.L = this.L.ToList();
    memento.M = ((global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.CloneableDummy)(this.M is not null ? this.M.Clone() : null));
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
      var cast = ((global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.ValidMementoTest.Memento)memento);
      this.D = ((global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.ValidMementoTest.Memento)cast!).D;
      this._e = ((global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.ValidMementoTest.Memento)cast!)._e;
      this.A = ((global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.ValidMementoTest.Memento)cast!).A;
      this.B = ((global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.ValidMementoTest.Memento)cast!).B;
      this.E = ((global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.ValidMementoTest.Memento)cast!).E;
      this.L = ((global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.ValidMementoTest.Memento)cast!).L;
      this.M = ((global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.ValidMementoTest.Memento)cast!).M;
      this.N = ((global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.ValidMementoTest.Memento)cast!).N;
    }
    catch (global::System.InvalidCastException icex)
    {
      throw new global::System.ArgumentException("Incorrect memento type", nameof(memento), icex);
    }
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
