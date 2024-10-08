using Metalama.Framework.Aspects;
using Moyou.Aspects.Memento;

[assembly: AspectOrder(AspectOrderDirection.RunTime, typeof(MementoCreateHookAttribute), typeof(MementoAttribute))]
[assembly: AspectOrder(AspectOrderDirection.RunTime, typeof(MementoRestoreHookAttribute), typeof(MementoAttribute))]
[assembly: AspectOrder(AspectOrderDirection.RunTime, typeof(MementoIgnoreAttribute), typeof(MementoAttribute))]