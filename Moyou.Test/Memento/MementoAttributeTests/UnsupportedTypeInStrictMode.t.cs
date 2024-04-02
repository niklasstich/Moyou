// Warning MOYOU1001 on `A`: `Member UnsupportedTypeInStrictMode.A of type object on type UnsupportedTypeInStrictMode is neither a value type nor implements ICloneable nor is a supported standard collection and MementoAttribute is set to Strict. Please either implement ICloneable on type object or mark the member with the MementoIgnoreAttribute and manage storing and restoring the state of the member manually in hook methods marked with the MementoCreateHookAttribute and MementoRestoreHookAttribute or alternatively mark UnsupportedTypeInStrictMode's Memento attribute as Loose to apply value-type assigning semantics to all unsupported members.`
using Moyou.Aspects.Memento;
namespace Moyou.CompileTimeTest.MementoTests.MementoAttributeTests;
[Memento]
internal class UnsupportedTypeInStrictMode : global::Moyou.Aspects.Memento.IOriginator
{
  public object A { get; set; }
  private record Memento : global::Moyou.Aspects.Memento.IMemento
  {
  }
  public global::Moyou.Aspects.Memento.IMemento CreateMemento()
  {
    return (global::Moyou.Aspects.Memento.IMemento)this.CreateMementoImpl();
  }
  private global::Moyou.Aspects.Memento.IMemento CreateMementoImpl()
  {
    var memento = new global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.UnsupportedTypeInStrictMode.Memento();
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
      var cast = ((global::Moyou.CompileTimeTest.MementoTests.MementoAttributeTests.UnsupportedTypeInStrictMode.Memento)memento);
    }
    catch (global::System.InvalidCastException icex)
    {
      throw new global::System.ArgumentException("Incorrect memento type", nameof(memento), icex);
    }
  }
}
