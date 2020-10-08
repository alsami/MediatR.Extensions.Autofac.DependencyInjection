namespace MediatR.Extensions.Autofac.DependencyInjection.Tests.Behaviors
{
    public interface INoopRequest<out TResponse> : IRequest<TResponse>
    {
        
    }
}