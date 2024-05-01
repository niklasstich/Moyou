using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Diagnostics;

namespace Moyou.Diagnostics;

/// <summary>
/// Warning diagnostics for Moyou. IDs MOYOU1xyy where x is the category and yy is the number.
/// </summary>
[CompileTime]
public static class Warnings
{
    /// <summary>
    /// Memento - MOYOU10xx
    /// </summary>
    public static class Memento
    {
        // MOYOU1001: Member {0} of type {1} on type {2} is neither a value type nor implements ICloneable nor is a
        // supported standard collection and MementoAttribute is set to Strict. Please either implement ICloneable on
        // type {1} or mark the member with the MementoIgnoreAttribute and manage storing and restoring the state of the
        // member manually in hook methods marked with the MementoCreateHookAttribute and MementoRestoreHookAttribute or
        // alternatively mark {2}'s Memento attribute as Loose to apply value-type assigning semantics to all
        // unsupported members.
        public static string NonSupportedMemberInStrictModeId => "MOYOU1001";

        // {0} Member, {1} MemberType, {2} TargetType
        public static string NonSupportedMemberInStrictModeMessageFormat =>
            "Member {0} of type {1} on type {2} is neither a value type nor implements ICloneable nor is a supported standard collection and MementoAttribute is set to Strict. Please either implement ICloneable on type {1} or mark the member with the MementoIgnoreAttribute and manage storing and restoring the state of the member manually in hook methods marked with the MementoCreateHookAttribute and MementoRestoreHookAttribute or alternatively mark {2}'s Memento attribute as Loose to apply value-type assigning semantics to all unsupported members.";

        public static string NonSupportedMemberInStrictModeTitle => "Non-supported member in strict mode";
        public static string NonSupportedMemberInStrictModeCategory => "Memento";
    }

    /// <summary>
    /// Singleton - MOYOU11xx
    /// </summary>
    public static class Singleton
    {
        // MOYOU1101: Singleton class {0} should have no accessible constructors.
        // Found constructors with signatures: {1}
        public static string HasAccessibleConstructorsId => "MOYOU1101";
        public static string HasAccessibleConstructorsMessageFormat =>
            "Singleton class {0} should have no accessible constructors. Found constructors with signatures: {1}";
        public static string HasAccessibleConstructorsTitle => "Singleton class should have no accessible constructors";
        public static string HasAccessibleConstructorsCategory => "Singleton";
        
    }
}