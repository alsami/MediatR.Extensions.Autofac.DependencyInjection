using System;

namespace MediatR.Extensions.Autofac.DependencyInjection.Shared.Dto;

public class CustomerDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }
}