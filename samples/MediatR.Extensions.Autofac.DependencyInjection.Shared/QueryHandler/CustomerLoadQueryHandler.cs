using System.Threading;
using System.Threading.Tasks;
using MediatR.Extensions.Autofac.DependencyInjection.Shared.Dto;
using MediatR.Extensions.Autofac.DependencyInjection.Shared.Exceptions;
using MediatR.Extensions.Autofac.DependencyInjection.Shared.Queries;
using MediatR.Extensions.Autofac.DependencyInjection.Shared.Repositories;

namespace MediatR.Extensions.Autofac.DependencyInjection.Shared.QueryHandler
{
    public class CustomerLoadQueryHandler : IRequestHandler<CustomerLoadQuery, CustomerDto>
    {
        private readonly ICustomersRepository customersRepository;

        public CustomerLoadQueryHandler(ICustomersRepository customersRepository)
        {
            this.customersRepository = customersRepository;
        }

        public async Task<CustomerDto> Handle(CustomerLoadQuery request, CancellationToken cancellationToken)
        {
            var customer = this.customersRepository.FindCustomer(request.Id);

            if (customer == null)
            {
                throw new CustomerNotFoundException();
            }

            return await Task
                .FromResult(new CustomerDto
                {
                    Id = customer.Id,
                    Name = customer.Name
                })
                .ConfigureAwait(false);
        }
    }
}