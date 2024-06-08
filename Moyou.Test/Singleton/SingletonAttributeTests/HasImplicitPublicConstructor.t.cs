// Warning MOYOU1101 on `HasImplicitPublicConstructor`: `Singleton class HasImplicitPublicConstructor should have no accessible constructors. Found constructors with signatures: (void)`
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
