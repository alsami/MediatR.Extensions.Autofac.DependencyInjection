using System.Threading;
using System.Threading.Tasks;
using MediatR.Extensions.Autofac.DependencyInjection.Tests.Commands;

namespace MediatR.Extensions.Autofac.DependencyInjection.Tests.Handler
{
    public class CommandHandler : IRequestHandler<VoidCommand>
    {
        public Task<Unit> Handle(VoidCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}