using System;
using MediatR.Extensions.Autofac.DependencyInjection.Shared.Dto;

namespace MediatR.Extensions.Autofac.DependencyInjection.Shared.Queries;

public class CustomerLoadQuery : IRequest<CustomerDto>
{
    public Guid Id { get; }

    public CustomerLoadQuery(Guid id)
    {
        this.Id = id;
    }
}