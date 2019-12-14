using System;
using MediatR.Extensions.Autofac.DependencyInjection.Samples.Shared.Notifications;
using System.Threading.Tasks;
using System.Threading;

namespace MediatR.Extensions.Autofac.DependencyInjection.Samples.Shared.NotificationHandler
{
    public class CustomerAddedNotificationHandler : INotificationHandler<CustomerAddedNotification>
    {
        public async Task Handle(CustomerAddedNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Received CustomerAddedNotification for Customer {notification.Name}");

            await Task.CompletedTask.ConfigureAwait(false);
        }
    }
}