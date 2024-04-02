using Moyou.Aspects.Memento;
namespace Moyou.CompileTimeTest.MementoTests.RestoreHookAttributeTests;
[Memento]
internal class ValidRestoreHook : global::Moyou.Aspects.Memento.IOriginator
{
  [MementoRestoreHook]
  private void RestoreMementoHook(Memento memento)
  {
    memento.Foo = "bar";
  }
  private record Memento : global::Moyou.Aspects.Memento.IMemento
  {
    public string Foo { get; set; }
  }
  public global::Moyou.Aspects.Memento.IMemento CreateMemento()
  {
    return (global::Moyou.Aspects.Memento.IMemento)this.CreateMementoImpl();
  }
  private global::Moyou.Aspects.Memento.IMemento CreateMementoImpl()
  {
    var memento = new global::Moyou.CompileTimeTest.MementoTests.RestoreHookAttributeTests.ValidRestoreHook.Memento();
    return (global::Moyou.Aspects.Memento.IMemento)memento;
  }
  public void RestoreMemento(global::Moyou.Aspects.Memento.IMemento memento)
  {
    this.RestoreMementoImpl(memento);
    var memento_1 = memento;
    this.RestoreMementoHook((global::Moyou.CompileTimeTest.MementoTests.RestoreHookAttributeTests.ValidRestoreHook.Memento)memento_1);
    return;
  }
  private void RestoreMementoImpl(global::Moyou.Aspects.Memento.IMemento memento)
  {
    try
    {
      var cast = ((global::Moyou.CompileTimeTest.MementoTests.RestoreHookAttributeTests.ValidRestoreHook.Memento)memento);
    }
    catch (global::System.InvalidCastException icex)
    {
      throw new global::System.ArgumentException("Incorrect memento type", nameof(memento), icex);
    }
  }
}
