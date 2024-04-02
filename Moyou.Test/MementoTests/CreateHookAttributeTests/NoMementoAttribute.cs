using Moyou.Aspects.Memento;

namespace Moyou.CompileTimeTest.MementoTests.CreateHookAttributeTests;
internal class NoMementoAttribute
{
    [MementoCreateHook]
    public void CreateMementoHook()
    {
    }
}
