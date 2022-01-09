using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MediatR.Extensions.Autofac.DependencyInjection;

internal class MediatRConfigurationBuilder
{
    private readonly Assembly[] handlersFromAssembly;

    private readonly List<Type> internalCustomPipelineBehaviorTypes = new();
    private readonly List<Type> internalCustomStreamPipelineBehaviorTypes = new();

    private MediatRConfigurationBuilder(Assembly[] handlersFromAssembly)
    {
        if (handlersFromAssembly == null || !handlersFromAssembly.Any() || handlersFromAssembly.All(x => x == null))
        {
            throw new ArgumentNullException(nameof(handlersFromAssembly), $"Must provide assemblies in order to request {nameof(Mediator)}");
        }

        this.handlersFromAssembly = handlersFromAssembly;
    }

    public static MediatRConfigurationBuilder Create(Assembly[] handlersFromAssembly) => new(handlersFromAssembly);

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
    
    public MediatRConfigurationBuilder WithCustomStreamPipelineBehaviors(IEnumerable<Type> customStreamPipelineBehaviorTypes)
    {
        foreach (var customStreamPipelineBehaviorType in customStreamPipelineBehaviorTypes)
        {
            this.WithCustomStreamPipelineBehavior(customStreamPipelineBehaviorType);
        }

        return this;
    }

    public MediatRConfiguration Build() => new(this.handlersFromAssembly, this.internalCustomPipelineBehaviorTypes.ToArray(), this.internalCustomStreamPipelineBehaviorTypes.ToArray());
}