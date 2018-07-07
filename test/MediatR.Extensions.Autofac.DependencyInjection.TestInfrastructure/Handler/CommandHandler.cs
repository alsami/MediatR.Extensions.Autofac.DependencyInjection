using System.Threading;
using System.Threading.Tasks;
using MediatR.Extensions.Autofac.DependencyInjection.TestInfrastructure.Commands;

namespace MediatR.Extensions.Autofac.DependencyInjection.TestInfrastructure.Handler
{
    public class CommandHandler : IRequestHandler<VoidCommand>
    {
        public Task<Unit> Handle(VoidCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}