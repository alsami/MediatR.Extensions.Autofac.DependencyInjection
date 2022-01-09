using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection.Tests.Behaviors;
using MediatR.Extensions.Autofac.DependencyInjection.Tests.Commands;
using MediatR.Extensions.Autofac.DependencyInjection.Tests.ExceptionActions;
using MediatR.Extensions.Autofac.DependencyInjection.Tests.ExceptionHandler;
using MediatR.Pipeline;
using Xunit;
// ReSharper disable UnusedVariable

namespace MediatR.Extensions.Autofac.DependencyInjection.Tests;

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
        this.container = this.builder.RegisterMediatR(typeof(ResponseCommand).Assembly, typeof(ResponseCommand).Assembly)
            .Build();

        Assert.True(this.container.IsRegistered<IMediator>(), "Mediator not registered!");
        Assert.True(this.container.IsRegistered<ISender>(), "ISender not registered!");
        Assert.True(this.container.IsRegistered<IPublisher>(), "IPublisher not registered!");
        Assert.True(this.container.IsRegistered<ServiceFactory>(), "ServiceFactory not registered");
        Assert.True(this.container.IsRegistered<IPipelineBehavior<ResponseCommand, Response>>(), "PiplineBehavior not registered");
        Assert.True(this.container.IsRegistered<IRequestHandler<ResponseCommand, Response>>(), "Responsehandler not registered");
        Assert.True(this.container.IsRegistered<IRequestHandler<VoidCommand>>(), "Voidhandler not registered");
        Assert.True(this.container.IsRegistered<IRequestPreProcessor<VoidCommand>>(), "Void Request Pre Processor not registered");
        Assert.True(this.container.IsRegistered<IRequestPostProcessor<VoidCommand, Unit>>(), "Void Request Post Processor not registered");
        Assert.True(this.container.IsRegistered<INotificationHandler<SampleNotification>>());

        var mediator = this.container.Resolve<IMediator>();
        var responseHandler = this.container.Resolve<IRequestHandler<ResponseCommand, Response>>();
        var voidhandler = this.container.Resolve<IRequestHandler<VoidCommand>>();
        var notificationhandler = this.container.Resolve<INotificationHandler<SampleNotification>>();
    }
        
    [Fact]
    public async Task ContainerBuilderExtensions_AddMediatRWithAssembliesAndCustomBehaviorsResolveTypes_ExpectInstances()
    {
        this.container = this.builder.RegisterMediatR(typeof(ResponseCommand).Assembly, typeof(LoggingBehavior<,>), typeof(NoopBehavior<,>))
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
        Assert.Equal(6, behaviors.Count());
        await mediator.Send(new ResponseCommand());
        Assert.Equal(1, NoopBehavior<ResponseCommand, Response>.HitCount);
    }
        
    [Fact]
    public async Task ContainerBuilderExtensions_AddMediatRWithAssembliesAndCustomBehaviorsUnconstrainted_ResolveTypes_ExpectInstances()
    {
        this.container = this.builder.RegisterMediatR(typeof(ResponseCommand).Assembly, typeof(LoggingBehavior<,>), typeof(UnconstrainedBehavior<,>))
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
        Assert.Equal(6, behaviors.Count());
        await mediator.Send(new UnconstraintedCommand());
    }

    [Fact]
    public void ContainerBuilderExtensions_AddMediatRNullAssemblies_ExpectExceptions()
    {
        Assert.Throws<ArgumentNullException>(() => this.builder.RegisterMediatR((Assembly) null));
        Assert.Throws<ArgumentNullException>(() => this.builder.RegisterMediatR((ICollection<Assembly>) null));
    }

    [Fact]
    public async Task
        ContainerBuilderExtensions_AddMediatR_Call_Throwing_Handler_Exception_Handlers_Called()
    {
        var currentContainer = new ContainerBuilder()
            .RegisterMediatR(typeof(CommandThatThrowsArgumentException).Assembly)
            .Build();

        var mediator = currentContainer.Resolve<IMediator>();
            
        await mediator.Send(new CommandThatThrowsArgumentException());
        Assert.Equal(1, NonSpecificExceptionHandler.CallCount);
        Assert.Equal(1, CommandThatThrowsArgumentExceptionHandler.CallCount);
        // NonSpecificExceptionHandler must be called last!
        Assert.True(NonSpecificExceptionHandler.CallTime.Ticks > CommandThatThrowsArgumentExceptionHandler.CallTime.Ticks);
    }
        
    [Fact]
    public async Task
        ContainerBuilderExtensions_AddMediatR_Call_Throwing_Handler_Exception_Actions_Called()
    {
        var currentContainer = new ContainerBuilder()
            .RegisterMediatR(typeof(CommandThatThrowsNullRefException).Assembly)
            .Build();

        var mediator = currentContainer.Resolve<IMediator>();

        try
        {
            await mediator.Send(new CommandThatThrowsNullRefException());
        }
        catch (NullReferenceException)
        {
            Assert.Equal(1, CommandThatThrowsNullRefExceptionActionHandler.CallCount);
            return;
        }
            
        Assert.True(false, "This code shouldn't be reached");
    }

    public void Dispose()
    {
        this.container?.Dispose();
    }
}