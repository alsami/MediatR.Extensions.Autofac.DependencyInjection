using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using Autofac;

namespace MediatR.Extensions.Autofac.DependencyInjection
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder
            AddMediatR(this ContainerBuilder builder, params Assembly[] assemblies) =>
            AddMediatRInternal(builder, assemblies);

        public static ContainerBuilder AddMediatR(this ContainerBuilder builder, IEnumerable<Assembly> assemblies) =>
            AddMediatRInternal(builder, assemblies);

        public static ContainerBuilder AddMediatR(this ContainerBuilder builder, ICollection<Assembly> assemblies) =>
            AddMediatRInternal(builder, assemblies);

        public static ContainerBuilder AddMediatR(this ContainerBuilder builder, IEnumerable<Type> customBehaviorTypes,
            IEnumerable<Assembly> assemblies)
            => AddMediatRInternal(builder, assemblies, customBehaviorTypes);

        public static ContainerBuilder AddMediatR(this ContainerBuilder builder, Assembly assembly,
            params Type[] customBehaviorTypes)
            => AddMediatRInternal(builder, new[] {assembly}, customBehaviorTypes);

        private static ContainerBuilder AddMediatRInternal(ContainerBuilder builder, IEnumerable<Assembly> assemblies,
            IEnumerable<Type> customBehaviorTypes = null)
        {
            var enumeratedAssemblies = assemblies as Assembly[] ?? assemblies.ToArray();

            if (enumeratedAssemblies == null || !enumeratedAssemblies.Any() || enumeratedAssemblies.All(x => x == null))
            {
                throw new ArgumentNullException(nameof(assemblies),
                    $"Must provide assemblies in order to request {nameof(Mediator)}");
            }

            var enumeratedCustomBehaviorTypes 
                = customBehaviorTypes as Type[] ?? customBehaviorTypes?.ToArray() ?? Array.Empty<Type>();

            builder.RegisterModule(new MediatRModule(enumeratedAssemblies, enumeratedCustomBehaviorTypes));

            return builder;
        }
    }
}