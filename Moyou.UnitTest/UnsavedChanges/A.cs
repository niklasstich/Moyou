using Moyou.Aspects.UnsavedChanges;

namespace Moyou.UnitTest.UnsavedChanges;

[UnsavedChanges]
public partial class A
{
    public B B { get; set; }
    public IEnumerable<B?> Bs { get; set; }
    public IEnumerable<B>? Bs2 { get; set; }
    public void SetUnsavedChanges() => _internalUnsavedChanges = true;
}