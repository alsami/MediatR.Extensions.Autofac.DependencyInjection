using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection.TestInfrastructure.Commands;
using MediatR.Pipeline;
using Xunit;

namespace MediatR.Extensions.Autofac.DependencyInjection.IntegrationTests
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
            this.container = this.builder.AddMediatR(typeof(ResponseCommand).Assembly)
                .Build();

            Assert.True(this.container.IsRegistered<IMediator>(), "Mediator not registered!");
            Assert.True(this.container.IsRegistered<ServiceFactory>(), "ServiceFactory not registered");
            Assert.True(this.container.IsRegistered<IPipelineBehavior<ResponseCommand, Response>>(), "PiplineBehavior not registered");
            Assert.True(this.container.IsRegistered<IPipelineBehavior<ResponseCommand, Unit>>(), "PiplineBehavior not registered");
            Assert.True(this.container.IsRegistered<IRequestHandler<ResponseCommand, Response>>(), "Responsehandler not registered");
            Assert.True(this.container.IsRegistered<IRequestHandler<VoidCommand>>(), "Voidhandler not registered");
            Assert.True(this.container.IsRegistered<INotificationHandler<SampleNotification>>());

            var mediator = this.container.Resolve<IMediator>();
            var responseHandler = this.container.Resolve<IRequestHandler<ResponseCommand, Response>>();
            var voidhandler = this.container.Resolve<IRequestHandler<VoidCommand>>();
            var notificationhandler = this.container.Resolve<INotificationHandler<SampleNotification>>();
        }

        [Fact]
        public void ContainerBuilderExtensions_AddMediatRNullAssemblies_ExpectExceptions()
        {
            Assert.Throws<ArgumentNullException>(() => this.builder.AddMediatR(null));
            Assert.Throws<ArgumentNullException>(() => this.builder.AddMediatR((Assembly) null));
            Assert.Throws<ArgumentNullException>(() => this.builder.AddMediatR((ICollection<Assembly>) null));
        }
        

        public void Dispose()
        {
            this.container?.Dispose();
        }
    }
}