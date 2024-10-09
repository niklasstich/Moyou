using NUnit.Framework;

namespace Moyou.UnitTest.Memento;

[TestFixture]
public class PartialClassWithoutNestedMementoClassTest
{
    [Test]
    public void CreateMementoHookIsCalled()
    {
        var sut = new PartialClassWithoutNestedMementoClass
        {
            A = 123
        };
        var memento = sut.CreateMemento();
        
        Assert.That(sut.A, Is.EqualTo(123));
        sut.RestoreMemento(memento);
        Assert.That(sut.A, Is.EqualTo(456));
    }
    
    [Test]
    public void RestoreMementoHookIsCalled()
    {
        var sut = new PartialClassWithoutNestedMementoClass
        {
            B = 123
        };
        var memento = sut.CreateMemento();
        
        Assert.That(sut.B, Is.EqualTo(123));
        sut.RestoreMemento(memento);
        Assert.That(sut.B, Is.EqualTo(666));
    }
}