using MediatR.Extensions.Autofac.DependencyInjection.TestInfrastructure.Behaviors;

namespace MediatR.Extensions.Autofac.DependencyInjection.TestInfrastructure.Commands
{
    public class ResponseCommand : INoopRequest<Response>
    {
    }
}