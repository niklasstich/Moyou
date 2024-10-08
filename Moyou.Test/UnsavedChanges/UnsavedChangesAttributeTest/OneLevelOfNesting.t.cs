using Moyou.Aspects.UnsavedChanges;
namespace Moyou.CompileTimeTest.UnsavedChanges.UnsavedChangesAttributeTest;
[UnsavedChanges]
public class OneLevelOfNesting : global::Moyou.Aspects.UnsavedChanges.IUnsavedChanges
{
  public A A { get; set; }
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
    return (global::System.Boolean)(this._internalUnsavedChanges || this.A.UnsavedChanges);
  }
  public void ResetUnsavedChanges()
  {
    this._internalUnsavedChanges = false;
    this.A.ResetUnsavedChanges();
  }
}
[UnsavedChanges]
public class A : global::Moyou.Aspects.UnsavedChanges.IUnsavedChanges
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