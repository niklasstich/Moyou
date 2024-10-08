namespace Moyou.Aspects.Factory;

// public class FactoryAttributeOptions : IHierarchicalOptions<INamedType>
// {
//     public INamedType? AbstractFactoryType { get; set; }
//     public object ApplyChanges(object changes, in ApplyChangesContext context)
//     {
//         var other = (FactoryAttributeOptions)changes;
//         return new FactoryAttributeOptions
//         {
//             AbstractFactoryType = other.AbstractFactoryType ?? AbstractFactoryType
//         };
//     }
// }