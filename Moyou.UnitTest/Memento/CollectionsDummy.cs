using Moyou.Aspects.Memento;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace Moyou.UnitTest.Memento;

#pragma warning disable 0649
#pragma warning disable 8618

[Memento]
internal partial class CollectionsDummy
{
    //Copy via To[Type] methods
    public Dictionary<int, string> IntStringDict { get; set; }
    public List<string> StringList { get; set; }
    public List<int> IntList { get; set; }
    public HashSet<int> IntHashSet { get; set; }
    public string[] StringArray { get; set; }

    //immutable - assignment only
    public ReadOnlyCollection<string> ReadOnlyCollection { get; set; }
    public ReadOnlyDictionary<int, string> ReadOnlyDictionary { get; set; }
    public FrozenDictionary<int, int> FrozenDictionary { get; set; }
    public FrozenSet<int> FrozenHashSet { get; set; }
    public ImmutableList<int> ImmutableList { get; set; }
    public ImmutableHashSet<int> ImmutableHashSet { get; set; }
    public ImmutableArray<int> ImmutableArray { get; set; }
    public ImmutableDictionary<int, int> ImmutableDictionary { get; set; }
    public ImmutableSortedDictionary<int, int> ImmutableSortedDictionary { get; set; }
    public ImmutableSortedSet<int> ImmutableSortedSet { get; set; }
    public Lookup<int, string> Lookup { get; set; }


    private record Memento
    {
    }
}

#pragma warning restore 0649
#pragma warning restore 8618
