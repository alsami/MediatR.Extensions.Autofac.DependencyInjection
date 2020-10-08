using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Extensions.Autofac.DependencyInjection.Tests.Commands;

namespace MediatR.Extensions.Autofac.DependencyInjection.Tests.Handler
{
    public class ResponseCommandHandler : IRequestHandler<ResponseCommand, Response>
    {
        public async Task<Response> Handle(ResponseCommand request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new Response(DateTime.UtcNow)).ConfigureAwait(false);
        }
    }
}