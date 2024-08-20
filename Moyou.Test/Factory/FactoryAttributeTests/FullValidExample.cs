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
    public WindowsButton(string text)
    {
        Text = text;
    }
    
    public string Text { get; set; }
}

public class WindowsWindow : IWindow
{
    
}

public class MacButton : IButton
{
    public MacButton(string text)
    {
        Text = text;
    }
    private string _text;

    public string Text
    {
        get => _text;
        set {
            // OS call
            _text = value;
        }
    }
}

public class MacWindow : IWindow
{
    
}
