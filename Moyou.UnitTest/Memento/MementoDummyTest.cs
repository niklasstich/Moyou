using NUnit.Framework;

namespace Moyou.UnitTest.Memento;

[TestFixture]
public class MementoDummyTest
{
    [Test]
    public void TestMementoDummy_CreateMementoAndRestoreMemento_RestoresExpectedState()
    {
        // Arrange
        var initialObj = new object();
        var item2 = new object();
        var enumerable = new List<object> { initialObj, item2 };
        var initialCloneable = new CloneableDummy { Foo = 1 };
        var enumerable2 = new List<CloneableDummy> { initialCloneable };
        var mementoDummy = new MementoDummy
        {
            A = 1,
            C = "test",
            D = 2,
            E = initialObj,
            F = 2,
            L = enumerable,
            M = initialCloneable,
            N = new Dictionary<int, int>{{1, 123}, {2, 222}},
            O = enumerable2
        };

        //double check that values are as expected before changing them 
        Assert.Multiple(() =>
        {
            Assert.That(mementoDummy.A, Is.EqualTo(1));
            Assert.That(mementoDummy.C, Is.EqualTo("test"));
            Assert.That(mementoDummy.D, Is.EqualTo(2));
            Assert.That(mementoDummy.E, Is.EqualTo(initialObj));
            Assert.That(mementoDummy.F, Is.EqualTo(2));
            Assert.That(mementoDummy.L, Has.Count.EqualTo(2));
            Assert.That(mementoDummy.L[0], Is.EqualTo(initialObj));
            Assert.That(mementoDummy.L[1], Is.EqualTo(item2));
            Assert.That(mementoDummy.M.Foo, Is.EqualTo(1));
            Assert.That(mementoDummy.M, Is.EqualTo(initialCloneable));
            Assert.That(mementoDummy.N, Is.EquivalentTo(new Dictionary<int, int> { { 1, 123 }, { 2, 222 } }));
            Assert.That(mementoDummy.O, Has.Count.EqualTo(1));
            Assert.That(mementoDummy.O[0], Is.EqualTo(initialCloneable));
            Assert.That(mementoDummy.Hook, Is.Null);
        });

        // Act
        //get memento
        var memento = mementoDummy.CreateMemento();
        //change all properties, including ignored ones
        mementoDummy.A = 3;
        var newObj = new object();
        mementoDummy.E = newObj;
        mementoDummy.F = 4;
        mementoDummy.L.RemoveAll(_ => true);
        mementoDummy.L.Add(new object());
        mementoDummy.N[1] = 321;
        mementoDummy.N.Remove(2);
        mementoDummy.M.Foo = 2;
        mementoDummy.O.Clear();
        

        mementoDummy.RestoreMemento(memento);


        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(mementoDummy.A, Is.EqualTo(1));
            Assert.That(mementoDummy.C, Is.EqualTo("test"));
            Assert.That(mementoDummy.D, Is.EqualTo(2));
            Assert.That(mementoDummy.E, Is.EqualTo(initialObj));
            Assert.That(mementoDummy.F, Is.EqualTo(4));
            Assert.That(mementoDummy.L, Has.Count.EqualTo(2));
            Assert.That(mementoDummy.L[0], Is.EqualTo(initialObj));
            Assert.That(mementoDummy.L[1], Is.EqualTo(item2));
            Assert.That(mementoDummy.M.Foo, Is.EqualTo(1));
            Assert.That(mementoDummy.M, Is.Not.EqualTo(initialCloneable));
            Assert.That(mementoDummy.N, Is.EquivalentTo(new Dictionary<int, int> { { 1, 123 }, { 2, 222 } }));
            Assert.That(mementoDummy.O, Has.Count.EqualTo(1));
            Assert.That(mementoDummy.O[0], Is.EqualTo(initialCloneable));
            Assert.That(mementoDummy.Hook, Is.EqualTo("hook set and restored"));
        });
    }
}