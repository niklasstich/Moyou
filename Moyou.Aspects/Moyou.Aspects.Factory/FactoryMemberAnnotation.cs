using Metalama.Framework.Aspects;
using Metalama.Framework.Code;

namespace Moyou.Aspects.Factory;

[CompileTime]
public class FactoryMemberAnnotation : IAnnotation<INamedType>
{
    public IEnumerable<IRef<IDeclaration>> MemberTypes { get; set; }
}