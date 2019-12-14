# [3.1.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/3.1.0) (2019-12-14)

## Features

* It is now possible to pass in behaviors that implement `IPipelineBehavior<TRequest, TResponse>` that will be resolved after `RequestPreProcessorBehavior<TRequest, Tresponse>` and before `RequestPostProcessorBehavior<TRequest, Tresponse>`

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

// Very important here is that we pass in the behavior we want to be executed first in the end.
// `Autofac` resolves what ever was registered last in the first place when having multiple implementations for one interface and/or generics
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

// Very important here is that we pass in the behavior we want to be executed first in the end.
// Unless it is constrainted to a specific type. Then it will always be executed first.
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