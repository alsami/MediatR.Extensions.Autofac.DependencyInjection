using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using MediatR.Extensions.Autofac.DependencyInjection.Shared.Commands;
using MediatR.Extensions.Autofac.DependencyInjection.Shared.Exceptions;
using MediatR.Extensions.Autofac.DependencyInjection.Shared.Queries;
using MediatR.Extensions.Autofac.DependencyInjection.Shared.Repositories;

namespace MediatR.Extensions.Autofac.DependencyInjection.ConsoleApp;

public static class Program
{
    public static async Task Main(string[] _)
    {
        var ctx = new CancellationToken();
        var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(ctx);

        Console.CancelKeyPress += (x, y) =>
        {
            y.Cancel = true;
            cancellationTokenSource.Cancel(false);
        };

        var builder = new ContainerBuilder();

        var configuration = MediatRConfigurationBuilder.Create("<YourLicenseGoesHere>", typeof(CustomerLoadQuery).Assembly)
            .WithAllOpenGenericHandlerTypesRegistered()
            .Build();
        builder.RegisterMediatR(configuration);

        builder.RegisterType<CustomersRepository>()
            .As<ICustomersRepository>()
            .SingleInstance();

        var container = builder.Build();

        var lifetimeScope = container.Resolve<ILifetimeScope>();

        var googleCustomerAddCommand = new CustomerAddCommand(Guid.NewGuid(), "google");

        await using (var scope = lifetimeScope.BeginLifetimeScope())
        {
            var mediator = scope.Resolve<IMediator>();

            await mediator.Send(googleCustomerAddCommand, ctx);
        }

        await using (var scope = lifetimeScope.BeginLifetimeScope())
        {
            var mediator = scope.Resolve<IMediator>();

            var customer = await mediator.Send(new CustomerLoadQuery(googleCustomerAddCommand.Id), ctx);

            Console.WriteLine(googleCustomerAddCommand.Name == customer.Name);

            try
            {
                await mediator.Send(new CustomerLoadQuery(Guid.Empty), ctx);
            }
            catch (CustomerNotFoundException)
            {
                Console.WriteLine("Expected that the customer could not be found bc we didn't add him b4.");
            }
        }

        Console.ReadKey();
    }
}