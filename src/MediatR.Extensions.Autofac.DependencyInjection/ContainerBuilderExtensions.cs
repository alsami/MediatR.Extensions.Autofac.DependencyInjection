using System;
using System.Collections.Generic;
using System.Linq;
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

        private static ContainerBuilder AddMediatRInternal(ContainerBuilder builder, IEnumerable<Assembly> assemblies)
        {
            var enumerableAssemblies = assemblies as Assembly[] ?? assemblies.ToArray();
            
            if (enumerableAssemblies == null || !enumerableAssemblies.Any() || enumerableAssemblies.All(x => x == null))
            {
                throw new ArgumentNullException(nameof(assemblies), $"Must provide assemblies in order to request {nameof(Mediator)}");
            }
            
            builder.RegisterModule(new MediatRModule(enumerableAssemblies));

            return builder;
        }
    }
}