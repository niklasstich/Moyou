using Moyou.Aspects.Factory;

namespace Moyou.CompileTimeTest.Factory.FactoryAttributeTests;

[AbstractFactory]
public interface AbstractUIFactory
{
    IButton CreateButton();
}

// [Factory(AbstractFactoryType = typeof(AbstractUIFactory))]
public class WindowsUIFactory
{

}

[Factory(AbstractFactoryType = typeof(AbstractUIFactory))]
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
    
}

public class MacButton : IButton
{
}

public class MacWindow : IWindow
{
    
}
