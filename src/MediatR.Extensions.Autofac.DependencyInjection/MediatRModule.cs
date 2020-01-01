using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Features.Variance;
using MediatR.Pipeline;
using Module = Autofac.Module;

namespace MediatR.Extensions.Autofac.DependencyInjection
{
    internal class MediatRModule : Module
    {
        private readonly Assembly[] assemblies;
        private readonly Type[] customBehaviorTypes;

        public MediatRModule(Assembly[] assemblies, Type[] customBehaviorTypes)
        {
            this.assemblies = assemblies;
            this.customBehaviorTypes = customBehaviorTypes;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            var openHandlerTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(IRequestExceptionHandler<,,>),
                typeof(IRequestExceptionAction<,>),
                typeof(INotificationHandler<>),
            };

            foreach (var openHandlerType in openHandlerTypes)
            {
                builder.RegisterAssemblyTypes(this.assemblies)
                    .AsClosedTypesOf(openHandlerType);
            }
            
            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            
            builder.RegisterGeneric(typeof(RequestExceptionActionProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestExceptionProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            
            foreach (var customBehaviorType in this.customBehaviorTypes)
            {
                builder.RegisterGeneric(customBehaviorType)
                    .As(typeof(IPipelineBehavior<,>));
            }

            builder.Register<ServiceFactory>(outerContext =>
            {
                var innerContext = outerContext.Resolve<IComponentContext>();

                return serviceType => innerContext.Resolve(serviceType);
            });
        }
    }
}