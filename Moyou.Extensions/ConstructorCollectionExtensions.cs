using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Code.Collections;

namespace Moyou.Extensions;

[CompileTime]
public static class ConstructorCollectionExtensions
{
    [CompileTime]
    public static IConstructor GetDefaultConstructor(this IConstructorCollection constructorCollection) =>
        constructorCollection.First(ctor =>
            ctor.Parameters.Count == 0 &&
            ctor.Accessibility == Accessibility.Public
        );
}