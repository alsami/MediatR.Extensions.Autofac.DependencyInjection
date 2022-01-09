using System;
using System.Reflection;

namespace MediatR.Extensions.Autofac.DependencyInjection;

internal class MediatRConfiguration
{
    public Assembly[] HandlersFromAssemblies { get; }

    public Type[] CustomPipelineBehaviors { get; }

    public Type[] CustomStreamPipelineBehaviors { get; }

    public MediatRConfiguration(Assembly[] fromAssemblies, Type[]? customPipelineBehaviors = null, Type[]? customStreamPipelineBehaviors = null)
    {
        this.HandlersFromAssemblies = fromAssemblies ?? throw new ArgumentNullException(nameof(fromAssemblies));
        this.CustomPipelineBehaviors = customPipelineBehaviors ?? Array.Empty<Type>();
        this.CustomStreamPipelineBehaviors = customStreamPipelineBehaviors ?? Array.Empty<Type>();
    }
}