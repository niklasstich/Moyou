using Moyou.Aspects.UnsavedChanges;

namespace Moyou.CompileTimeTest.UnsavedChanges.UnsavedChangesAttributeTest;

[UnsavedChanges]
public class NestedIEnumerable
{
    public IEnumerable<B> Bs { get; set; }
}

[UnsavedChanges]
public class B
{
    
}