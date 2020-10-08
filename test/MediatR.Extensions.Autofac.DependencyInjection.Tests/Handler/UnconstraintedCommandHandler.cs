using System.Threading;
using System.Threading.Tasks;
using MediatR.Extensions.Autofac.DependencyInjection.Tests.Commands;

namespace MediatR.Extensions.Autofac.DependencyInjection.Tests.Handler
{
    public class UnconstraintedCommandHandler : IRequestHandler<UnconstraintedCommand, int>
    {
        public Task<int> Handle(UnconstraintedCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}