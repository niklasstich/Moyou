using System.Diagnostics.CodeAnalysis;
using Metalama.Framework.Advising;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;

namespace Moyou.Aspects.Factory;

public class FactoryMemberAspect : IAspect<INamedType>
{
    public List<(INamedType, INamedType)> TargetTuples { get; }

    public FactoryMemberAspect(List<(INamedType, INamedType)> targetTuples)
    {
        TargetTuples = targetTuples;
    }


    [SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly")] //property is argument
    public void BuildAspect(IAspectBuilder<INamedType> builder)
    {
        //write an annotation on the target type containing the factory members and primary interface
        var annotations = TargetTuples
            .Select(tup => new FactoryMemberAnnotation(tup.Item1.ToRef(), tup.Item2.ToRef()));
        foreach (var annotation in annotations)
        {
            builder.AddAnnotation(annotation, true);
        }
    }
}