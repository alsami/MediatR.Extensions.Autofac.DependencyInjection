using System.Threading;
using System.Threading.Tasks;
using MediatR.Extensions.Autofac.DependencyInjection.TestInfrastructure.Commands;

namespace MediatR.Extensions.Autofac.DependencyInjection.TestInfrastructure.Handler
{
    public class UnconstraintedCommandHandler : IRequestHandler<UnconstraintedCommand, int>
    {
        public Task<int> Handle(UnconstraintedCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}