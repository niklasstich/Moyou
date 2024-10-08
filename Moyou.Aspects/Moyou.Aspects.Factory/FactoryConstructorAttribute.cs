using Metalama.Framework.Aspects;

namespace Moyou.Aspects.Factory;

[AttributeUsage(AttributeTargets.Constructor)]
[RunTimeOrCompileTime]
public class FactoryConstructorAttribute : Attribute
{
    
}