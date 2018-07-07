using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Extensions.AutoFac.DependencyInjection.TestInfrastructure.Commands;

namespace MediatR.Extensions.AutoFac.DependencyInjection.TestInfrastructure.Handler
{
    public class PingCommandHandler : IRequestHandler<PingCommand, PingResponse>
    {
        public async Task<PingResponse> Handle(PingCommand request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new PingResponse(DateTime.UtcNow)).ConfigureAwait(false);
        }
    }
}