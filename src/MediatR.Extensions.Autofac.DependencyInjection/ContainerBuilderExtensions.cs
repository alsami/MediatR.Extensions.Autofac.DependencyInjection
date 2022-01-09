using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;

namespace MediatR.Extensions.Autofac.DependencyInjection;

public static class ContainerBuilderExtensions
{
    public static ContainerBuilder
        RegisterMediatR(this ContainerBuilder builder, params Assembly[] assemblies) => RegisterMediatRInternal(builder, assemblies);

    public static ContainerBuilder RegisterMediatR(
        this ContainerBuilder builder, 
        IEnumerable<Assembly> assemblies) => RegisterMediatRInternal(builder, assemblies);

    public static ContainerBuilder RegisterMediatR(
        this ContainerBuilder builder, 
        IEnumerable<Type> customPipeLineBehaviorTypes,
        IEnumerable<Assembly> assemblies) => RegisterMediatRInternal(builder, assemblies, customPipeLineBehaviorTypes);
    
    public static ContainerBuilder RegisterMediatR(
        this ContainerBuilder builder, 
        IEnumerable<Type> customPipeLineBehaviorTypes,
        IEnumerable<Type> customStreamPipeLineBehaviorTypes,
        IEnumerable<Assembly> assemblies) => RegisterMediatRInternal(builder, assemblies, customPipeLineBehaviorTypes, customStreamPipeLineBehaviorTypes);

    public static ContainerBuilder RegisterMediatR(
        this ContainerBuilder builder, 
        Assembly assembly,
        params Type[] customPipeLineBehaviorTypes) => RegisterMediatRInternal(builder, new[] {assembly}, customPipeLineBehaviorTypes);

    private static ContainerBuilder RegisterMediatRInternal(
        ContainerBuilder builder, 
        IEnumerable<Assembly> assemblies,
        IEnumerable<Type>? customPipeLineBehaviorTypes = null,
        IEnumerable<Type>? customStreamPipelineBehaviorTypes = null)
    {
        var enumeratedAssemblies = assemblies as Assembly[] ?? assemblies.ToArray();

        var configuration = MediatRConfigurationBuilder.Create(enumeratedAssemblies)
            .WithCustomPipelineBehaviors(customPipeLineBehaviorTypes ?? Array.Empty<Type>())
            .WithCustomStreamPipelineBehaviors(customStreamPipelineBehaviorTypes ?? Array.Empty<Type>())
            .Build();

        builder.RegisterModule(new MediatRModule(configuration));

        return builder;
    }
}