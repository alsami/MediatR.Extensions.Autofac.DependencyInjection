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

    [Fact]
    public void WithLicenseKeyFactory_Null_ThrowsArgumentNullException()
    {
        var act = () => MediatRConfigurationBuilder
            .Create(LicenseKey, typeof(ResponseCommand).Assembly)
            .WithLicenseKeyFactory(null);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Create_NullLicenseKey_MediatorResolvable()
    {
        var configuration = MediatRConfigurationBuilder
            .Create(null, typeof(ResponseCommand).Assembly)
            .WithAllOpenGenericHandlerTypesRegistered()
            .Build();

        this.container = new ContainerBuilder().RegisterMediatR(configuration).Build();

        this.container.Resolve<IMediator>().Should().NotBeNull();
    }

    [Fact]
    public void WithLicenseKeyFactory_LicenseRead_FactoryReceivesProviderResolvingContainerServices()
    {
        var keySource = new KeySource();
        var invocations = 0;
        IKeySource resolvedKeySource = null;

        var configuration = MediatRConfigurationBuilder
            .Create(null, typeof(ResponseCommand).Assembly)
            .WithAllOpenGenericHandlerTypesRegistered()
            .WithLicenseKeyFactory(serviceProvider =>
            {
                invocations++;
                resolvedKeySource = (IKeySource)serviceProvider.GetService(typeof(IKeySource));
                return resolvedKeySource?.Key;
            })
            .Build();

        var builder = new ContainerBuilder();
        builder.RegisterInstance(keySource).As<IKeySource>();
        this.container = builder.RegisterMediatR(configuration).Build();

        ReadCurrentLicense(this.container.Resolve(LicenseAccessorType));

        invocations.Should().Be(1);
        resolvedKeySource.Should().BeSameAs(keySource);
    }

    [Fact]
    public void RegisterMediatR_NoLicenseKeyFactory_LicenseReadFromConfiguredKey()
    {
        this.container = BuildContainer(new ContainerBuilder());

        var license = ReadCurrentLicense(this.container.Resolve(LicenseAccessorType));

        license.Should().NotBeNull();
    }

    private static object ReadCurrentLicense(object licenseAccessor)
        => LicenseAccessorType.GetProperty("Current")!.GetValue(licenseAccessor);

    private static IContainer BuildContainer(ContainerBuilder builder)
    {
        var configuration = MediatRConfigurationBuilder
            .Create(LicenseKey, typeof(ResponseCommand).Assembly)
            .WithAllOpenGenericHandlerTypesRegistered()
            .Build();

        return builder.RegisterMediatR(configuration).Build();
    }

    public interface IKeySource
    {
        string Key { get; }
    }

    private sealed class KeySource : IKeySource
    {
        public string Key => "KeyFromContainer";
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
