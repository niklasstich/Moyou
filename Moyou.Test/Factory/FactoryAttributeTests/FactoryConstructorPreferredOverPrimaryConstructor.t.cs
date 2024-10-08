using Moyou.Aspects.Factory;
namespace Moyou.CompileTimeTest.Factory.FactoryAttributeTests;
[Factory]
[FactoryMember(TargetType = typeof(ClassE))]
public class FactoryE
{
  public global::Moyou.CompileTimeTest.Factory.FactoryAttributeTests.InterfaceE CreatenterfaceE(global::System.Int32 foobar, global::System.String barbaz, global::System.Object bobo)
  {
    return new global::Moyou.CompileTimeTest.Factory.FactoryAttributeTests.ClassE((global::System.Int32)foobar, (global::System.String)barbaz, bobo);
  }
}
public interface InterfaceE
{
}
public class ClassE : InterfaceE
{
  public ClassE()
  {
  }
  [FactoryConstructor]
  public ClassE(int foobar, string barbaz, object bobo)
  {
  }
}