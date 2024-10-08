namespace Moyou.UnitTest.UnsavedChanges;

internal static class UnsavedChangesTestHelper
{
    internal static A GetFullStructure()
    {
        var b = new B
        {
            C = new C(),
            C1 = new C(),
            Cs = new List<C>
            {
                new(), new()
            }
        };
        var a = new A
        {
            Bs2 = null,
            Bs = new List<B?> { null, b },
            B = b
        };
        return a;
    }
}