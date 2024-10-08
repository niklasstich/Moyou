using NUnit.Framework;

namespace Moyou.UnitTest.Factory;

public class FactoryTest
{
    [Test]
    public void FactoryInstantiatesCorrectly()
    {
        var sut = new Factory();
        var typeA = sut.CreateTypeA(321);
        var typeB = sut.CreateTypeB();
        
        Assert.Multiple(() =>
        {
            Assert.That(typeA, Is.Not.Null);
            Assert.That(typeB, Is.Not.Null);
        });
        
        Assert.Multiple(() =>
        {
            Assert.That(typeA.Foo, Is.EqualTo(321));
            Assert.That(typeB.Bar, Is.EqualTo("foobar"));
        });
    }
}