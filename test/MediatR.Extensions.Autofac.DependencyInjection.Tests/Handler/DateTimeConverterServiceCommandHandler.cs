using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using MediatR.Extensions.Autofac.DependencyInjection.Tests.Commands;
using MediatR.Extensions.Autofac.DependencyInjection.Tests.Services;

namespace MediatR.Extensions.Autofac.DependencyInjection.Tests.Handler;

public class ConvertEstDateTimeCommandHandler : IRequestHandler<ConvertEstDateTimeCommand, DateTime>
{
    private readonly IDateTimeConverterService dateTimeConverterService;

    public ConvertEstDateTimeCommandHandler([KeyFilter("EST")] IDateTimeConverterService dateTimeConverterService)
    {
        this.dateTimeConverterService = dateTimeConverterService;
    }
    
    public Task<DateTime> Handle(ConvertEstDateTimeCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(this.dateTimeConverterService.ConvertDateTime(request.DateTime));
    }
}

public class ConvertUtcDateTimeCommandHandler : IRequestHandler<ConvertUtcDateTimeCommand, DateTime>
{
    private readonly IDateTimeConverterService dateTimeConverterService;

    public ConvertUtcDateTimeCommandHandler([KeyFilter("UTC")] IDateTimeConverterService dateTimeConverterService)
    {
        this.dateTimeConverterService = dateTimeConverterService;
    }
    
    public Task<DateTime> Handle(ConvertUtcDateTimeCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(this.dateTimeConverterService.ConvertDateTime(request.DateTime));
    }
}