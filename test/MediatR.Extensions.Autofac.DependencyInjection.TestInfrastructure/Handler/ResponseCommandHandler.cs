using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Extensions.AutoFac.DependencyInjection.TestInfrastructure.Commands;

namespace MediatR.Extensions.AutoFac.DependencyInjection.TestInfrastructure.Handler
{
    public class QueryCommandHandler : IRequestHandler<QueryCommand, QueryResponse>
    {
        public async Task<QueryResponse> Handle(QueryCommand request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new QueryResponse(DateTime.UtcNow)).ConfigureAwait(false);
        }
    }
}