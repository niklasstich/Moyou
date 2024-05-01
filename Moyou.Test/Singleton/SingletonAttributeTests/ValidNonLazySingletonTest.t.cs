using Moyou.Aspects.Singleton;
namespace Moyou.CompileTimeTest.Singleton.SingletonAttributeTests;
[Singleton(Lazy = false)]
public class ValidNonLazySingletonTest
{
  private ValidNonLazySingletonTest()
  {
  }
  private static global::Moyou.CompileTimeTest.Singleton.SingletonAttributeTests.ValidNonLazySingletonTest _instance;
  static ValidNonLazySingletonTest()
  {
    global::Moyou.CompileTimeTest.Singleton.SingletonAttributeTests.ValidNonLazySingletonTest._instance = new global::Moyou.CompileTimeTest.Singleton.SingletonAttributeTests.ValidNonLazySingletonTest();
  }
  public static global::Moyou.CompileTimeTest.Singleton.SingletonAttributeTests.ValidNonLazySingletonTest Instance
  {
    get
    {
      return (global::Moyou.CompileTimeTest.Singleton.SingletonAttributeTests.ValidNonLazySingletonTest)global::Moyou.CompileTimeTest.Singleton.SingletonAttributeTests.ValidNonLazySingletonTest._instance;
    }
  }
}
