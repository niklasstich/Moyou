using Moyou.Aspects.UnsavedChanges;

namespace Moyou.UnitTest.UnsavedChanges;

[UnsavedChanges]
public partial class B
{
    public C C { get; set; }
    public C? C1 { get; set; }
    public IEnumerable<C> Cs { get; set; }
    public void SetUnsavedChanges() => _internalUnsavedChanges = true;
}