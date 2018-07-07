using System;
using System.Collections.Generic;
using MediatR.Extensions.Autofac.DependencyInjection.Samples.Shared.Entities;

namespace MediatR.Extensions.Autofac.DependencyInjection.Samples.Shared.Repositories
{
    public interface ICustomersRepository
    {
        bool AddCustomer(Customer customer);

        ICollection<Customer> GetAll(); 
        
        Customer FindCustomer(Guid id);
    }
}