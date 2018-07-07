using System;

namespace MediatR.Extensions.AutoFac.DependencyInjection.TestInfrastructure.Commands
{
    public class PingResponse
    {
        public DateTime PingReceivedAt { get; }

        public PingResponse(DateTime pingReceivedAt)
        {
            this.PingReceivedAt = pingReceivedAt;
        }
    }
}