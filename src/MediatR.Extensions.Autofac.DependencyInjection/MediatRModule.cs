using System.Reflection;
using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection.Extensions;
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
        builder
            .RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            .AsImplementedInterfaces()
            .ApplyTargetScope(this.mediatRConfiguration.RegistrationScope);

        foreach (var openHandlerType in this.mediatRConfiguration.OpenGenericTypesToRegister)
        {
            builder.RegisterAssemblyTypes(this.mediatRConfiguration.HandlersFromAssemblies)
                .AsClosedTypesOf(openHandlerType)
                .ApplyTargetScope(this.mediatRConfiguration.RegistrationScope);
        }

        foreach (var builtInPipelineBehaviorType in this.builtInPipelineBehaviorTypes)
        {
            this.RegisterGeneric(builder, builtInPipelineBehaviorType, typeof(IPipelineBehavior<,>));
        }
            
        foreach (var customBehaviorType in this.mediatRConfiguration.CustomPipelineBehaviors)
        {
            this.RegisterGeneric(builder, customBehaviorType, typeof(IPipelineBehavior<,>));
        }
        
        foreach (var customBehaviorType in this.mediatRConfiguration.CustomStreamPipelineBehaviors)
        {
            this.RegisterGeneric(builder, customBehaviorType, typeof(IStreamPipelineBehavior<,>));
        }
    }

    private void RegisterGeneric(ContainerBuilder builder, Type implementationType, Type asType)
    {
        builder.RegisterGeneric(implementationType)
            .As(asType)
            .ApplyTargetScope(this.mediatRConfiguration.RegistrationScope);
    }
}