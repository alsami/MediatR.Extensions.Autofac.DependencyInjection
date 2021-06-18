using System.Threading;
using System.Threading.Tasks;
using MediatR.Extensions.Autofac.DependencyInjection.Tests.Commands;
using MediatR.Pipeline;

namespace MediatR.Extensions.Autofac.DependencyInjection.Tests.PreProcessor
{
    public class PingRequestPreProcessor : IRequestPreProcessor<VoidCommand>
    {
        public Task Process(VoidCommand request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}