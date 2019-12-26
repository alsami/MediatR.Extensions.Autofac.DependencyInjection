using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection.TestInfrastructure.Behaviors;
using MediatR.Extensions.Autofac.DependencyInjection.TestInfrastructure.Commands;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Xunit;
// ReSharper disable UnusedVariable

namespace MediatR.Extensions.Autofac.DependencyInjection.Tests
{
    public class ContainerBuilderExtensionsTests : IDisposable
    {
        private readonly ContainerBuilder builder;
        private IContainer container;

        public ContainerBuilderExtensionsTests()
        {
            this.builder = new ContainerBuilder();
        }

        [Fact]
        public void ContainerBuilderExtensions_AddMediatRWithAssembliesResolveTypes_ExpectInstances()
        {
            this.container = this.builder.AddMediatR(typeof(ResponseCommand).Assembly, typeof(ResponseCommand).Assembly)
                .Build();

            Assert.True(this.container.IsRegistered<IMediator>(), "Mediator not registered!");
            Assert.True(this.container.IsRegistered<ServiceFactory>(), "ServiceFactory not registered");
            Assert.True(this.container.IsRegistered<IPipelineBehavior<ResponseCommand, Response>>(),
                "PiplineBehavior not registered");
            Assert.True(this.container.IsRegistered<IPipelineBehavior<ResponseCommand, Unit>>(),
                "PiplineBehavior not registered");
            Assert.True(this.container.IsRegistered<IRequestHandler<ResponseCommand, Response>>(),
                "Responsehandler not registered");
            Assert.True(this.container.IsRegistered<IRequestHandler<VoidCommand>>(), "Voidhandler not registered");
            Assert.True(this.container.IsRegistered<INotificationHandler<SampleNotification>>());

            var mediator = this.container.Resolve<IMediator>();
            var responseHandler = this.container.Resolve<IRequestHandler<ResponseCommand, Response>>();
            var voidhandler = this.container.Resolve<IRequestHandler<VoidCommand>>();
            var notificationhandler = this.container.Resolve<INotificationHandler<SampleNotification>>();
        }
        
        [Fact]
        public async Task ContainerBuilderExtensions_AddMediatRWithAssembliesAndCustomBehaviorsResolveTypes_ExpectInstances()
        {
            this.container = this.builder.AddMediatR(typeof(ResponseCommand).Assembly, typeof(LoggingBehavior<,>), typeof(NoopBehavior<,>))
                .Build();

            Assert.True(this.container.IsRegistered<IMediator>(), "Mediator not registered!");
            Assert.True(this.container.IsRegistered<ServiceFactory>(), "ServiceFactory not registered");
            Assert.True(this.container.IsRegistered<IPipelineBehavior<ResponseCommand, Response>>(),
                "PiplineBehavior not registered");
            Assert.True(this.container.IsRegistered<IRequestHandler<ResponseCommand, Response>>(),
                "Responsehandler not registered");
            Assert.True(this.container.IsRegistered<IRequestHandler<VoidCommand>>(), "Voidhandler not registered");
            Assert.True(this.container.IsRegistered<INotificationHandler<SampleNotification>>());

            var mediator = this.container.Resolve<IMediator>();
            var responseHandler = this.container.Resolve<IRequestHandler<ResponseCommand, Response>>();
            var voidhandler = this.container.Resolve<IRequestHandler<VoidCommand>>();
            var notificationhandler = this.container.Resolve<INotificationHandler<SampleNotification>>();
            var behaviors = this.container.Resolve<IEnumerable<IPipelineBehavior<ResponseCommand, Response>>>();
            Assert.Equal(4, behaviors.Count());
            await mediator.Send(new ResponseCommand());
            Assert.Equal(1, NoopBehavior<ResponseCommand, Response>.HitCount);
        }
        
        [Fact]
        public async Task ContainerBuilderExtensions_AddMediatRWithAssembliesAndCustomBehaviorsUnconstrainted_ResolveTypes_ExpectInstances()
        {
            this.container = this.builder.AddMediatR(typeof(ResponseCommand).Assembly, typeof(LoggingBehavior<,>), typeof(UnconstraintedBehavior<,>))
                .Build();

            Assert.True(this.container.IsRegistered<IMediator>(), "Mediator not registered!");
            Assert.True(this.container.IsRegistered<ServiceFactory>(), "ServiceFactory not registered");
            Assert.True(this.container.IsRegistered<IPipelineBehavior<UnconstraintedCommand, int>>(),
                "PiplineBehavior not registered");
            Assert.True(this.container.IsRegistered<IRequestHandler<UnconstraintedCommand, int>>(),
                "Responsehandler not registered");

            var mediator = this.container.Resolve<IMediator>();
            var responseHandler = this.container.Resolve<IRequestHandler<UnconstraintedCommand, int>>();
            var behaviors = this.container.Resolve<IEnumerable<IPipelineBehavior<UnconstraintedCommand, int>>>();
            Assert.Equal(4, behaviors.Count());
            await mediator.Send(new UnconstraintedCommand());
        }

        [Fact]
        public void ContainerBuilderExtensions_AddMediatRNullAssemblies_ExpectExceptions()
        {
            Assert.Throws<ArgumentNullException>(() => this.builder.AddMediatR((Assembly) null));
            Assert.Throws<ArgumentNullException>(() => this.builder.AddMediatR((ICollection<Assembly>) null));
        }


        public void Dispose()
        {
            this.container?.Dispose();
        }
    }
}