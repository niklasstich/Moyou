// Warning MOYOU1002 on `NoMementoNestedClass`: `Class NoMementoNestedClass does not have a nested class named Memento. One will be automatically generated. Please note that if you plan on using MementoCreateHook and MementoRestoreHook attributes, you need to either define the nested class yourself or use a fully qualified name (e.g. NoMementoNestedClass.Memento) in the signature of your hook methods.`
using Moyou.Aspects.Memento;
namespace Moyou.CompileTimeTest.MementoTests.MementoAttributeTests;
[Memento]
internal class NoMementoNestedClass : global::Moyou.Aspects.Memento.IOriginator
{
  string A { get; set; }
  public global::Moyou.Aspects.Memento.IMemento CreateMemento()
  {
    return (global::Moyou.Aspects.Memento.IMemento)this.CreateMementoImpl();
  }
  private global::Moyou.Aspects.Memento.IMemento CreateMementoImpl()
  {
    var memento = new global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.NoMementoNestedClass.Memento();
    memento.A = this.A;
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
      var cast = ((global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.NoMementoNestedClass.Memento)memento);
      this.A = ((global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.NoMementoNestedClass.Memento)cast!).A;
    }
    catch (global::System.InvalidCastException icex)
    {
      throw new global::System.ArgumentException("Incorrect memento type", nameof(memento), icex);
    }
  }
  class Memento : global::Moyou.Aspects.Memento.IMemento
  {
    public global::System.String A;
  }
}