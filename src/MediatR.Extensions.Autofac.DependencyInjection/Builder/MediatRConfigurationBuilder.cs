using System.Reflection;

namespace MediatR.Extensions.Autofac.DependencyInjection.Builder;

public class MediatRConfigurationBuilder
{
    private readonly Assembly[] handlersFromAssembly;

    private readonly HashSet<Type> internalCustomPipelineBehaviorTypes = new();
    private readonly HashSet<Type> internalCustomStreamPipelineBehaviorTypes = new();
    private readonly HashSet<Type> internalOpenGenericHandlerTypesToRegister = new();

    private MediatRConfigurationBuilder(Assembly[] handlersFromAssembly)
    {
        if (handlersFromAssembly == null || !handlersFromAssembly.Any() || handlersFromAssembly.All(x => x == null))
        {
            throw new ArgumentNullException(nameof(handlersFromAssembly),
                $"Must provide assemblies in order to request {nameof(Mediator)}");
        }

        this.handlersFromAssembly = handlersFromAssembly;
    }

    public static MediatRConfigurationBuilder Create(params Assembly[] handlersFromAssembly) => new(handlersFromAssembly);

    public MediatRConfigurationBuilder WithCustomPipelineBehavior(Type customPipelineBehaviorType)
    {
        if (customPipelineBehaviorType is null)
        {
            throw new ArgumentNullException(nameof(customPipelineBehaviorType));
        }

        this.internalCustomPipelineBehaviorTypes.Add(customPipelineBehaviorType);

        return this;
    }

    public MediatRConfigurationBuilder WithCustomPipelineBehaviors(IEnumerable<Type> customPipelineBehaviorTypes)
    {
        if (customPipelineBehaviorTypes is null)
        {
            throw new ArgumentNullException(nameof(customPipelineBehaviorTypes));
        }
        
        foreach (var customPipelineBehaviorType in customPipelineBehaviorTypes)
        {
            this.WithCustomPipelineBehavior(customPipelineBehaviorType);
        }

        return this;
    }

    public MediatRConfigurationBuilder WithCustomStreamPipelineBehavior(Type customStreamPipelineBehaviorType)
    {
        if (customStreamPipelineBehaviorType is null)
        {
            throw new ArgumentNullException(nameof(customStreamPipelineBehaviorType));
        }

        this.internalCustomStreamPipelineBehaviorTypes.Add(customStreamPipelineBehaviorType);

        return this;
    }

    public MediatRConfigurationBuilder WithAllOpenGenericHandlerTypesRegistered()
    {
        foreach (var openGenericHandlerType in KnownHandlerTypes.AllTypes)
        {
            this.AddOpenGenericHandlerToRegister(openGenericHandlerType);
        }

        return this;
    }

    private void AddOpenGenericHandlerToRegister(Type openHandlerType)
    {
        this.internalOpenGenericHandlerTypesToRegister.Add(openHandlerType);
    }

    public MediatRConfigurationBuilder WithOpenGenericHandlerTypeToRegister(Type openGenericHandlerType)
    {
        if (!KnownHandlerTypes.AllTypes.Contains(openGenericHandlerType))
        {
            throw new ArgumentException(
                $"Invalid open-generic handler-type {openGenericHandlerType.Name}",
                nameof(openGenericHandlerType));
        }

        this.AddOpenGenericHandlerToRegister(openGenericHandlerType);

        return this;
    }

    public MediatRConfigurationBuilder WithRequestHandlersManuallyRegistered()
    {
        foreach (var openGenericHandlerType in KnownHandlerTypes.AllTypes.Where(type => type != typeof(IRequestHandler<,>)))
        {
            this.AddOpenGenericHandlerToRegister(openGenericHandlerType);
        }

        return this;
    }

    public MediatRConfigurationBuilder WithCustomStreamPipelineBehaviors(IEnumerable<Type> customStreamPipelineBehaviorTypes)
    {
        foreach (var customStreamPipelineBehaviorType in customStreamPipelineBehaviorTypes)
        {
            this.WithCustomStreamPipelineBehavior(customStreamPipelineBehaviorType);
        }

        return this;
    }

    public MediatRConfiguration Build()
        => new(
            this.handlersFromAssembly,
            this.internalOpenGenericHandlerTypesToRegister.ToArray(),
            this.internalCustomPipelineBehaviorTypes.ToArray(),
            this.internalCustomStreamPipelineBehaviorTypes.ToArray());
}