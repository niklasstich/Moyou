using Moyou.Aspects.UnsavedChanges;
namespace Moyou.CompileTimeTest.UnsavedChanges.UnsavedChangesAttributeTest;
[UnsavedChanges]
public class NoNesting : global::Moyou.Aspects.UnsavedChanges.IUnsavedChanges
{
  public string Foobar { get; set; }
  internal int _barfoo;
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