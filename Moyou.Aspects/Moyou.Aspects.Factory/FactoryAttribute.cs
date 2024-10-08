using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Metalama.Framework.Advising;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Diagnostics;
using Moyou.Diagnostics;
using Moyou.Extensions;

namespace Moyou.Aspects.Factory;

[AttributeUsage(AttributeTargets.Class)]
public class FactoryAttribute : TypeAspect
{
    private static readonly DiagnosticDefinition<INamedType> ErrorNoSuitableConstructor =
        new(Errors.Factory.NoSuitableConstructorId, Severity.Error,
            Errors.Factory.NoSuitableConstructorMessageFormat,
            Errors.Factory.NoSuitableConstructorTitle,
            Errors.Factory.NoSuitableConstructorCategory);

    private static readonly DiagnosticDefinition<INamedType> ErrorMultipleMarkedConstructors =
        new(Errors.Factory.MultipleMarkedConstructorsId, Severity.Error,
            Errors.Factory.MultipleMarkedConstructorsMessageFormat,
            Errors.Factory.MultipleMarkedConstructorTitle,
            Errors.Factory.MultipleMarkedConstructorCategory);

    [SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly")] //property is argument
    public override void BuildAspect(IAspectBuilder<INamedType> builder)
    {
        base.BuildAspect(builder);

        //read the annotations from FactoryMemberAspect and process all tuples 
        var annotations = builder.Target.Enhancements().GetAnnotations<FactoryMemberAnnotation>();
        var tuples = annotations.Select(annotation => annotation.AsTuple()).ToList();
        foreach (var tuple in tuples)
        {
            AddMemberToFactory(builder, tuple);
        }
    }

    private void AddMemberToFactory(IAspectBuilder<INamedType> builder, (INamedType, INamedType) tuple)
    {
        var memberType = tuple.Item1;
        var primaryInterface = tuple.Item2;
        var trimmedInterfaceName = primaryInterface.Name.StartsWith("I")
            ? primaryInterface.Name[1..]
            : primaryInterface.Name;
        if (memberType.HasPublicDefaultConstructor() &&
            !memberType.Constructors.Any(ctor => ctor.HasAttribute<FactoryConstructorAttribute>()))
        {
            builder.IntroduceMethod(nameof(CreateTemplateDefaultConstructor), IntroductionScope.Instance,
                buildMethod: methodBuilder =>
                {
                    //drop the leading 'I' from the interface in the method name
                    methodBuilder.Name = $"Create{trimmedInterfaceName}";
                    methodBuilder.Accessibility = Accessibility.Public;
                }, args: new { TInterface = primaryInterface, memberType });
        }
        else
        {
            HandleNonDefaultConstructor(builder, memberType, trimmedInterfaceName, primaryInterface);
        }
    }

    private static void HandleNonDefaultConstructor(IAspectBuilder<INamedType> builder, INamedType memberType,
        string trimmedInterfaceName, INamedType primaryInterface)
    {
        IConstructor? constructor;
        if (memberType.Constructors.Count == 1)
        {
            constructor = memberType.Constructors.Single();
        }
        else
        {
            try
            {
                constructor =
                    memberType.Constructors.SingleOrDefault(ctor => ctor.HasAttribute<FactoryConstructorAttribute>());
            }
            catch (InvalidOperationException iox)
            {
                //only one constructor with attribute allowed
                foreach (var markedCtor in memberType.Constructors.Where(ctor =>
                             ctor.HasAttribute<FactoryConstructorAttribute>()))
                {
                    builder.Diagnostics.Report(ErrorMultipleMarkedConstructors.WithArguments(memberType), markedCtor);
                }

                return;
            }
        }

        if (constructor == null)
        {
            //no constructor is marked
            builder.Diagnostics.Report(ErrorNoSuitableConstructor.WithArguments(memberType), memberType);
            return;
        }

        builder.IntroduceMethod(nameof(CreateTemplate), IntroductionScope.Instance, buildMethod: builder =>
        {
            builder.Name = $"Create{trimmedInterfaceName}";
            builder.Accessibility = Accessibility.Public;
            //add all constructor parameters to factory method
            foreach (var constructorParameter in constructor.Parameters)
            {
                builder.AddParameter(constructorParameter.Name, constructorParameter.Type);
            }
        }, args: new { TInterface = primaryInterface, constructor });
    }

    [Template]
    public static TInterface CreateTemplateDefaultConstructor<[CompileTime] TInterface>(
        [CompileTime] INamedType memberType)
    {
        var constructor = meta.CompileTime(memberType.Constructors.GetPublicDefaultConstructor());
        return constructor.Invoke()!;
    }

    [Template]
    public static TInterface CreateTemplate<[CompileTime] TInterface>([CompileTime] IConstructor constructor)
    {
        return constructor.Invoke(constructor.Parameters.Select(param => (IExpression)param.Value!));
    }
}