using System.Threading;
using System.Threading.Tasks;

namespace MediatR.Extensions.Autofac.DependencyInjection.Tests.Behaviors;

public class NoopBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : class, INoopRequest<TResponse>
{
    public static int HitCount = 0;
        
    public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        Interlocked.Increment(ref HitCount);
        return next();
    }
}