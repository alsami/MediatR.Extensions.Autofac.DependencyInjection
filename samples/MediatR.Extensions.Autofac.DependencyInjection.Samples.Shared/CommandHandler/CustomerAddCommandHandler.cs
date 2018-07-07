using System.Threading;
using System.Threading.Tasks;
using MediatR.Extensions.Autofac.DependencyInjection.Samples.Shared.Commands;
using MediatR.Extensions.Autofac.DependencyInjection.Samples.Shared.Entities;
using MediatR.Extensions.Autofac.DependencyInjection.Samples.Shared.Exceptions;
using MediatR.Extensions.Autofac.DependencyInjection.Samples.Shared.Notifications;
using MediatR.Extensions.Autofac.DependencyInjection.Samples.Shared.Repositories;

namespace MediatR.Extensions.Autofac.DependencyInjection.Samples.Shared.CommandHandler
{
    public class CustomerAddCommandHandler : IRequestHandler<CustomerAddCommand>
    {
        private readonly ICustomersRepository customersRepository;
        private readonly IMediator mediator;

        public CustomerAddCommandHandler(ICustomersRepository customersRepository, IMediator mediator)
        {
            this.customersRepository = customersRepository;
            this.mediator = mediator;
        }

        public async Task<Unit> Handle(CustomerAddCommand request, CancellationToken cancellationToken)
        {
            if (!this.customersRepository.AddCustomer(new Customer(request.Id, request.Name)))
            {
                throw new CustomerAlreadyExistsException();
            }

            await this.mediator
                .Publish(new CustomerAddedNotification(request.Name), cancellationToken)
                .ConfigureAwait(false);

            return Unit.Value;
        }
    }
}