using Moyou.Aspects.UnsavedChanges;

namespace Moyou.CompileTimeTest.UnsavedChanges.UnsavedChangesAttributeTest;

[UnsavedChanges]
public class NestedIEnumerable
{
    public IEnumerable<B> Bs { get; set; }
    public D D { get; set; }
}

[UnsavedChanges]
public class B
{
    
}

[UnsavedChanges]
public class D
{
    
}