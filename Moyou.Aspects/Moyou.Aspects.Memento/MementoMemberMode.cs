using Metalama.Framework.Aspects;

namespace Moyou.Aspects.Memento;

/// <summary>
/// Defines which members of the target type should be included in the memento.
/// </summary>
[CompileTime]
public enum MementoMemberMode
{
    /// <summary>
    /// Include both properties and fields of the target type.
    /// </summary>
    All,
    /// <summary>
    /// Include only fields of the target type.
    /// </summary>
    FieldsOnly,
    /// <summary>
    /// Include only properties of the target type.
    /// </summary>
    PropertiesOnly
}