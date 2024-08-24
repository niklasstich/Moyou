using Metalama.Framework.Aspects;

namespace Moyou.Aspects.UnsavedChanges;

[RunTimeOrCompileTime]
public interface IUnsavedChanges
{
    bool UnsavedChanges { get; }
    void ResetUnsavedChanges();
}