using Moyou.Aspects.Singleton;
namespace Moyou.CompileTimeTest.Singleton.SingletonAttributeTests;
[Singleton]
public class ValidSingletonTest
{
  private ValidSingletonTest()
  {
  }
  private static global::System.Lazy<global::Moyou.CompileTimeTest.Singleton.SingletonAttributeTests.ValidSingletonTest> _instance;
  static ValidSingletonTest()
  {
    global::Moyou.CompileTimeTest.Singleton.SingletonAttributeTests.ValidSingletonTest._instance = new global::System.Lazy<global::Moyou.CompileTimeTest.Singleton.SingletonAttributeTests.ValidSingletonTest>(() => new global::Moyou.CompileTimeTest.Singleton.SingletonAttributeTests.ValidSingletonTest());
  }
  public static global::Moyou.CompileTimeTest.Singleton.SingletonAttributeTests.ValidSingletonTest Instance
  {
    get
    {
      return (global::Moyou.CompileTimeTest.Singleton.SingletonAttributeTests.ValidSingletonTest)global::Moyou.CompileTimeTest.Singleton.SingletonAttributeTests.ValidSingletonTest._instance.Value;
    }
  }
}
