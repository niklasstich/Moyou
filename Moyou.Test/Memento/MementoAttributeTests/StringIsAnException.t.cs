using Moyou.Aspects.Memento;
namespace Moyou.CompileTimeTest.MementoTests.MementoAttributeTests;
[Memento]
public class StringIsAnException : global::Moyou.Aspects.Memento.IOriginator
{
  public string DontCloneMe { get; set; }
  private record Memento : global::Moyou.Aspects.Memento.IMemento
  {
    public global::System.String DontCloneMe;
  }
  public global::Moyou.Aspects.Memento.IMemento CreateMemento()
  {
    return (global::Moyou.Aspects.Memento.IMemento)this.CreateMementoImpl();
  }
  private global::Moyou.Aspects.Memento.IMemento CreateMementoImpl()
  {
    var memento = new global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.StringIsAnException.Memento();
    memento.DontCloneMe = this.DontCloneMe;
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
      var cast = ((global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.StringIsAnException.Memento)memento);
      this.DontCloneMe = ((global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.StringIsAnException.Memento)cast!).DontCloneMe;
    }
    catch (global::System.InvalidCastException icex)
    {
      throw new global::System.ArgumentException("Incorrect memento type", nameof(memento), icex);
    }
  }
}
