using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Autofac;

namespace MediatR.Extensions.Autofac.DependencyInjection
{
    public static class ContainerExtensions
    {
        public static void
            AddMediatR(this ContainerBuilder builder, params Assembly[] assemblies) =>
            AddMediatRInternal(builder, assemblies);

        public static void AddMediatR(this ContainerBuilder builder, IEnumerable<Assembly> assemblies) =>
            AddMediatRInternal(builder, assemblies);

        public static void AddMediatR(this ContainerBuilder builder, ICollection<Assembly> assemblies) =>
            AddMediatRInternal(builder, assemblies);

        private static void AddMediatRInternal(ContainerBuilder builder, IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }
            
            builder.RegisterModule(new MediatRModule(assemblies as Assembly[] ?? assemblies.ToArray()));
        }
    }
}