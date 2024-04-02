using Moyou.Aspects.Memento;
namespace Moyou.CompileTimeTest.MementoTests.MementoIgnoreAttributeTests;
[Memento(StrictnessMode = MementoStrictnessMode.Loose)]
public class IgnoreOneMember : global::Moyou.Aspects.Memento.IOriginator
{
  [MementoIgnore]
  public string IgnoreMe { get; set; }
  public string DontIgnoreMe { get; set; }
  private record Memento : global::Moyou.Aspects.Memento.IMemento
  {
    public global::System.String DontIgnoreMe;
  }
  public global::Moyou.Aspects.Memento.IMemento CreateMemento()
  {
    return (global::Moyou.Aspects.Memento.IMemento)this.CreateMementoImpl();
  }
  private global::Moyou.Aspects.Memento.IMemento CreateMementoImpl()
  {
    var memento = new global::Moyou.CompileTimeTest.MementoTests.MementoIgnoreAttributeTests.IgnoreOneMember.Memento();
    memento.DontIgnoreMe = this.DontIgnoreMe;
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
      var cast = ((global::Moyou.CompileTimeTest.MementoTests.MementoIgnoreAttributeTests.IgnoreOneMember.Memento)memento);
      this.DontIgnoreMe = ((global::Moyou.CompileTimeTest.MementoTests.MementoIgnoreAttributeTests.IgnoreOneMember.Memento)cast!).DontIgnoreMe;
    }
    catch (global::System.InvalidCastException icex)
    {
      throw new global::System.ArgumentException("Incorrect memento type", nameof(memento), icex);
    }
  }
}
