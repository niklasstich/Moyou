using Metalama.Framework.Aspects;
using Moyou.Aspects.Memento;

[assembly: AspectOrder(typeof(MementoCreateHookAttribute), typeof(MementoAttribute))]
[assembly: AspectOrder(typeof(MementoRestoreHookAttribute), typeof(MementoAttribute))]
[assembly: AspectOrder(typeof(MementoIgnoreAttribute), typeof(MementoAttribute))]