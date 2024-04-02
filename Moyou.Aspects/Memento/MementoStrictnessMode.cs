using Metalama.Framework.Aspects;

namespace Moyou.Aspects.Memento;

/// <summary>
/// Defines the strictness of the memento aspect.
/// </summary>
[CompileTime]
public enum MementoStrictnessMode
{
    /// <summary>
    /// In this mode, when the memento attribute encounters a member that is neither a value type, a known default collection type nor implements <see cref="ICloneable"/>, it will simply assign the value of the original member to the memento member.
    /// Please note that for reference types, this means that the memento member will reference the same object as the original member and all modifications to the member will remain after restoring the memento.
    /// This is fine if the member is for example immutable or if the member itself supports the memento pattern.
    /// </summary>
    Loose,
    /// <summary>
    /// In this mode, the memento attribute will throw a warning when it encounters a member that is neither a value type, a known default collection type nor implements <see cref="ICloneable"/>.
    /// You must then either implement the <see cref="ICloneable"/> interface or mark the member with the <see cref="MementoIgnoreAttribute"/> and manage storing and restoring the state of the member manually in hook methods marked with the
    /// <see cref="MementoCreateHookAttribute"/> and <see cref="MementoRestoreHookAttribute"/>.
    /// </summary>
    Strict
}