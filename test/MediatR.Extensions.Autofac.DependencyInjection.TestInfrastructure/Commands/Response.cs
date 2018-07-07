using System;

namespace MediatR.Extensions.Autofac.DependencyInjection.TestInfrastructure.Commands
{
    public class Response
    {
        public DateTime PingReceivedAt { get; }

        public Response(DateTime pingReceivedAt)
        {
            this.PingReceivedAt = pingReceivedAt;
        }
    }
}