using System;

namespace MediatR.Extensions.AutoFac.DependencyInjection.TestInfrastructure.Commands
{
    public class Pong
    {
        public DateTime PingReceivedAt { get; }

        public Pong(DateTime pingReceivedAt)
        {
            this.PingReceivedAt = pingReceivedAt;
        }
    }
}