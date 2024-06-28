# [12.1.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/12.1.0) (2024-06-28)

## Features

* Update `MediatR` to `12.3.0`

# [12.0.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/12.0.0) (2024-01-22)

## Breaking changes

* Update `Autofac` to `8.0.0`. For the list of breaking-changes, take a look at the [official release](https://github.com/autofac/Autofac/releases/tag/v8.0.0)

# [11.3.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/11.3.0) (2023-11-21)

## Chores

* Update `MediatR` to `12.2.0`

# [11.2.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/11.2.0) (2023-08-28)

## Chores

* Update `Autofac` to `7.1.0`

# [11.1.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/11.1.0) (2023-08-02)

## Chores

* Update `Autofac` to `7.0.1`
* Update `MediatR` to `11.1.1` 

# [11.0.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/11.0.0) (2023-03-08)

## Breaking changes

* Remove obsolete extension method to register `MediatR`. From now on you will require to use `builder.RegisterMediatR(MediatRConfiguration)` and provide a `MediatRConfiguration` using `MediatRConfigurationBuilder`.
* `Autofac` has been updated to version `7.0.0`. There are a set of breaking changes. Some of them will not have any effects on you but you will need to evaluate that. Please [check out this link](https://github.com/autofac/Autofac/releases/tag/v7.0.0) for more details.

## Chores

* Update `Autofac` to `7.0.0`
* Update `MediatR` to `11.0.1`

# [10.0.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/10.0.0) (2023-02-17)

## Breaking changes

* Remove obsolete extension method to register `MediatR`. From now on you will require to use `builder.RegisterMediatR(MediatRConfiguration)` and provide a `MediatRConfiguration` using `MediatRConfigurationBuilder`.
* `MediatR` has been updated to version `12.0.0`. There are a set of breaking changes that you will need to fix. Please [follow the migration guide](https://github.com/jbogard/MediatR/wiki/Migration-Guide-11.x-to-12.0) for more details.

## Features

* Add support to register a custom implementation that implements all mediator interface (`IMediator`, `ISender`, `IPublisher`)
* Add support to register a custom implementation of `INotificationPublisher`
* Add support for `IRequestHandler<>` which is required for `MediatR` version `12.0.0`
* Add `ServiceProviderWrapper` that implements `IServiceProvider` and uses `ILifeTimeScope` to resolve services
* Add `ServiceProviderWrapper` to the container but only if `typeof(IServiceProvider)` has not been registered yet
* Re-add support for `netstandard2.0` to align with `MediatR`

# [9.2.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/9.2.0) (2023-01-16)

## Features

* Add possibility to register `MediatR` dependencies as either `Transient` or `Scoped`. `Transient` translates to `InstancePerDependency` whereas `Scoped` translates to `InstancePerLifetimeScope`

## Deprecation

* All extensions method on `ContainerBuilder` have been marked as deprecated and will be removed with version `10.0.0`. Please use `builder.RegisterMediatR(MediatRConfiguration)`

# [9.1.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/9.1.0) (2022-11-23)

## Features

* Update `MediatR` to version `11.0.0`
* Update `Autofac` to version `6.5.0`

# [9.0.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/9.0.0) (2022-09-30)

## Features

* Update `MediatR` to version `11.0.0`

## Breaking changes

* There is a small breaking-change regarding the order of parameters in classes that implement `IPipelineBehavior<TRequest, TResponse>`. Please see the release-notes of [MediatR](https://github.com/jbogard/MediatR/releases/tag/v11.0.0) for more details.

# [8.2.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/8.2.0) (2022-09-07)

## Features

* Expose `MediatRConfiguration` and `MediatRConfigurationBuilder` as a new way to build the `MediatR` configuration
* Add new extension that takes an instance of `MediatRConfiguration` as a parameter
* Allow automatic registration to be disabled using `MediatRConfiguration`. Closes [#10](https://github.com/alsami/MediatR.Extensions.Autofac.DependencyInjection/issues/10)

# [8.1.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/8.1.0) (2022-05-31)

## Features

* Update `Autofac` to `6.4.0`

# [8.0.1](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/8.0.1) (2022-01-30)

## Features

* Update `MediatR` to `10.0.1`

# [8.0.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/8.0.0) (2022-01-09)

## Breaking changes

* `MediatR` version 10.0.0 dropped support for `netstandard2.0`. From now own only `netstandard2.1` is supported
* Previously marked obsolete methods `AddMediatR` have been removed

## Features

* Add support for `IStreamRequestHandler<,>` and `IStreamRequestPipelineBehavior<,>`

# [7.4.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/7.4.0) (2021-11-12)

## Features

* Update `Autofac` to `6.3.0`

# [7.3.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/7.3.0) (2021-06-21)

## Features

* Register types `IRequestPreProcessor<>` and `IRequestPostProcessor<,>` of provided assembly. Implements [#7](https://github.com/alsami/MediatR.Extensions.Autofac.DependencyInjection/issues/7)

# [7.2.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/7.2.0) (2021-04-23)

## Features

* Upgrade `Autofac` to version `6.2.0`

# [7.1.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/7.1.0) (2021-01-09)

## Features

* Upgrade `Autofac` to version `6.1.0`
* Add dedicated TFM for `netstandard2.1`

## Fixes

* Make sure release-notes URL is correctly set in package meta-data.

# [7.0.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/7.0.0) (2020-10-08)

## Breaking Changes

* `MediatR` has been updated to version `9.0.0`. This release contains minor breaking changes. Check out the [blog-post](https://jimmybogard.com/mediatr-9-0-released/) for more details.

# [6.0.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/6.0.0) (2020-09-29)

## Breaking Changes

* `Autofac` has been updated to version `6.0.0`. This release contains many new features but also breaking-changes. Check out this [blog-post](https://alistairevans.co.uk/2020/09/28/autofac-6-0-released/) for more information.

## Deprecation notice

* `AddMediatR` has been marked as deprecated and will be removed with version `7.0.0`.

## Features

* New extensions were added that are more aligned with the `Autofac` syntax for registering dependencies. Please use `RegisterMediatR` instead of `AddMediatR`


# [5.3.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/5.3.0) (2020-08-02)

## Features

* Update `MediatR` to version `8.1.0`

# [5.2.1](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/5.2.1) (2020-07-03)

## Features

* Update `MediatR` to version `8.0.2`. This enables nullable features of C#8

# [5.2.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/5.2.0) (2020-06-11)

## Chore

* Update `Autofac` to version `5.2.0`

# [5.1.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/5.1.0) (2020-02-22)

## Chore

* Update `Autofac` to version `5.1.1`
* Update sample project dependencies

# [5.0.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/5.0.0) (2020-01-29)

## Breaking changes

* `Autofac` has been updated to `5.0.0`. The release of `Autofac` contains breaking changes, mostly making the container immutable. You can read more about the changes [here](https://www.paraesthesia.com/archive/2020/01/27/autofac-5-released/).

# [4.0.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/4.0.0) (2020-01-01)

## Breaking changes

* `MediatR` has been updated to `8.0.0`. This major release contains small breaking changes. Check out this [post](https://jimmybogard.com/mediatr-8-0-released) for more information.
* Open types were previously registered with implementing interfaces which can cause handlers two be [called twice](https://github.com/jbogard/MediatR/issues/462) when a class implements `IRequestHandler<,>` and `INotificationHandler<>`. To prevent this from happening, `.AsImplementedInterfaces()` has been removed from the registration.
If you wish to register it like before, use this code after calling the extension method `AddMediatR`.

```csharp
var openHandlerTypes = new[]
{
    typeof(IRequestHandler<,>),
    typeof(IRequestExceptionHandler<,,>),
    typeof(IRequestExceptionAction<,>),
    typeof(INotificationHandler<>),
};

foreach (var openHandlerType in openHandlerTypes)
{
    builder.RegisterAssemblyTypes(this.assemblies)
        .AsClosedTypesOf(openHandlerType)
        .AsImplementedInterfaces();
}
```
* Two extensions that were likely not used have been removed from `ContainerBuilderExtensions`.

## Features

* Support for `MediatR` version `8.0.0` by registering the new behaviors and open-handlers for handling exceptions. Check out this [post](https://jimmybogard.com/mediatr-8-0-released) for more information.


# [3.1.1](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/3.1.1) (2019-12-26)

## Bugfixes

* Make sure that `MediatRModule` does not throw when no custom types are passed into the extension methods, fixes [#4](https://github.com/cleancodelabs/MediatR.Extensions.Autofac.DependencyInjection/issues/4)

# [3.1.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/3.1.0) (2019-12-14)

## Features

* It is now possible to pass in behaviors that implement `IPipelineBehavior<TRequest, TResponse>`.

### Given sample with two custom behaviors without constraints:

```csharp
public LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) 
    {
        _logger = logger;
    }

    public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        _logger.LogInformation("Got request at {requestTime} and content", DateTime.UtcNow().ToString("O"), JsonConvert.SerializeObject(request));
        
        return next();
    }
}

public CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
{
    private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;
    private readonly ICache _cache;

    public CachingBehavior(ILogger<CachingBehavior<TRequest, TResponse>> logger, ICache cache) 
    {
        _logger = logger;
        _cache = cache;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var key = GenerateKeyFromRequest(request);

        _cache.TryGetValue(key, out var cachedResponse) 
        {
            _logger.LogInformation("{key} found in cache, using cached-value", key);
            return await cachedResponse;
        }

        var response = next();

        _cache.Set(key, response);
        
        return await response;
    }
}
```

We would register `MediatR` like that

```csharp
var containerBuilder = new ContainerBuilder();

// Since there are two default behaviors the execution order would now be the following
// 1. LoggingBehavior<TRequest, TResponse>
// 2. CachingBehavior<TRequest, TResponse>
// 3. RequestPreProcessorBehavior<TRequest, TResponse>
// 4. RequestPostProcessorBehavior<TRequest, TResponse>
containerBuilder.RegisterMediatR(typeof(SomeClassThatIsInSameAssemblyAsMediatRTypes).Assembly, 
    typeof(CachingBehavior<,>), typeof(LoggingBehavior<,>));
```

### Given sample with two custom behaviors with and without constraints:

```csharp
public LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) 
    {
        _logger = logger;
    }

    public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        _logger.LogInformation("Got request at {requestTime} and content", DateTime.UtcNow().ToString("O"), JsonConvert.SerializeObject(request));
        
        return next();
    }
}

public CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : class, ICacheableRequest<TResponse>
{
    private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;
    private readonly ICache _cache;

    public CachingBehavior(ILogger<CachingBehavior<TRequest, TResponse>> logger, ICache cache) 
    {
        _logger = logger;
        _cache = cache;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var key = GenerateKeyFromRequest(request);

        _cache.TryGetValue(key, out var cachedResponse) 
        {
            _logger.LogInformation("{key} found in cache, using cached-value", key);
            return await cachedResponse;
        }

        var response = next();

        _cache.Set(key, response);
        
        return await response;
    }
}
```

We would register `MediatR` like that again

```csharp
var containerBuilder = new ContainerBuilder();

// Since there are two default behaviors, two custom behaviors and one with a constraint, the execution order would now be the following
// 1. CachingBehavior<TRequest, TResponse>
// 2. LoggingBehavior<TRequest, TResponse>
// 3. RequestPreProcessorBehavior<TRequest, TResponse>
// 4. RequestPostProcessorBehavior<TRequest, TResponse>
containerBuilder.RegisterMediatR(typeof(SomeClassThatIsInSameAssemblyAsMediatRTypes).Assembly, 
    typeof(CachingBehavior<,>), typeof(LoggingBehavior<,>));
```

For more specifics please checkout the [tests](test/MediatR.Extensions.Autofac.DependencyInjection.Tests/ContainerBuilderExtensionsTests.cs)

# [3.0.1](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/3.0.1) (2019-08-20)

## Features

* Update Autofac to version 4.9.4

# [3.0.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/3.0.0) (2019-05-05)

## Features

* Update MediatR to version 7.0.0. Please see the [changelog](https://github.com/jbogard/MediatR/releases/tag/v7.0.0) of MediatR since it contains breaking-changes for the request post-processors.

# [2.1.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/2.1.0) (2019-03-29)

## Features

* Update Autofac to version 4.9.2

# [2.1.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/2.1.0) (2019-03-17)

## Features

* Update Autofac to version 4.9.1
* Update Autofac.Extensions.DependencyInjection 4.4.0

## Chore

* Adjust copyright

# [2.0.1](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/2.0.1) (2019-01-25)

## Bugfixes

* Support older .NET-Framework versions by using `typeof(T).GetTypeInfo().Assembly` rather than `typeof(T).Assembly`. Closes [#1](https://github.com/cleancodelabs/MediatR.Extensions.Autofac.DependencyInjection/issues/1)

# [2.0.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/2.0.0) (2018-12-11)

## Features

* Update to MediatR 6.0.0
* Update Autofac.Extensions.DependencyInjection 4.3.0

## Possible breaking changes

* The latest implementation might contain breaking changes, if you did overrider MediatR's default behaviours. For more information please checkout the [release-notes](https://github.com/jbogard/MediatR/releases)

# [1.2.1](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/1.1.0) (2018-08-11)

## Chore

* Adjust package-infos

# [1.2.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/1.1.0) (2018-07-30)

## Features

* Update MediatR to  5.1.0

# [1.1.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/1.1.0) (2018-07-14)

## Features

* Allow chaining of method-calls

# [1.0.1](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/1.0.1) (2018-07-08)

## Chore

* update project file

# [1.0.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/1.0.0) (2018-07-07)

## Intial Release

* Allow MediatR and it's components to be registered via an extension method for Autofac.