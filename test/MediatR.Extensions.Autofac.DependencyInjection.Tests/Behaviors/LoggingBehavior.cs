using System.Threading;
using System.Threading.Tasks;

namespace MediatR.Extensions.Autofac.DependencyInjection.Tests.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        return next();
    }
}