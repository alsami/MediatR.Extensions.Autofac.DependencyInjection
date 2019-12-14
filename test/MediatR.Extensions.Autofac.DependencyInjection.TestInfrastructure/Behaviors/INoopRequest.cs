namespace MediatR.Extensions.Autofac.DependencyInjection.TestInfrastructure.Behaviors
{
    public interface INoopRequest<out TResponse> : IRequest<TResponse>
    {
        
    }
}