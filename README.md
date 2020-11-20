# MediatR.Extensions.Autofac.DependencyInjection
[![Build Status](https://travis-ci.com/alsami/MediatR.Extensions.Autofac.DependencyInjection.svg?branch=master)](https://travis-ci.com/alsami/MediatR.Extensions.Autofac.DependencyInjection)
[![codecov](https://codecov.io/gh/alsami/MediatR.Extensions.Autofac.DependencyInjection/branch/master/graph/badge.svg)](https://codecov.io/gh/alsami/MediatR.Extensions.Autofac.DependencyInjection)

[![NuGet](https://img.shields.io/nuget/vpre/MediatR.Extensions.Autofac.DependencyInjection.svg)](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection)
![Nuget](https://img.shields.io/nuget/dt/MediatR.Extensions.Autofac.DependencyInjection)

This is a cross platform library, written in .netstandard 2.0, that serves as an extension for [autofac's containerbuilder](https://autofac.org/).
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
        // this will add all your Request- and Notificationhandler
        // that are located in the same project as your program-class
        builder.RegisterMediatR(typeof(Program).Assembly);
        
        var container = builder.Build();
        
        var mediator = container.Resolve<IMediator>();
        
        var response = await mediator.Send(new PingCommand());
        
        // more code here
    }
}
```

For more information about the usage please check out the [samples](https://github.com/cleancodelabs/MediatR.Extensions.Autofac.DependencyInjection/tree/master/samples) I have provided.
