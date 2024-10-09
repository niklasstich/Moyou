using Moyou.Aspects.Memento;

namespace Moyou.UnitTest.Memento;

[Memento]
public partial class PartialClassWithoutNestedMementoClass
{
    public int A { get; set; }
    public int B { get; set; }
    
    [MementoCreateHook]
    private void CreateMemento(PartialClassWithoutNestedMementoClass.Memento memento)
    {
        memento.A = 456;
    }
    [MementoRestoreHook]
    private void RestoreMemento(PartialClassWithoutNestedMementoClass.Memento memento)
    {
        B = 666;
    }
}