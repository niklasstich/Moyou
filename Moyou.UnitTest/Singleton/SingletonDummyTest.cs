using NUnit.Framework;

namespace Moyou.UnitTest.Singleton;

[TestFixture]
public class SingletonDummyTest
{
    [Test]
    public void ConstructorCalledLazily()
    {
        Assert.That(SingletonDummy.ConstructorCalled, Is.False);
        
        var instance = SingletonDummy.Instance;
        
        Assert.That(SingletonDummy.ConstructorCalled, Is.True);
    }
}