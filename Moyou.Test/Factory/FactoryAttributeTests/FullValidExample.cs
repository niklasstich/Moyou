using Moyou.Aspects.Factory;

namespace Moyou.CompileTimeTest.Factory.FactoryAttributeTests;

[Factory]
[FactoryMember(TargetType = typeof(WindowsButton))]
[FactoryMember(TargetType = typeof(WindowsWindow))]
public class WindowsUIFactory
{

}

[Factory]
[FactoryMember(TargetType = typeof(MacButton))]
[FactoryMember(TargetType = typeof(MacWindow))]
public class MacUIFactory
{
    
}

public interface IButton
{
}

public interface IWindow
{
    
}

public class WindowsButton : IButton
{
}

public class WindowsWindow : IWindow
{
    private WindowsWindow()
    {
        
    }

    [FactoryConstructor]
    public WindowsWindow(WindowsButton button)
    {
        
    }
}

public class MacButton : IButton
{
}

public class MacWindow : IWindow
{
    public MacWindow(int foobar, string barbaz, object bobo)
    {
        
    }
}
