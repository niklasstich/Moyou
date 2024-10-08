using Moyou.Aspects.UnsavedChanges;

namespace Moyou.CompileTimeTest.UnsavedChanges.UnsavedChangesAttributeTest;

[UnsavedChanges]
public class NestedNullable
{
    public C C { get; set; }
    public C? CNullable { get; set; }
    public IEnumerable<C> CNotNullableEnumerable { get; set; }
    public IEnumerable<C?> CNotNullableEnumerableNullableT { get; set; }
    public IEnumerable<C>? CNullableEnumerable { get; set; }
    public IEnumerable<C?>? CNullableEnumerableNullableT { get; set; }
    public List<C>? CNullableList { get; set; }
}

[UnsavedChanges]
public class C
{
    
}