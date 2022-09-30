using System.Reflection;
using Autofac;
using MediatR.Pipeline;
using Module = Autofac.Module;

namespace MediatR.Extensions.Autofac.DependencyInjection;

internal class MediatRModule : Module
{
    private readonly MediatRConfiguration mediatRConfiguration;
    
    private readonly Type[] builtInPipelineBehaviorTypes =
    {
        typeof(RequestPostProcessorBehavior<,>),
        typeof(RequestPreProcessorBehavior<,>),
        typeof(RequestExceptionActionProcessorBehavior<,>),
        typeof(RequestExceptionProcessorBehavior<,>),
    };

    public MediatRModule(MediatRConfiguration mediatRConfiguration)
    {
        this.mediatRConfiguration = mediatRConfiguration;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            .AsImplementedInterfaces()
            .InstancePerDependency();

        foreach (var openHandlerType in this.mediatRConfiguration.OpenGenericTypesToRegister)
        {
            builder.RegisterAssemblyTypes(this.mediatRConfiguration.HandlersFromAssemblies)
                .AsClosedTypesOf(openHandlerType)
                .InstancePerDependency();
        }

        foreach (var builtInPipelineBehaviorType in this.builtInPipelineBehaviorTypes)
        {
            RegisterGeneric(builder, builtInPipelineBehaviorType, typeof(IPipelineBehavior<,>));
        }
            
        foreach (var customBehaviorType in this.mediatRConfiguration.CustomPipelineBehaviors)
        {
            RegisterGeneric(builder, customBehaviorType, typeof(IPipelineBehavior<,>));
        }
        
        foreach (var customBehaviorType in this.mediatRConfiguration.CustomStreamPipelineBehaviors)
        {
            RegisterGeneric(builder, customBehaviorType, typeof(IStreamPipelineBehavior<,>));
        }

        builder
            .Register<ServiceFactory>(outerContext =>
            {
                var innerContext = outerContext.Resolve<IComponentContext>();

                return serviceType => innerContext.Resolve(serviceType);
            })
            .InstancePerDependency();
    }

    private static void RegisterGeneric(ContainerBuilder builder, Type implementationType, Type asType)
    {
        builder.RegisterGeneric(implementationType)
            .As(asType)
            .InstancePerDependency();
    }
}