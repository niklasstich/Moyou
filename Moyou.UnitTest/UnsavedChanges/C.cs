using Moyou.Aspects.UnsavedChanges;

namespace Moyou.UnitTest.UnsavedChanges;

[UnsavedChanges]
public partial class C
{
    private int Foobar { get; set; }
    public void SetUnsavedChanges() => _internalUnsavedChanges = true;
}