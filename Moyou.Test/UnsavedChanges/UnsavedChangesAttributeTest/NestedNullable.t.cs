using Moyou.Aspects.UnsavedChanges;
namespace Moyou.CompileTimeTest.UnsavedChanges.UnsavedChangesAttributeTest;
[UnsavedChanges]
public class NestedNullable : global::Moyou.Aspects.UnsavedChanges.IUnsavedChanges
{
  public C C { get; set; }
  public C? CNullable { get; set; }
  public IEnumerable<C> CNotNullableEnumerable { get; set; }
  public IEnumerable<C?> CNotNullableEnumerableNullableT { get; set; }
  public IEnumerable<C>? CNullableEnumerable { get; set; }
  public IEnumerable<C?>? CNullableEnumerableNullableT { get; set; }
  public List<C>? CNullableList { get; set; }
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
    return (global::System.Boolean)(this._internalUnsavedChanges || this.C.UnsavedChanges || (this.CNullable?.UnsavedChanges ?? false) || global::System.Linq.Enumerable.Any(((global::System.Collections.Generic.IEnumerable<global::Moyou.Aspects.UnsavedChanges.IUnsavedChanges>)this.CNotNullableEnumerable), v_1 => v_1.UnsavedChanges) || global::System.Linq.Enumerable.Any(((global::System.Collections.Generic.IEnumerable<global::Moyou.Aspects.UnsavedChanges.IUnsavedChanges?>)this.CNotNullableEnumerableNullableT), v_2 => v_2?.UnsavedChanges ?? false) || CNullableEnumerable is null ? false : CNullableEnumerable.Any(v => v.UnsavedChanges) || CNullableEnumerableNullableT is null ? false : CNullableEnumerableNullableT.Any(v => v?.UnsavedChanges ?? false) || CNullableList is null ? false : CNullableList.Any(v => v.UnsavedChanges));
  }
  public void ResetUnsavedChanges()
  {
    this._internalUnsavedChanges = false;
    this.C.ResetUnsavedChanges();
    this.CNullable?.ResetUnsavedChanges();
    foreach (var val in this.CNotNullableEnumerable)
    {
      val.ResetUnsavedChanges();
    }
    foreach (var val_1 in this.CNotNullableEnumerableNullableT)
    {
      val_1?.ResetUnsavedChanges();
    }
    if (this.CNullableEnumerable is not null)
    {
      foreach (var val_2 in this.CNullableEnumerable!)
      {
        val_2.ResetUnsavedChanges();
      }
    }
    if (this.CNullableEnumerableNullableT is not null)
    {
      foreach (var val_3 in this.CNullableEnumerableNullableT!)
      {
        val_3?.ResetUnsavedChanges();
      }
    }
    if (this.CNullableList is not null)
    {
      foreach (var val_4 in this.CNullableList!)
      {
        val_4.ResetUnsavedChanges();
      }
    }
  }
}
[UnsavedChanges]
public class C : global::Moyou.Aspects.UnsavedChanges.IUnsavedChanges
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