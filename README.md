# MediatR.Extensions.Autofac.DependencyInjection

[![Build Application](https://github.com/alsami/MediatR.Extensions.Autofac.DependencyInjection/actions/workflows/push.yml/badge.svg?branch=main&event=push)](https://github.com/alsami/MediatR.Extensions.Autofac.DependencyInjection/actions/workflows/push.yml)
[![codecov](https://codecov.io/gh/alsami/MediatR.Extensions.Autofac.DependencyInjection/branch/master/graph/badge.svg)](https://codecov.io/gh/alsami/MediatR.Extensions.Autofac.DependencyInjection)

[![NuGet](https://img.shields.io/nuget/vpre/MediatR.Extensions.Autofac.DependencyInjection.svg)](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection)
![Nuget](https://img.shields.io/nuget/dt/MediatR.Extensions.Autofac.DependencyInjection)

This is a cross platform library, supporting .NET standard 2.0, that serves as an extension for [Autofac's containerbuilder](https://autofac.org/).
It will register all necessary classes and interfaces of Jimmy Bogard's [MediatR](https://github.com/jbogard/MediatR) implementation to the autofac-container 
so you can use cqrs and the [mediator pattern](https://sourcemaking.com/design_patterns/mediator) right ahread without worrying about setting up required infrastracture code.

## Installation

This package is available via nuget. You can install it using Visual-Studio-Nuget-Browser or by using the dotnet-cli

```
dotnet add package MediatR.Extensions.Autofac.DependencyInjection
```

If you want to add a specific version of this package

```
dotnet add package MediatR.Extensions.Autofac.DependencyInjection --version 1.0.0
```

For more information please visit the official [dotnet-cli documentation](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-add-package).

## Usage

After you have successfully installed the package go ahead to your class and use the extension.

```c#
public class Pong 
{
    public DateTime ResponseSendAt { get; }
    
    public Pong(responseSendAt)
    {
        this.ResponseSendAt = responseSendAt;
    }
}

public class PingCommand : IRequest<Pong> {} // Command

public class PingCommandHandler : IRequestHandler<PingCommand, Pong>
{
    public async Task Handle(PingCommand request, CancellationToken cancellationToken) 
    {
        return await Task.FromResult(new Pong(DateTime.UtcNow)).ConfigureAwait(false);
    } 
}

public class Program 
{
    public async Task Main(string[] args)
    {
        var builder = new ContainerBuilder();

        var configuration = MediatRConfigurationBuilder
            .Create("yourLicenseKeyGoesHere", typeof(PingCommand).Assembly)
            .WithAllOpenGenericHandlerTypesRegistered()
            .Build();

        // this will add all your Request- and Notificationhandler
        // that are located in the same project as your program-class
        builder.RegisterMediatR(configuration);
        
        var container = builder.Build();
        
        var mediator = container.Resolve<IMediator>();
        
        var response = await mediator.Send(new PingCommand());
        
        // more code here
    }
}
```

By default all `MediatR` dependencies will be registered as transient (`InstancePerDependency`). Starting with version `9.2.0` you can configure the scope

```csharp
var builder = new ContainerBuilder();

var configuration = MediatRConfigurationBuilder
    .Create("yourLicenseKeyGoesHere", typeof(PingCommand).Assembly)
    .WithAllOpenGenericHandlerTypesRegistered()
    .WithRegistrationScope(RegistrationScope.Scoped) // currently only supported values are `Transient` and `Scoped`
    .Build();

builder.RegisterMediatR(configuration);
```

## License key

`MediatR` requires a license key. You can read more about how to acquire one [here](https://github.com/LuckyPennySoftware/MediatR/releases). There are two ways to supply it.

Pass the key to `Create`:

```csharp
var configuration = MediatRConfigurationBuilder
    .Create("yourLicenseKeyGoesHere", typeof(PingCommand).Assembly)
    .WithAllOpenGenericHandlerTypesRegistered()
    .Build();
```

Or, starting with version `14.3.0`, read it from a service registered in the container, such as a secret store, with `WithLicenseKeyFactory`.
If you only use the factory or environment variables, you can pass `null` to `Create`.

```csharp
var configuration = MediatRConfigurationBuilder
    .Create(null, typeof(PingCommand).Assembly)
    .WithAllOpenGenericHandlerTypesRegistered()
    .WithLicenseKeyFactory(serviceProvider => serviceProvider.GetRequiredService<ISecretStore>().Get("MediatR"))
    .Build();
```

`MediatR` uses the first key it finds, in this order: the factory result, the key passed to `Create`, the static `Mediator.LicenseKey`, then environment variables.

Keep the following in mind when using the factory:

* It receives the root service provider, not the lifetime scope that resolved `IMediator`.
  Autofac's root scope also resolves `InstancePerLifetimeScope` services, so only resolve what you need to read the key and don't keep a reference to it.
* It runs at most once per process, on a background thread, the first time a `Mediator` is constructed.
* Exceptions thrown by the factory are only logged when an `ILoggerFactory` is registered in the container, so register logging to see them.

For more information about the usage please check out the [samples](https://github.com/alsami/MediatR.Extensions.Autofac.DependencyInjection/tree/main/samples) and [tests](https://github.com/alsami/MediatR.Extensions.Autofac.DependencyInjection/tree/main/test) of the solution.
