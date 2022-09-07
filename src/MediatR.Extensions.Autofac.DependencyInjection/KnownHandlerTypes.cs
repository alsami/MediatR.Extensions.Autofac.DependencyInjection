using System;
using MediatR.Pipeline;

namespace MediatR.Extensions.Autofac.DependencyInjection;

public static class KnownHandlerTypes
{
    public static readonly Type[] AllTypes =
    {
        typeof(IRequestPreProcessor<>),
        typeof(IRequestHandler<,>),
        typeof(IStreamRequestHandler<,>),
        typeof(IRequestPostProcessor<,>),
        typeof(IRequestExceptionHandler<,,>),
        typeof(IRequestExceptionAction<,>),
        typeof(INotificationHandler<>),
    };
}