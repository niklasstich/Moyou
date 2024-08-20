using Metalama.Framework.Aspects;
using Moyou.Aspects.Factory;

[assembly: AspectOrder(AspectOrderDirection.CompileTime, typeof(FactoryMemberAspect), typeof(FactoryAttribute), typeof(AbstractFactoryAttribute))]