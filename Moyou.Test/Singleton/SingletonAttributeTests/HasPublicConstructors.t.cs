// Warning MOYOU1101 on `HasPublicConstructors`: `Singleton class HasPublicConstructors should have no accessible constructors. Found constructor with signature: (int a).`
// Warning MOYOU1101 on `HasPublicConstructors`: `Singleton class HasPublicConstructors should have no accessible constructors. Found constructor with signature: (int a, float b).`
using Moyou.Aspects.Singleton;
namespace Moyou.CompileTimeTest.Singleton.SingletonAttributeTests;
[Singleton]
public class HasPublicConstructors
{
  private HasPublicConstructors()
  {
    A = 1;
  }
  public HasPublicConstructors(int a)
  {
    A = a;
  }
  public HasPublicConstructors(int a, float b)
  {
  }
  public int A { get; set; }
  private static global::System.Lazy<global::Moyou.CompileTimeTest.Singleton.SingletonAttributeTests.HasPublicConstructors> _instance;
  static HasPublicConstructors()
  {
    global::Moyou.CompileTimeTest.Singleton.SingletonAttributeTests.HasPublicConstructors._instance = new global::System.Lazy<global::Moyou.CompileTimeTest.Singleton.SingletonAttributeTests.HasPublicConstructors>(() => new global::Moyou.CompileTimeTest.Singleton.SingletonAttributeTests.HasPublicConstructors());
  }
  public static global::Moyou.CompileTimeTest.Singleton.SingletonAttributeTests.HasPublicConstructors Instance
  {
    get
    {
      return (global::Moyou.CompileTimeTest.Singleton.SingletonAttributeTests.HasPublicConstructors)global::Moyou.CompileTimeTest.Singleton.SingletonAttributeTests.HasPublicConstructors._instance.Value;
    }
  }
}
