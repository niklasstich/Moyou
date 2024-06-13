// Warning MOYOU1102 on `HasImplicitPublicConstructor`: `Singleton class HasImplicitPublicConstructor shouldn't have an implicit public constructor. Consider defining your own private constructor to override the implicit public constructor.`
using Moyou.Aspects.Singleton;
namespace Moyou.CompileTimeTest.Singleton.SingletonAttributeTests;
[Singleton]
public class HasImplicitPublicConstructor
{
  private static global::System.Lazy<global::Moyou.CompileTimeTest.Singleton.SingletonAttributeTests.HasImplicitPublicConstructor> _instance;
  static HasImplicitPublicConstructor()
  {
    global::Moyou.CompileTimeTest.Singleton.SingletonAttributeTests.HasImplicitPublicConstructor._instance = new global::System.Lazy<global::Moyou.CompileTimeTest.Singleton.SingletonAttributeTests.HasImplicitPublicConstructor>(() => new global::Moyou.CompileTimeTest.Singleton.SingletonAttributeTests.HasImplicitPublicConstructor());
  }
  public static global::Moyou.CompileTimeTest.Singleton.SingletonAttributeTests.HasImplicitPublicConstructor Instance
  {
    get
    {
      return (global::Moyou.CompileTimeTest.Singleton.SingletonAttributeTests.HasImplicitPublicConstructor)global::Moyou.CompileTimeTest.Singleton.SingletonAttributeTests.HasImplicitPublicConstructor._instance.Value;
    }
  }
}
