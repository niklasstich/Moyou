using Moyou.Aspects.UnsavedChanges;

namespace Moyou.CompileTimeTest.UnsavedChanges.UnsavedChangesAttributeTest;

[UnsavedChanges]
public class NoNesting
{
    public string Foobar { get; set; }
    internal int _barfoo;
}