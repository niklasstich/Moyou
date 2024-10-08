using Metalama.Framework.Aspects;

namespace Moyou.Aspects.Factory;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
[RunTimeOrCompileTime]
public class FactoryMemberAttribute : Attribute
{
    public Type TargetType { get; set; }
    public Type? PrimaryInterface { get; set; }
}