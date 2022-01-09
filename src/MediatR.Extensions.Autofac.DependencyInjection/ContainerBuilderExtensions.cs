using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;

namespace MediatR.Extensions.Autofac.DependencyInjection;

public static class ContainerBuilderExtensions
{
    public static ContainerBuilder
        RegisterMediatR(this ContainerBuilder builder, params Assembly[] assemblies)
        => RegisterMediatRInternal(builder, assemblies);

    public static ContainerBuilder RegisterMediatR(this ContainerBuilder builder, IEnumerable<Assembly> assemblies)
        => RegisterMediatRInternal(builder, assemblies);

    public static ContainerBuilder RegisterMediatR(this ContainerBuilder builder, IEnumerable<Type> customBehaviorTypes,
        IEnumerable<Assembly> assemblies)
        => RegisterMediatRInternal(builder, assemblies, customBehaviorTypes);

    public static ContainerBuilder RegisterMediatR(this ContainerBuilder builder, Assembly assembly,
        params Type[] customBehaviorTypes)
        => RegisterMediatRInternal(builder, new[] {assembly}, customBehaviorTypes);

    private static ContainerBuilder RegisterMediatRInternal(ContainerBuilder builder, IEnumerable<Assembly> assemblies,
        IEnumerable<Type>? customBehaviorTypes = null)
    {
        var enumeratedAssemblies = assemblies as Assembly[] ?? assemblies.ToArray();

        if (enumeratedAssemblies == null || !enumeratedAssemblies.Any() || enumeratedAssemblies.All(x => x == null))
        {
            throw new ArgumentNullException(nameof(assemblies),
                $"Must provide assemblies in order to request {nameof(Mediator)}");
        }

        var enumeratedCustomBehaviorTypes = customBehaviorTypes as Type[] ?? customBehaviorTypes?.ToArray() ?? Array.Empty<Type>();

        builder.RegisterModule(new MediatRModule(enumeratedAssemblies, enumeratedCustomBehaviorTypes));

        return builder;
    }
}