using System;

namespace MediatR.Extensions.Autofac.DependencyInjection.Shared.Commands;

public class CustomerAddCommand : IRequest<Unit>
{
    public Guid Id { get; }

    public string Name { get; }

    public CustomerAddCommand(Guid id, string name)
    {
        this.Id = id;
        this.Name = name;
    }
}