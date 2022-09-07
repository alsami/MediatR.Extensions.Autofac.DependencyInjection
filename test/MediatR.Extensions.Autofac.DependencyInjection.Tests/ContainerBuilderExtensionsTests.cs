using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using FluentAssertions;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using MediatR.Extensions.Autofac.DependencyInjection.Tests.Behaviors;
using MediatR.Extensions.Autofac.DependencyInjection.Tests.Commands;
using MediatR.Extensions.Autofac.DependencyInjection.Tests.ExceptionActions;
using MediatR.Extensions.Autofac.DependencyInjection.Tests.ExceptionHandler;
using MediatR.Extensions.Autofac.DependencyInjection.Tests.Handler;
using MediatR.Pipeline;
using Xunit;
// ReSharper disable UnusedVariable

namespace MediatR.Extensions.Autofac.DependencyInjection.Tests;

public class ContainerBuilderExtensionsTests : IAsyncLifetime
{
    private readonly ContainerBuilder builder;
    private IContainer container;

    public ContainerBuilderExtensionsTests()
    {
        this.builder = new ContainerBuilder();
    }

    [Fact]
    public void RegisterMediatR_ConfigurationProvided_ExpectInstances()
    {
        var configuration = MediatRConfigurationBuilder
            .Create(typeof(ResponseCommand).Assembly)
            .WithAllOpenGenericHandlerTypesRegistered()
            .Build();

        this.container = this.builder.RegisterMediatR(configuration).Build();
        
        this.AssertServiceRegistered();
        this.AssertServiceResolvable();
    }
    
    [Fact]
    public void RegisterMediatR_Manual_ExpectInstances()
    {
        var configuration = MediatRConfigurationBuilder
            .Create(typeof(ResponseCommand).Assembly)
            .WithRequestHandlersManuallyRegistered()
            .Build();

         this.builder
            .RegisterMediatR(configuration)
            .RegisterType<ResponseCommandHandler>()
            .As<IRequestHandler<ResponseCommand, Response>>();

         this.container = this.builder.Build();
        
         Assert.True(this.container.IsRegistered<IRequestHandler<ResponseCommand, Response>>(), "Responsehandler not registered");
    }
    
    [Fact]
    public void RegisterMediatR_ConfigurationProvidedWithCustomBehaviors_Resolvable()
    {
        var configuration = MediatRConfigurationBuilder
            .Create(typeof(ResponseCommand).Assembly)
            .WithCustomPipelineBehavior(typeof(LoggingBehavior<,>))
            .WithAllOpenGenericHandlerTypesRegistered()
            .Build();

        this.container = this.builder.RegisterMediatR(configuration).Build();
        
        this.AssertServiceRegistered();
        this.AssertServiceResolvable();
        this.container.IsRegistered(typeof(IPipelineBehavior<ResponseCommand, Response>));
        var behaviors = this.container.Resolve<IEnumerable<IPipelineBehavior<ResponseCommand, Response>>>();
        behaviors
            .Select(type => type.GetType())
            .Should()
            .Contain(typeof(LoggingBehavior<ResponseCommand, Response>));
    }

    [Fact]
    public void ContainerBuilderExtensions_RegisterMediatRWithAssembliesResolveTypes_ExpectInstances()
    {
        this.container = this.builder
            .RegisterMediatR(typeof(ResponseCommand).Assembly, typeof(ResponseCommand).Assembly)
            .Build();

        this.AssertServiceRegistered();
        this.AssertServiceResolvable();
    }

    [Fact]
    public async Task ContainerBuilderExtensions_RegisterMediatRWithAssembliesAndCustomBehaviorsResolveTypes_ExpectInstances()
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
        this.container.Resolve<IRequestHandler<ResponseCommand, Response>>();
        this.container.Resolve<IRequestHandler<VoidCommand>>();
        this.container.Resolve<INotificationHandler<SampleNotification>>();
        var behaviors = this.container.Resolve<IEnumerable<IPipelineBehavior<ResponseCommand, Response>>>();
        Assert.Equal(6, behaviors.Count());
        await mediator.Send(new ResponseCommand());
        Assert.Equal(1, NoopBehavior<ResponseCommand, Response>.HitCount);
    }
        
    [Fact]
    public async Task ContainerBuilderExtensions_RegisterMediatRWithAssembliesAndCustomBehaviorsUnconstrainted_ResolveTypes_ExpectInstances()
    {
        this.container = this.builder.RegisterMediatR(typeof(ResponseCommand).Assembly, typeof(LoggingBehavior<,>))
            .Build();

        Assert.True(this.container.IsRegistered<IMediator>(), "Mediator not registered!");
        Assert.True(this.container.IsRegistered<ServiceFactory>(), "ServiceFactory not registered");
        Assert.True(this.container.IsRegistered<IPipelineBehavior<UnconstraintedCommand, int>>(),
            "PiplineBehavior not registered");
        Assert.True(this.container.IsRegistered<IRequestHandler<UnconstraintedCommand, int>>(),
            "Responsehandler not registered");

        var mediator = this.container.Resolve<IMediator>();
        this.container.Resolve<IRequestHandler<UnconstraintedCommand, int>>();
        var behaviors = this.container.Resolve<IEnumerable<IPipelineBehavior<UnconstraintedCommand, int>>>();
        Assert.Equal(5, behaviors.Count());
        await mediator.Send(new UnconstraintedCommand());
    }

    [Fact]
    public void ContainerBuilderExtensions_RegisterMediatRNullAssemblies_ExpectExceptions()
    {
        Assert.Throws<ArgumentNullException>(() => this.builder.RegisterMediatR(((Assembly) null!)!));
        Assert.Throws<ArgumentNullException>(() => this.builder.RegisterMediatR(((ICollection<Assembly>) null)!));
    }

    [Fact]
    public async Task
        ContainerBuilderExtensions_RegisterMediatR_Call_Throwing_Handler_Exception_Handlers_Called()
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
        ContainerBuilderExtensions_RegisterMediatR_Call_Throwing_Handler_Exception_Actions_Called()
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
    
    private void AssertServiceResolvable()
    {
        this.container.Resolve<IMediator>();
        this.container.Resolve<IRequestHandler<ResponseCommand, Response>>();
        this.container.Resolve<IRequestHandler<VoidCommand>>();
        this.container.Resolve<INotificationHandler<SampleNotification>>();
    }

    private void AssertServiceRegistered()
    {
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
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        if (this.container is null)
        {
            return;
        }

        await this.container.DisposeAsync();
    }
}