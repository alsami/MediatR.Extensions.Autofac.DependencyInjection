using System.Reflection;

namespace MediatR.Extensions.Autofac.DependencyInjection;

public class MediatRConfiguration
{
    internal Assembly[] HandlersFromAssemblies { get; }

    internal Type[] CustomPipelineBehaviors { get; }

    internal Type[] CustomStreamPipelineBehaviors { get; }
    
    internal Type[] OpenGenericTypesToRegister { get; }
    
    internal RegistrationScope RegistrationScope { get; }

    internal MediatRConfiguration(
        Assembly[] fromAssemblies,
        Type[] openGenericTypesToRegister,
        Type[]? customPipelineBehaviors = null,
        Type[]? customStreamPipelineBehaviors = null,
        RegistrationScope registrationScope = RegistrationScope.Transient)
    {
        this.HandlersFromAssemblies = fromAssemblies ?? throw new ArgumentNullException(nameof(fromAssemblies));
        this.OpenGenericTypesToRegister = openGenericTypesToRegister ?? throw new ArgumentNullException(nameof(openGenericTypesToRegister));
        this.CustomPipelineBehaviors = customPipelineBehaviors ?? Array.Empty<Type>();
        this.CustomStreamPipelineBehaviors = customStreamPipelineBehaviors ?? Array.Empty<Type>();
        this.RegistrationScope = registrationScope;
    }
}