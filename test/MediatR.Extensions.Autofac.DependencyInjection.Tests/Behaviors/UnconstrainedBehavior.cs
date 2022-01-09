using System.Threading;
using System.Threading.Tasks;

namespace MediatR.Extensions.Autofac.DependencyInjection.Tests.Behaviors;

public class UnconstrainedBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        return next();
    }
}