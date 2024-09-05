using Metalama.Framework.Aspects;

namespace Moyou.Diagnostics;

[CompileTime]
public class Errors
{
    public static class Factory
    {
        // MOYOU2201: No TargetType in [FactoryMember] on factory {0}.
        public static string NoTargetTypeInMemberAttributeId => "MOYOU2201";

        public static string NoTargetTypeInMemberAttributeMessageFormat =>
            "No TargetType in [FactoryMember] on factory {0}.";

        public static string NoTargetTypeInMemberAttributeTitle => "No TargetType in [FactoryMember].";

        public static string NoTargetTypeInMemberAttributeCategory => "Factory";


        // MOYOU2202: TargetType {0} doesn't implement any interfaces in [FactoryMember] on factory {1}.
        public static string TypeDoesntImplementAnyInterfacesId => "MOYOU2202";

        public static string TypeDoesntImplementAnyInterfacesMessageFormat =>
            "TargetType doesn't implement any interfaces in [FactoryMember] on factory {0}.";

        public static string TypeDoesntImplementAnyInterfacesTitle => "TargetType doesn't implement any interfaces.";

        public static string TypeDoesntImplementAnyInterfacesCategory => "Factory";


        // MOYOU2203: TargetType {0} in [FactoryMember] on factory {1} implements multiple interfaces.
        // You must define which one to use for the return type of the factory by defining it via the PrimaryInterface property.
        public static string AmbiguousInterfacesOnTargetTypeId => "MOYOU2203";

        public static string AmbiguousInterfacesOnTargetTypeMessageFormat =>
            "TargetType in [FactoryMember] on factory {0} implements multiple interfaces. " +
            "You must define which one to use for the return type of the factory by defining it via the PrimaryInterface property.";

        public static string AmbiguousInterfacesOnTargetTypeTitle => "Ambiguous interfaces on TargetType.";

        public static string AmbiguousInterfacesOnTargetTypeCategory => "Factory";
        

        // MOYOU2204: TargetType {0} in [FactoryMember] on factory {1} does not implement PrimaryInterface {2}.
        public static string TargetTypeDoesntImplementPrimaryInterfaceId => "MOYOU2204";

        public static string TargetTypeDoesntImplementPrimaryInterfaceMessageFormat =>
            "TargetType in [FactoryMember] on factory {0} does not implement PrimaryInterface.";

        public static string TargetTypeDoesntImplementPrimaryInterfaceTitle =>
            "TargetType doesn't implement PrimaryInterface.";

        public static string TargetTypeDoesntImplementPrimaryInterfaceCategory => "Factory";


        // MOYOU2205: Factory member {0} has no public default constructor or constructor marked with [FactoryConstructor].
        public static string NoSuitableConstructorId => "MOYOU2205";

        public static string NoSuitableConstructorMessageFormat =>
            "Factory member {0} has no public default constructor or constructor marked with [FactoryConstructor].";

        public static string NoSuitableConstructorTitle => "Factory member has no suitable constructor.";

        public static string NoSuitableConstructorCategory => "Factory";


        // MOYOU2206: Factory member {0} has more than one constructor marked with [FactoryConstructor].
        public static string MultipleMarkedConstructorsId => "MOYOU2206";

        public static string MultipleMarkedConstructorsMessageFormat =>
            "Factory member {0} has more than one constructor marked with [FactoryConstructor].";

        public static string MultipleMarkedConstructorTitle => "Factory member has more than one marked constructor.";
        public static string MultipleMarkedConstructorCategory => "Factory";
    }
}