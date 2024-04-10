using NUnit.Framework;

namespace Moyou.UnitTest.Singleton;

[TestFixture]
public class SingletonNonLazyDummyTest
{
    [Test]
    public void ConstructorCalledNonLazily()
    {
        Assert.That(SingletonNonLazyDummy.ConstructorCalled, Is.True);
        
        var instance = SingletonNonLazyDummy.Instance;
        
        Assert.That(SingletonNonLazyDummy.ConstructorCalled, Is.True);
    }
}