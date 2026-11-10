using System.Reflection;
using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection.Extensions;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
        RegisterLicenseRelatedClasses(builder);

        builder.RegisterType<ServiceProviderWrapper>()
            .As<IServiceProvider>()
            .InstancePerDependency()
            .IfNotRegistered(typeof(IServiceProvider));
        
        builder
            .RegisterType(this.mediatRConfiguration.MediatorType)
            .As<IMediator>()
            .As<IPublisher>()
            .As<ISender>()
            .ApplyTargetScope(this.mediatRConfiguration.RegistrationScope);

        builder
            .RegisterType(this.mediatRConfiguration.NotificationPublisherType)
            .As<INotificationPublisher>()
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

    private void RegisterLicenseRelatedClasses(ContainerBuilder builder)
    {
        const string baseNamespace = "MediatR.Licensing";
        
        var mediatrAssembly = typeof(INotificationPublisher).Assembly;

        var licenseAccessorType = mediatrAssembly.GetRequiredType($"{baseNamespace}.LicenseAccessor");
        var licenseAccessorConstructor = licenseAccessorType.GetRequiredPublicConstructor(
            typeof(MediatRServiceConfiguration), typeof(IServiceProvider), typeof(ILoggerFactory));

        builder.Register((ctx) =>
            {
                var mediatorServiceConfiguration = new MediatRServiceConfiguration
                {
                    LicenseKey = this.mediatRConfiguration.LicenseKey,
                    LicenseKeyFactory = this.mediatRConfiguration.LicenseKeyFactory,
                };
                // SingleInstance, so ctx is the root scope; resolve now because ctx is only valid during this call.
                var serviceProvider = ctx.Resolve<IServiceProvider>();
                var loggerFactory = ctx.ResolveOptional<ILoggerFactory>() ?? new NullLoggerFactory();
                return licenseAccessorConstructor.Invoke([mediatorServiceConfiguration, serviceProvider, loggerFactory]);
            })
            .As(licenseAccessorType)
            .SingleInstance();

        var licenseValidatorType = mediatrAssembly.GetRequiredType($"{baseNamespace}.LicenseValidator");
        var licenseValidatorConstructor = licenseValidatorType.GetRequiredPublicConstructor(typeof(ILoggerFactory));

        builder.Register((ctx) =>
            {
                var loggerFactory = ctx.ResolveOptional<ILoggerFactory>() ?? new NullLoggerFactory();
                return licenseValidatorConstructor.Invoke([loggerFactory]);
            })
            .As(licenseValidatorType)
            .SingleInstance();
    }

    private void RegisterGeneric(ContainerBuilder builder, Type implementationType, Type asType)
    {
        builder.RegisterGeneric(implementationType)
            .As(asType)
            .ApplyTargetScope(this.mediatRConfiguration.RegistrationScope);
    }
}