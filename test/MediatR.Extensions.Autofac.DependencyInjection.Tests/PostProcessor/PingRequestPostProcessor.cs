using System.Threading;
using System.Threading.Tasks;
using MediatR.Extensions.Autofac.DependencyInjection.Tests.Commands;
using MediatR.Pipeline;

namespace MediatR.Extensions.Autofac.DependencyInjection.Tests.PostProcessor;

public class PingRequestPostProcessor : IRequestPostProcessor<VoidCommand, Unit>
{
    public Task Process(VoidCommand request, Unit response, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}