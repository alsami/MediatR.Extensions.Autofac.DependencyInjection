using System.Reflection;
using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;

namespace MediatR.Extensions.Autofac.DependencyInjection;

public static class ContainerBuilderExtensions
{
    [Obsolete($"Please use the extension to pass in an instance of {nameof(MediatRConfiguration)} using {nameof(MediatRConfigurationBuilder)} to build it. Will be removed with version 10.0.0", false)]
    public static ContainerBuilder
        RegisterMediatR(this ContainerBuilder builder, params Assembly[] assemblies) => RegisterMediatRInternal(builder, assemblies);

    [Obsolete($"Please use the extension to pass in an instance of {nameof(MediatRConfiguration)} using {nameof(MediatRConfigurationBuilder)} to build it. Will be removed with version 10.0.0", false)]
    public static ContainerBuilder RegisterMediatR(
        this ContainerBuilder builder, 
        IEnumerable<Assembly> assemblies) => RegisterMediatRInternal(builder, assemblies);

    // ReSharper disable once UnusedMember.Global
    [Obsolete($"Please use the extension to pass in an instance of {nameof(MediatRConfiguration)} using {nameof(MediatRConfigurationBuilder)} to build it. Will be removed with version 10.0.0", false)]
    public static ContainerBuilder RegisterMediatR(
        this ContainerBuilder builder, 
        IEnumerable<Type> customPipeLineBehaviorTypes,
        IEnumerable<Assembly> assemblies) => RegisterMediatRInternal(builder, assemblies, customPipeLineBehaviorTypes);
    
    // ReSharper disable once UnusedMember.Global
    [Obsolete($"Please use the extension to pass in an instance of {nameof(MediatRConfiguration)} using {nameof(MediatRConfigurationBuilder)} to build it. Will be removed with version 10.0.0", false)]
    public static ContainerBuilder RegisterMediatR(
        this ContainerBuilder builder, 
        IEnumerable<Type> customPipeLineBehaviorTypes,
        IEnumerable<Type> customStreamPipeLineBehaviorTypes,
        IEnumerable<Assembly> assemblies) => RegisterMediatRInternal(builder, assemblies, customPipeLineBehaviorTypes, customStreamPipeLineBehaviorTypes);

    [Obsolete($"Please use the extension to pass in an instance of {nameof(MediatRConfiguration)} using {nameof(MediatRConfigurationBuilder)} to build it. Will be removed with version 10.0.0", false)]
    public static ContainerBuilder RegisterMediatR(
        this ContainerBuilder builder, 
        Assembly assembly,
        params Type[] customPipeLineBehaviorTypes) => RegisterMediatRInternal(builder, new[] {assembly}, customPipeLineBehaviorTypes);

    public static ContainerBuilder RegisterMediatR(
        this ContainerBuilder builder,
        MediatRConfiguration configuration)
    {
        builder.RegisterModule(new MediatRModule(configuration));

        return builder;
    }

    private static ContainerBuilder RegisterMediatRInternal(
        ContainerBuilder builder, 
        IEnumerable<Assembly> assemblies,
        IEnumerable<Type>? customPipeLineBehaviorTypes = null,
        IEnumerable<Type>? customStreamPipelineBehaviorTypes = null)
    {
        var enumeratedAssemblies = assemblies as Assembly[] ?? assemblies.ToArray();

        var configuration = MediatRConfigurationBuilder
            .Create(enumeratedAssemblies)
            .WithAllOpenGenericHandlerTypesRegistered()
            .WithCustomPipelineBehaviors(customPipeLineBehaviorTypes ?? Array.Empty<Type>())
            .WithCustomStreamPipelineBehaviors(customStreamPipelineBehaviorTypes ?? Array.Empty<Type>())
            .Build();

        builder.RegisterModule(new MediatRModule(configuration));

        return builder;
    }
}