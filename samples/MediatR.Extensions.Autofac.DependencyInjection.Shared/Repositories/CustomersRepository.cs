using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MediatR.Extensions.Autofac.DependencyInjection.Shared.Entities;

namespace MediatR.Extensions.Autofac.DependencyInjection.Shared.Repositories
{
    public class CustomersRepository : ICustomersRepository
    {
        private readonly ConcurrentDictionary<Guid, Customer> customersByCustomerId =
            new ConcurrentDictionary<Guid, Customer>();

        public bool AddCustomer(Customer customer)
        {
            var redudantCustomerName = this.customersByCustomerId
                .Values
                .Any(existingCustomer =>
                    string.Equals(existingCustomer.Name, customer.Name, StringComparison.OrdinalIgnoreCase));

            return !redudantCustomerName && this.customersByCustomerId.TryAdd(customer.Id, customer);
        }

        public ICollection<Customer> GetAll()
        {
            return this.customersByCustomerId.Values;
        }

        public Customer FindCustomer(Guid id)
        {
            this.customersByCustomerId.TryGetValue(id, out var customer);

            return customer;
        }
    }
}