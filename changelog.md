# [2.0.1](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/2.0.1) (2018-01-25)

## Bugfixes

* Support older .NET-Framework versions by using `typeof(T).GetTypeInfo().Assembly` rather than `typeof(T).Assembly`. Closes [#1](https://github.com/cleancodelabs/MediatR.Extensions.Autofac.DependencyInjection/issues/1)

# [2.0.0](https://www.nuget.org/packages/MediatR.Extensions.Autofac.DependencyInjection/2.0.0) (2018-12-11)

## Features

* Update to MediatR 6.0.0 and Autofac.Extensions.DependencyInjection 4.8.1

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