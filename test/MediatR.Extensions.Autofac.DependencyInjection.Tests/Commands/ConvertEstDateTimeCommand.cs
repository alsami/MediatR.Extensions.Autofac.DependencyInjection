using System;
using MediatR.Extensions.Autofac.DependencyInjection.Tests.Services;

namespace MediatR.Extensions.Autofac.DependencyInjection.Tests.Commands;

public class ConvertEstDateTimeCommand : IRequest<DateTime>
{
    public DateTime DateTime { get; }

    public ConvertEstDateTimeCommand(DateTime dateTime)
    {
        this.DateTime = dateTime;
    }
}

public class ConvertUtcDateTimeCommand : IRequest<DateTime>
{
    public DateTime DateTime { get; }

    public ConvertUtcDateTimeCommand(DateTime dateTime)
    {
        this.DateTime = dateTime;
    }
}