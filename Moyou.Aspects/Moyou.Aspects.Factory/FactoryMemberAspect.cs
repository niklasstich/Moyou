using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;

namespace Moyou.Aspects.Factory;

public class FactoryMemberAspect : IAspect<INamedType>
{
    public List<(INamedType, INamedType?)> TargetTuples { get; }

    public FactoryMemberAspect(List<(INamedType, INamedType?)> targetTuples)
    {
        TargetTuples = targetTuples;
        Debugger.Break();
    }


    [SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly")] //property is argument
    public void BuildAspect(IAspectBuilder<INamedType> builder)
    {
        Debugger.Break();
        //TODO: write an annotation on the target type containing the factory members
    }
}