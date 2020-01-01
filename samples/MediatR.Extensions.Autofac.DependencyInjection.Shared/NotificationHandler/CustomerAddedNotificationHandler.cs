using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Extensions.Autofac.DependencyInjection.Shared.Notifications;

namespace MediatR.Extensions.Autofac.DependencyInjection.Shared.NotificationHandler
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