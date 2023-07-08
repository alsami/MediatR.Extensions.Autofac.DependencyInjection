using System.Reflection;
using Autofac.Builder;
using Autofac.Features.Scanning;

namespace MediatR.Extensions.Autofac.DependencyInjection;

public class MediatRConfiguration
{
    internal IReadOnlyDictionary<Type?, Action<IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>>>? InternalCustomRegistrationAction
    {
        get;
    }
    
    internal Assembly[] HandlersFromAssemblies { get; }
    
    internal Type MediatorType { get; }

    internal Type NotificationPublisherType { get; }

    internal Type[] CustomPipelineBehaviors { get; }

    internal Type[] CustomStreamPipelineBehaviors { get; }
    
    internal Type[] OpenGenericTypesToRegister { get; }
    
    internal RegistrationScope RegistrationScope { get; }
    
    internal MediatRConfiguration(Assembly[] fromAssemblies,
        Type mediatorType,
        Type notificationPublisherType,
        Type[] openGenericTypesToRegister,
        Type[]? customPipelineBehaviors = null,
        Type[]? customStreamPipelineBehaviors = null,
        RegistrationScope registrationScope = RegistrationScope.Transient,
        IReadOnlyDictionary<Type?, Action<IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>>>?
            internalCustomRegistrationAction = null)
    {
        this.InternalCustomRegistrationAction = internalCustomRegistrationAction;
        this.HandlersFromAssemblies = fromAssemblies ?? throw new ArgumentNullException(nameof(fromAssemblies));
        this.MediatorType = mediatorType ?? throw new ArgumentNullException(nameof(mediatorType));
        this.NotificationPublisherType = notificationPublisherType ?? throw new ArgumentNullException(nameof(notificationPublisherType));
        this.OpenGenericTypesToRegister = openGenericTypesToRegister ?? throw new ArgumentNullException(nameof(openGenericTypesToRegister));
        this.CustomPipelineBehaviors = customPipelineBehaviors ?? Array.Empty<Type>();
        this.CustomStreamPipelineBehaviors = customStreamPipelineBehaviors ?? Array.Empty<Type>();
        this.RegistrationScope = registrationScope;
    }
    
    internal void OpenGenericTypesToRegisterCallback(Type type, IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> registeredType)
    {
        if (this.InternalCustomRegistrationAction?.TryGetValue(type, out var action) == true)
        {
            action.Invoke(registeredType);
        }
    }
}