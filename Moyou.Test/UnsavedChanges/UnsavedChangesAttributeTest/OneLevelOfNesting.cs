using Moyou.Aspects.UnsavedChanges;

namespace Moyou.CompileTimeTest.UnsavedChanges.UnsavedChangesAttributeTest;

[UnsavedChanges]
public class OneLevelOfNesting
{
    public A A { get; set; }
}

[UnsavedChanges]
public class A
{
    
}