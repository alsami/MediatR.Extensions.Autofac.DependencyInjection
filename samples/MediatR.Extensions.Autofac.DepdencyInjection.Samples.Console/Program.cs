using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Samples.Shared.Commands;
using MediatR.Extensions.Autofac.DependencyInjection.Samples.Shared.Exceptions;
using MediatR.Extensions.Autofac.DependencyInjection.Samples.Shared.Queries;
using MediatR.Extensions.Autofac.DependencyInjection.Samples.Shared.Repositories;

namespace MediatR.Extensions.Autofac.DepdencyInjection.Samples.Console
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var ctx = new CancellationToken();
            var ctxs = CancellationTokenSource.CreateLinkedTokenSource(ctx);

            System.Console.CancelKeyPress += (x, y) =>
            {
                y.Cancel = true;
                ctxs.Cancel(false);
            };
            
            var builder = new ContainerBuilder();
            
            builder.AddMediatR(typeof(CustomerLoadQuery).Assembly);

            builder.RegisterType<CustomersRepository>()
                .As<ICustomersRepository>()
                .SingleInstance();

            var container = builder.Build();

            var lifetimeScope = container.Resolve<ILifetimeScope>();

            var googleCustomerAddCommand = new CustomerAddCommand(Guid.NewGuid(), "google");

            using (var scope = lifetimeScope.BeginLifetimeScope())
            {
                var mediator = scope.Resolve<IMediator>();

                await mediator.Send(googleCustomerAddCommand, ctx);
            }

            using (var scope = lifetimeScope.BeginLifetimeScope())
            {
                var mediator = scope.Resolve<IMediator>();

                var customer = await mediator.Send(new CustomerLoadQuery(googleCustomerAddCommand.Id), ctx);
                
                System.Console.WriteLine(googleCustomerAddCommand.Name == customer.Name);

                try
                {
                    await mediator.Send(new CustomerLoadQuery(Guid.Empty), ctx);
                }
                catch (CustomerNotFoundException e)
                {
                    System.Console.WriteLine("Expected that the customer could not be found bc we didn't add him b4.");
                }
            }

            System.Console.ReadKey();
        }
    }
}