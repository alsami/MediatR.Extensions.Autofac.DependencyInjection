using System;
using Autofac;
using MediatR.Extensions.AutoFac.DependencyInjection.TestInfrastructure.Commands;
using MediatR.Pipeline;
using Xunit;

namespace MediatR.Extensions.Autofac.DependencyInjection.IntegrationTests
{
    public class ContainerExtensionsTest : IDisposable
    {
        private readonly ContainerBuilder builder;
        private IContainer container;

        public ContainerExtensionsTest()
        {
            this.builder = new ContainerBuilder();
        }
        
        [Fact]
        public void ContainerExtensions_AddMediatRWithAssembliesResolveTypes_ExpectInstances()
        {
            this.builder.AddMediatR(typeof(ResponseCommand).Assembly);
            this.container = this.builder.Build();

            Assert.True(this.container.IsRegistered<IMediator>(), "Mediator not registered!");
            Assert.True(this.container.IsRegistered<ServiceFactory>(), "ServiceFactory not registered");
            Assert.True(this.container.IsRegistered<IPipelineBehavior<ResponseCommand, Response>>(), "PiplineBehavior not registered");
            Assert.True(this.container.IsRegistered<IPipelineBehavior<ResponseCommand, Unit>>(), "PiplineBehavior not registered");
            Assert.True(this.container.IsRegistered<IRequestHandler<ResponseCommand, Response>>(), "Responsehandler not registered");
            Assert.True(this.container.IsRegistered<IRequestHandler<VoidCommand>>(), "Voidhandler not registered");
        }

        public void Dispose()
        {
            this.container?.Dispose();
        }
    }
}