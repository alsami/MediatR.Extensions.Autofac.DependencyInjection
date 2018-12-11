using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Extensions.Autofac.DependencyInjection.TestInfrastructure.Commands;

namespace MediatR.Extensions.Autofac.DependencyInjection.TestInfrastructure.Handler
{
    public class SampleNotificationHandler : INotificationHandler<SampleNotification>
    {
        public async Task Handle(SampleNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Received notification: {notification.Message}");
            await Task.CompletedTask;
        }
    }
}