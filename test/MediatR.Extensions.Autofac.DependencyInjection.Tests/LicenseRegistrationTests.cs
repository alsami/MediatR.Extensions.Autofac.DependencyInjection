using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentAssertions;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using MediatR.Extensions.Autofac.DependencyInjection.Extensions;
using MediatR.Extensions.Autofac.DependencyInjection.Tests.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace MediatR.Extensions.Autofac.DependencyInjection.Tests;

public class LicenseRegistrationTests : IAsyncLifetime
{
    private const string LicenseKey = "LicenseKey";

    private static readonly Type LicenseAccessorType =
        typeof(Mediator).Assembly.GetType("MediatR.Licensing.LicenseAccessor", throwOnError: true);

    private static readonly Type LicenseValidatorType =
        typeof(Mediator).Assembly.GetType("MediatR.Licensing.LicenseValidator", throwOnError: true);

    private IContainer container;

    [Fact]
    public void RegisterMediatR_NoLoggerFactory_LicenseAccessorResolvedAsSingleton()
    {
        this.container = BuildContainer(new ContainerBuilder());

        var first = this.container.Resolve(LicenseAccessorType);
        var second = this.container.Resolve(LicenseAccessorType);

        first.Should().NotBeNull();
        first.Should().BeSameAs(second);
    }

    [Fact]
    public void RegisterMediatR_NoLoggerFactory_LicenseValidatorResolvedAsSingleton()
    {
        this.container = BuildContainer(new ContainerBuilder());

        var first = this.container.Resolve(LicenseValidatorType);
        var second = this.container.Resolve(LicenseValidatorType);

        first.Should().NotBeNull();
        first.Should().BeSameAs(second);
    }

    [Fact]
    public async Task RegisterMediatR_PopulatedWithLogging_MediatorSendsCommand()
    {
        var builder = new ContainerBuilder();
        builder.Populate(new ServiceCollection().AddSingleton<ILoggerFactory, NullLoggerFactory>());
        this.container = BuildContainer(builder);

        var mediator = this.container.Resolve<IMediator>();
        var response = await mediator.Send(new ResponseCommand());

        response.Should().NotBeNull();
    }

    [Fact]
    public void RegisterMediatR_RepeatedResolves_DoNotThrow()
    {
        this.container = BuildContainer(new ContainerBuilder());

        var resolveRepeatedly = () =>
        {
            for (var i = 0; i < 5; i++)
            {
                this.container.Resolve<IMediator>();
            }
        };

        resolveRepeatedly.Should().NotThrow();
    }

    [Fact]
    public void GetRequiredPublicConstructor_SignatureMissing_ThrowsNamingTypeAndParameters()
    {
        var act = () => typeof(Response).GetRequiredPublicConstructor(typeof(string), typeof(int));

        act.Should().Throw<InvalidOperationException>()
            .Which.Message.Should()
            .Contain(typeof(Response).FullName)
            .And.Contain(typeof(string).FullName)
            .And.Contain(typeof(int).FullName);
    }

    [Fact]
    public void GetRequiredPublicConstructor_SignatureExists_ReturnsConstructor()
    {
        var constructor = typeof(Response).GetRequiredPublicConstructor(typeof(DateTime));

        constructor.GetParameters().Should().ContainSingle()
            .Which.ParameterType.Should().Be(typeof(DateTime));
    }

    private static IContainer BuildContainer(ContainerBuilder builder)
    {
        var configuration = MediatRConfigurationBuilder
            .Create(LicenseKey, typeof(ResponseCommand).Assembly)
            .WithAllOpenGenericHandlerTypesRegistered()
            .Build();

        return builder.RegisterMediatR(configuration).Build();
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
