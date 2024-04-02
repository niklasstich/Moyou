using Moyou.Aspects.Memento;
namespace Moyou.CompileTimeTest.MementoTests.MementoIgnoreAttributeTests;
[Memento(StrictnessMode = MementoStrictnessMode.Loose, MemberMode = MementoMemberMode.FieldsOnly)]
public class IgnoreOnFieldsOnly : global::Moyou.Aspects.Memento.IOriginator
{
  [MementoIgnore]
  public string _ignoreMe;
  public string _dontIgnoreMe;
  public string IgnoreMeToo { get; set; }
  private record Memento : global::Moyou.Aspects.Memento.IMemento
  {
    public global::System.String _dontIgnoreMe;
  }
  public global::Moyou.Aspects.Memento.IMemento CreateMemento()
  {
    return (global::Moyou.Aspects.Memento.IMemento)this.CreateMementoImpl();
  }
  private global::Moyou.Aspects.Memento.IMemento CreateMementoImpl()
  {
    var memento = new global::Moyou.CompileTimeTest.MementoTests.MementoIgnoreAttributeTests.IgnoreOnFieldsOnly.Memento();
    memento._dontIgnoreMe = this._dontIgnoreMe;
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
      var cast = ((global::Moyou.CompileTimeTest.MementoTests.MementoIgnoreAttributeTests.IgnoreOnFieldsOnly.Memento)memento);
      this._dontIgnoreMe = ((global::Moyou.CompileTimeTest.MementoTests.MementoIgnoreAttributeTests.IgnoreOnFieldsOnly.Memento)cast!)._dontIgnoreMe;
    }
    catch (global::System.InvalidCastException icex)
    {
      throw new global::System.ArgumentException("Incorrect memento type", nameof(memento), icex);
    }
  }
}
