using Metalama.Framework.Aspects;

namespace Moyou.Aspects.UnsavedChanges;

[RunTimeOrCompileTime]
public interface IUnsavedChanges
{
    /// <summary>
    /// True if this object or any child object has unsaved changes.
    /// </summary>
    bool UnsavedChanges { get; }
    /// <summary>
    /// Resets unsaved changes in this object and all child objects that implement <see cref="IUnsavedChanges"/>.
    /// </summary>
    void ResetUnsavedChanges();
}