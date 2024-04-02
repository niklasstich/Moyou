using Moyou.Aspects.Memento;
namespace Moyou.CompileTimeTest.MementoTests.MementoIgnoreAttributeTests;
[Memento(StrictnessMode = MementoStrictnessMode.Loose)]
public class IgnoreAllMembers : global::Moyou.Aspects.Memento.IOriginator
{
  [MementoIgnore]
  public string IgnoreMe { get; set; }
  [MementoIgnore]
  public string IgnoreMeToo { get; set; }
  private record Memento : global::Moyou.Aspects.Memento.IMemento
  {
  }
  public global::Moyou.Aspects.Memento.IMemento CreateMemento()
  {
    return (global::Moyou.Aspects.Memento.IMemento)this.CreateMementoImpl();
  }
  private global::Moyou.Aspects.Memento.IMemento CreateMementoImpl()
  {
    var memento = new global::Moyou.CompileTimeTest.MementoTests.MementoIgnoreAttributeTests.IgnoreAllMembers.Memento();
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
      var cast = ((global::Moyou.CompileTimeTest.MementoTests.MementoIgnoreAttributeTests.IgnoreAllMembers.Memento)memento);
    }
    catch (global::System.InvalidCastException icex)
    {
      throw new global::System.ArgumentException("Incorrect memento type", nameof(memento), icex);
    }
  }
}
