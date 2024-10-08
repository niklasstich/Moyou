using Moyou.Aspects.Factory;
namespace Moyou.CompileTimeTest.Factory.FactoryAttributeTests;
[Factory]
[FactoryMember(TargetType = typeof(WindowsButton))]
[FactoryMember(TargetType = typeof(WindowsWindow))]
public class WindowsUIFactory
{
  public global::Moyou.CompileTimeTest.Factory.FactoryAttributeTests.IButton CreateButton()
  {
    return (global::Moyou.CompileTimeTest.Factory.FactoryAttributeTests.IButton)new global::Moyou.CompileTimeTest.Factory.FactoryAttributeTests.WindowsButton()!;
  }
  public global::Moyou.CompileTimeTest.Factory.FactoryAttributeTests.IWindow CreateWindow(global::Moyou.CompileTimeTest.Factory.FactoryAttributeTests.WindowsButton button)
  {
    return new global::Moyou.CompileTimeTest.Factory.FactoryAttributeTests.WindowsWindow((global::Moyou.CompileTimeTest.Factory.FactoryAttributeTests.WindowsButton)button);
  }
}
[Factory]
[FactoryMember(TargetType = typeof(MacButton))]
[FactoryMember(TargetType = typeof(MacWindow))]
public class MacUIFactory
{
  public global::Moyou.CompileTimeTest.Factory.FactoryAttributeTests.IButton CreateButton()
  {
    return (global::Moyou.CompileTimeTest.Factory.FactoryAttributeTests.IButton)new global::Moyou.CompileTimeTest.Factory.FactoryAttributeTests.MacButton()!;
  }
  public global::Moyou.CompileTimeTest.Factory.FactoryAttributeTests.IWindow CreateWindow(global::System.Int32 foobar, global::System.String barbaz, global::System.Object bobo)
  {
    return new global::Moyou.CompileTimeTest.Factory.FactoryAttributeTests.MacWindow((global::System.Int32)foobar, (global::System.String)barbaz, bobo);
  }
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