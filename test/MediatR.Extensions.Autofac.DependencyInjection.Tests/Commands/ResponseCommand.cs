using MediatR.Extensions.Autofac.DependencyInjection.Tests.Behaviors;

namespace MediatR.Extensions.Autofac.DependencyInjection.Tests.Commands;

public class ResponseCommand : INoopRequest<Response>
{
}