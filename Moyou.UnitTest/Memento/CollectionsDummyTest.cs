using NUnit.Framework;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace Moyou.UnitTest.Memento;

[TestFixture]
internal class CollectionsDummyTest
{
    [Test]
    public void TestCollectionsDummy_CreateMementoAndRestoreMemento_RestoresExpectedState()
    {
        // Arrange
        var intStringDict = new Dictionary<int, string> { { 1, "one" }, { 2, "two" } };
        var stringList = new List<string> { "one", "two" };
        var intList = new List<int> { 1, 2 };
        var intHashSet = new HashSet<int> { 1, 2 };
        string[] stringArray = ["one", "two"];

        var readOnlyCollection = new ReadOnlyCollection<string>(stringList);
        var readOnlyDictionary = new ReadOnlyDictionary<int, string>(intStringDict);
        var frozenDictionary = new Dictionary<int, int> { { 1, 1 }, { 2, 2 } }.ToFrozenDictionary();
        var frozenHashSet = intHashSet.ToFrozenSet();
        ImmutableList<int> immutableList = [1, 2];
        ImmutableHashSet<int> immutableHashSet = [1, 2];
        ImmutableArray<int> immutableArray = [1, 2];
        var immutableDictionary = ImmutableDictionary.Create<int, int>();
        var immutableSortedDictionary = ImmutableSortedDictionary.Create<int, int>();
        ImmutableSortedSet<int> immutableSortedSet = [1, 2];
        var lookup = (Lookup<int, string>)stringList.ToLookup(str => str.Length);
        var collectionsDummy = new CollectionsDummy
        {
            IntStringDict = intStringDict,
            StringList = stringList,
            IntList = intList,
            IntHashSet = intHashSet,
            StringArray = stringArray,

            ReadOnlyCollection = readOnlyCollection,
            ReadOnlyDictionary = readOnlyDictionary,
            FrozenDictionary = frozenDictionary,
            FrozenHashSet = frozenHashSet,
            ImmutableList = immutableList,
            ImmutableHashSet = immutableHashSet,
            ImmutableArray = immutableArray,
            ImmutableDictionary = immutableDictionary,
            ImmutableSortedDictionary = immutableSortedDictionary,
            ImmutableSortedSet = immutableSortedSet,
            Lookup = lookup
        };

        // Act
        var memento = collectionsDummy.CreateMemento();
        collectionsDummy.IntStringDict[3] = "drei";
        collectionsDummy.StringList.Add("foobar");
        collectionsDummy.IntList.Add(1230);
        collectionsDummy.IntHashSet.Add(345);
        collectionsDummy.StringArray[0] = "three";

        collectionsDummy.ReadOnlyCollection = new ReadOnlyCollection<string>(new List<string> { "three", "four" });
        collectionsDummy.ReadOnlyDictionary = new ReadOnlyDictionary<int, string>(new Dictionary<int, string> { { 3, "three" }, { 4, "four" } });
        collectionsDummy.FrozenDictionary = new Dictionary<int, int>().ToFrozenDictionary();
        collectionsDummy.FrozenHashSet = new HashSet<int>().ToFrozenSet();
        collectionsDummy.ImmutableList = ImmutableList<int>.Empty;
        collectionsDummy.ImmutableHashSet = ImmutableHashSet<int>.Empty;
        collectionsDummy.ImmutableArray = ImmutableArray<int>.Empty;
        collectionsDummy.ImmutableDictionary = ImmutableDictionary<int, int>.Empty;
        collectionsDummy.ImmutableSortedDictionary = ImmutableSortedDictionary<int, int>.Empty;
        collectionsDummy.ImmutableSortedSet = ImmutableSortedSet<int>.Empty;
        collectionsDummy.Lookup = (Lookup<int, string>)new List<string>{ "three", "four" }.ToLookup(str => str.Length);

        collectionsDummy.RestoreMemento(memento);

        // Assert
        Assert.Multiple(() =>
        {
            // Assert that the original state was restored
            Assert.That(collectionsDummy.IntStringDict, Is.EquivalentTo(new Dictionary<int, string> { { 1, "one" }, { 2, "two" } }));
            Assert.That(collectionsDummy.StringList, Is.EquivalentTo(new List<string> { "one", "two" }));
            Assert.That(collectionsDummy.IntList, Is.EquivalentTo(new List<int> { 1, 2 }));
            Assert.That(collectionsDummy.IntHashSet, Is.EquivalentTo(new HashSet<int> { 1, 2 }));
            Assert.That(collectionsDummy.StringArray, Is.EquivalentTo(new string[] { "one", "two" }));
            // Assert that the original object was restored
            Assert.That(collectionsDummy.ReadOnlyCollection, Is.EqualTo(readOnlyCollection));
            Assert.That(collectionsDummy.ReadOnlyDictionary, Is.EqualTo(readOnlyDictionary));
            Assert.That(collectionsDummy.FrozenDictionary, Is.EqualTo(frozenDictionary));
            Assert.That(collectionsDummy.FrozenHashSet, Is.EqualTo(frozenHashSet));
            Assert.That(collectionsDummy.ImmutableList, Is.EqualTo(immutableList));
            Assert.That(collectionsDummy.ImmutableHashSet, Is.EqualTo(immutableHashSet));
            Assert.That(collectionsDummy.ImmutableArray, Is.EqualTo(immutableArray));
            Assert.That(collectionsDummy.ImmutableDictionary, Is.EqualTo(immutableDictionary));
            Assert.That(collectionsDummy.ImmutableSortedDictionary, Is.EqualTo(immutableSortedDictionary));
            Assert.That(collectionsDummy.ImmutableSortedSet, Is.EqualTo(immutableSortedSet));
            Assert.That(collectionsDummy.Lookup, Is.EqualTo(lookup));
        });


    }
}
