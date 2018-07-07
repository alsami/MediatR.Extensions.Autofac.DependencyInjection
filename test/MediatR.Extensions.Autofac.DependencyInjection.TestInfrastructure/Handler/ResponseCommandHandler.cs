using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Extensions.AutoFac.DependencyInjection.TestInfrastructure.Commands;

namespace MediatR.Extensions.AutoFac.DependencyInjection.TestInfrastructure.Handler
{
    public class ResponseCommandHandler : IRequestHandler<ResponseCommand, Response>
    {
        public async Task<Response> Handle(ResponseCommand request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new Response(DateTime.UtcNow)).ConfigureAwait(false);
        }
    }
}