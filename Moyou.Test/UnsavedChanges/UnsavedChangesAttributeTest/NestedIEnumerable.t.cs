using Moyou.Aspects.UnsavedChanges;
namespace Moyou.CompileTimeTest.UnsavedChanges.UnsavedChangesAttributeTest;
[UnsavedChanges]
public class NestedIEnumerable : global::Moyou.Aspects.UnsavedChanges.IUnsavedChanges
{
  public IEnumerable<B> Bs { get; set; }
  private global::System.Boolean _internalUnsavedChanges = (global::System.Boolean)false;
  public global::System.Boolean UnsavedChanges
  {
    get
    {
      return (global::System.Boolean)this.GetUnsavedChanges();
    }
  }
  private global::System.Boolean GetUnsavedChanges()
  {
    return (global::System.Boolean)(this._internalUnsavedChanges || global::System.Linq.Enumerable.Any(((global::System.Collections.Generic.IEnumerable<global::Moyou.Aspects.UnsavedChanges.IUnsavedChanges>)this.Bs), v_1 => v_1.UnsavedChanges));
  }
  public void ResetUnsavedChanges()
  {
    this._internalUnsavedChanges = false;
    foreach (var value in (global::System.Collections.Generic.IEnumerable<global::Moyou.Aspects.UnsavedChanges.IUnsavedChanges>)this.Bs)
    {
      value.ResetUnsavedChanges();
    }
  }
}
[UnsavedChanges]
public class B : global::Moyou.Aspects.UnsavedChanges.IUnsavedChanges
{
  private global::System.Boolean _internalUnsavedChanges = (global::System.Boolean)false;
  public global::System.Boolean UnsavedChanges
  {
    get
    {
      return (global::System.Boolean)this.GetUnsavedChanges();
    }
  }
  private global::System.Boolean GetUnsavedChanges()
  {
    return (global::System.Boolean)this._internalUnsavedChanges;
  }
  public void ResetUnsavedChanges()
  {
    this._internalUnsavedChanges = false;
  }
}