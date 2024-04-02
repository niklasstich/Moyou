using Moyou.Aspects.Memento;
namespace Moyou.CompileTimeTest.MementoTests.CreateHookAttributeTests;
[Memento]
internal class TwoValidHooks : global::Moyou.Aspects.Memento.IOriginator
{
  [MementoCreateHook]
  private void CreateMementoHook(Memento memento)
  {
    memento.Foo = "hook1";
  }
  [MementoCreateHook]
  private void CreateMementoHook2(Memento memento)
  {
    memento.Foo = "hook2";
  }
  private record Memento : global::Moyou.Aspects.Memento.IMemento
  {
    public string Foo { get; set; }
  }
  public global::Moyou.Aspects.Memento.IMemento CreateMemento()
  {
    global::Moyou.Aspects.Memento.IMemento memento_1;
    global::Moyou.Aspects.Memento.IMemento memento;
    memento = (global::Moyou.Aspects.Memento.IMemento)this.CreateMementoImpl();
    this.CreateMementoHook((global::Moyou.CompileTimeTest.MementoTests.CreateHookAttributeTests.TwoValidHooks.Memento)memento);
    memento_1 = (global::Moyou.Aspects.Memento.IMemento)memento!;
    this.CreateMementoHook2((global::Moyou.CompileTimeTest.MementoTests.CreateHookAttributeTests.TwoValidHooks.Memento)memento_1);
    return (global::Moyou.Aspects.Memento.IMemento)memento_1!;
  }
  private global::Moyou.Aspects.Memento.IMemento CreateMementoImpl()
  {
    var memento = new global::Moyou.CompileTimeTest.MementoTests.CreateHookAttributeTests.TwoValidHooks.Memento();
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
      var cast = ((global::Moyou.CompileTimeTest.MementoTests.CreateHookAttributeTests.TwoValidHooks.Memento)memento);
    }
    catch (global::System.InvalidCastException icex)
    {
      throw new global::System.ArgumentException("Incorrect memento type", nameof(memento), icex);
    }
  }
}
