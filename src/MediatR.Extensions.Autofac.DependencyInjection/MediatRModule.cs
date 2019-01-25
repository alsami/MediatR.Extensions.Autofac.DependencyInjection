using System.Reflection;
using Autofac;
using MediatR.Pipeline;
using Module = Autofac.Module;

namespace MediatR.Extensions.Autofac.DependencyInjection
{
    internal class MediatRModule : Module
    {
        private readonly Assembly[] assemblies;

        public MediatRModule(Assembly[] assemblies)
        {
            this.assemblies = assemblies;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            var openHandlerTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(INotificationHandler<>),
            };

            foreach (var openHandlerType in openHandlerTypes)
            {
                builder.RegisterAssemblyTypes(this.assemblies)
                    .AsClosedTypesOf(openHandlerType)
                    .AsImplementedInterfaces();
            }

            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            builder.Register<ServiceFactory>(outerContext =>
            {
                var innerContext = outerContext.Resolve<IComponentContext>();

                return type => innerContext.Resolve(type);
            });
        }
    }
}