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
            builder.RegisterAssemblyTypes(typeof(IMediator).Assembly)
                .AsImplementedInterfaces();

            var openGenericTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(INotificationHandler<>),
            };

            foreach (var openGenericType in openGenericTypes)
            {
                builder.RegisterAssemblyTypes(this.assemblies)
                    .AsClosedTypesOf(openGenericType)
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