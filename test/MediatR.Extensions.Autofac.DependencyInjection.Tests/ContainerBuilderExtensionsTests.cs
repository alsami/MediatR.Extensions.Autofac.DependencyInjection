using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Features.AttributeFilters;
using FluentAssertions;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using MediatR.Extensions.Autofac.DependencyInjection.Tests.Behaviors;
using MediatR.Extensions.Autofac.DependencyInjection.Tests.Commands;
using MediatR.Extensions.Autofac.DependencyInjection.Tests.CustomTypes;
using MediatR.Extensions.Autofac.DependencyInjection.Tests.Handler;
using MediatR.Extensions.Autofac.DependencyInjection.Tests.Services;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

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
    public void RegisterMediatR_RegistrationScopeScoped_InstancesSameInScope()
    {
        var configuration = MediatRConfigurationBuilder
            .Create(typeof(ResponseCommand).Assembly)
            .WithAllOpenGenericHandlerTypesRegistered()
            .WithRegistrationScope(RegistrationScope.Scoped)
            .Build();

        this.container = this.builder.RegisterMediatR(configuration).Build();
        
        this.AssertServiceRegistered();
        this.AssertServiceResolvable();
        var mediatorOne = this.container.Resolve<IMediator>();
        var mediatorTwo = this.container.Resolve<IMediator>();
        mediatorOne.Should().BeSameAs(mediatorTwo);
    }
    
    [Fact]
    public void RegisterMediatR_RegistrationScopeTransient_InstancesNotSameInScope()
    {
        var configuration = MediatRConfigurationBuilder
            .Create(typeof(ResponseCommand).Assembly)
            .WithAllOpenGenericHandlerTypesRegistered()
            .WithRegistrationScope(RegistrationScope.Transient)
            .Build();

        this.container = this.builder.RegisterMediatR(configuration).Build();
        
        this.AssertServiceRegistered();
        this.AssertServiceResolvable();
        var mediatorOne = this.container.Resolve<IMediator>();
        var mediatorTwo = this.container.Resolve<IMediator>();
        mediatorOne.Should().NotBeSameAs(mediatorTwo);
    }
    
    [Fact]
    public void RegisterMediatR_CustomMediatorProvided_ExpectInstances()
    {
        var configuration = MediatRConfigurationBuilder
            .Create(typeof(ResponseCommand).Assembly)
            .UseMediatorType(typeof(CustomMediator))
            .WithAllOpenGenericHandlerTypesRegistered()
            .Build();

        this.container = this.builder.RegisterMediatR(configuration).Build();
        
        this.AssertServiceRegistered();
        this.AssertServiceResolvable();
        var publisher = this.container.Resolve<IMediator>();
        publisher.Should().BeOfType<CustomMediator>();
    }
    
    [Fact]
    public void RegisterMediatR_CustomPublisherProvided_ExpectInstances()
    {
        var configuration = MediatRConfigurationBuilder
            .Create(typeof(ResponseCommand).Assembly)
            .UseNotificationPublisher(typeof(CustomNotificationPublisher))
            .WithAllOpenGenericHandlerTypesRegistered()
            .Build();

        this.container = this.builder.RegisterMediatR(configuration).Build();
        
        this.AssertServiceRegistered();
        this.AssertServiceResolvable();
        var publisher = this.container.Resolve<INotificationPublisher>();
        publisher.Should().BeOfType<CustomNotificationPublisher>();
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
    public void RegisterMediatR_ServiceProviderNotProvided_WrapperResolved()
    {
        var configuration = MediatRConfigurationBuilder
            .Create(typeof(ResponseCommand).Assembly)
            .WithAllOpenGenericHandlerTypesRegistered()
            .Build();

        this.container = this.builder.RegisterMediatR(configuration).Build();
        
        this.AssertServiceRegistered();
        this.AssertServiceResolvable();
        var serviceProvider = this.container.Resolve<IServiceProvider>();
        serviceProvider.Should().BeOfType<ServiceProviderWrapper>();
    }
    
    [Fact]
    public void RegisterMediatR_ServiceProviderProvidedFromOutside_AutofacServiceProviderResolved()
    {
        this.builder.Populate(new ServiceCollection());
        
        var configuration = MediatRConfigurationBuilder
            .Create(typeof(ResponseCommand).Assembly)
            .WithAllOpenGenericHandlerTypesRegistered()
            .Build();

        this.container = this.builder.RegisterMediatR(configuration).Build();
        
        this.AssertServiceRegistered();
        this.AssertServiceResolvable();
        var serviceProvider = this.container.Resolve<IServiceProvider>();
        serviceProvider.Should().BeOfType<AutofacServiceProvider>();
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
    public void RegisterMediatR_ConfigurationProvidedWithOpenGenericCallback()
    {
        var configuration = MediatRConfigurationBuilder
            .Create(typeof(ResponseCommand).Assembly)
            .WithAllOpenGenericHandlerTypesRegistered()
            .WithOpenGenericHandlerTypeToRegisterCallback(x =>
            {
                x.WithAttributeFiltering();
            })
            .Build();
        this.builder.RegisterType<EstDateTimeConverterService>().Keyed<IDateTimeConverterService>("EST");
        this.builder.RegisterType<UtcDateTimeConverterService>().Keyed<IDateTimeConverterService>("UTC");
        this.builder.RegisterMediatR(configuration);
        this.container = this.builder.Build();

        this.container.ResolveKeyed<IDateTimeConverterService>("EST").Should().BeOfType<EstDateTimeConverterService>();
        this.container.ResolveKeyed<IDateTimeConverterService>("UTC").Should().BeOfType<UtcDateTimeConverterService>();
        Assert.True(this.container.IsRegistered<IRequestHandler<ConvertEstDateTimeCommand, DateTime>>(), "EST KeyFilter Response handler not registered");
        Assert.True(this.container.IsRegistered<IRequestHandler<ConvertUtcDateTimeCommand, DateTime>>(), "UTC KeyFilter Response handler not registered");
    }
    
    private void AssertServiceResolvable()
    {
        this.container.Resolve<IServiceProvider>();
        this.container.Resolve<IMediator>();
        this.container.Resolve<IRequestHandler<ResponseCommand, Response>>();
        this.container.Resolve<IRequestHandler<VoidCommand>>();
        this.container.Resolve<INotificationHandler<SampleNotification>>();
    }

    private void AssertServiceRegistered()
    {
        Assert.True(this.container.IsRegistered<IServiceProvider>(), "IServiceProvider not registered!");
        Assert.True(this.container.IsRegistered<INotificationPublisher>(), "INotificationPublisher not registered!");
        Assert.True(this.container.IsRegistered<IMediator>(), "Mediator not registered!");
        Assert.True(this.container.IsRegistered<ISender>(), "ISender not registered!");
        Assert.True(this.container.IsRegistered<IPublisher>(), "IPublisher not registered!");
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