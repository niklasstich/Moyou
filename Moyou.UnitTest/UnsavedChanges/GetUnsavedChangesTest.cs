using NUnit.Framework;

namespace Moyou.UnitTest.UnsavedChanges;

[TestFixture]
public class GetUnsavedChangesTest
{
    [Test]
    public void UnsavedChanges_Directly()
    {
        var sut = UnsavedChangesTestHelper.GetFullStructure();
        
        Assert.That(sut.UnsavedChanges, Is.False);
        sut.SetUnsavedChanges();
        Assert.That(sut.UnsavedChanges, Is.True);
    }

    [Test]
    public void UnsavedChanges_OneNesting_EnumerableNullableT()
    {
        var sut = UnsavedChangesTestHelper.GetFullStructure();
        
        Assert.That(sut.UnsavedChanges, Is.False);
        sut.Bs.ElementAt(1)!.SetUnsavedChanges();
        Assert.That(sut.UnsavedChanges, Is.True);
    }

    [Test]
    public void UnsavedChanges_OneNesting_TypeDirectly()
    {
        var sut = UnsavedChangesTestHelper.GetFullStructure();
        
        Assert.That(sut.UnsavedChanges, Is.False);
        sut.B.SetUnsavedChanges();
        Assert.That(sut.UnsavedChanges, Is.True);
    }

    [Test]
    public void UnsavedChanges_TwoNesting_TypeDirectly()
    {
        var sut = UnsavedChangesTestHelper.GetFullStructure();
        
        Assert.That(sut.UnsavedChanges, Is.False);
        sut.Bs.ElementAt(1)!.C.SetUnsavedChanges();
        Assert.That(sut.UnsavedChanges, Is.True);
    }

    [Test]
    public void UnsavedChanges_TwoNesting_TypeNullable()
    {
        var sut = UnsavedChangesTestHelper.GetFullStructure();
        
        Assert.That(sut.UnsavedChanges, Is.False);
        sut.Bs.ElementAt(1)!.C1.SetUnsavedChanges();
        Assert.That(sut.UnsavedChanges, Is.True);
    }

    [Test]
    [TestCase(0)]
    [TestCase(1)]
    public void UnsavedChanges_TwoNesting_Enumerable(int index)
    {
        var sut = UnsavedChangesTestHelper.GetFullStructure();
        
        Assert.That(sut.UnsavedChanges, Is.False);
        sut.Bs.ElementAt(1)!.Cs.ElementAt(index).SetUnsavedChanges();
        Assert.That(sut.UnsavedChanges, Is.True);
    }

}